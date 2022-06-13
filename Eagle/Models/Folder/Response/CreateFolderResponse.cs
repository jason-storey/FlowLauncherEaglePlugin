using System.Collections.Generic;

namespace Eagle.Models
{

    public class CreateFolderResponse
    {
        public string status { get; set; }
        public Data data { get; set; }

        public class Data
        {
            public string id { get; set; }
            public string name { get; set; }
            public List<object> images { get; set; }
            public List<object> folders { get; set; }
            public long modificationTime { get; set; }
            public ImagesMappings imagesMappings { get; set; }
            public List<object> tags { get; set; }
            public List<object> children { get; set; }
            public bool isExpand { get; set; }
        }
        
        public class ImagesMappings {}
    }
}