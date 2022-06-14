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
}