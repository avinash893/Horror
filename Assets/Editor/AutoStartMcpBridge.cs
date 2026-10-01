using UnityEditor;
using UnityEngine;
using MCPForUnity.Editor.Services;

[InitializeOnLoad]
public static class AutoStartMcpBridge
{
    static AutoStartMcpBridge()
    {
        EditorApplication.delayCall += async () =>
        {
            try
            {
                var service = new BridgeControlService();
                if (!service.IsRunning)
                {
                    Debug.Log("[MCP AutoStart] Starting MCP Bridge...");
                    bool started = await service.StartAsync();
                    Debug.Log($"[MCP AutoStart] Bridge started: {started}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[MCP AutoStart] Could not start bridge: {ex.Message}");
            }
        };
    }
}
