using Checkup.Core.Models;
using Checkup.Core.Models.Enums;
using Checkup.Core.Models.Pieces;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace Checkup.Core.Models
{
    public class GameState
    {
        internal GameState() { }
        
        public bool IsBlackTurn { get; set; } = false;
        internal Board BoardState { get; set; } = new Board();
        public List<Move> Moves { get; set; } = new List<Move>(); 
        internal GameFlags Flags { get; set; } = new GameFlags();

        /// <summary>
        /// Determines whether the specified piece has not made any moves yet.
        /// </summary>
        /// <param name="piece">The piece to check for its move history. Cannot be null.</param>
        /// <returns>true if the specified piece has not moved; otherwise, false.</returns>
        internal bool IsFirstMove(BasePiece piece)
        {
            return !Moves.Any(m => m.MovedPiece == piece);
        }
        public (int x, int y)? EnPassantTarget { get; set; }
        public GameResult Result { get; set; } = GameResult.Ongoing;

        public GameEndReason EndReason { get; set; } = GameEndReason.None;
    }
}
