using Checkup.Core.Models;

namespace Checkup.Core.Services
{
    public interface IChessEngineService
    {
        public void ClickedSquare(int x, int y);
        public void SelectPiece(int row, int col) { }
        public void MovePiece(int toRow, int toCol) { }
        public void GetValidMoves(int row, int col) { }
        public void IsCheckmate() { }
        public Board Board { get; set; }
        void SetPiecesOnBoard();
    }
}