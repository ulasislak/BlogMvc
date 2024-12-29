using AutoMapper;
using BLL.AbstractService;
using BlogMvc.Models;
using BlogMvc.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger,IPostService postService,IMapper mapper)
        {
            _logger = logger;
            _postService = postService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var posts=await _postService.GetAllPostAsync();
            return View(_mapper.Map<List<PostViewModel>>(posts));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
