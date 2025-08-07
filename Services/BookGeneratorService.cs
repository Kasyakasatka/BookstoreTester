using Bogus;
using BookstoreTester.Mvc.Constants;
using BookstoreTester.Mvc.DTOs;
using BookstoreTester.Mvc.Enums;
using BookstoreTester.Mvc.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BookstoreTester.Mvc.Services;

public class BookGeneratorService : IBookGeneratorService
{
    private readonly ILogger<BookGeneratorService> _logger;

    public BookGeneratorService(ILogger<BookGeneratorService> logger)
    {
        _logger = logger;
    }

    public List<BookResponseDto> GenerateBooks(BookRequestDto request)
    {
       
        var bookLanguage = request.Language;
        var fakerLanguageString = bookLanguage.GetDescription();
        var combinedSeed = request.Seed + request.PageNumber;
        var bookFaker = new Faker(fakerLanguageString);
        bookFaker.Random = new Randomizer(combinedSeed);
        _logger.LogInformation("Generating books for language: {Language}, seed: {Seed}, page: {PageNumber}",
        fakerLanguageString, request.Seed, request.PageNumber);
        var titleGenerator = GetTitleGenerator(bookFaker, bookLanguage);
        var books = new List<BookResponseDto>();
        for (int i = 0; i < request.PageSize; i++)
        {
            var bookIndex = (request.PageNumber - 1) * request.PageSize + i + 1;
            var likesReviewsSeed = combinedSeed + i;
            var randomizer = new Randomizer(likesReviewsSeed);
            var likesCount = (int)Math.Floor(request.Likes);
            if (randomizer.Double() < request.Likes % 1)
            {
                likesCount++;
            }
            var reviewsCount = (int)Math.Floor(request.Reviews);
            if (randomizer.Double() < request.Reviews % 1)
            {
                reviewsCount++;
            }
            var authorFaker = new Faker(fakerLanguageString);
            authorFaker.Random = new Randomizer(combinedSeed + i + BookConstants.AuthorSeedOffset);
            var authors = new List<string>();
            int numberOfAuthors = authorFaker.Random.Number(BookConstants.MinAuthors, BookConstants.MaxAuthors);
            for (int j = 0; j < numberOfAuthors; j++)
            {
                authors.Add(authorFaker.Person.FullName);
            }
            var reviews = GenerateReviews(reviewsCount, fakerLanguageString, combinedSeed + i + BookConstants.ReviewsSeedOffset);
            string bookTitle = titleGenerator();
            string publisherName = bookFaker.Company.CompanyName();
            publisherName = CleanPublisherName(publisherName, bookLanguage);
            var bookCoverUrl = GenerateBookCover(bookTitle, combinedSeed + i);
            var book = new BookResponseDto
            {
                Index = bookIndex,
                Isbn = bookFaker.Commerce.Ean13(),
                Title = bookTitle,
                Authors = authors,
                Publisher = publisherName,
                Likes = likesCount,
                Reviews = reviews,
                CoverImageUrl = bookCoverUrl
            };

            books.Add(book);
        }

        return books;
    }
    private static Func<string> GetTitleGenerator(Faker faker, BookLanguages language)
    {
        return language switch
        {
            BookLanguages.Russian => () => faker.Name.JobTitle(),
            BookLanguages.Japanese => () => faker.Name.FullName(),
            _ => () => ToTitleCase(string.Join(" ", faker.Random.Words(faker.Random.Number(BookConstants.MinTitleWords, BookConstants.MaxTitleWords))))
        };
    }
    private static string ToTitleCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }
        TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
        return textInfo.ToTitleCase(input);
    }
    private static string CleanPublisherName(string name, BookLanguages language)
    {
        return language switch
        {
            BookLanguages.Japanese => name.Replace(BookConstants.UnwantedPublisherSuffix, "").Replace(BookConstants.UnwantedPublisherConjunction, "").Trim(),
            _ => name
        };
    }
    private static string GenerateBookCover(string title, int seed)
    {
        var encodedTitle = Uri.EscapeDataString(title);
        var random = new Random(seed);
        var r = random.Next(BookConstants.MinColorValue, BookConstants.MaxColorValue);
        var g = random.Next(BookConstants.MinColorValue, BookConstants.MaxColorValue);
        var b = random.Next(BookConstants.MinColorValue, BookConstants.MaxColorValue);
        var randomColor = $"{r:X2}{g:X2}{b:X2}";
        return string.Format(BookConstants.ImageUrlTemplate, BookConstants.BookCoverSize, randomColor, BookConstants.TextColor, encodedTitle);
    }
    private static List<ReviewDto> GenerateReviews(int count, string language, int seed)
    {
        var reviews = new List<ReviewDto>();
        if (count == 0) return reviews;
        var bookLanguage = EnumExtensions.GetEnumValueFromDescription<BookLanguages>(language);
        var reviewGenerator = GetReviewGenerator(bookLanguage);

        for (int i = 0; i < count; i++)
        {
            var reviewFaker = new Faker(language);
            reviewFaker.Random = new Randomizer(seed + i);
            reviews.Add(new ReviewDto
            {
                Author = reviewFaker.Person.FullName,
                Text = reviewGenerator(reviewFaker)
            });
        }
        return reviews;
    }
    private static Func<Faker, string> GetReviewGenerator(BookLanguages language)
    {
        return language switch
        {
            BookLanguages.Russian => (f) => f.Lorem.Paragraph(),
            BookLanguages.Japanese => (f) => f.Lorem.Paragraph(),
            _ => (f) => f.Commerce.ProductDescription()
        };
    }
}