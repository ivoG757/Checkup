using Checkup.Core.Models;

namespace Checkup.Core.Services
{
    public interface IChessEngineService
    {
        public void SelectPiece(int row, int col) { }
        public void MovePiece(int toRow, int toCol) { }
        public List<(int x, int y)> GetValidMoves(int row, int col);
        public void IsCheckmate() { }
        public bool MovePiece(int fromX, int fromY, int toX, int toY);
        public GameState GameState { get; set; }
        void SetPiecesOnBoard();
    }
}