using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Collectively.Common.Extensions;
using Collectively.Common.Types;
using Machine.Specifications;

namespace Collectively.Common.Tests.Specs.Extensions
{
    public class PaginationExcentsions_specs
    {
        protected static int CurrentPage;
        protected static int ResultsPerPage;
        protected static int TotalCount;
        protected static int TotalPages;
        protected static IEnumerable<string> Collection;
        protected static HttpResponseHeaders Headers;
    }

    [Subject("PaginationExtensions ToPagedResult")]
    public class When_converting_to_paged_results : PaginationExcentsions_specs
    {
        protected static PagedResult<string> Result;
        
        Establish context = () =>
        {
            ResultsPerPage = 8;
            CurrentPage = 1;
            Collection = new List<string> {
                "a", "b", "c", "d", "e", "f", "g", "h",
                "a", "b", "c", "d", "e", "f", "g", "h",
                "a", "b", "c", "d", "e", "f", "g", "h"};
            TotalCount = Collection.Count();
            TotalPages = TotalCount/ResultsPerPage + 1;

            var msg = new HttpResponseMessage();
            msg.Headers.Add("X-Total-Count", TotalCount.ToString());
            var link = $"<http://url/route?page={CurrentPage + 1}&results={ResultsPerPage}>; rel=\"next\"," +
                       $"<http://url/route?page={TotalPages}&results={ResultsPerPage}>; rel=\"last\"," +
                       $"<http://url/route?page=1&results={ResultsPerPage}>; rel=\"first\",";
            msg.Headers.Add("Link", link);
            Headers = msg.Headers;
        };

        Because of = () => Result = Collection.ToPagedResult(Headers);

        It should_return_paged_result = () => Result.ShouldBeOfExactType<PagedResult<string>>();
        It should_not_be_empty = () => Result.IsNotEmpty.ShouldBeTrue();
        It should_return_correct_page_nuber = () => Result.CurrentPage.ShouldEqual(CurrentPage);
        It should_return_correct_amount_of_results = () => Result.ResultsPerPage.ShouldEqual(ResultsPerPage);
        It should_return_correct_total_count_number = () => Result.TotalResults.ShouldEqual(TotalCount);
        It should_return_correct_total_pages_number = () => Result.TotalPages.ShouldEqual(TotalPages);
    }

    [Subject("PaginationExtensions ToPagedResult")]
    public class When_converting_to_paged_results_and_collection_is_empty : PaginationExcentsions_specs
    {
        protected static PagedResult<string> Result;

        Establish context = () =>
        {
            ResultsPerPage = 8;
            CurrentPage = 1;
            Collection = new List<string>();
            TotalCount = Collection.Count();

            var msg = new HttpResponseMessage();
            Headers = msg.Headers;
        };

        Because of = () => Result = Collection.ToPagedResult(Headers);

        It should_return_paged_result = () => Result.ShouldBeOfExactType<PagedResult<string>>();
        It should_be_empty = () => Result.IsEmpty.ShouldBeTrue();
        It should_return_correct_page_nuber = () => Result.CurrentPage.ShouldEqual(0);
        It should_return_correct_amount_of_results = () => Result.ResultsPerPage.ShouldEqual(0);
        It should_return_correct_total_count_number = () => Result.TotalResults.ShouldEqual(0);
        It should_return_correct_total_pages_number = () => Result.TotalPages.ShouldEqual(0);
    }

    [Subject("PaginationExtensions ToPagedResult")]
    public class When_converting_to_paged_results_and_headers_are_missing : PaginationExcentsions_specs
    {
        protected static PagedResult<string> Result;

        Establish context = () =>
        {
            ResultsPerPage = int.MaxValue;
            CurrentPage = 1;
            Collection = new List<string> {
                "a", "b", "c", "d", "e", "f", "g", "h",
                "a", "b", "c", "d", "e", "f", "g", "h",
                "a", "b", "c", "d", "e", "f", "g", "h"};
            TotalCount = Collection.Count();
            TotalPages = 1;

            var msg = new HttpResponseMessage();
            Headers = msg.Headers;
        };

        Because of = () => Result = Collection.ToPagedResult(Headers);

        It should_return_paged_result = () => Result.ShouldBeOfExactType<PagedResult<string>>();
        It should_not_be_empty = () => Result.IsNotEmpty.ShouldBeTrue();
        It should_return_correct_page_nuber = () => Result.CurrentPage.ShouldEqual(CurrentPage);
        It should_return_correct_amount_of_results = () => Result.ResultsPerPage.ShouldEqual(ResultsPerPage);
        It should_return_correct_total_count_number = () => Result.TotalResults.ShouldEqual(TotalCount);
        It should_return_correct_total_pages_number = () => Result.TotalPages.ShouldEqual(TotalPages);
    }
}