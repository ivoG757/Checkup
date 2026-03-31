using Checkup.Core.Models;
using Checkup.Core.Models.Interfaces;
using Checkup.Core.Models.Pieces;
using Checkup.Core.Services;
using Checkup.Infrastructure.ChessEngine;
using System.ComponentModel;
using System.Net.NetworkInformation;

namespace Checkup.App.ViewModels;

public class BoardViewModel : INotifyPropertyChanged
{
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

    private void SyncFromEngine()
    {
        for (int r = 0; r < 8; r++)
        {
            for (int c = 0; c < 8; c++)
            {
                Squares[r * 8 + c].Piece = ChessEngine.Board.Squares[r, c];
                Squares[r * 8 + c].IsSelected = false;
            }
        }
    }

    private (int x, int y)? _selectedPosition;
    public void ClickedSquare(int x, int y)
    {
        // If nothing selected, select this square (if it has a piece)
        if (_selectedPosition == null)
        {
            var piece = ChessEngine.Board.Squares[x, y];
            if (piece == null)
                return;

            _selectedPosition = (x, y);
            Squares[x * 8 + y].IsSelected = true;
            return;
        }

        // If clicked same square again, unselect
        if (_selectedPosition.Value == (x, y))
        {
            Squares[x * 8 + y].IsSelected = false;
            _selectedPosition = null;
            return;
        }

        var (fromX, fromY) = _selectedPosition.Value;
        var movingPiece = ChessEngine.Board.Squares[fromX, fromY];

        // Attempt move
        if (!ChessEngine.MovePiece(fromX, fromY, x, y))
        {
            // If move failed, keep selection or unselect if clicked on previously selected square handled above
            return;
        }

        // Update viewmodels: clear old, set new, clear selection
        Squares[fromX * 8 + fromY].Piece = null;
        Squares[fromX * 8 + fromY].IsSelected = false;

        Squares[x * 8 + y].Piece = movingPiece;
        Squares[x * 8 + y].IsSelected = false;

        _selectedPosition = null;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}