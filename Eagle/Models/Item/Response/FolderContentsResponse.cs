using System.Collections.Generic;

namespace Eagle.Models
{
    public class FolderContentsResponse
    {
        public string status { get; set; }
        public List<File> data { get; set; }
       
        public class File
        {
            public string id { get; set; }
            public string name { get; set; }
            public double size { get; set; }
            public object btime { get; set; }
            public object mtime { get; set; }
            public string ext { get; set; }
            public List<object> tags { get; set; }
            public List<string> folders { get; set; }
            public bool isDeleted { get; set; }
            public string url { get; set; }
            public string annotation { get; set; }
            public object modificationTime { get; set; }
            public int height { get; set; }
            public int width { get; set; }
            public object lastModified { get; set; }
            public double? duration { get; set; }
            public bool? noThumbnail { get; set; }
        }
        
    }
}