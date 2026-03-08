using Checkup.App.ViewModels;
using Checkup.Core.Models.Enums;
using ChessColor = Checkup.Core.Models.Enums.Color;
namespace Checkup.App.Views;

public partial class BoardView : ContentView
{
    private readonly BoardViewModel _viewModel;

    public BoardView()
    {
        InitializeComponent();

        _viewModel = new BoardViewModel();

        BuildGrid();
    }

    private void BuildGrid()
    {
        for (int i = 0; i < 8; i++)
        {
            BoardGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            BoardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        }

        for (int row = 0; row < 8; row++)
        {
            for (int col = 0; col < 8; col++)
            {
                var square = new Label
                {
                    BackgroundColor = (row + col) % 2 == 0 ? Colors.DarkGray : Colors.Black,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 50
                };

                int r = row;
                int c = col;

                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) =>
                {
                    _viewModel.PlacePiece(r, c);
                    UpdateSquare(square, r, c);
                };
                square.GestureRecognizers.Add(tap);

                Grid.SetRow(square, row);
                Grid.SetColumn(square, col);

                BoardGrid.Children.Add(square);

                // Initialize the text for empty squares
                UpdateSquare(square, row, col);
            }
        }
    }
    private void UpdateSquare(Label square, int row, int col)
    {
        var piece = _viewModel.Board.Squares[row, col].Piece;
        var color = _viewModel.Board.Squares[row, col].Color;
        square.FontFamily = "Segoe UI Symbol";

        // Map Piece enum to chess symbols
        string symbol = (piece, color) switch
        {
            (Piece.Pawn, ChessColor.White) => "♙",
            (Piece.Knight, ChessColor.White) => "♘",
            (Piece.Bishop, ChessColor.White) => "♗",
            (Piece.Rook, ChessColor.White) => "♖",
            (Piece.Queen, ChessColor.White) => "♕",
            (Piece.King, ChessColor.White) => "♔",
            (Piece.Pawn, ChessColor.Black) => "♟",
            (Piece.Knight, ChessColor.Black) => "♞",
            (Piece.Bishop, ChessColor.Black) => "♝",
            (Piece.Rook, ChessColor.Black) => "♜",
            (Piece.Queen, ChessColor.Black) => "♛",
            (Piece.King, ChessColor.Black) => "♚",
            _ => ""
        };

        square.Text = symbol;
    }
}