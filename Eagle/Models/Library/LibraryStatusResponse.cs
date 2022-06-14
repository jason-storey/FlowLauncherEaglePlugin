using System.Collections.Generic;

namespace Eagle.Models.Library
{
    public class LibraryStatusResponse
    {
        public string status { get; set; }
        public Data data { get; set; }
        
        public class Library
        {
            public string path { get; set; }
            public string name { get; set; }
        }
        
            public class Child
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<Child> children { get; set; }
        public object modificationTime { get; set; }
        public List<string> tags { get; set; }
        public string password { get; set; }
        public string passwordTips { get; set; }
        public string orderBy { get; set; }
        public bool? sortIncrease { get; set; }
        public List<Condition> conditions { get; set; }
        public int imageCount { get; set; }
        public int size { get; set; }
        public string vstype { get; set; }
        public Styles styles { get; set; }
        public string parent { get; set; }
        public string pinyin { get; set; }
        public bool isExpand { get; set; }
        public int index { get; set; }
        public bool isVisible { get; set; }
        public string icon { get; set; }
    }

    public class Condition
    {
        public List<Rule> rules { get; set; }
        public string match { get; set; }

        public string HashKey { get; set; }
    }

    public class Data
    {
        public List<Folder> folders { get; set; }
        public List<SmartFolder> smartFolders { get; set; }
        public List<object> quickAccess { get; set; }
        public List<TagsGroup> tagsGroups { get; set; }
        public long modificationTime { get; set; }
        public string applicationVersion { get; set; }
        public Library library { get; set; }
    }

    public class Rule
    {
        public string property { get; set; }
        public string method { get; set; }
        public List<object> value { get; set; }

        public string HashKey { get; set; }
    }

    public class SmartFolder
    {
        public string id { get; set; }
        public string icon { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public object modificationTime { get; set; }
        public List<Condition> conditions { get; set; }
        public List<Child> children { get; set; }
    }

    public class Styles
    {
        public int depth { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
    }

    public class TagsGroup
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> tags { get; set; }
        public string color { get; set; }
    }
    
    public class Folder
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<Child> children { get; set; }
        public object modificationTime { get; set; }
        public List<object> tags { get; set; }
        public string icon { get; set; }
        public string password { get; set; }
        public string passwordTips { get; set; }
        public string orderBy { get; set; }
        public bool? sortIncrease { get; set; }
    }
    
    }
}