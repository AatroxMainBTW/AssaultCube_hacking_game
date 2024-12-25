// See https://aka.ms/new-console-template for more information
using AssaultCubeCheatMenu.Models;
using AssaultCubeCheatMenu.UI;
using AssaultCubeCheatMenu.Utils;
using Swed32;


//Init
Swed swed = new Swed("ac_client");

//get module base
IntPtr client = swed.GetModuleBase("ac_client.exe");


//init renderer later

// entities
Entity localPlayer = new Entity();

List<Entity> entities = new List<Entity>();

Console.WriteLine("Starting cheat menu...");

while (true)
{
    IntPtr entityList = swed.ReadPointer(client, Offsets.entityListAccess);
    localPlayer.Address = swed.ReadPointer(entityList, 0x4);
    localPlayer.Position = new System.Numerics.Vector3
    {
        X = swed.ReadFloat(entityList, Offsets.xAxisOffset),
        Y = swed.ReadFloat(entityList, Offsets.yAxisOffset),
        Z = swed.ReadFloat(entityList, Offsets.zAxisOffset),
    };
    //localPlayer.Team = swed.ReadInt(localPlayer.Address, Offsets.team);
    localPlayer.Health = swed.ReadFloat(entityList, Offsets.healthOffset);
    //localPlayer.Name = swed.Read(entityList, Offsets.healthOffset);
    //AMMO Later

    int a = 0;
    //entities.Clear();
}