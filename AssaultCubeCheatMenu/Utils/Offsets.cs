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
        public static int baseAddressPlayer = 0x0017E0A8;
        public static int entityListAccess = 0x0018AC04;
        public static int entityAccess = 0x04;
        public static int playerAccess = 0x00;
        //2- Offset
        //TODO Need to investigate about that
        public static int StepOffset = 0x4;

        //2.2- Player or NPC attributes
        public static int healthOffset = 0xEC;
        public static int armorOffset = 0xF0;
        public static int nameOffset = 0x205;
        public static int teamOffset = 0x30C;

        //2.3- Ammo 
        public static int ammoMTP57Offset = 0x140;
        public static int ammoMK77Offset = 0x12C;
        public static int ammoHEOffset = 0x144;
        public static int ammoAD80Offset = 0x13C;
        public static int ammoV19Offset = 0x134;
        public static int ammoARD10Offset = 0x138;
        public static int ammoTMPOffset = 0x130;

        //2.4- Player movement attributes
        public static int xHeadAxisOffset = 0x4;
        public static int yHeadAxisOffset = 0x8;
        public static int zHeadAxisOffset = 0x0C;
        public static int xAxisOffset = 0x2C;
        public static int yAxisOffset = 0x30;
        public static int zAxisOffset = 0x28;
        public static int rotationOffset = 0x34;
        public static int viewDirectionOffset = 0x38;

    }
}
