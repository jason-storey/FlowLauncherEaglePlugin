namespace Eagle.Models
{
    public class SearchItemsRequest
    {
        public int Limit = 200;
        public OrderType OrderType;
        public string Search;
        public string Extension;
        public string[] tags;
        public string[] folders;
        public bool Descending = false;
        public string Name;
    }

    public enum OrderType
    {
        NAME,CREATEDATE,FILESIZE,RESOLUTION
    }
}