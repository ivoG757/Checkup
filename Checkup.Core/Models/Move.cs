using Checkup.Core.Models;
using Checkup.Core.Models.Pieces;

namespace Checkup.Core.Models
{
    public class Move
    {
        public Move((int x, int y) from, (int x, int y) to)
        {
            From = from;
            To = to;
        }
        internal (int x, int y) From { get; set; }
        internal (int x, int y) To { get; set; }

        internal BasePiece? MovedPiece { get; set; }
        internal BasePiece? CapturedPiece { get; set; }

        internal bool IsCastling { get; set; }
        internal bool IsEnPassant { get; set; }
        internal bool IsPromotion { get; set; }
    }
}