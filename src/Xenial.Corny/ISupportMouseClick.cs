using System;
using System.Linq;

namespace Xenial.Delicious.Corny
{
    public interface ISupportMouseClick
    {
        void OnMouseClick(MouseClickEventArgs args);

        event EventHandler<MouseClickEventArgs> MouseClick;
    }
}
