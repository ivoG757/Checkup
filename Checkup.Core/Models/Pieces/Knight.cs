using Checkup.Core.Models.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models.Pieces
{
    public class Knight : BasePiece
    {
        public override bool IsBlack { get; set; } = false;
        public override PieceType Type => PieceType.Knight;

        public override List<(int x, int y)> GetAttackedSquares(Board board, int currentX, int currentY)
        {
            throw new NotImplementedException();
        }

        public override List<(int x, int y)> GetValidMoves(GameState state, int currentX, int currentY)
        {
            var board = state.BoardState;

            var knightsMovements = new List<(int x, int y)>
            {
                (x: currentX - 1, y: currentY - 2), // up left
                (x: currentX + 1, y: currentY - 2), // up right
                (x: currentX - 1, y: currentY + 2), // down left
                (x: currentX + 1, y: currentY + 2), // down right
                (x: currentX - 2, y: currentY - 1), // left up
                (x: currentX - 2, y: currentY + 1), // left down
                (x: currentX + 2, y: currentY - 1), // right up
                (x: currentX + 2, y: currentY + 1)  // right down
            };
            var validMoves = new List<(int x, int y)>();
            foreach (var square in knightsMovements)
            {
                var (x, y) = square;
                
                if (IsInBounds(x, y))
                {
                    if(board.Squares[x, y] == null)
                    {
                        validMoves.Add((x, y));
                        continue;
                    }
                    if (board.Squares[x, y].IsBlack != IsBlack)
                    {
                        // Can capture opponent's piece
                        validMoves.Add((x, y));
                    }
                }
            }
            return validMoves;
        }

    }
}
