using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BookstoreTester.Mvc.DTOs;
using BookstoreTester.Mvc.Services;
using System.Threading.Tasks;
using BookstoreTester.Mvc.Models;

namespace BookstoreTester.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBookGeneratorService _bookGeneratorService;

        public HomeController(IBookGeneratorService bookGeneratorService)
        {
            _bookGeneratorService = bookGeneratorService;
        }
        [HttpGet("/")]
        public IActionResult Index([FromQuery] BookRequestDto request)
        {
            return View(request);
        }

        [HttpGet("api/books")]
        public IActionResult GetBooks([FromQuery] BookRequestDto request)
        {
            var books = _bookGeneratorService.GenerateBooks(request);
            return Ok(books);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}