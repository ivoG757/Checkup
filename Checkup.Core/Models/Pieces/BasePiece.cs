using Checkup.Core.Models.Interfaces;

namespace Checkup.Core.Models.Pieces
{
    public abstract class BasePiece : IPiece
    {
        public abstract bool IsBlack { get; set; }
        public abstract char Symbol { get; }
        public abstract List<(int x, int y)> GetValidMoves(Board board, int currentX, int currentY);

        /// <summary>
        /// Helper to ensure coordinates are on the board
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        protected bool IsInBounds(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;
    }
}
