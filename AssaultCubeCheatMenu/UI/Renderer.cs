using ClickableTransparentOverlay;
using ImGuiNET;
using System;
using System.Collections.Generic;

namespace AssaultCubeCheatMenu.UI
{
    public class Renderer : Overlay
    {
        private bool aimbotActive = false;
        private bool godModeActive = false;
        private bool infiniteAmmo = false; // Added for infinite ammo

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

            // God Mode Checkbox
            ImGui.Checkbox("Activate God Mode", ref godModeActive);

            // Infinite Ammo Checkbox
            ImGui.Checkbox("Infinite Ammo", ref infiniteAmmo); 
            ImGui.Separator();

            ImGui.End();
        }
    }
}
