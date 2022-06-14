using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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

        public Task<List<File>> GetFilesWithAnyOfTheTags(CancellationToken token, params string[] tags) =>
            SearchAsync(token, A.Search.WithTags(tags));

        public async Task<List<TagGroup>> GetAllTags()
        {
            var info = (await _api.GetLibraryInfo(CancellationToken.None));
            return info.data.tagsGroups.Select(ModelFactory.ToTagGroup).ToList();
        }

        public async Task OpenLibrary(string path) => await (_api.OpenLibrary(path,CancellationToken.None));

        public async Task<List<string>> GetLibraries() => await (_api.GetLibraries(CancellationToken.None));
    }

    public enum FolderColors
    {
        red,orange,green,yellow,aqua,blue,purple,pink
    }
    
    public class TagGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Color { get; set; }
    }
    
    
}