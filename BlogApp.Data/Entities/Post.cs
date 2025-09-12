using BlogApp.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [NotMapped] // Это свойство не будет сохраняться в БД
        public string? TagsInput { get; set; }

        // Навигационные свойства
        public User User { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PostTag>? PostTags { get; set; }
    }
}