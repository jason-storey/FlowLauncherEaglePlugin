using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Eagle;
using Eagle.Models;

namespace Flow.Launcher.Plugin.EagleCool
{
    public class EagleCool : IAsyncPlugin,IDisposable
    {
        private PluginInitContext _context;

        private Api _api;

        private CancellationTokenSource _cancellationTokenSource;
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

        private async Task<List<Result>> CreateSearchResults(Query query, CancellationTokenSource cts)
        {

            var req = CreateSearchRequest(query);
            var files = await _api.Search(req, cts.Token);
            if (files == null || files.data.Count <= 0) return NoResultsFound();

            List<Result> results = new List<Result>();

            foreach (var file in files.data)
            {
                results.Add(await ToResult(file,cts.Token));
            }

            return results;
        }

        private Search CreateSearchRequest(Query query)
        {
            var tokens = query.Search;

            if (!tokens.Contains(' '))
                return new Search
                {
                    Keyword = query.Search
                };
            
            var parts = tokens.Split(' ');
            
            var req = new Search();
            
            foreach (var part in parts) 
                ApplyToRequest(ref req, part);
            
            return req;
        }

        private void ApplyToRequest(ref Search req, string part)
        {
            var filter = part.Split(':');
            if (filter.Length < 1)
            {
                return;
            }

            if (filter[0].Equals("t"))
            {
                req.tags = filter[1].Split(',').Select(UpperCaseFirstChar).ToArray();
            }
            
            if (filter[0].Equals("n"))
            {
                req.Name = filter[1];
            }
            
            if (filter[0].Equals("ext"))
            {
                req.Extension = filter[1];
            }

        }

        static public string UpperCaseFirstChar(string text) {
            return Regex.Replace(text, "^[a-z]", m => m.Value.ToUpper());
        }
        
        private static List<Result> CreateFailureResults(Exception ex)
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
                new Result
                {
                    Title = "No Results Found",
                    AutoCompleteText = ""
                }
            };
        }

        public async Task<Result> ToResult(SearchItemsResponse.ItemData item,CancellationToken token)
        {
            var tags = string.Join(',', item.tags);
            var subtitle = item.annotation;

            if (subtitle.Length > 100)
                subtitle = subtitle.Substring(0, 100)+"...";

            string title = item.name;
            //if (tags != null) title += $" [{tags}]";

            if (string.IsNullOrWhiteSpace(subtitle) && tags != null)
                subtitle = tags;
            
            if (string.IsNullOrWhiteSpace(subtitle))
                subtitle = item.url;

            var thumbnail = await _api.GetThumbnailFor(item.id,token);

            string path = thumbnail?.data ?? "";

            path = HttpUtility.UrlDecode(path);
            return new Result
            {
                Title = title,
                SubTitle = subtitle,
                IcoPath = path,
                Action = PerformAction(item,path)
            };
        }

        Func<ActionContext, bool> PerformAction(SearchItemsResponse.ItemData item,string thumbPath)
        {
            string launchUrl = $"eagle://item/{item.id}";
            
            return (ctx) =>
            {
                Process.Start("explorer.exe", launchUrl);
                // _context.API.OpenDirectory(launchUrl);
                return true;
            };
        }

        private async Task<List<Result>> CreateFolderResults(Query query, CancellationTokenSource cts)
        {
            var folders = await _api.GetFolders(cts.Token);
            if (folders == null || folders.data.Count <= 0) return NoResultsFound();

            var keyword = query.Search;

            var results = folders.data.Where(x => MatchesQuery(x, query)).Select(ToResult);
            return results.ToList();
        }

        private Result ToResult(ListFolderResponse.Folder folder, int index) =>
            new Result
            {
                Title = folder.name,
                SubTitle = folder.description,
            };

        private bool MatchesQuery(ListFolderResponse.Folder folder, Query query)
        {
            var searchString = folder.name + folder.description;
            return searchString.ToLower().Contains(query.Search.ToLower());
        }

        private ImageSource Icon()
        {
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.UriSource = new Uri("./icon.png");
            img.EndInit();
            return img;
        }

        public Task InitAsync(PluginInitContext context)
        {
            _context = context;
            try
            {
                _api = new Api();
            }
            catch{}
            return Task.CompletedTask;
        }


        public void Dispose() => _api.Dispose();
    }
}