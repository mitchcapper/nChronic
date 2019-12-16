using System.Collections.Generic;

namespace Chronic.Handlers
{
	//This handler is no longer used, instead MultiSRHandler is used in its place as it is more flexible.  Left behind in case used as a base class by 3rd party handlers.
	public class PSRHandler : SRPHandler
    {
        public override Span Handle(IList<Token> tokens, Options options)
        {
            var tokensToHandle = new List<Token> { tokens[1], tokens[2], tokens[0] };
            return base.Handle(tokensToHandle, options);
        }
    }
}