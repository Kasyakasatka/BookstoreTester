using BookstoreTester.Mvc.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookstoreTester.Mvc.Services
{
    public interface IBookGeneratorService
    {
        List<BookResponseDto> GenerateBooks(BookRequestDto request);
    }
}