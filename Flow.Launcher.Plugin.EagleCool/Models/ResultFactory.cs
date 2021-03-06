using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Eagle;
using static Flow.Launcher.Plugin.EagleCool.ResultModelFactory;

namespace Flow.Launcher.Plugin.EagleCool
{
#pragma warning disable CS1591
    public static class ResultFactory
    {
        public static List<Result> GetLaunchEagleResults(EagleService eagle) =>
            new Results
            {
                { "Eagle Is Not Running", "Would you like to launch it?", "Images/eagle.png", () => eagle.Launch() }
            };
        
        public static async Task<List<Result>> GetLibraryChangeResults(EagleService eagle)
        {
            var libraries = await eagle.GetLibraries();

            return Results.Create(libraries, (l,r) =>
            {
                r.Title = l.Name;
                r.SubTitle = l.Path;
                r.ContextData = l.Path;
                r.IcoPath = "Images/library.png";
                r.Action = x =>
                {
                    eagle.OpenLibrary(r.SubTitle);
                    return true;
                };
            });
        }
        
        public static async Task<List<Result>> GetTagGroupResults(EagleService eagle,CancellationToken token,LibrarySummary library,Action<TagGroup> action)
        {
            library = await eagle.GetLibrarySummary(token);
            var tagGroups = library.Tags.OrderByDescending(x => x.Tags.Count).ToList();
    
            return Results.Create(tagGroups, (t,r) =>
            {
                r.Title = t.Name;
                r.SubTitle = t.Tags.Count > 0 ? $"{t.Tags.Count} Items" : "[None]";
                r.IcoPath = "Images/taggroup.png";
                r.ContextData = new TagGroupContext
                {
                    Group = t
                };
                r.Action =x =>
                {
                    action?.Invoke(t);
                    return true;
                };
            });
        }
        
        public static async Task<List<Result>> GetItemAndFolderResults(Query query,EagleService eagle,IPublicAPI api, CancellationToken cts)
        {
            var results = new Results();
            results.AddRange(await GetItemQueryResults(query,eagle,api, cts));
            results.AddRange(await GetFolderQueryResults(eagle,query,cts));
            return results;
        }

       public static async Task<List<Result>> GetFolderQueryResults(EagleService eagle,Query query, CancellationToken cts)
        {
            var folders = await eagle.SearchFoldersAsync(query.Search,cts);
            var r = new Results();
            r.AddRange( folders.Select(ToResult));
            foreach (var singleFolders in folders.Where(x=>x != null))
            {
                if(singleFolders.ChildFolders != null && singleFolders.ChildFolders.Count > 0)
                    r.AddRange(singleFolders.ChildFolders.Select(ToResult));
            }

            return r;
        }

       public static List<Result> HandleTagGroupSelection(TagGroupContext tg,EagleService eagle,IMessenger messenger) =>
           Results.Create(tg.Group.Tags, (t, r) =>
           {
               r.Title = t;
               r.IcoPath = "Images/tag.png";
               r.Action = x =>
               {
                   eagle.OpenToTag(t);
                   messenger.Say("Eagle Api Limitation","Cannot open on specific tags with the API, go feature request the eagle.cool website!");
                   return true;
               };
           });
     
       
       
       public static List<Result> HandleFolderSelection(FolderContext f, EagleService eagle, IMessenger messenger)
       {
           Results results = new Results();
           if (!f.Folder.HasSubFolders)
           {
               results.Add(ToResult(f.Folder));
               return results;
           }

           foreach (var child in f.Folder.ChildFolders) 
               results.Add(ToResult(child));
           
           return results;
       }
       
        
       
       static async Task<List<Result>> GetItemQueryResults(Query query,EagleService eagle,IPublicAPI api, CancellationToken cts)
       {
           var files = await eagle.SearchAsync(cts, query.Search);
           var r = new List<Result>();
           foreach (var file in files)
               r.Add(await CreateResultFromFile(file,eagle,api, query.Search));
           return r;
       }
        
       static async Task<Result> CreateResultFromFile(File file,EagleService eagle,IPublicAPI api,string keyword)
       {
           var annotation = SanitizeAnnotation(file);
           return new()
           {
               Title = file.Name,
               SubTitle = annotation,
               SubTitleToolTip = string.Join(" , ", file.Tags),
               TitleToolTip = string.Join(" , ", file.Tags),
               TitleHighlightData = api.FuzzySearch(keyword,file.Name).MatchData,
               IcoPath = await eagle.GetThumbnailPathFor(file.Id),
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
       
       public static async Task<List<Result>> GetOpenResults(EagleService eagle,CancellationToken token,LibrarySummary library, bool includeFolders = true)
       {
           List<Result> results = new List<Result>();

           results.Add(new Result
           {
               Title = "Open Eagle",
               SubTitle = $"Open Library '{library.Name}'",
               IcoPath = "Images/eagle.png",
               Action = x =>
               {
                   Process.Start(library.Path);
                   return true;
               }
           });

           if (!includeFolders) return results;
            
           var topFolders = await eagle.GetRootFolders(token);
           results.AddRange(topFolders.Select(ToResult));

           return results;
       }


       public static List<Result> FromException(Exception ex,string message = "")
       {
           Exception baseEx = ex;
           while (baseEx.InnerException != null)
           {
               baseEx = ex;
           }
            
           var r = new List<Result>
           {
               new()
               {
                   Title = $"Failed: {ex.Message}",
                   SubTitle = baseEx.Message,
                   TitleToolTip = baseEx.StackTrace,
                   SubTitleToolTip = ex.StackTrace,
                   AutoCompleteText = "",
                   IcoPath = "Images/exception.png"
               }
           };

           if (!string.IsNullOrWhiteSpace(message))
               r[0].Title = message;
           return r;
       }
    }
}