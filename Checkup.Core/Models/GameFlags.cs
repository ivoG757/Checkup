using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models
{
    internal class GameFlags
    {
        internal bool WhiteKingMoved {  get; set; }
        internal bool BlackKingMoved { get; set; }

        internal bool WhiteLeftRookMoved { get; set; }
        internal bool WhiteRightRookMoved { get; set; }
        internal bool BlackLeftRookMoved { get; set; }
        internal bool BlackRightRookMoved { get; set; }
    }
}
