namespace HrNewsPortal.Models
{
    public class HrNewsClientSettings
    {
        public string WebApi { get; set; }

        public string WebApiVersion { get; set; }

        public int MaxTakeItem { get; set; }

        public string HrNewsApiUrl {
            get { return $"{WebApi}/v{WebApiVersion}/"; }
        }
    }
}
