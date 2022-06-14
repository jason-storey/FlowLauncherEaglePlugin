using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.EagleCool
{
    public abstract class BasePlugin : IPluginHandler
    {
        public virtual Task InitAsync(PluginInitContext context)
        {
            _context = context;
            return Task.CompletedTask;
        }

        public virtual async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            _cancellationTokenSource?.Cancel();
            var cts = _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                return await GetResults(query,cts.Token);
            }
            catch (Exception ex)
            {
                return new List<Result> { ExceptionResult(ex) };
            }
        }

        protected List<Result> Dummy(params string[] entries) =>
            entries.Select(entry => new Result
            {
                Title = entry
            }).ToList();


        protected List<Result> Single(Result result) => new List<Result> { result }; 

        protected abstract Task<List<Result>> GetResults(Query query,CancellationToken token);

        public virtual void Dispose()
        {
        }

        public virtual List<Result> ResultSelected(Result selected)
        {
            return new List<Result>();
        }

        public virtual Task Reload() => Task.CompletedTask;

        protected IPublicAPI Api => _context.API;


        protected List<Result> Empty() => new List<Result>();
        protected Task<List<Result>> EmptyTask() => Task.FromResult(new List<Result>());

        Result ExceptionResult(Exception ex)
        {
            var result = new Result();

            result.Title = "Something Went Wrong";
            result.AutoCompleteText = "";
            result.SubTitle = ex.Message;
            result.SubTitleToolTip = ex.StackTrace;
            return result;
        }
        CancellationTokenSource _cancellationTokenSource;
        PluginInitContext _context;
    }
}