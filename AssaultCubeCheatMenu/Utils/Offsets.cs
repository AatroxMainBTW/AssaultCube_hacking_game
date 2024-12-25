using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssaultCubeCheatMenu.Utils
{
    public static class Offsets
    {
        //1- ac_client.exe
        public static int baseAddressPlayer = 0x190844;
        public static int entityListAccess = 0x0018AC04;

        //2- Offset
        //TODO Need to investigate about that
        public static int StepOffset = 0x30;

        //2.2- Player or NPC attributes
        public static int healthOffset = 0xEC;
        public static int armorOffset = 0xF0;
        public static int nameOffset = 0x205;

        //2.3- Ammo 
        public static int ammoMTP57Offset = 0x140;
        public static int ammoMK77Offset = 0x12C;
        public static int ammoHEOffset = 0x144;
        //TODO Need to add sniper ammo, shotgun, double pistol, submachine gun and carabine.

        //2.4- Player movement attributes
        public static int xAxisOffset = 0x28;
        public static int yAxisOffset = 0x2C;
        public static int zAxisOffset = 0x30;
        public static int rotationOffset = 0x34;
        public static int viewDirectionOffset = 0x38;

    }
}
