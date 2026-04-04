using Checkup.Core.Common;
using Checkup.Core.Common.Enums;

namespace Checkup.Core.Models.Pieces
{
    public class Bishop : BasePiece
    {
        public override bool IsBlack { get; set; } = false;
        public override PieceType Type => PieceType.Bishop;

        public override List<(int x, int y)> GetAttackedSquares(Board board, int currentX, int currentY)
        {
            throw new NotImplementedException();
        }

        public override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {
            return GetSlidingMoves(board, currentX, currentY, new (int, int)[]
           {
                (-1, -1), // Up-Left
                (-1, 1),  // Up-Right
                (1, -1),  // Down-Left
                (1, 1),   // Down-Right
           });
        }
    }
}
