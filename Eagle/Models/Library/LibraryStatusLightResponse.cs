using System.Collections.Generic;

public class LibraryStatusLightResponse
{
    public string status { get; set; }
    public Data data { get; set; }
        
    public class Library
    {
        public string path { get; set; }
        public string name { get; set; }
    }
        
    public class Data
    {
       
        public List<TagsGroup> tagsGroups { get; set; }
        public Library library { get; set; }
    }

    
    public class TagsGroup
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<string> tags { get; set; }
        public string color { get; set; }
    }
    
    
}