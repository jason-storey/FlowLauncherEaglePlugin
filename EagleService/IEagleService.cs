using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Eagle.Models;

namespace Eagle
{
    public interface IEagleService
    {
        Task<IEnumerable<Folder>> GetFoldersAsync();
        Task<List<File>> SearchAsync(CancellationToken token, string search);
        Task<List<File>> SearchAsync(CancellationToken token,Search search);
        Task SetFolderColor(string id, FolderColors color);
        Task SetFolderName(string id, string name,string description ="");
        Task<List<File>> GetFilesInFolderAsync(string id);
        Task<List<File>> GetFilesInFolderAsync(string id,CancellationToken token);
        Task<List<File>> GetFilesInFolderRecursivelyAsync(string id, CancellationToken token);
        Task<List<File>> GetFilesWithAnyOfTheTags(CancellationToken token, params string[] tags);
        Task<List<TagGroup>> GetAllTags();
        Task OpenLibrary(string path);
        Task<List<Library>> GetLibraries();

        Library GetCurrentLibrary();
        
        Task<string> GetThumbnailPathFor(string fileId);
        
        
        
    }
}