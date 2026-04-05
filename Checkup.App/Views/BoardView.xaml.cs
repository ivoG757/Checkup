
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
                var background = new BoxView();
                var piece = new Image();
                var highlightDot = new BoxView();

                var square = new Grid();

                //layers
                square.Children.Add(background);
                square.Children.Add(piece);
                square.Children.Add(highlightDot);

                //dot style
                highlightDot.Color = Color.FromArgb("#a0a0a0");
                highlightDot.WidthRequest = 25;
                highlightDot.HeightRequest = 25;
                highlightDot.CornerRadius = 15;
                highlightDot.HorizontalOptions = LayoutOptions.Center;
                highlightDot.VerticalOptions = LayoutOptions.Center;
                highlightDot.IsVisible = false; // default hidden

                int r = row;
                int c = col;

                var vm = _viewModel.Squares[r * 8 + c];
                square.BindingContext = vm;

                //background binding
                background.SetBinding(BoxView.ColorProperty, nameof(SquareViewModel.BackgroundColor));

                //piece binding
                piece.SetBinding(Image.SourceProperty, nameof(SquareViewModel.PieceImage));

                //dot visibility binding
                highlightDot.SetBinding(BoxView.IsVisibleProperty, nameof(SquareViewModel.IsHighlighted));

                var tap = new TapGestureRecognizer();
                tap.Tapped += (s, e) =>
                {
                    _viewModel.ClickedSquare(r, c);
                };

                square.GestureRecognizers.Add(tap);

                Grid.SetRow(square, row);
                Grid.SetColumn(square, col);

                BoardGrid.Children.Add(square);
            }
        }
    }
}