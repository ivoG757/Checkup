using Checkup.Core.Common;
using Checkup.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models.Pieces
{
    public class Rook : BasePiece
    {
        public override bool IsBlack { get; set; } = false;
        public override char Symbol => IsBlack ? (char)ChessPiecesEmojisBlack.Rook : (char)ChessPiecesEmojisWhite.Rook;

        public override List<(int x, int y)> GetValidMoves(Board board, int x, int y)
        {
            var validMoves = new List<(int, int)>();

            /*
              (0,0) (0,1) (0,2)
              (1,0) (1,1) (1,2)
              (2,0) (2,1) (2,2)
            */

            // Up
            for (int i = x - 1; i >= 0; i--)
            {
                if (board.Squares[i, y] == null)
                    validMoves.Add((i, y));
                else
                {
                    if (board.Squares[i, y].IsBlack != IsBlack)
                        validMoves.Add((i, y)); // can capture 
                    break; // blocked
                }
            }

            // Down
            for (int i = x + 1; i < 8; i++)
            {
                if (board.Squares[i, y] == null)
                    validMoves.Add((i, y));
                else
                {
                    if (board.Squares[i, y].IsBlack != IsBlack)
                        validMoves.Add((i, y));
                    break;
                }
            }

            // Left
            for (int j = y - 1; j >= 0; j--)
            {
                if (board.Squares[x, j] == null)
                    validMoves.Add((x, j));
                else
                {
                    if (board.Squares[x, j].IsBlack != IsBlack)
                        validMoves.Add((x, j));
                    break;
                }
            }

            // Right
            for (int j = y + 1; j < 8; j++)
            {
                if (board.Squares[x, j] == null)
                    validMoves.Add((x, j));
                else
                {
                    if (board.Squares[x, j].IsBlack != IsBlack)
                        validMoves.Add((x, j));
                    break;
                }
            }

            return validMoves;
        }
    }
}
