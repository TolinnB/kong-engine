using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nkast.Aether.Physics2D.Common;
using nkast.Aether.Physics2D.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/**/
// Converts units between display units and simulation units
// Can handle both scalar and vector conversions
/**/
namespace Kong_Engine
{
    public static class ConvertUnits
    {
        private static float _displayUnitsToSimUnitsRatio = 100f; // Change as needed

        public static float ToSimUnits(float displayUnits)
        {
            return displayUnits / _displayUnitsToSimUnitsRatio;
        }

        public static Vector2 ToSimUnits(Vector2 displayUnits)
        {
            return displayUnits / _displayUnitsToSimUnitsRatio;
        }

        public static float ToDisplayUnits(float simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        public static Vector2 ToDisplayUnits(Vector2 simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }
    }
}