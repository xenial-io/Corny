using System;
using System.Collections.Generic;
using System.Text;

namespace Xenial.Delicious.Corny.Data
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Is record type")]
    public record Character(char? Content, Color? Foreground, Color? Background)
    {
        public static readonly Character Empty = new Character(null, null, null);
    }
}
