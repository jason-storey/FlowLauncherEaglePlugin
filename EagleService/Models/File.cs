using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Eagle
{
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

        public void OpenInEagle() => Process.Start("explorer.exe",$"eagle://item/{Id}");
    }
}