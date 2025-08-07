namespace BookstoreTester.Mvc.DTOs
{
    public class BookResponseDto
    {
        public int Index { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<string> Authors { get; set; } = new();
        public string Publisher { get; set; } = string.Empty;
        public int Likes { get; set; }
        public List<ReviewDto> Reviews { get; set; } = new();
        public string CoverImageUrl { get; set; } = string.Empty;
    }
}
