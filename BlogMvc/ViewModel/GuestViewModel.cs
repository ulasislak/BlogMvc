using Microsoft.Extensions.Hosting;

namespace BlogMvc.ViewModel
{
    public class GuestViewModel:BaseVm
    {
        public string Mail { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePic { get; set; }
        public List<PostViewModel> Posts { get; set; }
        public List<string>? FollowedTags { get; set; } = new List<string>();
    }
}
