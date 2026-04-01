using Checkup.Core.Common;

namespace Checkup.Core.Models.Pieces
{
    public class Bishop : BasePiece
    {
        public override bool IsBlack { get; set; } = false;
        public override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {
            var validMoves = new List<(int x, int y)>();
           
            validMoves.AddRange(CheckDirection(board, currentX, currentY, 1, 1));   // Bottom-right diagonal
            validMoves.AddRange(CheckDirection(board, currentX, currentY, 1, -1));  // Top-right diagonal
            validMoves.AddRange(CheckDirection(board, currentX, currentY, -1, 1));  // Bottom-left diagonal
            validMoves.AddRange(CheckDirection(board, currentX, currentY, -1, -1)); // Top-left diagonal

            return validMoves;
        }
        private List<(int, int)> CheckDirection(Board board, int currentX, int currentY, int dx, int dy)
        {
            //2 , 4
            var validMoves = new List<(int x, int y)>();

            int j = currentY + dy;
            int x = currentX + dx;

            while (IsInBounds(x, j))
            {
                if (board.Squares[x, j] == null)
                {
                    validMoves.Add((x, j));
                }
                else
                {
                    if (board.Squares[x, j].IsBlack != this.IsBlack)
                    {
                        validMoves.Add((x, j)); // Can capture opponent's piece
                    }
                    break; // Stop checking in this direction after hitting a piece
                }
                x += dx;
                j += dy;
            }
            return validMoves;
        }
    }
}
