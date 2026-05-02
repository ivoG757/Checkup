using Checkup.Core.Models.Enums;

namespace Checkup.Core.Models.Pieces
{
    internal class Bishop : BasePiece
    {
        public Bishop(bool isBlack) : base(isBlack) { }

        public override PieceType Type => PieceType.Bishop;

        private (int x, int y)[] Directions => new (int, int)[]
        {
            (-1, -1), // Up-Left
            (-1, 1),  // Up-Right
            (1, -1),  // Down-Left
            (1, 1),   // Down-Right
        };
        internal override List<(int x, int y)> GetAttackedSquares(Board board, int currentX, int currentY)
        {
            return GetSlidingAttacks(board, currentX, currentY, Directions);
        }

        internal override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {
           return GetSlidingMoves(board, currentX, currentY, Directions);
        }
    }
}
