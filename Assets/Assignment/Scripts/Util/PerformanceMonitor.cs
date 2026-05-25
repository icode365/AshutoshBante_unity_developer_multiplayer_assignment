using UnityEngine;
using Unity.Profiling;

public class BuildPerformanceMonitor : MonoBehaviour
{
    private float deltaTime;
    private int ping;
    private string pingText = "Ping: N/A";

    // Profiler counters available in runtime builds
    private ProfilerRecorder vertexRecorder;
    private ProfilerRecorder drawCallRecorder;

    private void OnEnable()
    {
        // Bind recorders to system rendering pipelines
        vertexRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");
        drawCallRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
    }

    private void OnDisable()
    {
        // Clean up recorders to prevent memory leaks
        vertexRecorder.Dispose();
        drawCallRecorder.Dispose();
    }

    private void Start()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            InvokeRepeating(nameof(UpdatePing), 0.5f, 2.0f);
        }
    }

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void UpdatePing()
    {
        System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
        p.PingCompleted += (s, e) => {
            if (e.Reply != null && e.Reply.Status == System.Net.NetworkInformation.IPStatus.Success)
            {
                ping = (int)e.Reply.RoundtripTime;
                pingText = $"Ping: {ping} ms";
            }
        };
        try { p.SendAsync("8.8.8.8", 1000); }
        catch { pingText = "Ping: Error"; }
    }

    private void OnGUI()
    {
        float fps = 1.0f / deltaTime;
        string fpsText = $"FPS: {Mathf.RoundToInt(fps)}";

        // Read dynamic runtime counts safely
        long vertices = vertexRecorder.Valid ? vertexRecorder.LastValue : 0;
        long drawCalls = drawCallRecorder.Valid ? drawCallRecorder.LastValue : 0;

        string verticesText = vertices > 0 ? FormatNumber(vertices) : "Unsupported";
        string drawCallsText = drawCalls > 0 ? drawCalls.ToString() : "Unsupported";

        GUI.Box(new Rect(10, 10, 500, 200), "");
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.normal.textColor = Color.white;
        style.fontSize = 35;
        style.padding = new RectOffset(15, 0, 12, 0);

        string display = $"{fpsText}\n{pingText}\nVertices: {verticesText}\nDraw Calls: {drawCallsText}";
        GUI.Label(new Rect(10, 10, 200, 85), display, style);
    }

    private string FormatNumber(long num)
    {
        if (num >= 1000000) return (num / 1000000f).ToString("F1") + "M";
        if (num >= 1000) return (num / 1000f).ToString("F1") + "K";
        return num.ToString();
    }
}