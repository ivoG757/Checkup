using Checkup.Core.Models.Enums;
using Checkup.Core.Models.Interfaces;

namespace Checkup.Core.Models.Pieces
{
    public abstract class BasePiece : IPiece
    {
        public abstract bool IsBlack { get; set; }
        public abstract PieceType Type { get; }

        /// <summary>
        /// Calculates all valid moves for a piece located at the specified position on the given board.
        /// </summary>
        /// <remarks>The definition of a valid move depends on the specific piece and game rules
        /// implemented by the derived class.</remarks>
        /// <param name="board">The game board on which to evaluate possible moves. Must not be null.</param>
        /// <param name="currentX">The zero-based column index of the piece's current position.</param>
        /// <param name="currentY">The zero-based row index of the piece's current position.</param>
        /// <returns>A list of coordinate pairs representing valid destination positions for the piece. The list is empty if no
        /// valid moves are available.</returns>
        public abstract List<(int x, int y)> GetValidMoves(GameState state, int currentX, int currentY);

        /// <summary>
        /// Returns all squares that are attacked (controlled) by this piece from the specified position.
        /// </summary>
        /// <remarks>
        /// Attacked squares represent all positions this piece could capture on, regardless of whether a move is currently legal.
        /// This method does not consider:
        /// - Whether moving would leave the king in check
        /// - Turn order
        /// - Pins or other game constraints
        /// 
        /// This differs from <see cref="GetValidMoves"/> which returns only legal moves.
        /// </remarks>
        /// <param name="board">The current board state.</param>
        /// <param name="currentX">The piece's current row.</param>
        /// <param name="currentY">The piece's current column.</param>
        /// <returns>A list of coordinates representing all attacked squares.</returns>
        public abstract List<(int x, int y)> GetAttackedSquares(Board board, int currentX, int currentY);

        //TODO: Add a capture method that can be called when a piece is captured 

        /// <summary>
        /// Helper to ensure coordinates are on the board
        /// </summary>
        /// <param name="x">The x-coordinate of the starting position of the piece.</param>
        /// <param name="y">The y-coordinate of the starting position of the piece.</param>
        /// <returns></returns>
        protected bool IsInBounds(int x, int y) => x >= 0 && x < 8 && y >= 0 && y < 8;

        /// <summary>
        /// Calculates all possible attack squares for a sliding piece from a given position in specified directions.
        /// </summary>
        /// <remarks>The method stops searching in a direction when it encounters a friendly piece or
        /// after including the first enemy-occupied square. Only squares within the bounds of the board are
        /// considered.</remarks>
        /// <param name="board">The board on which to evaluate possible attacks.</param>
        /// <param name="x">The x-coordinate of the starting position of the sliding piece.</param>
        /// <param name="y">The y-coordinate of the starting position of the sliding piece.</param>
        /// <param name="directions">An array of direction vectors, each represented as a tuple of x and y offsets, indicating the directions in
        /// which the piece can slide.</param>
        /// <returns>A list of coordinate pairs representing all squares that the sliding piece can attack, including empty
        /// squares and the first enemy-occupied square in each direction.</returns>
        protected List<(int x, int y)> GetSlidingAttacks(Board board, int x, int y, (int dx, int dy)[] directions)
        {
            var result = new List<(int, int)>();

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx;
                int ny = y + dy;

                while (IsInBounds(nx, ny))
                {
                    var piece = board.Squares[nx, ny];

                    if (piece == null)
                    {
                        result.Add((nx, ny));
                    }
                    else
                    {
                        result.Add((nx, ny)); // attack includes capture square
                        break;
                    }

                    nx += dx;
                    ny += dy;
                }
            }
            return result;
        }

        /// <summary>
        /// Calculates all valid sliding moves from a given position in specified directions on the board.
        /// </summary>
        /// <remarks>A move is considered valid if the destination square is empty or occupied by an
        /// opposing piece. The sliding stops when a piece of the same color is encountered or the edge of the board is
        /// reached.</remarks>
        /// <param name="board">The board on which to evaluate possible moves.</param>
        /// <param name="x">The x-coordinate of the starting position.</param>
        /// <param name="y">The y-coordinate of the starting position.</param>
        /// <param name="directions">An array of direction vectors, where each tuple specifies the change in x and y for a single direction to
        /// slide.</param>
        /// <returns>A list of coordinate pairs representing all valid destination squares that can be reached by sliding from
        /// the starting position in the given directions. The list may be empty if no moves are available.</returns>
        protected List<(int x, int y)> GetSlidingMoves(Board board, int x, int y, (int dx, int dy)[] directions)
        {
            var result = new List<(int, int)>();

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx;
                int ny = y + dy;

                while (IsInBounds(nx, ny))
                {
                    var piece = board.Squares[nx, ny];

                    if (piece == null)
                    {
                        result.Add((nx, ny));
                    }
                    else
                    {
                        if (piece.IsBlack == IsBlack)
                        { 
                            break; 
                        }
                        result.Add((nx, ny)); 
                        break;
                    }

                    nx += dx;
                    ny += dy;
                }
            }
            return result;
        }
    }
}
