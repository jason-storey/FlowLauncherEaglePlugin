using System.Collections.Generic;

namespace Eagle.Models
{

    public class ListFolderResponse
    {
        public string status { get; set; }
        public List<Folder> data { get; set; }

        public class Folder
        {
            public string id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public List<object> children { get; set; }
            public object modificationTime { get; set; }
            public List<string> tags { get; set; }
            public int imageCount { get; set; }
            public int descendantImageCount { get; set; }
            public string pinyin { get; set; }
            public List<string> extendTags { get; set; }
        }
    }

}