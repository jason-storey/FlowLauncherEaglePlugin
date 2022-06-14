using System.Collections.Generic;

namespace Eagle.Models
{
    public class AddManyFromUrlRequest
    {
        public List<Item> items { get; set; }
        public string folderId { get; set; }

        public class Item
        {
            public string url { get; set; }
            public string name { get; set; }
            public string website { get; set; }
            public string annotation { get; set; }
            public List<string> tags { get; set; }
            public object modificationTime { get; set; }
            public Headers headers { get; set; }
        }

        public class Headers
        {
            public string referer { get; set; }
        }
    }
}