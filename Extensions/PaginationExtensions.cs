using System;
using System.Collections.Generic;
using Coolector.Common.Types;
using System.Linq;
using System.Net.Http.Headers;
using Coolector.Common.Queries;
using Nancy.Helpers;

namespace Coolector.Common.Extensions
{
    public static class PaginationExtensions
    {
        public static PagedResult<T> PaginateWithoutLimit<T>(this IEnumerable<T> values)
            => values.Paginate(1, int.MaxValue);

        public static PagedResult<T> Paginate<T>(this IEnumerable<T> values, IPagedQuery query)
            => values.Paginate(query.Page, query.Results);

        public static PagedResult<T> Paginate<T>(this IEnumerable<T> values,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
                page = 1;

            if (resultsPerPage <= 0)
                resultsPerPage = 10;

            var isEmpty = values.Any() == false;
            if (isEmpty)
                return PagedResult<T>.Empty;

            var totalResults = values.Count();
            var totalPages = (int) Math.Ceiling((decimal) totalResults/resultsPerPage);
            var data = values.Limit(page, resultsPerPage).ToList();

            return PagedResult<T>.Create(data, page, resultsPerPage, totalPages, totalResults);
        }

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> collection, IPagedQuery query)
            => collection.Limit(query.Page, query.Results);

        public static IEnumerable<T> Limit<T>(this IEnumerable<T> collection,
            int page = 1, int resultsPerPage = 10)
        {
            if (page <= 0)
                page = 1;

            if (resultsPerPage <= 0)
                resultsPerPage = 10;

            var skip = (page - 1)*resultsPerPage;
            var data = collection.Skip(skip)
                .Take(resultsPerPage);

            return data;
        }

        public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> collection, HttpResponseHeaders headers)
        {
            var totalResults = headers.GetValues("X-Total-Count").FirstOrDefault();
            var totalResultsInt = int.Parse(totalResults);

            var link = headers.GetValues("Link").First();
            var totalPages = int.Parse(GetValueFromLink(link, "last", "page"));

            var currentPage = 0;
            var nextPage = GetValueFromLink(link, "next", "page");
            var prevPage = GetValueFromLink(link, "prev", "page");
            if (nextPage != null)
                currentPage = int.Parse(nextPage) - 1;
            else if (prevPage != null)
                currentPage = int.Parse(prevPage) + 1;

            var resultsPerPage = int.Parse(GetValueFromLink(link, "first", "results"));

            return PagedResult<T>.Create(collection, currentPage, resultsPerPage, totalPages, totalResultsInt);
        }

        private static string GetValueFromLink(string link, string rel, string paramName)
        {
            var specificLink = link.Split(',')
                .FirstOrDefault(x => x.Contains(rel));

            if (specificLink == null)
                return null;

            specificLink = specificLink.Split(';').First()
                .Remove(specificLink.IndexOf('>'), 1)
                .Remove(specificLink.IndexOf('<'), 1);

            var uri = new Uri(specificLink);
            var param = HttpUtility.ParseQueryString(uri.Query).Get(paramName);

            return param;
        }
    }
}