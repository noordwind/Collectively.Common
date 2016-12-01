using System;
using Nancy;

namespace Coolector.Common.Services
{
    public interface IExceptionHandler
    {
        void Handle(Exception exception, NancyContext context,
            string name = "Request details", params string[] tags);
    }
}