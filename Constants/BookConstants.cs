namespace BookstoreTester.Mvc.Constants
{
    public static class BookConstants
    {

        public const int MinTitleWords = 2;
        public const int MaxTitleWords = 4;
        public const int ReviewsSeedOffset = 100;
        public const int AuthorSeedOffset = 100;
        public const int MinAuthors = 1;
        public const int MaxAuthors = 3;
        public const string UnwantedPublisherSuffix = "Sons";
        public const string UnwantedPublisherConjunction = "and";
        public const string ImageUrlTemplate = "https://dummyimage.com/{0}/{1}/{2}&text={3}";
        public const string BookCoverSize = "400x600";
        public const string TextColor = "000";
        public const int MinColorValue = 64;
        public const int MaxColorValue = 193;
    }
}