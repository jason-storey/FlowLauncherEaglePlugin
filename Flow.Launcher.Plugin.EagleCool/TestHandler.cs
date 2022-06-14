using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.EagleCool
{
    public class TestHandler : BaseHandler
    {
        protected override void Init()
        {
            
        }

        protected override Task<List<Result>> GetResults(Query query, CancellationToken token)
        {
            return Task.FromResult(GetFilterTypes(query));
        }

        public override List<Result> ResultSelected(Result selected)
        {
            if (selected.ContextData is SearchContextData s) return HandleSearching(s);
            if (selected.ContextData is FilterContextData d) return HandleFiltering(d);
            return Empty();
        }

        List<Result> HandleSearching(SearchContextData ctx)
        {
            return Dummy("Seems you are doing a search for: " + ctx.Query.Search + " / " + ctx.Query.RawQuery);
        }

        List<Result> HandleFiltering(FilterContextData filter)
        {
            var result = new Result
            {
                Title = "You chose: " + filter.filterType + ", to filter query: " + filter.query.Search + "/ " + filter.query.FirstSearch,
                ContextData = new SearchContextData{Query = filter.query}
            };
            return Single(result);
        }


        List<Result> GetFilterTypes(Query query)
        {
            return new List<Result>
            {
                CreateFilterType(query,"All"),
                CreateFilterType(query,"Tags"),
                CreateFilterType(query,"Exact Name"),
                CreateFilterType(query,"Id")
            };
        }

        Result CreateFilterType(Query query,string category) =>
            new()
            {
                Title = category,
                SubTitle = $"Search by {category}",
                AutoCompleteText = "",
                ContextData = new FilterContextData
                {
                    query = query,
                    filterType = category
                }
            };
    }

    public class FilterContextData
    {
        public Query query;
        public string filterType;
    }

    public class SearchContextData
    {
        public Query Query;
    }
    
}