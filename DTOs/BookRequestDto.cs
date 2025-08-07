using BookstoreTester.Mvc.Enums;

namespace BookstoreTester.Mvc.DTOs
{
    public class BookRequestDto
    {
        public BookLanguages Language { get; set; } = BookLanguages.English;
        public int Seed { get; set; }
        public double Likes { get; set; }
        public double Reviews { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
