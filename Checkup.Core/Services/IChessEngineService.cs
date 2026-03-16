namespace Checkup.Core.Services
{
    public interface IChessEngineService
    {
        public void SelectPiece(int row, int col) { }
        public void MovePiece(int toRow, int toCol) { }
        public void GetValidMoves(int row, int col) { }
        public void IsCheckmate() { }
    }
}