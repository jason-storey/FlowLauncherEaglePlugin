using System.Collections.Generic;

namespace Eagle.Models
{
    public class AddFromUrlRequest
    {        
        public string url { get; set; }
        public string name { get; set; }
        public string website { get; set; }
        public List<string> tags { get; set; }
        public string annotation { get; set; }
        public long modificationTime { get; set; }
        public string folderId { get; set; }
        public Headers headers { get; set; }

        public class Headers
        {
            public string referer { get; set; }
        }
    }
}