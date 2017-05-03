using System.Collections.Generic;
using System.Linq;

namespace Chronic.Handlers
{
    public class ORGRHandler : IHandler
    {
        public Span Handle(IList<Token> tokens, Options options)
        {
            // 4th day last week
            var outerSpan = tokens.Skip(2).Take(2).GetAnchor(options);
            return Utils.HandleORR(tokens.Take(2).ToList(), outerSpan, options);
        }
    }
}