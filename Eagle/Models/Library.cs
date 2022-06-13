using System.Collections.Generic;

namespace Eagle.Models.Library
{
    public class Condition
    {
        public string match { get; set; }
        public List<Rule> rules { get; set; }

        public string HashKey { get; set; }
    }

    public class Child
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<object> children { get; set; }
        public object modificationTime { get; set; }
        public List<string> tags { get; set; }
        public string iconColor { get; set; }
        public string password { get; set; }
        public string passwordTips { get; set; }
        public string coverId { get; set; }
        public string orderBy { get; set; }
        public bool sortIncrease { get; set; }
        public string icon { get; set; }
    }

    public class Data
    {
        public List<Folder> folders { get; set; }
        public List<SmartFolder> smartFolders { get; set; }
        public List<QuickAccess> quickAccess { get; set; }
        public List<TagsGroup> tagsGroups { get; set; }
        public long modificationTime { get; set; }
        public string applicationVersion { get; set; }
    }

    public class Folder
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<Child> children { get; set; }
        public object modificationTime { get; set; }
        public List<string> tags { get; set; }
        public string iconColor { get; set; }
        public string password { get; set; }
        public string passwordTips { get; set; }
        public string coverId { get; set; }
        public string orderBy { get; set; }
        public bool sortIncrease { get; set; }
        public string icon { get; set; }
    }

    public class QuickAccess
    {
        public string type { get; set; }
        public string id { get; set; }
    }

    public class Root
    {
        public string status { get; set; }
        public Data data { get; set; }
    }

    public class Rule
    {
        public string method { get; set; }
        public string property { get; set; }
        public object value { get; set; }

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
        public string orderBy { get; set; }
        public bool sortIncrease { get; set; }
    }

    public class TagsGroup
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> tags { get; set; }
        public string color { get; set; }
    }
}