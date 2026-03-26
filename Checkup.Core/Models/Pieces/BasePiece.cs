using Checkup.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models.Pieces
{
    namespace Checkup.Core.Models.Pieces
    {
        public abstract class BasePiece : IPiece
        {
            public bool IsBlack { get; set; }
            public abstract char Symbol { get; }
            public abstract List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY);
        }
    }
}
