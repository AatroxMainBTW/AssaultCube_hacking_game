using ClickableTransparentOverlay;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace AssaultCubeCheatMenu.UI
{
    public class Renderer : Overlay
    {
        public bool aimbotActive = false;
        public bool godModeActive = false;
        public bool infiniteAmmoActive = false;
        public bool infiniteArmorActive = false;
        public bool enableAimingOnEnemy = false;

        public string currentUser {get; set;}
        public string cheatVersion { get; set; }

        protected override void Render()
        {
            ImGui.Begin("AssaultCube Cheat Menu");

            // Header
            ImGui.Text("AssaultCube Cheat Menu");
            ImGui.Separator();

            // Current user and version
            ImGui.Text($"User: {currentUser}");
            ImGui.Text($"Version: {cheatVersion}");
            ImGui.Separator();

            // Aimbot Checkbox
            ImGui.Checkbox("Activate Aimbot", ref aimbotActive);
            if (aimbotActive && enableAimingOnEnemy)
            {
                ImGui.TextColored(new Vector4(0, 1, 0, 1), "Aimbot: aiming");
            }
            else if (aimbotActive)
            {
                ImGui.TextColored(new Vector4(1, 0, 0, 1), "Aimbot: idle");
            }
            // God Mode Checkbox
            ImGui.Checkbox("Activate God Mode", ref godModeActive);

            // Infinite Ammo Checkbox
            ImGui.Checkbox("Infinite Ammo", ref infiniteAmmoActive); 
            ImGui.Checkbox("Infinite Armor", ref infiniteArmorActive);
            ImGui.End();
        }
    }
}
