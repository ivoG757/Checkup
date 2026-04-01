using Checkup.Core.Models.Pieces;

namespace Checkup.App.Services.Interfaces
{
    public interface IPieceImageProvider
    {
        public string GetImagePath(BasePiece piece);
    }
}