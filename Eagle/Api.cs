using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Eagle.Models;
namespace Eagle
{
    public class Api : BaseApi
    {
        public async Task<Info?> Status(CancellationToken token) =>
            await GetAsync<Info>("application/info",token);

        public async Task<CreateFolderResponse?> CreateFolder(CreateFolderRequest request,CancellationToken token) =>
            await PostAsync<CreateFolderRequest, CreateFolderResponse>("folder/create", request,token);

        public async Task<RenameFolderResponse?> RenameFolder(RenameFolderRequest request,CancellationToken token) =>
            await PostAsync<RenameFolderRequest, RenameFolderResponse>("folder/rename", request,token);

        public async Task<UpdateFolderResponse?> RenameFolder(UpdateFolderRequest request,CancellationToken token) =>
            await PostAsync<UpdateFolderRequest, UpdateFolderResponse>("folder/rename", request,token);

        public async Task<ListFolderResponse?> GetFolders(CancellationToken cancellationToken) =>
            await GetAsync<ListFolderResponse>("folder/list",cancellationToken);

        public async Task<ListRecentFolderResponse?> GetRecentFolders(CancellationToken token) =>
            await GetAsync<ListRecentFolderResponse>("folder/listRecent",token);

        public async Task<SearchItemsResponse?> Search(SearchItemsRequest request,CancellationToken token) => 
            await GetAsync<SearchItemsResponse>(CreateSearchUri(request),token);

        public async Task<ThumbnailResponse?> GetThumbnailFor(string id,CancellationToken token) => await GetAsync<ThumbnailResponse>(CreateThumbnailUrl(id), token);

        Uri CreateThumbnailUrl(string id)
        {
            UriBuilder builder = new UriBuilder(GetUri("item/thumbnail"));
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["id"] = id.ToString();
            builder.Query = query.ToString();
            return builder.Uri;
        }
        
        Uri CreateSearchUri(SearchItemsRequest request)
        {
            UriBuilder builder = new UriBuilder(GetUri("item/list"));
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["limit"] = request.Limit.ToString();

            string ordering = request.OrderType.ToString();
            if (request.Descending) ordering = "-" + ordering;
            query["orderby"] = ordering;

            if(!string.IsNullOrWhiteSpace(request.Search))
                query["keyword"] = request.Search;

            if(!string.IsNullOrWhiteSpace(request.Extension))
                query["ext"] = request.Extension;

            if (request.tags != null && request.tags.Length > 0)
            {
                var tags = string.Join(',', request.tags);
                query["tags"] = tags;
            }

            if (request.folders != null && request.folders.Length > 0)
            {
                var folders = string.Join(',', request.folders);
                query["folders"] = folders;
            }
            builder.Query = query.ToString();
            return builder.Uri;
        }

    }
}