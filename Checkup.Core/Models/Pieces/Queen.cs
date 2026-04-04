using Checkup.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models.Pieces
{
    public class Queen : BasePiece
    {
        public override bool IsBlack { get; set; } = true;
        public override PieceType Type => PieceType.Queen;

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
                (1, 0),   // Down
                (-1, 0),  // Up
                (0, 1),   // Right
                (0, -1)   // Left
            });
        }
    }
}
