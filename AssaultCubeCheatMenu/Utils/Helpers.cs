using Swed32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AssaultCubeCheatMenu.Utils
{
    public static class Helpers
    {

        //To read of strings too
        public static string ReadString(IntPtr baseAddress, int offset, Swed swed, int maxLength = 32)
        {
            byte[] buffer = swed.ReadBytes(baseAddress + offset, maxLength);
            int nullIndex = Array.IndexOf(buffer, (byte)0);
            if (nullIndex >= 0)
            {
                return Encoding.ASCII.GetString(buffer, 0, nullIndex);
            }
            return Encoding.ASCII.GetString(buffer);
        }

        public static string TeamConverter(int Team)
        {
            if (Team == 1)
            {
                return "RVSF";
            }
            else if (Team == 0)
            {
                return "CLA";
            }
            return "Not in a team N/A";
        }
    }
}
