using System.Collections.Generic;

namespace Eagle.Models
{
    public class SearchItemsResponse
    {
        public string status { get; set; }
        public List<ItemData> data { get; set; }
        
        public class ItemData
        {
            public string id { get; set; }
            public string name { get; set; }
            public int size { get; set; }
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
            public List<Palette> palettes { get; set; }
        }

        public class Palette
        {
            public List<int> color { get; set; }
            public double ratio { get; set; }
        }
    }
}