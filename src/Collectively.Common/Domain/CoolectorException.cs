using System;

namespace Collectively.Common.Domain
{
    public abstract class CollectivelyException : Exception
    {
        public string Code { get; }

        protected CollectivelyException()
        {
        }

        protected CollectivelyException(string code)
        {
            Code = code;
        }

        protected CollectivelyException(string message, params object[] args) : this(string.Empty, message, args)
        {
        }

        protected CollectivelyException(string code, string message, params object[] args) : this(null, code, message, args)
        {
        }

        protected CollectivelyException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        protected CollectivelyException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}