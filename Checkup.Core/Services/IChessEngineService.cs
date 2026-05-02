using Checkup.Core.Models;
using Checkup.Core.Models.Pieces;

namespace Checkup.Core.Services
{
    public interface IChessEngineService
    {
        List<(int x, int y)> GetValidMoves(int row, int col);
        List<(int x, int y)> GetValidAttacks(int row, int col);
        void IsCheckmate() { }
        public BasePiece? GetPiece(int row, int col);
        public bool MovePiece(int xFrom, int yFrom, int xTo, int yTo);
        void SetPiecesOnBoard();
    }
}