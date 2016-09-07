// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System.Collections.Generic;

namespace Tbasic.Parsing
{
    internal class TerminalScanner : DefaultScanner
    {
        public TerminalScanner(string buffer)
            : base(buffer)
        {
        }

        protected override char EscapeCharacter
        {
            get {
                return '`'; // use the backtick as an escape character. this way we don't have to constantly use \\ in paths.
            }
        }

        public override IScanner Scan(IEnumerable<char> buffer)
        {
            return new TerminalScanner(buffer.ToString());
        }

        public override bool NextString(out string parsed)
        {
            string token = BuffNextWord();
            if (token == null) {
                parsed = null;
            }
            else if (!base.NextString(out parsed)) {
                parsed = token;
                AdvanceScanner(parsed);
            }
            return (parsed != null);
        }
    }
}
