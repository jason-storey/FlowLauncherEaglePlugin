using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.EagleCool
{
#pragma warning disable CS1591
    public class QueryLookup : IEnumerable<QueryMatch>
    {
        public async Task<List<Result>> Execute(Query query,CancellationToken token)
        {
            var matching = _matches.FirstOrDefault(x => x.IsMatch(query));
            if (matching != null) return await matching.Execute(token);

            return await Task.FromResult(new List<Result>());
        }

        #region Plumbing

        public void Add(QueryMatch match) => _matches.Add(match);
        public IEnumerator<QueryMatch> GetEnumerator() => _matches.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public QueryLookup() => _matches = new List<QueryMatch>();
        readonly List<QueryMatch> _matches;
        
        

        #endregion
    }
}