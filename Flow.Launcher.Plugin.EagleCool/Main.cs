using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
#pragma warning disable CS1591
namespace Flow.Launcher.Plugin.EagleCool
{

    public class EagleCool : IAsyncPlugin,IDisposable,IContextMenu
    {
        IPluginHandler Create() => new EagleFancierPlugin();

        #region Plumbing

        public Task<List<Result>> QueryAsync(Query query, CancellationToken token) => 
            _handler.QueryAsync(query,token);

        public async Task InitAsync(PluginInitContext context)
        {
            _handler = Create();
            await _handler.InitAsync(context);
        }
        
        public List<Result> LoadContextMenus(Result selectedResult) => _handler.ResultSelected(selectedResult);
        
        public void Dispose() => _handler.Dispose();
        
        IPluginHandler _handler;
        
        #endregion
    }
}