using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace Xenial.Delicious.Corny
{
    public interface ISupportKeyPress
    {
        void OnKeyPress(KeyPressEventArgs args);

        event EventHandler<KeyPressEventArgs> KeyPress;
    }
}
