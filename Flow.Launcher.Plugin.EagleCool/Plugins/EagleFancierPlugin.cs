using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eagle;
using static Flow.Launcher.Plugin.EagleCool.ResultFactory;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.EagleCool
{
    public class EagleFancierPlugin : BasePlugin
    {
        DateTimeOffset _lastLibraryRefresh;
        public override async Task InitAsync(PluginInitContext context)
        {
            await base.InitAsync(context);
            _messenger = new FlowLauncherApiMessenger(Api);
            _eagle = new EagleService();
            await RefreshLibrary(CancellationToken.None);
            _runner = new EagleQueryRunner(_eagle, Api,_messenger,_library);
        }

        async Task RefreshLibrary(CancellationToken token)
        {
            _library = await _eagle.GetLibrarySummary(token);
            _lastLibraryRefresh = DateTimeOffset.UtcNow;
        }

        bool HasntRefreshedInAWhile() => (DateTimeOffset.Now - _lastLibraryRefresh).TotalSeconds > 20;
        
        protected override async Task<List<Result>> GetResults(Query query, CancellationToken token)
        {
            try
            {
                return await _runner.Execute(query, token);
            }
            catch (Exception ex)
            {
                return FromException(ex);
            }
        }

        public override async Task Reload()
        {
            if(_eagle != null)
                _library = await _eagle.GetLibrarySummary(CancellationToken.None);
        }

        public override List<Result> ResultSelected(Result selected)
        {
            if (selected.ContextData is TagGroupContext tg) return HandleTagGroupSelection(tg,_eagle,_messenger);
            if (selected.ContextData is FolderContext f) return HandleFolderSelection(f, _eagle, _messenger);
            return base.ResultSelected(selected);
        }


        #region Plumbing

        LibrarySummary _library;
        EagleService _eagle;
        QueryLookup _queries;
        IMessenger _messenger;
        EagleQueryRunner _runner;

        #endregion
    }
}