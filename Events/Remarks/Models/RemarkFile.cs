namespace Coolector.Common.Events.Remarks.Models
{
    public class RemarkFile
    {
        public string Size { get; }
        public string Url { get; }
        public string Metadata { get; }
        
        protected RemarkFile() {}

        public RemarkFile(string size, string url, string metadata)
        {
            Size = size;
            Url = url;
            Metadata = metadata;
        }
    }
}