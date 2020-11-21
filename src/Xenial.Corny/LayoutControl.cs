using System;
using System.Linq;

namespace Xenial.Delicious.Corny
{
    public class LayoutControl : Control
    {
        public int? Rows { get; set; }

        public int Columns { get; set; } = 2;

        public override void Render()
        {
        }
    }
}
