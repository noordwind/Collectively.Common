using Machine.Specifications;
using Moq;
using Collectively.Common.Types;
using It = Machine.Specifications.It;
using System.IO;
using Collectively.Common.Extensions;
using Collectively.Common.Security;
using Collectively.Common.ServiceClients;
using Collectively.Common.ServiceClients.Queries;

namespace Collectively.Common.Tests.Specs.Services
{
    public abstract class ServiceClient_specs
    {
        protected static IServiceClient ServiceClient;
        protected static Mock<IHttpClient> HttpClientMock;
        protected static Mock<IServiceAuthenticatorClient> ServiceAuthenticatorClientMock;
        protected static string ServiceName = "test";
        protected static string Endpoint = "users";

        protected static void Initialize()
        {
            HttpClientMock = new Mock<IHttpClient>();
            ServiceAuthenticatorClientMock = new Mock<IServiceAuthenticatorClient>();
            ServiceClient = new ServiceClient(HttpClientMock.Object, ServiceAuthenticatorClientMock.Object);
        }
    }

    [Subject("ServiceClient GetAsync")]
    public class when_invoking_service_client_get_async : ServiceClient_specs
    {
        static AwaitResult<Maybe<dynamic>> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ServiceClient.GetAsync<dynamic>(ServiceName, Endpoint).Await();

        It should_call_http_client_get_async = () => HttpClientMock.Verify(x => x.GetAsync($"http://{ServiceName}", Endpoint), Times.Once);

        It should_return_empty_result = () => Result.AsTask.Result.HasNoValue.ShouldBeTrue();
    }

    [Subject("ServiceClient GetCollectionAsync")]
    public class when_invoking_service_client_get_collection_async : ServiceClient_specs
    {
        static AwaitResult<Maybe<PagedResult<dynamic>>> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ServiceClient.GetCollectionAsync<dynamic>(ServiceName, Endpoint).Await();

        It should_call_http_client_get_async = () => HttpClientMock.Verify(x => x.GetAsync($"http://{ServiceName}", Endpoint), Times.Once);

        It should_return_empty_result = () => Result.AsTask.Result.HasNoValue.ShouldBeTrue();
    }

    [Subject("ServiceClient GetFilteredCollectionAsync")]
    public class when_invoking_service_client_get_filtered_collection_async : ServiceClient_specs
    {
        protected static Maybe<PagedResult<dynamic>> Result;
        protected static BrowseUsers Query;
        protected static string QueryString;

        private Establish context = () =>
        {
            Initialize();
            Query = new BrowseUsers { Page = 1, Results = 10 };
            QueryString = Endpoint.ToQueryString(Query);
        };

        Because of = () => Result = ServiceClient
            .GetFilteredCollectionAsync<BrowseUsers, dynamic>(Query, ServiceName, Endpoint).Result;

        It should_call_http_client_get_async = () => HttpClientMock.Verify(x => x.GetAsync($"http://{ServiceName}", QueryString), Times.Once);

        It should_return_empty_result = () => Result.HasNoValue.ShouldBeTrue();
    }

    [Subject("ServiceClient GetStreamAsync")]
    public class when_invoking_service_client_get_stream_async : ServiceClient_specs
    {
        static AwaitResult<Maybe<Stream>> Result;

        Establish context = () => Initialize();

        Because of = () => Result = ServiceClient.GetStreamAsync(ServiceName, Endpoint).Await();

        It should_call_http_client_get_async = () => HttpClientMock.Verify(x => x.GetAsync($"http://{ServiceName}", Endpoint), Times.Once);

        It should_return_empty_result = () => Result.AsTask.Result.HasNoValue.ShouldBeTrue();
    }
}