using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalRenderer
{
    internal class FractelBranch
    {
        public Vector2 Start { get; set; }
        public Vector2 End { get; set; }

        public float Length { get; set; }
        public float Rotation { get; set; }

        public List<FractelBranch> Branches { get; set; }

        public FractelBranch()
        {
            Start = new Vector2(0, 0);
            End = new Vector2(10, 10);

            Length = 0;
            Rotation = 135F;

            Branches = new List<FractelBranch>();
        }


    }
}
