using AssaultCubeCheatMenu.Networking;
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
        public string textFieldChat = "";
        private static int selectedServerIndex = -1;
        private static string newServerName = "";

        public string currentUser { get; set; }
        public string cheatVersion { get; set; }

        private static List<Server> servers = new List<Server>();

        private Client client = new Client();
        private Server server = new Server();
        private List<string> chatMessages = new List<string>();
        private string connectionErrorMessage = "";

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
            ImGui.Separator();

            // Server List UI
            ImGui.Begin("Server List");
            if (ImGui.Button("Create Server"))
            {
                CreateServer();
            }

            ImGui.SameLine();
            if (ImGui.Button("Join Server") && selectedServerIndex >= 0)
            {
                JoinServer(servers[selectedServerIndex].Name);
            }

            // Server Table
            if (ImGui.BeginTable("ServerTable", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
            {
                ImGui.TableSetupColumn("Server Name");
                ImGui.TableSetupColumn("Ping (ms)");
                ImGui.TableSetupColumn("Players");
                ImGui.TableHeadersRow();

                for (int i = 0; i < servers.Count; i++)
                {
                    Server server = servers[i];
                    ImGui.TableNextRow();

                    ImGui.TableSetColumnIndex(0);
                    if (ImGui.Selectable(server.Name, selectedServerIndex == i))
                    {
                        selectedServerIndex = i;
                    }

                    ImGui.TableSetColumnIndex(1);
                    ImGui.Text($"{server.PingMs}");

                    ImGui.TableSetColumnIndex(2);
                    ImGui.Text($"{server.Players.Count}");
                }

                ImGui.EndTable();
            }

            // Display Players
            if (selectedServerIndex >= 0)
            {
                Server selectedServer = servers[selectedServerIndex];
                ImGui.Text($"Players in {selectedServer.Name}:");
                foreach (string player in selectedServer.Players)
                {
                    ImGui.BulletText(player);
                }
            }

            ImGui.Separator();

            // Chat Input
            if (!string.IsNullOrWhiteSpace(connectionErrorMessage))
            {
                ImGui.TextColored(new Vector4(1, 0, 0, 1), connectionErrorMessage); // Red error message
            }

            ImGui.InputText("", ref textFieldChat, 256);
            ImGui.SameLine();
            if (ImGui.Button("Send"))
            {
                SendChatMessage();
            }

            // Chat Messages
            ImGui.Separator();
            ImGui.Text("Chat Messages:");
            foreach (string message in chatMessages)
            {
                ImGui.TextWrapped(message);
            }

            ImGui.End();
        }

        private void CreateServer()
        {
            try
            {
                server.OnClientMessage += HandleClientMessage;
                server.Start(5000); // Port number
                chatMessages.Add("Server started.");
                connectionErrorMessage = ""; // Clear error message
                server.Players.Add(currentUser);

                server.Name = "Fouzi Test";
                servers.Add(server);
            }
            catch (Exception ex)
            {
                connectionErrorMessage = $"Error starting server: {ex.Message}";
            }
        }

        private void JoinServer(string serverName)
        {
            try
            {
                client.OnMessageReceived += HandleServerMessage;
                client.Connect("26.218.231.178", 5000); // Replace with actual server IP and port
                chatMessages.Add($"Joined server: {serverName}");
                connectionErrorMessage = ""; // Clear error message
            }
            catch (Exception ex)
            {
                connectionErrorMessage = $"Failed to join server: {ex.Message}";
            }
        }

        private void SendChatMessage()
        {
            if (!string.IsNullOrWhiteSpace(textFieldChat))
            {
                client.SendMessage(textFieldChat);
                chatMessages.Add($"You: {textFieldChat}");
                textFieldChat = "";
            }
        }

        private void HandleServerMessage(string message)
        {
            chatMessages.Add($"Server: {message}");
        }

        private void HandleClientMessage(string message)
        {
            chatMessages.Add($"Client: {message}");
        }
    }
}
