using System;
using System.ComponentModel;
using System.Linq;

namespace Xenial.Delicious.Corny
{
    public class KeyPressEventArgs : CancelEventArgs
    {
        public ConsoleKeyInfo KeyInfo { get; }

        public KeyPressEventArgs(ConsoleKeyInfo keyInfo)
            => KeyInfo = keyInfo;
    }
}
