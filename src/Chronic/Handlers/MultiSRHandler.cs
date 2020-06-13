using System.Collections.Generic;
using System.Linq;

namespace Chronic.Handlers
{
    public class MultiSRHandler : IHandler
    {
        public virtual Span Handle(IList<Token> tokens, Options options)
        {
			var pointer = tokens.First(token => token.IsTaggedAs<Pointer>());
			if (tokens.First().IsTaggedAs<Pointer>())//if we are starting with a pointer then it is a multi arrow situation
				tokens = tokens.Skip(1).ToList();

			var now = options.Clock();
            var span = new Span(now, now.AddSeconds(1));
            var at_token = tokens.FirstOrDefault(token => token.IsTaggedAs<SeparatorAt>());
            var pointer_token = tokens.FirstOrDefault(token => token.IsTaggedAs<Pointer>());
            var grabberTokens = tokens
                .SkipWhile(token => token.IsNotTaggedAs<Grabber>())
                .ToList();
            if (pointer_token != null) {
                var pointer_pos = tokens.IndexOf(pointer_token);
                var what = tokens.Skip(pointer_pos + 1);
                if (what.Any()) {
                    if (at_token != null) {
                        var at_pos = tokens.IndexOf(at_token);
                        if (at_pos > pointer_pos)
                            what = what.Take(at_pos - pointer_pos - 1);
                    }
                    span = what.GetAnchor(options);
                }
            }
            //if (grabberTokens.Any())
            //{
            //    var options2 = options.Clone();//if we are going to screw with the clock we probably shouldn't effect every call after this:)
            //    if (at_token != null)
            //        options2.Clock = () => options.Clock().Date;
            //    span = grabberTokens.GetAnchor(options2);

            //}
            //tokens = tokens.Where(a => a.IsNotTaggedAs<SeparatorAt>()).ToList();
            var scalarRepeaters = tokens
                .TakeWhile(token => token.IsNotTaggedAs<Pointer>() && token.IsNotTaggedAs<SeparatorAt>())
                .Where(token => token.IsNotTaggedAs<SeparatorComma>())
                .ToList();



            if (at_token != null) {
                var pos = tokens.IndexOf(at_token);
                var time_tokens = tokens.Skip(pos + 1).ToList();
                if (span?.Start == null)
                    return null;
                span = Utils.DayOrTime(span.Start.Value, time_tokens, options);

            }
            for (var index = 0; index < scalarRepeaters.Count - 1; index++)
            {
                var scalar = scalarRepeaters[index];
                var repeater = scalarRepeaters[++index];
                span = Handle(new List<Token>{ scalar, repeater, pointer}, span, options);
            }
            
            

            return span;
        }

        public Span Handle(IList<Token> tokens, Span span, Options options)
        {
            var distance = tokens[0].GetTag<Scalar>().Value;
            var repeater = tokens[1].GetTag<IRepeater>();
            var pointer = tokens[2].GetTag<Pointer>().Value;
            return repeater.GetOffset(span, distance, pointer);
        }
    }
}