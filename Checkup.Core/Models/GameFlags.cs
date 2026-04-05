using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models
{
    public class GameFlags
    {
        public bool WhiteKingMoved {  get; set; }
        public bool BlackKingMoved { get; set; }

        public bool WhiteLeftRookMoved { get; set; }
        public bool WhiteRightRookMoved { get; set; }

        public bool BlackLeftRookMoved { get; set; }
        public bool BlackRightRookMoved { get; set; }
    }
}
