using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eagle;
using static Flow.Launcher.Plugin.EagleCool.ResultFactory;

namespace Flow.Launcher.Plugin.EagleCool
{
#pragma warning disable CS1591
    public class EagleQueryRunner
    {
        public async Task<List<Result>> Execute(Query query,CancellationToken token)
        {
            if (!_eagle.IsRunning)
                return GetLaunchEagleResults(_eagle);

            Results results = new Results();
        
            var queries = new QueryMatchFactory(token,_library, _eagle, _api);

            _queryLookup = new QueryLookup
            {
                queries.LibraryShortcut,
                queries.OpenApplicationShortcut,
                queries.TagGroup(OnTagGroupSelected)
            };
            
            results = await _queryLookup.Execute(query,token);
            if (results.HasEntries) return results;

            
            await queries.AddIfMatch(query,results,
                queries.Library);
            await queries.AddIfMatch(query,results,
                queries.Open);
            
            results.AddRange(await GetItemAndFolderResults(query,_eagle,_api, token));
            return results;
        }

        #region Plumbing


        void OnTagGroupSelected(TagGroup tagGroup)
        {
            _eagle.OpenToTag(tagGroup.Name);
            Say("Eagle Api Limitation","Cannot open on specific tags with the API, go feature request the eagle.cool website!");
        }

        void Say(string title, string message) => _messenger.Say(title, message);
        
        public EagleQueryRunner(EagleService eagle,IPublicAPI api,IMessenger messenger,LibrarySummary library)
        {
            _eagle = eagle;
            _api = api;
            _messenger = messenger;
            _library = library;
        }
        
        readonly IMessenger _messenger;
        readonly LibrarySummary _library;
        readonly EagleService _eagle;
        readonly IPublicAPI _api;
        QueryLookup _queryLookup;

        #endregion
    }
}