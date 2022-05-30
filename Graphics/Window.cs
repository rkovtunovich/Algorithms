using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Graphics;

class Window : GameWindow
{
    protected override void OnLoad(EventArgs e)
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
