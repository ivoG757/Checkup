using Checkup.Core.Models.Interfaces;

namespace Checkup.Core.Models
{
    public class Move
    {
        public (int x, int y) From { get; set; }
        public (int x, int y) To { get; set; }

        public IPiece? MovedPiece { get; set; }
        public IPiece? CapturedPiece { get; set; }

        public bool IsCastling { get; set; }
        public bool IsEnPassant { get; set; }
        public bool IsPromotion { get; set; }
    }
}