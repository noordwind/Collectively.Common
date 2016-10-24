namespace Coolector.Common.Events.Remarks.Models
{
    public class RemarkFile
    {
        public string Name { get; }
        public string Size { get; }
        public string Url { get; }
        public string Metadata { get; }
        
        protected RemarkFile() {}

        public RemarkFile(string name, string size, string url, string metadata)
        {
            Name = name;
            Size = size;
            Url = url;
            Metadata = metadata;
        }
    }
}