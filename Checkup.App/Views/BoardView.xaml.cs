using Checkup.App.ViewModels;

using Checkup.Core.Models.Interfaces;
using Checkup.Core.Services;
using Checkup.Infrastructure.ChessEngine;
namespace Checkup.App.Views;

public partial class BoardView : ContentView
{
    private readonly BoardViewModel _viewModel;

    public BoardView()
    {
        InitializeComponent();

        _viewModel = new BoardViewModel(new ChessEngineService());

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
                    
                    _viewModel.ClickedSquare(r, c);
                    
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
        var piece = _viewModel.Board.Squares[row, col];
        square.FontFamily = "Segoe UI Symbol";

        var symbol = piece switch
        {
            IPiece p when p.IsBlack => p.Symbol.ToString(),
            IPiece p => p.Symbol.ToString(),
            _ => string.Empty
        };

        square.Text = symbol;
    }
}