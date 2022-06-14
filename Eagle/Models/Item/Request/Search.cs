namespace Eagle.Models
{
    public class Search
    {
        public int Limit = 200;
        public OrderType OrderType;
        public string Keyword;
        public string Extension;
        public string[] tags;
        public string[] folders;
        public bool Descending = false;
        public string Name;
        
    }
}