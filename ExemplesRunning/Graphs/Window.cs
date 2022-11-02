using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ExemplesRunning.Graphs;

class Window : GameWindow
{
    public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
    }

    protected override void OnLoad()
    {
        GL.ClearColor(0.1f, 0.2f, 0.3f, 1f);

        Console.WriteLine(GL.GetString(StringName.Version));
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        GL.Clear(ClearBufferMask.ColorBufferBit);
        SwapBuffers();
    }
}
