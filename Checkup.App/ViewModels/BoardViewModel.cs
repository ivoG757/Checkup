//using Android.Hardware.Lights;
//using AndroidX.Lifecycle;
using Checkup.Core.Models;
using Checkup.Core.Models.Interfaces;
using Checkup.Core.Models.Pieces;
using Checkup.Core.Services;
using Checkup.Infrastructure.ChessEngine;
using System.ComponentModel;
using System.Net.NetworkInformation;
//using static Android.Provider.DocumentsContract;

namespace Checkup.App.ViewModels;

public class BoardViewModel : INotifyPropertyChanged
{
    public Board Board => ChessEngine.Board;
    public IChessEngineService ChessEngine { get; }
    public List<SquareViewModel> Squares { get; } = new();
    public BoardViewModel(IChessEngineService service)
    {
        ChessEngine = service;

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                Squares.Add(new SquareViewModel(row, col));
            }
        }
        SyncFromEngine();
    }
    //private IPiece _selectedPiece { get; set; }
    private void SyncFromEngine()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Squares[r * 8 + c].Piece = ChessEngine.Board.Squares[r, c];
            }
        }
    }
    private (int x, int y)? _selectedPosition;
    public void ClickedSquare(int x, int y)
    {
        if (_selectedPosition == null)
        {
            _selectedPosition = (x, y);
            return;
        }

        var (fromX, fromY) = _selectedPosition.Value;

        var movingPiece = Board.Squares[fromX, fromY];

        if (!ChessEngine.MovePiece(fromX, fromY, x, y))
        { 
            return;
        }

        this.Squares[fromX * 8 + fromY].Piece = null;
        this.Squares[x * 8 + y].Piece = movingPiece;

        _selectedPosition = null;
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}