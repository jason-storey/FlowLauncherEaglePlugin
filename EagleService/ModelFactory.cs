using System;
using System.Collections.Generic;
using System.Text.Json;
using Eagle.Models;
using Eagle.Models.Library;

namespace Eagle
{
    public class ModelFactory
    {
        public static File ToFile(SearchItemsResponse.ItemData data)
        {
            var file = new File
            {
                Id = data.id,
                Name = data.name,
                Size = data.size ?? 0,
                Extension = data.ext,
                FolderIds = data.folders,
                IsDeleted = data.isDeleted,
                Url = data.url,
                Annotation = data.annotation,
                height = data.height,
                width = data.width,
                Duration = data.duration
            };

            if (data.modificationTime is JsonElement js) file.modificationTime = ConvertToDateTimeOffset(js);
            if (data.lastModified is JsonElement el) file.lastModified = ConvertToDateTimeOffset(el);

            List<string> tags = new List<string>();
            foreach (JsonElement tagData in data.tags)
            {
                var val = tagData.GetString();
                if(val != null) tags.Add(val);
            }

            file.Tags = tags;
            
            
            return file;
        }
        
        public static File CreateANewFile(FolderContentsResponse.File child)
        {
            var newFile = new File
            {
                Id = child.id,
                Name = child.name,
                Annotation = child.annotation,
                Extension = child.ext,
                width = child.width,
                height = child.height,
                FolderIds = child.folders,
                Size = child.size,
                Duration = child.duration ?? 0,
                Url = child.url ?? ""
            };

            List<string> tags = new List<string>();
            foreach (JsonElement element in child.tags)
            {
                var val = element.GetString();
                if (val != null) tags.Add(val);
            }

            newFile.Tags = tags;

            if (child.modificationTime is JsonElement js) newFile.modificationTime = ConvertToDateTimeOffset(js);
            if (child.lastModified is JsonElement el) newFile.lastModified = ConvertToDateTimeOffset(el);
            return newFile;
        }
        
        public static Folder ToFolder(ListFolderResponse.Folder data)
        {
            var folder = new Folder
            {
                Id = data.id,
                Name = data.name,
                Description = data.description,
                NumberOfItems = data.imageCount,
                NumberOfItemsIncludingChildren = data.descendantImageCount,
            };

            if (data.modificationTime is JsonElement js) folder.Modified = ConvertToDateTimeOffset(js);

            List<Folder> _children = new List<Folder>();
            foreach (JsonElement childObj in data.children)
            {
                var subFolder = DeserializeFolder(childObj);
                _children.Add(ToFolder(subFolder));
            }
            folder.ChildFolders = _children;
            return folder;
        }
       
        static DateTimeOffset? ConvertToDateTimeOffset(JsonElement element)
        {
            if (element.TryGetInt64(out var unixStamp))
                return DateTimeOffset.FromUnixTimeMilliseconds(unixStamp);
            return null;
        }

        static ListFolderResponse.Folder DeserializeFolder(JsonElement json) => 
            JsonSerializer.Deserialize<ListFolderResponse.Folder>(json.GetRawText());

        public static TagGroup ToTagGroup(LibraryResponse.TagsGroup data)
        {
            return new TagGroup
            {
                Id = data.id,
                Name = data.name,
                Tags = data.tags,
                Color = data.color,
            };
        }
    }
}