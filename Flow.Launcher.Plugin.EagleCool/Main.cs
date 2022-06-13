using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.EagleCool
{
    public class EagleCool : IAsyncPlugin,IDisposable
    {
        private PluginInitContext _context;

        public void Init(PluginInitContext context)
        {
            _context = context;
        }


        public Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            var results = new List<Result>();

            var testResult = new Result();

            testResult.Title = "Testing it works!";
            results.Add(testResult);
            return Task.FromResult(results);
        }

        public Task InitAsync(PluginInitContext context)
        {
            return Task.CompletedTask;
            
        }

        public void Dispose()
        {
        }
    }
}