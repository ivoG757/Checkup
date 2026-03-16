using System;
using System.Collections.Generic;
using System.Text;

namespace Checkup.Core.Models.Interfaces
{
    public interface IPiece
    {
        public bool IsBlack { get; set; }
        public char Symbol { get; }

        //public void Move()
        //{
        //    // TODO: Implement move logic here
        //}
    }
}
