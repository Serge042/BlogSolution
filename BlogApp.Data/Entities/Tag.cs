using System.Collections.Generic;

namespace BlogApp.Data.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Навигационное свойство
        public ICollection<PostTag> PostTags { get; set; }
    }
}