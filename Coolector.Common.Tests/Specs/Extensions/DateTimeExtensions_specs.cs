using System;
using Coolector.Common.Extensions;
using Machine.Specifications;

namespace Coolector.Common.Tests.Specs.Extensions
{
    [Subject("DateTimeExtensions ToTimestamp")]
    public class When_converting_date_time_to_js_timestamp
    {
        protected static DateTime DateTime;
        protected static long ExpectedValue = 946684800000;
        protected static long Result;

        Establish context = () => DateTime = new DateTime(2000, 01, 01);

        Because of = () => Result = DateTime.ToTimestamp();

        It should_equal_expected_value = () => Result.ShouldEqual(ExpectedValue);
    }
}