using Checkup.Core.Common.Enums;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Checkup.Core.Models.Interfaces
{
    public interface IPiece
    {
        bool IsBlack { get; set; }
        PieceType Type { get; }
        List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY);
    }
}
