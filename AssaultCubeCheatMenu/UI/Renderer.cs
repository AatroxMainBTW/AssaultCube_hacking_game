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

        // Variables for Join Server Popup
        private string joinServerIp = "";
        private string joinServerPort = "5000"; // Default port
        private bool isJoinPopupOpen = false;

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
            if (ImGui.Button("Join Server"))
            {
                isJoinPopupOpen = true; // Open the Join Server popup
            }

            ImGui.SameLine();
            if (ImGui.Button("Refresh Servers"))
            {
                RefreshServerList();
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
                    ImGui.Text(server.Name);

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

            // Join Server Popup
            if (isJoinPopupOpen)
            {
                ImGui.OpenPopup("Join Server");

                if (ImGui.BeginPopup("Join Server"))
                {
                    ImGui.Text("Enter server IP and Port:");

                    ImGui.InputText("IP Address", ref joinServerIp, 128);
                    ImGui.InputText("Port", ref joinServerPort, 10);

                    if (ImGui.Button("Connect"))
                    {
                        JoinServer(joinServerIp, joinServerPort);
                        ImGui.CloseCurrentPopup();
                    }

                    ImGui.SameLine();

                    if (ImGui.Button("Cancel"))
                    {
                        isJoinPopupOpen = false;
                        ImGui.CloseCurrentPopup();
                    }

                    ImGui.EndPopup();
                }
            }
        }

        private void CreateServer()
        {
            try
            {
                server.OnClientMessage += HandleClientMessage;
                server.Start(5000); // Port number
                chatMessages.Add($"Server '{server.Name}' started.");
                connectionErrorMessage = "";
                server.Players.Add(currentUser);
                server.Name = newServerName != "" ? newServerName : "Test Server"; // Name based on input or default
                servers.Add(server);

                // Notify other clients that a new server has been created
                // In a real system, this could involve broadcasting to other clients (like using a UDP server or messaging system)
            }
            catch (Exception ex)
            {
                connectionErrorMessage = $"Error starting server: {ex.Message}";
            }
        }

        private void JoinServer(string ipAddress, string port)
        {
            try
            {
                client.OnMessageReceived += HandleServerMessage;
                int parsedPort = int.Parse(port);
                client.Connect(ipAddress, parsedPort); // Connect using provided IP and port
                chatMessages.Add($"You joined the server: {ipAddress}:{port}");
                connectionErrorMessage = ""; // Clear error message
                ImGui.CloseCurrentPopup();
            }
            catch (Exception ex)
            {
                connectionErrorMessage = $"Failed to join server: {ex.Message}";
            }
        }

        private void RefreshServerList()
        { 
            int a = server._clients.Count();
            Console.WriteLine(a);
            /*
            try
            {
                // Check if the server already exists in the list, and avoid adding duplicates
                if (!servers.Contains(server))
                {
                    servers.Add(server);
                    chatMessages.Add("Server list refreshed. New servers added.");
                }
                else
                {
                    chatMessages.Add("Server list refreshed. No new servers.");
                }
                connectionErrorMessage = ""; // Clear error message
            }
            catch (Exception ex)
            {
                connectionErrorMessage = $"Error refreshing server list: {ex.Message}";
            }*/
        }

        private void SendChatMessage()
        {
            if (!string.IsNullOrWhiteSpace(textFieldChat))
            {
                // Add the message with the current user's name
                chatMessages.Add($"{currentUser}: {textFieldChat}");
                client.SendMessage(textFieldChat);
                textFieldChat = "";
            }
        }

        private void HandleServerMessage(string message)
        {
            // Handle server messages and append them to the chat
            chatMessages.Add($"Server: {message}");
        }

        private void HandleClientMessage(string message)
        {
            // Handle client messages and append them to the chat
            chatMessages.Add($"{currentUser}+{""}: {message}");
        }
    }
}
