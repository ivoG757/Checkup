using System;
using System.Collections.Generic;
using System.Text;
using Checkup.Core.Common;
using Checkup.Core.Models.Interfaces;
namespace Checkup.Core.Models.Pieces
{
    public class Pawn : IPiece
    {
        public bool HasMoved { get; set; } = false;
        public bool IsBlack { get; set; } = false;
        public char Symbol => IsBlack ? (char)ChessPiecesEmojisBlack.Pawn : (char)ChessPiecesEmojisWhite.Pawn;
    }
}
