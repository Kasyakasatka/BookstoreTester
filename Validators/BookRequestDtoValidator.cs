using BookstoreTester.Mvc.DTOs;
using FluentValidation;

namespace BookstoreTester.Mvc.Validators
{
    public class BookRequestDtoValidator : AbstractValidator<BookRequestDto>
    {
        public BookRequestDtoValidator()
        {
            RuleFor(x => x.Language).NotEmpty().WithMessage("Language cannot be empty.");
            RuleFor(x => x.Seed).GreaterThanOrEqualTo(0).WithMessage("Seed must be a positive number.");
            RuleFor(x => x.Likes).InclusiveBetween(0, 10).WithMessage("Likes must be between 0 and 10.");
            RuleFor(x => x.Reviews).GreaterThanOrEqualTo(0).WithMessage("Reviews must be a positive number.");
            RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }
}
