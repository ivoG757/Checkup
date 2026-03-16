using Checkup.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Services
{
    public class ChessEngineService : IChessEngineService
    {
        public Board Board { get; set; } = new Board();
        public ChessEngineService()
        {

        }
     
    }
}
