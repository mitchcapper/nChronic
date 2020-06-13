using System.Collections.Generic;
using System.Linq;
using Chronic.Tags.Repeaters;

namespace Chronic.Handlers
{
    public class RHandler : IHandler
    {
        public Span Handle(IList<Token> tokens, Options options)
        {
            if (tokens.Count == 1 && tokens[0].IsTaggedAs<ScalarDay>())
            {
                var day = (int)tokens[0].GetTag<ScalarDay>().Value;
                if (Time.IsMonthOverflow(options.Clock().Year, options.Clock().Month, day))
                {
                    return null;
                }

                var dayStart = Time.New(options.Clock().Year, options.Clock().Month, day);
                if (dayStart < options.Clock().Date)
                    dayStart = dayStart.AddMonths(1);

                return new Span(dayStart, dayStart.AddDays(1));
            }
            else if (tokens.Count >= 2
			   && tokens[0].IsTaggedAs<Grabber>()
			   && (tokens[1].IsTaggedAs<RepeaterDayName>() || tokens[1].IsTaggedAs<RepeaterWeekend>()))
			{
				var repeaterWeek = new RepeaterWeek(options);
                var is_repeater_day = tokens[1].IsTaggedAs<RepeaterDayName>();


                repeaterWeek.Now = is_repeater_day ? tokens[1].GetTag<RepeaterDayName>().Now : tokens[1].GetTag<RepeaterWeekend>().Now;
                if (repeaterWeek.Now == null && options.Context != Pointer.Type.Past) {
                    repeaterWeek.Now = options.Clock();
                    var next_week_day = GetFirstDateOfWeekday(repeaterWeek.Now.Value, is_repeater_day ? tokens[1].GetTag<RepeaterDayName>().Value : System.DayOfWeek.Saturday);
                    if (repeaterWeek.GetCurrentSpan(options.Context).End < next_week_day) {
                        var date = options.Clock().AddDays(7);
                        (options = options.Clone()).Clock = () => date;
                    }
                        //repeaterWeek.Now = repeaterWeek.GetNextSpan(Pointer.Type.Future).Start.Value.AddDays(1);
                    


                }
                var token = new Token("week");
				token.Tag(repeaterWeek);
				tokens.Insert(1, token);
			}

            var ddTokens = tokens.DealiasAndDisambiguateTimes(options);

            return ddTokens.GetAnchor(options);
        }
        public static System.DateTime GetFirstDateOfWeekday(System.DateTime start, System.DayOfWeek day) {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;

            return start.AddDays(daysToAdd);
        }
    }
}