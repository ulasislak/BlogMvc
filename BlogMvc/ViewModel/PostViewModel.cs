namespace BlogMvc.ViewModel
{
    public class PostViewModel:BaseVm
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateOnly? PublishedAt { get; set; }
        public string Tags { get; set; }
        public string? Views { get; set; }
        public int? GuestId { get; set; }        
        public GuestViewModel Guests { get; set; }
        public string Name { get; set; } // Yazarın adı
    }
}
