using System.Collections.Generic;

namespace Eagle
{
    public class TagGroup
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Color { get; set; }
    }

    public class Status
    {
        public TagGroup Tags { get; set; }
        public string LibraryName { get; set; }
        public string LibraryPath { get; set; }
    }

    public class LibrarySummary
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public object Tags { get; set; }
    }
}