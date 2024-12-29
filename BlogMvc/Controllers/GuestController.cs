using AutoMapper;
using BLL.AbstractService;
using BLL.AllDto;
using BlogMvc.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMvc.Controllers
{
    public class GuestController : Controller
    {
        private readonly IGuestService _guestService;
        private readonly IMapper _mapper;
        private readonly IPostService _postService;

        public GuestController(IGuestService guestService, IMapper mapper, IPostService postService)
        {
            _guestService = guestService;
            _mapper = mapper;
            _postService = postService;
        }

       

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(GuestViewModel guestViewModel)
        {


            await _guestService.AddGuestAsync(_mapper.Map<GuestDto>(guestViewModel));
            var newGuest = (await _guestService.GetAllGuestAsync()).FirstOrDefault(g => g.Mail == guestViewModel.Mail);
            if (newGuest != null)
            {
                HttpContext.Session.SetInt32("Id", newGuest.Id);
                return RedirectToAction("HomePage", new { id = newGuest.Id });
            }

            return View(guestViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Mail, string Password)
        {
            var GetAll = await _guestService.GetAllGuestAsync();
            var GuestControl = GetAll.FirstOrDefault(x => x.Mail == Mail && x.Password == Password);
            if (GuestControl != null)
            {
                HttpContext.Session.SetInt32("Id", GuestControl.Id);
                ViewBag.Guest = _mapper.Map<GuestViewModel>(GuestControl);
                return RedirectToAction("HomePage", new { id = GuestControl.Id });
            }
            ModelState.AddModelError(string.Empty, "Geçersiz mail veya şifre.");
            return View();
        }

        public async Task<IActionResult> HomePage(int Id)
        {
            var sessionGuestId = HttpContext.Session.GetInt32("Id");
            if (!sessionGuestId.HasValue || sessionGuestId.Value != Id)
            {
                return RedirectToAction("Login");
            }

            var guest = await _guestService.GetGuestByIdAsync(Id);
            var post=await _postService.GetAllPostAsync();
            if (guest != null)
            {
                var guestViewModel = _mapper.Map<GuestViewModel>(guest);
                guestViewModel.Posts=_mapper.Map<List<PostViewModel>>(post);
                return View(guestViewModel);
            }
            return RedirectToAction("Login");


        }

        [HttpGet]
        public async Task<IActionResult> WritingPage(int Id)
        {
            var sessionGuestId = HttpContext.Session.GetInt32("Id");
            if (!sessionGuestId.HasValue || sessionGuestId.Value != Id)
            {
                return RedirectToAction("Login");
            }

            var postViewModel = new PostViewModel { GuestId = Id }; // PostViewModel oluştur
            return View(postViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> WritingPage(PostViewModel postViewModel)
        {
            try
            {
                // PostDto nesnesini elle oluştur
                var postDto = new PostDto
                {
                    Title = postViewModel.Title,
                    Content = postViewModel.Content,
                    PublishedAt = postViewModel.PublishedAt,
                    Tags = postViewModel.Tags,
                    GuestId = postViewModel.GuestId
                };

                // PostDto'yu veritabanına ekle
                await _postService.AddPostAsync(postDto);

                // Başarılıysa yönlendir
                return RedirectToAction("ListPage");
            }
            catch (Exception ex)
            {
                // Hata durumunda model state'e hata ekle
                ModelState.AddModelError(string.Empty, "An error occurred while saving the post.");
                return View(postViewModel);
            }
        }
    }
}