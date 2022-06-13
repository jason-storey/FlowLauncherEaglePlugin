namespace Eagle.Models
{

    public class Info
    {
        public string status { get; set; }
        public Data data { get; set; }

        public class Data
        {
            public string version { get; set; }
            public object prereleaseVersion { get; set; }
            public string buildVersion { get; set; }
            public string execPath { get; set; }
            public string platform { get; set; }
        }
    }

}