using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Guest:BaseClass
    {
        public string Mail { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePic { get; set; }
        public List<Post> Posts { get; set; }
        public List<string>? FollowedTags { get; set; } = new List<string>();
    }
}
