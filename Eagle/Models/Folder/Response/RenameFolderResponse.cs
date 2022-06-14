using System.Collections.Generic;

namespace Eagle.Models
{

    public class RenameFolderResponse
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
            public int size { get; set; }
            public string vstype { get; set; }
            public Styles styles { get; set; }
            public bool isVisible { get; set; }

            public string HashKey { get; set; }
            public string newFolderName { get; set; }
            public bool editable { get; set; }
            public string pinyin { get; set; }
        }

        public class ImagesMappings
        {
        }

        public class Styles
        {
            public int depth { get; set; }
            public bool first { get; set; }
            public bool last { get; set; }
        }
    }
    
    
    
}