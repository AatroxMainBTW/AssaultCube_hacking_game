// See https://aka.ms/new-console-template for more information
using AssaultCubeCheatMenu.Models;
using AssaultCubeCheatMenu.UI;
using AssaultCubeCheatMenu.Utils;
using Swed32;
using System;
using System.Numerics;
using System.Runtime.InteropServices;


//Init
Swed swed = new Swed("ac_client");

//get module base
IntPtr client = swed.GetModuleBase("ac_client.exe");


//init renderer
Renderer renderer = new Renderer();
Thread rendererThread = new Thread(new ThreadStart(renderer.Start().Wait));
rendererThread.Start();

// entities
Entity localPlayer = new Entity();

List<Entity> entities = new List<Entity>();

// hotkey
//Aimbot
const int HOTKEY_AIM = 0x05;
//Godmode
const int HOTKEY_GOD = 0x06;

Console.WriteLine("Starting cheat menu...");
while (true)
{
    IntPtr entityList = swed.ReadPointer(client, Offsets.entityListAccess);
    IntPtr entityListAccess = swed.ReadPointer(entityList, Offsets.entityAccess);
    IntPtr playerClient = swed.ReadPointer(client, Offsets.baseAddressPlayer);
    localPlayer.Address = swed.ReadPointer(playerClient, Offsets.playerAccess);
    localPlayer.Position = new Vector3
    {
        X = swed.ReadFloat(playerClient, Offsets.xAxisOffset),
        Y = swed.ReadFloat(playerClient, Offsets.yAxisOffset),
        Z = swed.ReadFloat(playerClient, Offsets.zAxisOffset),
    };
    localPlayer.HeadPosition = new Vector3
    {
        X = swed.ReadFloat(playerClient, Offsets.xHeadAxisOffset),
        Y = swed.ReadFloat(playerClient, Offsets.yHeadAxisOffset),
        Z = swed.ReadFloat(playerClient, Offsets.zHeadAxisOffset),
    };
    localPlayer.Team = swed.ReadInt(playerClient, Offsets.teamOffset);
    localPlayer.TeamName = Helpers.TeamConverter(localPlayer.Team);
    localPlayer.Health = swed.ReadInt(playerClient, Offsets.healthOffset);
    localPlayer.Name = Helpers.ReadString(playerClient, Offsets.nameOffset, swed);
    renderer.currentUser = localPlayer.Name;
    //Check the build version later.
    renderer.cheatVersion = "V-" + "0.0.1";
    AmmoEntity playerAmmo = new AmmoEntity();
    playerAmmo.Mtp54Ammo = swed.ReadInt(playerClient, Offsets.ammoMTP57Offset);
    playerAmmo.Mk77Ammo = swed.ReadInt(playerClient, Offsets.ammoMK77Offset);
    playerAmmo.AD80Ammo = swed.ReadInt(playerClient, Offsets.ammoAD80Offset);
    playerAmmo.ADR10Ammo = swed.ReadInt(playerClient, Offsets.ammoARD10Offset);
    playerAmmo.TmpAmmo = swed.ReadInt(playerClient, Offsets.ammoTMPOffset);
    playerAmmo.HeAmmo = swed.ReadInt(playerClient, Offsets.ammoHEOffset);

    localPlayer.Ammos = playerAmmo;
    for (int i = 0; i < 32; i++)
    {
        IntPtr currentEntree = swed.ReadPointer(entityList, Offsets.entityAccess + (i * Offsets.StepOffset));
        if (currentEntree == IntPtr.Zero)
        {
            continue;
        }
        int health = swed.ReadInt(currentEntree, Offsets.healthOffset);
        int team = swed.ReadInt(currentEntree, Offsets.teamOffset);
        //To remove the weird entity outside the map "probably the spectator"
        if (team == 1 || team == 0)
        {
            if (health > 1 && health < 101 && localPlayer.Team != team)
            {
                Entity entity = new Entity();
                entity.Address = currentEntree;
                entity.Health = health;
                entity.Name = Helpers.ReadString(currentEntree, Offsets.nameOffset, swed);
                entity.Team = team;
                entity.TeamName = Helpers.TeamConverter(entity.Team);
                entity.Position = new Vector3
                {
                    X = swed.ReadFloat(currentEntree, Offsets.xAxisOffset),
                    Y = swed.ReadFloat(currentEntree, Offsets.yAxisOffset),
                    Z = swed.ReadFloat(currentEntree, Offsets.zAxisOffset),
                };
                entity.HeadPosition = new Vector3
                {
                    X = swed.ReadFloat(currentEntree, Offsets.xHeadAxisOffset),
                    Y = swed.ReadFloat(currentEntree, Offsets.yHeadAxisOffset),
                    Z = swed.ReadFloat(currentEntree, Offsets.zHeadAxisOffset),
                };
                entity.Distance = Vector3.Distance(entity.Position, localPlayer.Position);
                AmmoEntity ammoEntity = new AmmoEntity();
                ammoEntity.Mtp54Ammo = swed.ReadInt(currentEntree, Offsets.ammoMTP57Offset);
                ammoEntity.Mk77Ammo = swed.ReadInt(currentEntree, Offsets.ammoMK77Offset);
                ammoEntity.AD80Ammo = swed.ReadInt(currentEntree, Offsets.ammoAD80Offset);
                ammoEntity.ADR10Ammo = swed.ReadInt(currentEntree, Offsets.ammoARD10Offset);
                ammoEntity.TmpAmmo = swed.ReadInt(currentEntree, Offsets.ammoTMPOffset);
                ammoEntity.HeAmmo = swed.ReadInt(currentEntree, Offsets.ammoHEOffset);

                entity.Ammos = ammoEntity;
                entities.Add(entity);
            }
        }
    }
    entities = entities.OrderBy(o => o.Distance).ToList();


    //region AIMBOT PART "DON'T WORK FOR NOW"
    if (GetAsyncKeyState(HOTKEY_AIM) < 0 && renderer.aimbotActive && entities.Count > 0)
    {
        renderer.enableAimingOnEnemy = true;

        //calculate the angles

        Vector2 calculatedAngles =
            Helpers.CalculateAngles(localPlayer.Position,
            entities[0].Position);
        Vector3 newAngles = new Vector3(calculatedAngles.Y, calculatedAngles.X, 0);
        Vector2 direction = new Vector2((float)Math.Cos(newAngles.X), (float)Math.Sin(newAngles.X));
        swed.WriteFloat(playerClient, Offsets.rotationOffset, calculatedAngles.X);
        swed.WriteFloat(playerClient, Offsets.viewDirectionOffset, calculatedAngles.Y);
        // Write rotation angles
        //swed.WriteVec(playerClient, Offsets.rotationOffset, newAngles);
    }
    else
    {
        renderer.enableAimingOnEnemy = false;
    }
    //endregion

    //region godmode
    if (renderer.godModeActive)
    {
        swed.WriteInt(playerClient, Offsets.healthOffset,999);
    }
    //endregion

    //region infinite ammo
    if (renderer.infiniteAmmoActive)
    {
        swed.WriteInt(playerClient, Offsets.ammoAD80Offset, 999);
        swed.WriteInt(playerClient, Offsets.ammoARD10Offset, 999);
        swed.WriteInt(playerClient, Offsets.ammoHEOffset, 999);
        swed.WriteInt(playerClient, Offsets.ammoMK77Offset, 999);
        swed.WriteInt(playerClient, Offsets.ammoMTP57Offset, 999);
        swed.WriteInt(playerClient, Offsets.ammoTMPOffset, 999);
        swed.WriteInt(playerClient, Offsets.ammoV19Offset, 999);
    }
    //endregion

    //region infinite armor
    if (renderer.infiniteArmorActive)
    {
        swed.WriteInt(playerClient, Offsets.armorOffset, 999);
    }
    //endregion
    entities.Clear();
}
[DllImport("user32.dll")]
static extern short GetAsyncKeyState(int vKey);