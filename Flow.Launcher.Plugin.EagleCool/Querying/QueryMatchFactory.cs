using System;
using System.Threading;
using System.Threading.Tasks;
using Eagle;
using static Flow.Launcher.Plugin.EagleCool.ResultFactory;
namespace Flow.Launcher.Plugin.EagleCool
{
#pragma warning disable CS1591
    
    public class QueryMatchFactory
    {
        readonly CancellationToken _token;
        readonly LibrarySummary _library;
        readonly EagleService _eagle;
        readonly IPublicAPI _api;

        public QueryMatchFactory(CancellationToken token,LibrarySummary library,EagleService eagle,IPublicAPI api)
        {
            _token = token;
            _library = library;
            _eagle = eagle;
            _api = api;
        }

        public QueryMatch Library => QueryMatch.Create(c =>
                GetLibraryChangeResults(_eagle),
            "lib", "library");

        public QueryMatch Open => QueryMatch.Create(c =>
                GetOpenResults(_eagle,_token, _library,includeFolders: false),
            "open", "go", "execute", "enter");

        public QueryMatch LibraryShortcut =>
            QueryMatch.Create(_=> GetLibraryChangeResults(_eagle), "l" );
       
        public QueryMatch OpenApplicationShortcut =>
            QueryMatch.Create(c=>GetOpenResults(_eagle,_token,_library),"o","f" );

        public QueryMatch TagGroup(Action<TagGroup> onSelected)=>
            QueryMatch.Create(c=>GetTagGroupResults(_eagle,c,_library,onSelected),"t" );
        
        public async Task AddIfMatch(Query query, Results results,QueryMatch q)
        {
            if(q.IsMatch(query))
                results.AddRange(await q.Execute(_token));
        }

        
        
    }
}