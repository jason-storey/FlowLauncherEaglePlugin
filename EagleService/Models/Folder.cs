using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eagle.Models;

namespace Eagle
{
    public class Folder
    {
        public bool HasSubFolders => ChildFolders != null && ChildFolders.Count > 0;
        public bool HasItems => NumberOfItems > 0;

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Folder> ChildFolders { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public int NumberOfItems { get; set; }
        public int NumberOfItemsIncludingChildren { get; set; }
        public void OpenInEagle() => Process.Start("explorer.exe",$"eagle://folder/{Id}");
    }

    public class File
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
        public string Extension { get; set; }
        public List<string> Tags { get; set; }
        public List<string> FolderIds { get; set; }
        public bool IsDeleted { get; set; }
        public string Url { get; set; }
        public string Annotation { get; set; }
        public DateTimeOffset? modificationTime { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public DateTimeOffset? lastModified { get; set; }
        public double Duration { get; set; }
    }
}