using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Eagle.Models;
using Eagle.Models.Library;

namespace Eagle
{
    public class Api : BaseApi
    {
        public async Task<Info?> Status(CancellationToken token) =>
            await GetAsync<Info>("application/info",token);

        public async Task<CreateFolderResponse?> CreateFolder(CreateFolderRequest request,CancellationToken token) =>
            await PostAsync<CreateFolderRequest, CreateFolderResponse>("folder/create", request,token);

        public async Task UpdateFolder(string id, UpdateFolderRequest request, CancellationToken token)
        {
            request.folderId = id;
            await PostAsync<UpdateFolderRequest, UpdateFolderResponse>("folder/update", request, token);
        }

        public Task<LibraryStatusResponse?> GetLibraryInfo(CancellationToken token) => GetAsync<LibraryStatusResponse>("library/info",token);

        public Task<LibraryStatusLightResponse?> GetLibrarySummary(CancellationToken token) =>
            GetAsync<LibraryStatusLightResponse>("library/info", token);

        public async Task<RenameFolderResponse?> RenameFolder(RenameFolderRequest request,CancellationToken token) =>
            await PostAsync<RenameFolderRequest, RenameFolderResponse>("folder/rename", request,token);

        public async Task<UpdateFolderResponse?> RenameFolder(UpdateFolderRequest request,CancellationToken token) =>
            await PostAsync<UpdateFolderRequest, UpdateFolderResponse>("folder/rename", request,token);

        public async Task<ListFolderResponse?> GetFolders(CancellationToken cancellationToken) =>
            await GetAsync<ListFolderResponse>("folder/list",cancellationToken);

        public async Task<ListRecentFolderResponse?> GetRecentFolders(CancellationToken token) =>
            await GetAsync<ListRecentFolderResponse>("folder/listRecent",token);

        public async Task<SearchItemsResponse?> Search(Search request,CancellationToken token) => 
            await GetAsync<SearchItemsResponse>(CreateSearchUri(request),token);

        public async Task<StatusResponse?> GetThumbnailFor(string id,CancellationToken token) => await GetAsync<StatusResponse>(CreateThumbnailUrl(id), token);

        Uri CreateThumbnailUrl(string id)
        {
            UriBuilder builder = new UriBuilder(GetUri("item/thumbnail"));
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["id"] = id.ToString();
            builder.Query = query.ToString();
            return builder.Uri;
        }


        public async Task<FolderContentsResponse?> GetFilesInFolder(string id, CancellationToken token)
        {
            var uri = CreateUri("item/list", q =>
            {
                q["folders"] = id;
            });
            return await GetAsync<FolderContentsResponse>(uri, token);
        }

        Uri CreateUri(string basePath,Action<NameValueCollection> modifyQuery)
        {
            UriBuilder builder = new UriBuilder(GetUri(basePath));
            var query = HttpUtility.ParseQueryString(builder.Query);
            modifyQuery.Invoke(query);
            builder.Query = query.ToString();
            return builder.Uri;
        }


        Uri CreateSearchUri(Search request)
        {
            UriBuilder builder = new UriBuilder(GetUri("item/list"));
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["limit"] = request.Limit.ToString();

            string ordering = request.OrderType.ToString();
            if (request.Descending) ordering = "-" + ordering;
            query["orderby"] = ordering;

            if(!string.IsNullOrWhiteSpace(request.Keyword))
                query["keyword"] = request.Keyword;

            if(!string.IsNullOrWhiteSpace(request.Extension))
                query["ext"] = request.Extension;

            if (request.tags != null && request.tags.Length > 0)
            {
                var tags = string.Join(',', request.tags);
                query["tags"] = tags;
            }

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                query["name"] = request.Name;
            }
            
            if (request.folders != null && request.folders.Length > 0)
            {
                var folders = string.Join(',', request.folders);
                query["folders"] = folders;
            }
            builder.Query = query.ToString();
            return builder.Uri;
        }


        public Task<StatusResponse?> OpenLibrary(string path, CancellationToken token) => 
            PostAsync<LibrarySwitchRequest, StatusResponse>("library/switch", new LibrarySwitchRequest
            {
                libraryPath = path.Replace('\\','/')
            },token);

        public async Task<List<string>> GetLibraries(CancellationToken token) => 
           (await GetAsync<LibraryListResponse>("library/history",token))?.data;
        
    }
}