using System;

namespace Eagle.Models
{
    public class SearchBuilder
    {
        int _limit = 200;
        OrderType _orderType;
        string _keyword;
        string _extension;
        string[] _tags;
        string[] _folders;
        bool _descending = false;
        string _name;


        public SearchBuilder WithTags(params string[] tags)
        {
            _tags = tags;
            return this;
        }

        public SearchBuilder WithLimit(int limit)
        {
            _limit = Math.Min(limit, 200);
            return this;
        }

        public SearchBuilder WithOrderType(OrderType type)
        {
            _orderType = type;
            return this;
        }

        public SearchBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        
        public SearchBuilder WithKeyword(string keyword)
        {
            _keyword = keyword;
            return this;
        }

        public SearchBuilder WithFolders(params string[] folders)
        {
            _folders = folders;
            return this;
        }

        public SearchBuilder WithExtension(string extension)
        {
            _extension = extension;
            return this;
        }

        public SearchBuilder Descending()
        {
            _descending = true;
            return this;
        }

        public SearchBuilder Ascending()
        {
            _descending = false;
            return this;
        }
        
        
        
        

        public static implicit operator Search(SearchBuilder builder) => builder.Build();

        public Search Build() => new()
        {
            Limit = _limit,
            OrderType = _orderType,
            Keyword = _keyword,
            Extension = _extension,
            tags = _tags,
            folders = _folders,
            Descending = _descending,
            Name = _name
        };


    }
}