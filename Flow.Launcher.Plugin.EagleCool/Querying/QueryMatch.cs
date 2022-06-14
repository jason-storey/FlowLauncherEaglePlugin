using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.EagleCool
{
#pragma warning disable CS1591
    public class QueryMatch
    {
        public bool IsMatch(Query query) => query.IsAnyOf(Keys);
        
        public string[] Keys { get; set; }
        Func<CancellationToken,Task<List<Result>>> Action { get; set; }

        public static QueryMatch Create(Func<CancellationToken,Task<List<Result>>> action, params string[] keys) =>
            new()
            {
                Keys = keys,
                Action = action
            };

        public Task<List<Result>> Execute(CancellationToken token) => Action.Invoke(token);
    }
}