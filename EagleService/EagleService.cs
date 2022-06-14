using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Eagle.Models;
using Eagle.Models.Library;

namespace Eagle
{
    public class EagleService : IDisposable
    {
        
        public Task<IEnumerable<Folder>> GetFoldersAsync() => GetFoldersAsync(CancellationToken.None);
         async Task<IEnumerable<Folder>> GetFoldersAsync(CancellationToken token)
        {
            var response = await _api.GetFolders(token);
            return response is { status: "error" } ? Enumerable.Empty<Folder>() : response.data.Select(ModelFactory.ToFolder);
        }

         public Task<List<File>> SearchAsync(CancellationToken token, string search) =>
             SearchAsync(token, A.Search.WithKeyword(search));
         
        public async Task<List<File>> SearchAsync(CancellationToken token,Search search)
        {
            var response = await _api.Search(search,token);
            return response is { status: "error" } ? new List<File>() : response.data.Select(ModelFactory.ToFile).ToList();
        }

        #region Plumbing

        public void Dispose() => _api.Dispose();
        
        
        readonly Api _api;
        public EagleService() => _api = new Api();
        
        #endregion

        public bool IsRunning => Process.GetProcessesByName("Eagle").Length > 0;
        
        public async Task SetFolderColor(string id, FolderColors color)=>
            await _api.UpdateFolder(id, new UpdateFolderRequest
            {
                newColor = color.ToString()
            },CancellationToken.None);
        
        public async Task SetFolderName(string id, string name,string description ="")=>
            await _api.UpdateFolder(id, new UpdateFolderRequest
            {
                newName = name,
                newDescription = description
            },CancellationToken.None);

        public Task<List<File>> GetFilesInFolderAsync(string id) => 
            SearchAsync(CancellationToken.None, A.Search.WithFolders(id));
        
        
        public Task<List<File>> GetFilesInFolderAsync(string id,CancellationToken token) => 
            SearchAsync(token,A.Search.WithFolders(id));

        
        public async Task<List<File>> GetFilesInFolderRecursivelyAsync(string id, CancellationToken token)
        {
            var allFolders = (await GetFoldersAsync()).ToList();
            
            List<File> files = (await GetFilesInFolderAsync(id, token));

            var matching = allFolders.FirstOrDefault(x => x.Id == id);
            foreach (var childFolders in matching.ChildFolders)
            {
                files.AddRange((await GetFilesInFolderRecursivelyAsync(allFolders, childFolders.Id, token)));
            }
            return files;
        }
        
        async Task<List<File>> GetFilesInFolderRecursivelyAsync(List<Folder> allFolders,string id, CancellationToken token)
        {
            List<File> files = (await GetFilesInFolderAsync(id, token));

            var matching = allFolders.FirstOrDefault(x => x.Id == id);
            if (matching == null || matching.ChildFolders.Count <= 0) return files;
            
            foreach (var childFolders in matching.ChildFolders)
                files.AddRange((await GetFilesInFolderRecursivelyAsync(allFolders, childFolders.Id, token)));
            
            return files;
        }


        public async Task<LibrarySummary> GetLibrarySummary(CancellationToken token)
        {
            var response = await _api.GetLibrarySummary(token);
            return ModelFactory.ToLibrarySummary(response);
        }
        
        
        public string GetEagleExecutionPath()
        {
            var response = _api.Status(CancellationToken.None);
            return response.Result.data.execPath;
        }
        
        public Task<List<File>> GetFilesWithAnyOfTheTags(CancellationToken token, params string[] tags) =>
            SearchAsync(token, A.Search.WithTags(tags));

        public async Task<List<TagGroup>> GetAllTags(CancellationToken token)
        {
            LibraryStatusResponse? info = (await _api.GetLibraryInfo(token));
            return info.data.tagsGroups.Select(ModelFactory.ToTagGroup).ToList();
        }

        public async Task OpenLibrary(string path) => await (_api.OpenLibrary(path,CancellationToken.None));
        
       public async Task<List<Library>> GetLibraries() => 
           (await _api.GetLibraries(CancellationToken.None))
            .Select(ToLibrary).ToList();

       Library ToLibrary(string x)
       {
           return new Library
           {
               Name = SanitizeLibraryPathToName(x),
               Path = x
           };
       }

       public Library GetCurrentLibrary()
       {
           var info = _api.GetLibraryInfo(CancellationToken.None);
           return ToLibrary(info.Result.data.library.path);
       }

       string SanitizeLibraryPathToName(string path) => path.Split('\\').Last().Replace(".library", "");

        public async Task<string> GetThumbnailPathFor(string fileId)
        {
            var result = (await _api.GetThumbnailFor(fileId, CancellationToken.None)).data;
            return HttpUtility.UrlDecode(result);
        }

        public async Task<bool> GotoLibrary(string keyword)
        {
            var libraries = await GetLibraries();
            var found = libraries.FirstOrDefault(x => x.Name.Contains(keyword.ToLower().Trim()));
            if (found == null) return false;
            await OpenLibrary(found.Path);
            return true;
        }

        public async Task<List<Folder>> GetRootFolders(CancellationToken token)
        {
            var folders = await _api.GetFolders(token);
            return folders.data.Select(ModelFactory.ToFolder).ToList();
        }


        public Task<List<Folder>> SearchFoldersAsync(string search, CancellationToken token) => GetRootFolders(token);

        public bool Launch()
        {
            Process.Start(@"C:\Program Files (x86)\Eagle\Eagle.exe");
            return true;
        }

        public void OpenToTag(string tag) => Process.Start("explorer.exe",$"eagle://tag/{tag}");
    }
}