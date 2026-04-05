using Checkup.Core.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace Checkup.Core.Models
{
    public class GameState
    {
        public Board BoardState { get; set; } = new Board();
        public List<Move> Moves { get; set; } = new List<Move>(); 
        public GameFlags Flags { get; set; } = new GameFlags();

        /// <summary>
        /// Determines whether the specified piece has not made any moves yet.
        /// </summary>
        /// <param name="piece">The piece to check for its move history. Cannot be null.</param>
        /// <returns>true if the specified piece has not moved; otherwise, false.</returns>
        public bool IsFirstMove(IPiece piece)
        {
            return !Moves.Any(m => m.MovedPiece == piece);
        }
    }
}
