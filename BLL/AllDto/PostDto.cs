using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.AllDto
{
    public class PostDto:BaseDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateOnly? PublishedAt { get; set; }
        public string Tags { get; set; }
        public string? Views { get; set; }       
        public int? GuestId { get; set; }    
        public GuestDto Guests { get; set; }
        public string Name { get; set; } // Yazarın adı
    }
}
