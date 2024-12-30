using AutoMapper;
using BLL.AbstractService;
using BLL.AllDto;
using BlogMvc.ViewModel;
using DAL.Entities;
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
            var allPosts = await _postService.GetAllPostAsync();

            if (guest != null)
            {
                var guestViewModel = _mapper.Map<GuestViewModel>(guest);


                if (guest.FollowedTags != null && guest.FollowedTags.Any())
                {
                    var followedPosts = allPosts
                        .Where(post => post.Tags.Split(',').Any(tag => guest.FollowedTags.Contains(tag.Trim())))
                        .ToList();

                    guestViewModel.Posts = _mapper.Map<List<PostViewModel>>(followedPosts);
                }
                else
                {

                    guestViewModel.Posts = new List<PostViewModel>();
                }


                var allTags = allPosts
                    .SelectMany(post => post.Tags.Split(','))
                    .Select(tag => tag.Trim())
                    .Distinct()
                    .ToList();

                ViewBag.AllTags = allTags;
                guestViewModel.Id = Id;

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

                var postDto = new PostDto
                {
                    Title = postViewModel.Title,
                    Content = postViewModel.Content,
                    PublishedAt = postViewModel.PublishedAt,
                    Tags = postViewModel.Tags,
                    GuestId = postViewModel.GuestId,
                    Name = postViewModel.Name

                };


                await _postService.AddPostAsync(postDto);


                return RedirectToAction("HomePage");
            }
            catch (Exception ex)
            {
                // Hata durumunda model state'e hata ekle
                ModelState.AddModelError(string.Empty, "An error occurred while saving the post.");
                return View(postViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListPage(int guestId)
        {
            var sessionGuestId = HttpContext.Session.GetInt32("Id");
            if (!sessionGuestId.HasValue || sessionGuestId.Value != guestId)
            {
                return RedirectToAction("Login");
            }

            // Belirli bir kullanıcının yazılarını veritabanından çek
            var posts = await _postService.GetPostsByGuestIdAsync(guestId);
            var viewModel = _mapper.Map<List<PostViewModel>>(posts);

            // Eğer yazı yoksa, kullanıcıya bilgi mesajı göster
            if (viewModel == null || !viewModel.Any())
            {
                ViewBag.Message = "You have no posts yet.";
            }

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> ToggleFollowTag(string tag, int Id)
        {
            var sessionGuestId = HttpContext.Session.GetInt32("Id");
            if (!sessionGuestId.HasValue || sessionGuestId.Value != Id)
            {
                return RedirectToAction("Login");
            }

            var guest = await _guestService.GetGuestByIdAsync(Id);
            if (guest.FollowedTags.Contains(tag))
            {
                guest.FollowedTags.Remove(tag);
                Console.WriteLine($"Tag '{tag}' removed from followed tags.");
            }
            else
            {
                guest.FollowedTags.Add(tag);
                Console.WriteLine($"Tag '{tag}' added from followed tags.");
            }

            await _guestService.UpdateGuestAsync(guest, Id);
            Console.WriteLine("Guest updated in the database.");
            return RedirectToAction("HomePage", new { id = Id });
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int Id)
        {
            var sessionguestId = HttpContext.Session.GetInt32("Id");
            if (!sessionguestId.HasValue || sessionguestId.Value != Id)
            {
                return RedirectToAction("Login");
            }

            var guest = await _guestService.GetGuestByIdAsync(Id);
            var guestViewModel = _mapper.Map<GuestViewModel>(guest);


            return View(guestViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(GuestViewModel guestViewModel, IFormFile profilePic, int Id)
        {

            if (profilePic != null && profilePic.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images/profiles", profilePic.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePic.CopyToAsync(stream);
                }
                guestViewModel.ProfilePic = "\\\\Mac\\Home\\Pictures\\Photos Library.photoslibrary\\originals\\0" + profilePic.FileName;
            }

            var guest = _mapper.Map<GuestDto>(guestViewModel);
            await _guestService.UpdateGuestAsync(guest, Id);
            return RedirectToAction("Profile", new { Id = guestViewModel.Id });
        }

        public IActionResult Logout()
        {

            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> DeleteAccount(int Id)
        {
            var sessionguestId = HttpContext.Session.GetInt32("Id");
            if (!sessionguestId.HasValue || sessionguestId.Value != Id)
            {
                return RedirectToAction("Login");
            }

            var guest = await _guestService.GetGuestByIdAsync(Id);
            if (guest == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
                return RedirectToAction("Profile", new { Id = Id });
            }

            try
            {

                var posts = await _postService.GetPostsByGuestIdAsync(Id);
                foreach (var post in posts)
                {
                    await _postService.DeletePostAsync(post.Id);
                }

                var deleteResult = await _guestService.DeleteGuestAsync(Id);
                if (!deleteResult)
                {
                    TempData["ErrorMessage"] = "Hesap silinirken bir hata oluştu.";
                    return RedirectToAction("Profile", new { Id = Id });
                }

                // Oturumu temizle
                HttpContext.Session.Clear();
                TempData["SuccessMessage"] = "Hesabınız başarıyla silindi.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Hesap silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Profile", new { Id = Id });
            }
        }


        [HttpGet]
        public async Task<IActionResult> ReadingPage(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }


            var guest = await _guestService.GetGuestByIdAsync(post.GuestId.Value);
            if (guest == null)
            {
                return NotFound();
            }


            var viewModel = _mapper.Map<PostViewModel>(post);
            viewModel.Guests = _mapper.Map<GuestViewModel>(guest);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AuthorProfile(int id)
        {

            var guest = await _guestService.GetGuestByIdAsync(id);
            if (guest == null)
            {
                return NotFound();
            }


            var posts = await _postService.GetPostsByGuestIdAsync(id);


            var viewModel = _mapper.Map<GuestViewModel>(guest);
            viewModel.Posts = _mapper.Map<List<PostViewModel>>(posts);

            return View(viewModel);
        }
    }
}
