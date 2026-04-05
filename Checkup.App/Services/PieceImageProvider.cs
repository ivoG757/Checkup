using Checkup.Core.Models.Enums;
using Checkup.Core.Models.Interfaces;
using Checkup.Core.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.App.Services
{
    public static class PieceImageProvider
    {
        public static class Highlights
        {
            public static string MoveHighlight => "move_highlight.jpg";
            public static string CaptureHighlight => "capture_highlight.png";
            public static string CheckHighlight => "check_highlight.png";
        }
        public static string GetImagePath(IPiece piece)
        {
            string color = piece.IsBlack ? "black" : "white";

            return piece.Type switch
            {
                PieceType.Rook => $"{color}_rook.png",
                PieceType.Bishop => $"{color}_bishop.png",
                PieceType.Knight => $"{color}_knight.png",
                PieceType.Queen => $"{color}_queen.png",
                PieceType.King => $"{color}_king.png",
                PieceType.Pawn => $"{color}_pawn.png",
                _ => throw new Exception("Unknown piece")
            };
        }
    }
}
