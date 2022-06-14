using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eagle;
#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.EagleCool
{
    public class EagleFancierPlugin : BaseHandler
    {
        EagleService _eagle;
        protected override void Init() => _eagle = new EagleService();

        protected override async Task<List<Result>> GetResults(Query query, CancellationToken token)
        {
            if (query.Is("l"))
                return (await GetLibraryChangeResults());
            if(query.Is("o"))
                return  await GetExecutionResults();
            
            var results = new List<Result>();
            if (query.IsAnyOf("lib","library"))
                results.AddRange(await GetLibraryChangeResults());
            if (query.IsAnyOf("open", "go", "execute", "enter"))
                results.AddRange(await GetExecutionResults());
            results.AddRange(await GetQueryResults(query, token));
            return results;
        }
        
        
        async Task<List<Result>> GetQueryResults(Query query, CancellationToken cts)
        {
            var files = await _eagle.SearchAsync(cts, query.Search);
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
                TitleHighlightData = Api.FuzzySearch(keyword,file.Name).MatchData,
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
        
        

        Task<List<Result>> GetExecutionResults()
        {
            List<Result> results = new List<Result>();

            var info = _eagle.GetCurrentLibrary();
            var executionPath = _eagle.GetEagleExecutionPath();
            
            results.Add(new Result
            {
                Title = "Open Eagle",
                SubTitle = $"Open Library '{info.Name}'",
                Action = x =>
                {
                    Process.Start(executionPath);
                    return true;
                }
            });
            
            return Task.FromResult<List<Result>>(results);
        }


        async Task<List<Result>> GetLibraryChangeResults()
        {
            var libraries = await _eagle.GetLibraries();
            
            var results = new List<Result>();
            foreach (var library in libraries)
            {
                var result = new Result { Title = library.Name, SubTitle = library.Path,ContextData = library.Path };
                result.Action = OpenTheLibrary(result);
                results.Add(result);
            }

            return results;
        }

        Func<ActionContext,bool> OpenTheLibrary(Result result) => a =>
            {
               _eagle.OpenLibrary(result.SubTitle);
                return true;
            };
        
    }

    public static class QueryExtensions
    {
        public static bool Is(this Query q, string val) => q.Search.Trim().Equals(val.Trim(), StringComparison.OrdinalIgnoreCase);
        public static bool IsAnyOf(this Query q, params string[] val) => val.Any(q.Is);
    }
}