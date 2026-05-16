using Checkup.Core.Models;
using Checkup.Core.Models.Enums;
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
                Squares[r * 8 + c].Piece = ChessEngine.GetPiece(r, c);
                Squares[r * 8 + c].IsSelected = false;
                Squares[r * 8 + c].IsHighlighted = false;
                Squares[r * 8 + c].IsInCheck = false;
            }
        }
    }
    private List<(int x, int y)> _possibleMoves = new();
    private List<(int x, int y)> _possibleAttacks = new();
    public (int x, int y) HighLightKing { get; private set; }
    private (int x, int y)? _selectedPosition;
    public void ClickedSquare(int x, int y)
    {
        //if (ChessEngine.GetGameResult() == GameResult.WhiteWon || ChessEngine.GetGameResult() == GameResult.BlackWon || ChessEngine.GetGameResult() == GameResult.Draw)
        //{
        //    return;
        //}

        // If nothing selected, select this square (if it has a piece)
        if (_selectedPosition == null)
        {

            var piece = ChessEngine.GetPiece(x, y);

            if (piece == null)
            {
                return;
            }

            if (ChessEngine.IsBlackTurn() != ChessEngine.GetPiece(x, y)?.IsBlack)
            {
                return;
            }
            _possibleMoves = ChessEngine.GetValidMoves(x, y);
            _possibleAttacks = ChessEngine.GetValidAttacks(x, y);

            HighlightSquareColorState(_possibleMoves, true);
            HighLightAttackedSquareColorState(_possibleAttacks, true);

            _selectedPosition = (x, y);
            Squares[x * 8 + y].IsSelected = true;
            return;
        }

        // If clicked same square again, unselect
        if (_selectedPosition.Value == (x, y))
        {
            Squares[x * 8 + y].IsSelected = false;
            _selectedPosition = null;
            HighlightSquareColorState(_possibleMoves, false);
            HighLightAttackedSquareColorState(_possibleAttacks, false);

            return;
        }

        var (fromX, fromY) = _selectedPosition.Value;
        var movingPiece = ChessEngine.GetPiece(fromX, fromY);

        // Attempt move
        if (!ChessEngine.MovePiece(fromX, fromY, x, y))
        {
            // If move failed, keep selection or unselect if clicked on previously selected square handled above

            //ResetInteractionState();
            return;
        }

        //sync from engine to handle any special moves (castling, promotion, en passant) and to ensure consistency
        SyncFromEngine();
        _selectedPosition = null;

        if (ChessEngine.IsPlayerInCheck())
        {
            for (int r = 0; r < 8; r++)
            {
                for (int c = 0; c < 8; c++)
                {
                    var piece = ChessEngine.GetPiece(r, c);
                    if (piece != null && piece.Type == PieceType.King && piece.IsBlack == ChessEngine.IsBlackTurn())
                    {
                        HighLightKing = (r, c);
                        Squares[r * 8 + c].IsInCheck = true;
                        break;
                    }
                }
            }
        }
    }

    private void HighLightAttackedSquareColorState(List<(int x, int y)> possibleAttacks, bool v)
    {
        foreach (var (x, y) in possibleAttacks)
        {
            Squares[x * 8 + y].IsHighlightedAttack = v;
        }
    }

    public void HighlightSquareColorState(List<(int x, int y)> squares, bool IsHighlight)
    {
        foreach (var (x, y) in squares)
        {
            Squares[x * 8 + y].IsHighlighted = IsHighlight;
        }
    }
    private void ResetInteractionState()
    {
        HighlightSquareColorState(_possibleMoves, false);
        _possibleMoves.Clear();

        _selectedPosition = null;

        foreach (var square in Squares)
        {
            square.IsSelected = false;
            square.IsHighlighted = false;
            square.IsInCheck = false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}