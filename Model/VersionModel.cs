namespace ShriGo.Model
{
    public class VersionModel
    {
        public string LatestVersion { get; set; }

        public string MinimumVersion { get; set; }

        public bool ForceUpdate { get; set; }

        public string Message { get; set; }

        public string PlayStoreUrl { get; set; }
    }
}
