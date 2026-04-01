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

        public override List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY)
        {
            var validMoves = new List<(int x, int y)>();

            // Up
            for (int i = currentX - 1; i >= 0; i--)
            {
                if (board.Squares[i, currentY] == null)
                    validMoves.Add((i, currentY));
                else
                {
                    if (board.Squares[i, currentY].IsBlack != IsBlack)
                        validMoves.Add((i, currentY)); // can capture 
                    break; // blocked
                }
            }

            // Down
            for (int i = currentX + 1; i < 8; i++)
            {
                if (board.Squares[i, currentY] == null)
                    validMoves.Add((i, currentY));
                else
                {
                    if (board.Squares[i, currentY].IsBlack != IsBlack)
                        validMoves.Add((i, currentY));
                    break;
                }
            }

            // Left
            for (int j = currentY - 1; j >= 0; j--)
            {
                if (board.Squares[currentX, j] == null)
                    validMoves.Add((currentX, j));
                else
                {
                    if (board.Squares[currentX, j].IsBlack != IsBlack)
                        validMoves.Add((currentX, j));
                    break;
                }
            }

            // Right
            for (int j = currentY + 1; j < 8; j++)
            {
                if (board.Squares[currentX, j] == null)
                    validMoves.Add((currentX, j));
                else
                {
                    if (board.Squares[currentX, j].IsBlack != IsBlack)
                        validMoves.Add((currentX, j));
                    break;
                }
            }

            validMoves.AddRange(CheckDirection(board, currentX, currentY, 1, 1));   // Bottom-right diagonal
            validMoves.AddRange(CheckDirection(board, currentX, currentY, 1, -1));  // Top-right diagonal
            validMoves.AddRange(CheckDirection(board, currentX, currentY, -1, 1));  // Bottom-left diagonal
            validMoves.AddRange(CheckDirection(board, currentX, currentY, -1, -1)); // Top-left diagonal

            return validMoves;
        }
        private List<(int, int)> CheckDirection(Board board, int currentX, int currentY, int dx, int dy)
        {
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
