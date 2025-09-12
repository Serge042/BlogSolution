namespace BlogApp.Data.Entities
{
    public class PostTag
    {
        public int PostId { get; set; }
        public int TagId { get; set; }

        // Навигационные свойства
        public BlogApp.Data.Entities.Post Post { get; set; }
        public Tag Tag { get; set; }
    }
}