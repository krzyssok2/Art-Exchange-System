namespace LOGIC.Models
{
    public class ArtDataCreationModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ArtCreationCatgegoryModel Catgegory { get; set; }
        public string FileName { get; set; }
    }
    public class ArtCreationCatgegoryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
