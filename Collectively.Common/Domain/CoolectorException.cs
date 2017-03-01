using System;

namespace Collectively.Common.Domain
{
    public abstract class CoolectorException : Exception
    {
        public string Code { get; }

        protected CoolectorException()
        {
        }

        protected CoolectorException(string code)
        {
            Code = code;
        }

        protected CoolectorException(string message, params object[] args) : this(string.Empty, message, args)
        {
        }

        protected CoolectorException(string code, string message, params object[] args) : this(null, code, message, args)
        {
        }

        protected CoolectorException(Exception innerException, string message, params object[] args)
            : this(innerException, string.Empty, message, args)
        {
        }

        protected CoolectorException(Exception innerException, string code, string message, params object[] args)
            : base(string.Format(message, args), innerException)
        {
            Code = code;
        }
    }
}