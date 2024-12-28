using Swed32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

        public static Vector2 CalculateAngles(Vector3 from, Vector3 to)
        {

            float yaw;
            float pitch;

            //Delta values
            float deltaX = to.X - from.X;
            float deltaY = to.Y - from.Y;

            // calculate the yaw & convert radians to degrees
            yaw = (float)(Math.Atan2(deltaY, deltaX) * 180 / Math.PI);

            // calculate the pitch & convert radians to degrees
            float deltaZ = to.Z - from.Z;
            double distance = Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2));
            pitch = (float)(Math.Atan2(deltaZ, distance) * 180 / Math.PI);

            return new Vector2(yaw, pitch);
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
