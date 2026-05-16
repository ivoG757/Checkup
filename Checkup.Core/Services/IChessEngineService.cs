using Checkup.Core.Models;
using Checkup.Core.Models.Enums;
using Checkup.Core.Models.Pieces;

namespace Checkup.Core.Services
{
    public interface IChessEngineService
    {
        /// <summary>
        /// Returns a list of valid moves from the specified board position.
        /// </summary>
        /// <param name="row">The zero-based row index of the starting position.</param>
        /// <param name="col">The zero-based column index of the starting position.</param>
        /// <returns>A list of tuples representing the coordinates of valid moves. The list is empty if no valid moves are
        /// available.</returns>
        List<(int x, int y)> GetValidMoves(int row, int col);

        /// <summary>
        /// Returns a list of valid attack positions from the specified board coordinates.
        /// </summary>
        /// <param name="row">The zero-based row index of the starting position.</param>
        /// <param name="col">The zero-based column index of the starting position.</param>
        /// <returns>A list of tuples representing the coordinates (x, y) of all valid attack positions from the given location.
        /// The list is empty if no valid attacks are available.</returns>
        List<(int x, int y)> GetValidAttacks(int row, int col);

        /// <summary>
        /// Gets the piece located at the specified row and column on the board.
        /// </summary>
        /// <param name="row">The zero-based row index of the piece to retrieve. Must be within the valid range of the board.</param>
        /// <param name="col">The zero-based column index of the piece to retrieve. Must be within the valid range of the board.</param>
        /// <returns>The piece at the specified position, or null if the position is empty.</returns>
        public BasePiece? GetPiece(int row, int col);

        /// <summary>
        /// Undoes the last move made in the game, reverting the game state to what it was before that move was executed.
        /// </summary>
        public void UndoLastMove();

        /// <summary>
        /// Determines whether the current player is in check.
        /// </summary>
        /// <returns>true if the current player's king is under threat of capture; otherwise, false.</returns>
        public bool IsPlayerInCheck();

        /// <summary>
        /// Attempts to move a game piece from the specified source coordinates to the specified destination
        /// coordinates.
        /// </summary>
        /// <remarks>The move is only performed if it is valid according to the game rules. No action is
        /// taken if the move is invalid.</remarks>
        /// <param name="xFrom">The zero-based x-coordinate of the piece's current position.</param>
        /// <param name="yFrom">The zero-based y-coordinate of the piece's current position.</param>
        /// <param name="xTo">The zero-based x-coordinate of the destination position.</param>
        /// <param name="yTo">The zero-based y-coordinate of the destination position.</param>
        /// <returns>true if the piece was moved successfully; otherwise, false.</returns>
        public bool MovePiece(int xFrom, int yFrom, int xTo, int yTo);

        /// <summary>
        /// Gets the current result of the game.
        /// </summary>
        /// <returns>The current game result.</returns>
        public GameResult GetGameResult();

        /// <summary>
        /// Determines whether it is currently the black player's turn.
        /// </summary>
        /// <returns>true if it is the black player's turn; otherwise, false.</returns>
        public bool IsBlackTurn();
        
    }
}