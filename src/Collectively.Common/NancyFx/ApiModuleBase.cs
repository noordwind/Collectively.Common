using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Collectively.Common.Queries;
using Collectively.Common.Types;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace Collectively.Common.NancyFx
{
    public abstract class ApiModuleBase : NancyModule
    {
        protected readonly IMapper Mapper;

        protected ApiModuleBase(string modulePath = "") 
            : base(modulePath)
        { }

        protected ApiModuleBase(IMapper mapper, string modulePath = "")
            : base(modulePath)
        {
            Mapper = mapper;
        }

        protected FetchRequestHandler<TQuery, TResult> Fetch<TQuery, TResult>(Func<TQuery, Task<Maybe<TResult>>> fetch)
            where TQuery : IQuery, new() where TResult : class
        {
            var query = BindRequest<TQuery>();

            return new FetchRequestHandler<TQuery, TResult>(query, fetch, Negotiate, Request.Url, Mapper);
        }

        protected FetchRequestHandler<TQuery, TResult> FetchCollection<TQuery, TResult>(
            Func<TQuery, Task<Maybe<PagedResult<TResult>>>> fetch)
            where TQuery : IPagedQuery, new() where TResult : class
        {
            var query = BindRequest<TQuery>();

            return new FetchRequestHandler<TQuery, TResult>(query, fetch, Negotiate, Request.Url, Mapper);
        }

        protected T BindRequest<T>() where T : new()
        => Request.Body.Length == 0 && Request.Query == null
            ? new T()
            : this.Bind<T>();

        protected Response FromStream(Maybe<Stream> stream, string fileName, string contentType)
        {
            if (stream.HasNoValue)
                return HttpStatusCode.NotFound;

            var response = new StreamResponse(() => stream.Value, contentType);

            return response.AsAttachment(fileName);
        }
    }
}