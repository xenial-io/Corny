using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Xenial.Delicious.Corny
{
    public interface IControl : IRenderable
    {
        IList<IControl> Controls { get; }

        IControl? Parent { get; set; }

        IControl? FocusedControl { get; set; }

        bool IsFocused { get; set; }

        void Focus();

        bool SupportsFocus { get; }

        void SelectNextControl();

        void SelectPreviousControl();

        Point Location { get; set; }

        Padding Margin { get; set; }

        Padding Padding { get; set; }

        Point Position { get; }
    }
}
