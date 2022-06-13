using System.Collections.Generic;

namespace Eagle.Models
{

    public class ListRecentFolderResponse
    {
        public string status { get; set; }
        public List<Datum> data { get; set; }

        public class Datum
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public List<object> children { get; set; }
            public object modificationTime { get; set; }
            public List<object> tags { get; set; }
            public string password { get; set; }
            public string passwordTips { get; set; }
            public List<object> images { get; set; }
            public bool isExpand { get; set; }
            public string newFolderName { get; set; }
            public ImagesMappings imagesMappings { get; set; }
            public int imageCount { get; set; }
            public int descendantImageCount { get; set; }
            public string pinyin { get; set; }
            public List<object> extendTags { get; set; }
        }

        public class ImagesMappings
        {
        }
    }

}