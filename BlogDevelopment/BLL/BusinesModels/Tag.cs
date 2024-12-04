namespace BlogDevelopment.BLL.BusinesModels
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Связь с статьями через таблицу PostTag
        public ICollection<PostTag> PostTags { get; set; }
    }
}
