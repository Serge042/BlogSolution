using System;

namespace BlogApp.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; }
        public BlogApp.Data.Entities.Post Post { get; set; }
    }
}
