using System;
using Xunit;

namespace Chronic.Tests
{
    public class CustomParsingTest : ParsingTestsBase
    {

        protected override DateTime Now()
        {
            return Time.New(2006, 8, 16, 14, 0, 0);
        }

        [Fact]
        public void may_28_at_5_32_19pm()
        {
            Parse("may 28 at 5:32:19pm", new { Context = Pointer.Type.Past })
                .AssertEquals(Time.New(2006, 5, 28, 17, 32, 19));
        }

        [Fact]
        public void _7_days_from_now()
        {
            Parse(" 7 days from now")
                .AssertEquals(Time.New(2006, 8, 23, 14));
        }

		[Fact]
		public void _in_7_days_from_now()
		{
			Parse("in 7 days from now")
				.AssertEquals(Time.New(2006, 8, 23, 14));
		}

		[Fact]
		public void _in_7_hours_from_now() {
			Parse("in 7 hours from now")
				.AssertEquals(Now().AddHours(7));
		}
		[Fact]
		public void _in_7_pt_5_hours_from_now() {
			Parse("in 7.5 hours from now")
				.AssertEquals(Now().AddHours(7.5));
		}
		[Fact]
		public void _in_4_hours_12_min_from_now() {
			Parse("in 4 hours 12 minutes")
				.AssertEquals(Now().AddHours(4).AddMinutes(12));
		}
		[Fact]
		public void _in_2_days_28_hours_16_minutes_from_now() {
			Parse("in 2 days 28 hours 16 minutes")
				.AssertEquals(Now().AddDays(2).AddHours(28).AddMinutes(16));
		}
		[Fact]
        public void _in_7_days_from_dotdotdot()
        {
            Parse("in 7 days from").AssertEquals(Time.New(2006, 8, 23, 14));
        }

        [Fact]
        public void _7_days_from_now_at_midnight()
        {
            Parse(" 7 days from now at midnight")
                .AssertEquals(Time.New(2006, 8, 24));
        }
        [Fact]
        public void on_sat_at_9_am() {
            var day = DayOfWeek.Saturday;
            var hour = 9;
            Parse($"on {day} at {hour}am")
                .AssertEquals(Chronic.Tests.Parsing.BasicExpressionsTests.GetFirstDateOfWeekday(Now(), day).Date.AddHours(hour));
        }
        [Fact]
        public void sat_at_9_am() {
            var day = DayOfWeek.Saturday;
            var hour = 9;
            Parse($"{day} at {hour}am")
                .AssertEquals(Chronic.Tests.Parsing.BasicExpressionsTests.GetFirstDateOfWeekday(Now(), day).Date.AddHours(hour));
        }
        [Fact]
        public void next_sat_at_9_am() {
            var day = DayOfWeek.Saturday;
            var hour = 9;
            Parse($"next {day} at {hour} am")
                .AssertEquals(Chronic.Tests.Parsing.BasicExpressionsTests.GetFirstDateOfWeekday( Now(), day).AddDays(7).Date.AddHours(hour));
        }
        [Fact]
        public void this_sat_at_9_am() {
            var day = DayOfWeek.Saturday;
            var hour = 9;
            Parse($"this {day} at {hour} am")
                .AssertEquals(Chronic.Tests.Parsing.BasicExpressionsTests.GetFirstDateOfWeekday(Now(), day).Date.AddHours(hour));
        }
        [Fact]
        public void _9_am_on_sat() {
            var day = DayOfWeek.Saturday;
            var hour = 9;
            Parse($"{hour} am on {day}")
                .AssertEquals(Chronic.Tests.Parsing.BasicExpressionsTests.GetFirstDateOfWeekday(Now(), day).Date.AddHours(hour));
        }
        [Fact]
        public void in_25_days_at_11_am() {
            var days = 25;
            var hour = 11;
            Parse($"in {days} days at {hour}am").AssertEquals(Now().Date.AddDays(days).AddHours(hour));
        }
    //[Fact]
    //public void time_today_with_pointer() {
    //    //Parse("in 25 days at 12pm").AssertEquals(Now().Date.AddDays(25).AddHours(12));
    //    Parse("9 am", new { Context = Pointer.Type.Future }).AssertEquals(Now().Date.AddDays(1).AddHours(9));
    //}
        [Fact]
        public void seven_days_from_now_at_noon()
        {
            Parse(" seven days from now at noon")
                .AssertEquals(Now().Date.AddDays(7).AddHours(12));
        }
        [Fact]
        public void seven_days_from_now_at_23_00() {
            Parse(" seven days from now at 23:00")
                .AssertEquals(Now().Date.AddDays(7).AddHours(23));
        }

        [Fact]
        public void _2_weeks_ago()
        {
            Parse("2 weeks ago")
                .AssertEquals(Time.New(2006, 8, 02, 14));
        }

        [Fact]
        public void two_weeks_ago()
        {
            Parse("two weeks ago")
                .AssertEquals(Time.New(2006, 8, 02, 14));
        }


        public class WeekOrDayXinMonth : ParsingTestsBase
        {
            protected override DateTime Now()
            {
                return Time.New(2017, 05, 03, 14, 34, 13);
            }

            [Fact]
            public void first_week_january_is_parsed_correctly()
            {
                Parse("first week january").AssertStartsAt(Time.New(2018, 1, 1));
                Parse("first week january")  .AssertEndsAt(Time.New(2018, 1, 7));
            }
            [Fact]
            public void secondWord_week_january_is_parsed_correctly()
            {
                Parse("second week january").AssertStartsAt(Time.New(2018, 1, 7));
                Parse("second week january")  .AssertEndsAt(Time.New(2018, 1, 14));
                Parse("2nd week january").AssertStartsAt(Time.New(2018, 1, 7));
                Parse("2nd week january")  .AssertEndsAt(Time.New(2018, 1, 14));
            }
            [Fact]
            public void week3_january_is_parsed_correctly()
            {
                Parse("third week january").AssertStartsAt(Time.New(2018, 1, 14));
                Parse("third week january")  .AssertEndsAt(Time.New(2018, 1, 21));
            }
            [Fact]
            public void week4_january_is_parsed_correctly()
            {
                Parse("fourth week january").AssertStartsAt(Time.New(2018, 1, 21));
                Parse("fourth week january")  .AssertEndsAt(Time.New(2018, 1, 28));
            }
            [Fact]
            public void lastWeek_january_is_parsed_correctly()
            {
                Parse("last week january").AssertStartsAt(Time.New(2018, 1, 28));
                Parse("last week january")  .AssertEndsAt(Time.New(2018, 2, 1));
            }
            [Fact]
            public void lastWeek_inJanuary_is_parsed_correctly()
            {
                Parse("last week in january").AssertStartsAt(Time.New(2018, 1, 28));
                Parse("last week in january")  .AssertEndsAt(Time.New(2018, 2, 1));
            }

            [Fact]
            public void first_wednesday_in_october()
            {
                Parse("first wednesday in october").AssertEquals(Time.New(2017, 10, 4, 12));
            }
            [Fact]
            public void last_wednesday_in_october()
            {
                Parse("last wednesday in october").AssertEquals(Time.New(2017, 10, 25, 12));
            }
        }

        public class OverflowTests : ParsingTestsBase
        {
            protected override DateTime Now()
            {
                return Time.New(2016, 3, 9, 14, 34, 13);
            }

            [Fact]
            public void day4_last_week_shouldBe_from_sunday()
            {
                Parse("4th day last week").AssertEquals(Time.New(2016, 3, 2, 12));
            }
        }

        public class CanExtractTimeSpanFromSpan : ParsingTestsBase
        {
            protected override DateTime Now()
            {
                return Time.New(2006, 8, 16, 14, 34, 13);
            }

            [Fact]
            public void may_28_at_5_32_19pm() {
                var when = new DateTime(Now().Year, 05, 28, 17, 32, 19);
                Parse($"{when:MMM d} at {when:h:mm:ss tt}", new { Context = Pointer.Type.Past })
                    .AssertEquals(when);
            }
            [Fact]
            public void _7_days_and_two_hours_ago()
            {
                Parse("7 days and two hours ago", new { Context = Pointer.Type.Past })
                    .AssertEquals(Now().AddDays(-7).AddHours(-2));
            }

            [Fact]
            public void friday_9_oct()
            {
                Parse("friday 9 oct")
                    .AssertEquals(Time.New(2006, 10, 09, 12));
            }

            [Fact]
            public void second_week_in_january_is_parsed_correctly()
            {
                Parse("2nd week in january").AssertEquals(Time.New(2007, 1, 10, 12));
            }
            [Fact]
            public void second_week_january_is_parsed_correctly()
            {
                Parse("2nd week january").AssertEquals(Time.New(2007, 1, 10, 12));
            }
            [Fact]
            public void second_week_of_january_is_parsed_correctly()
            {
                Parse("2nd week of january").AssertEquals(Time.New(2007, 1, 10, 12));
            }
            [Fact]
            public void secondWord_week_january_is_parsed_correctly()
            {
                Parse("second week january").AssertEquals(Time.New(2007, 1, 10, 12));
            }
            [Fact]
            public void secondWord_week_of_january_is_parsed_correctly()
            {
                Parse("second week of january").AssertEquals(Time.New(2007, 1, 10, 12));
            }
            [Fact]
            public void secondWord_week_in_january_is_parsed_correctly()
            {
                Parse("second week in january").AssertEquals(Time.New(2007, 1, 10, 12));
            }

            [Fact]
            public void third_week_of_month_test_num()
            {
                Parse("3rd week in january").AssertEquals(Time.New(2007, 1, 17, 12));
            }
            [Fact]
            public void third_week_of_month_test()
            {
                Parse("third week in january").AssertEquals(Time.New(2007, 1, 17, 12));
            }
            [Fact]
            public void third_week_of_month_test2()
            {
                Parse("third week january").AssertEquals(Time.New(2007, 1, 17, 12));
            }
            [Fact]
            public void third_week_of_month_test3()
            {
                Parse("third week of january").AssertEquals(Time.New(2007, 1, 17, 12));
            }

            [Fact]
            public void fourth_week_of_month_test_num()
            {
                Parse("4th week in january").AssertEquals(Time.New(2007, 1, 24, 12));
            }
            [Fact]
            public void fourth_week_of_month_test()
            {
                Parse("fourth week in january").AssertEquals(Time.New(2007, 1, 24, 12));
            }
            [Fact]
            public void fourth_week_of_month_test2()
            {
                Parse("fourth week january").AssertEquals(Time.New(2007, 1, 24, 12));
            }
            [Fact]
            public void fourth_week_of_month_test3()
            {
                Parse("fourth week of january").AssertEquals(Time.New(2007, 1, 24, 12));
            }

            [Fact]
            public void last_week_of_month_test()
            {
                Parse("last week in january").AssertStartsAt(Time.New(2007, 1, 28));
                Parse("last week in january").  AssertEndsAt(Time.New(2007, 2, 1));
            }
            [Fact]
            public void last_week_of_month_test2()
            {
                Parse("last week january").AssertStartsAt(Time.New(2007, 1, 28));
                Parse("last week january").AssertEndsAt(Time.New(2007, 2, 1));
            }
            [Fact]
            public void last_week_of_month_test3()
            {
                Parse("last week of january").AssertStartsAt(Time.New(2007, 1, 28));
                Parse("last week of january").AssertEndsAt(Time.New(2007, 2, 1));
            }
        }

        public class StrangeTimesTest : ParsingTestsBase
        {
            protected override DateTime Now()
            {
                return Time.New(2017, 1, 1, 0, 0, 0);
            }

            [Fact]
            public void Feb30()
            {
                Parse("Feb 30").AssertStartsAt(Time.New(2017, 2, 30));
            }
            [Fact]
            public void April31()
            {
                Parse("April 31").AssertStartsAt(Time.New(2017, 4, 31));
            }
        }
    }
}
