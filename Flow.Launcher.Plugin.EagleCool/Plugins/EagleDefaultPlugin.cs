using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eagle;

namespace Flow.Launcher.Plugin.EagleCool
{
    public class EagleDefaultPlugin : IPluginHandler
    {
        public void Enable()
        {
            
        }

        public void Disable()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<List<Result>> QueryAsync(Query query, CancellationToken token)
        {
            _cancellationTokenSource?.Cancel();
            var cts = _cancellationTokenSource = new CancellationTokenSource();
            try
            {
                if (string.IsNullOrWhiteSpace(query.Search)) return NoResultsFound();
                return await CreateSearchResults(query, cts);
            }
            catch (Exception ex)
            {
                return CreateFailureResults(ex);
            }
        }

        async Task<List<Result>> CreateSearchResults(Query query, CancellationTokenSource cts)
        {
           var files = await _eagle.SearchAsync(cts.Token, query.Search);
           var results = new List<Result>();
           foreach (var file in files) 
               results.Add(await CreateResultFromFile(file,query.Search));
           return results;
        }

        async Task<Result> CreateResultFromFile(File file,string keyword)
        {
            var annotation = SanitizeAnnotation(file);
            return new()
            {
                Title = file.Name,
                SubTitle = annotation,
                SubTitleToolTip = string.Join(" , ", file.Tags),
                TitleToolTip = string.Join(" , ", file.Tags),
                TitleHighlightData = _context.API.FuzzySearch(keyword,file.Name).MatchData,
                IcoPath = await _eagle.GetThumbnailPathFor(file.Id),
                Action = c =>
                {
                    file.OpenInEagle();
                    return true;
                }
            };
        }

        static string SanitizeAnnotation(File file)
        {
            var trimmed = file.Annotation.Replace(Environment.NewLine, " ").Replace("\r","").Replace("\n","").Trim().Trim('\r','\n');
            if( trimmed.Length > 100) trimmed = trimmed.Substring(0, 100) + "...";
            return trimmed;
        }


        #region Plumbing

        PluginInitContext _context;
        EagleService _eagle;
        CancellationTokenSource _cancellationTokenSource;
        
        static List<Result> CreateFailureResults(Exception ex)
        {
            Exception baseEx = ex;
            while (baseEx.InnerException != null)
            {
                baseEx = ex;
            }
            
            return new List<Result>
            {
                new()
                {
                    Title = $"Failed: {ex.Message}",
                    SubTitle = baseEx.Message,
                    AutoCompleteText = ""
                }
            };
        }

        List<Result> NoResultsFound()
        {
            return new List<Result>
            {
                new()
                {
                    Title = "No Results Found",
                    AutoCompleteText = ""
                }
            };
        }

    
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task InitAsync(PluginInitContext context)
        {
            _context = context;
            _eagle = new EagleService();
            return Task.CompletedTask;
        }
        
        

        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        public void Dispose() => _eagle.Dispose();

        public List<Result> ResultSelected(Result selected)
        {
            return new List<Result>();
        }

        public Task Reload()
        {
            return Task.CompletedTask;
        }
    }
}