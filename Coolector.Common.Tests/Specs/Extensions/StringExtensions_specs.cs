using System.Collections.Generic;
using Coolector.Common.Extensions;
using Coolector.Common.Queries;
using Machine.Specifications;

namespace Coolector.Common.Tests.Specs.Extensions
{
    [Subject("StringExtensions EqualsCaseInvariant")]
    public class When_comparing_same_strings_with_different_cases
    {
        protected static bool Result;

        Because of = () => Result = "abcd".EqualsCaseInvariant("ABCD");

        It should_be_true = () => Result.ShouldBeTrue();
    }

    [Subject("StringExtensions Like")]
    public class When_checking_if_string_contains_substring_case_invariant
    {
        protected static bool Result;

        Because of = () => Result = "abcdefghijkl".Like("DEF");

        It should_be_true = () => Result.ShouldBeTrue();
    }


    [Subject("StringExtensions IsEmail")]
    public class When_checking_if_string_has_email_format
    {
        protected static bool Result;

        Because of = () => Result = "test@email.com".IsEmail();

        It should_be_true = () => Result.ShouldBeTrue();
    }

    [Subject("StringExtensions IsEmail")]
    public class When_checking_if_string_has_email_format_but_at_sign_is_missing
    {
        protected static bool Result;

        Because of = () => Result = "testemail.com".IsEmail();

        It should_be_false = () => Result.ShouldBeFalse();
    }


    [Subject("StringExtensions IsEmail")]
    public class When_checking_if_string_has_email_format_but_domain_is_missing
    {
        protected static bool Result;

        Because of = () => Result = "test@email".IsEmail();

        It should_be_false = () => Result.ShouldBeFalse();
    }


    [Subject("StringExtensions ToQueryString")]
    public class When_converting_string_to_query_string
    {
        protected class TestQuery : IQuery
        {
            public string Param1 { get; set; }
            public double Param2 { get; set; }
            public IEnumerable<string> Param3 { get; set; }
        }

        protected static TestQuery Query;
        protected static string Endpoint;
        protected static string Result;

        Establish context = () =>
        {
            Query = new TestQuery
            {
                Param1 = "test",
                Param2 = 5.5,
                Param3 = new[] {"test1", "test2"}
            };
            Endpoint = "http://localhost";
        };

        Because of = () => Result = Endpoint.ToQueryString(Query);

        It should_return_correct_query_string = () =>
        {
            Result.ShouldEqual($"http://localhost?param1=test&param2={5.5}&param3=test1,test2");
        };
    }
}