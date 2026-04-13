using Checkup.Core.Models.Enums;

namespace Checkup.Core.Models.Pieces
{
    internal class Bishop : BasePiece
    {
        public Bishop(bool isBlack) : base(isBlack) { }

        public override PieceType Type => PieceType.Bishop;

        internal override List<(int x, int y)> GetAttackedSquares(GameState state, int currentX, int currentY)
        {
            throw new NotImplementedException();
        }

        internal override List<(int x, int y)> GetValidMoves(GameState state, int currentX, int currentY)
        {
            return GetSlidingMoves(state, currentX, currentY, new (int, int)[]
           {
                (-1, -1), // Up-Left
                (-1, 1),  // Up-Right
                (1, -1),  // Down-Left
                (1, 1),   // Down-Right
           });
        }
    }
}
