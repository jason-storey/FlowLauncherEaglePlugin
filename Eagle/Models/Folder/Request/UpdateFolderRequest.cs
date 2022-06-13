namespace Eagle.Models
{

    public class UpdateFolderRequest
    {
        public string folderId { get; set; }
        public string newName { get; set; }
        public string newDescription { get; set; }
        public string newColor { get; set; }
    }
}