using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

public class Game : GameWindow
{
  private int vertexBufferObject;

  private int programShaderHandle;

  private int vertexArrayObject;

  public Game(int width, int height, string title)
    : base(GameWindowSettings.Default,
    new NativeWindowSettings()
    {
      Size = (width, height),
      Title = title,
    })
  {
    this.CenterWindow();
  }

  protected override void OnLoad()
  {
    GL.ClearColor(0.3f, 0.7f, 0.4f, 1.0f);

    float[] vertices = {
      -0.6f,0.4f,0.0f,
      0.4f,0.5f,0.0f,
      0.0f,0.9f,0.0f,
    };

    vertexArrayObject = GL.GenVertexArray();
    GL.BindVertexArray(vertexArrayObject);

    vertexBufferObject = GL.GenBuffer();
    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
    GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

    GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

    GL.EnableVertexAttribArray(0);


    string vertexShaderCode =
    @"
      #version 330 core
      layout (location = 0) in vec3 aPosition;

      void main()      {
          gl_Position = vec4(aPosition.x,aPosition.y,aPosition.z , 1.0);
      }
    ";

    string fragmentShaderCode =
    @"
      #version 330 core

      out vec4 FragColor;
      void main()
      {
        FragColor = vec4(1.0f, 0.5f, 0.2f, 1.0f);
      }
    ";

    int vertexShader = GL.CreateShader(ShaderType.VertexShader);
    GL.ShaderSource(vertexShader, vertexShaderCode);
    GL.CompileShader(vertexShader);

    int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
    GL.ShaderSource(fragmentShader, fragmentShaderCode);
    GL.CompileShader(fragmentShader);

    programShaderHandle = GL.CreateProgram();
    GL.AttachShader(programShaderHandle, vertexShader);
    GL.AttachShader(programShaderHandle, fragmentShader);

    GL.LinkProgram(programShaderHandle);

    GL.DeleteShader(vertexShader);
    GL.DeleteShader(fragmentShader);

    base.OnLoad();

  }


  protected override void OnRenderFrame(FrameEventArgs args)
  {
    base.OnRenderFrame(args);
    GL.Clear(ClearBufferMask.ColorBufferBit);

    GL.UseProgram(programShaderHandle);
    GL.BindVertexArray(vertexArrayObject);


    GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    SwapBuffers();

  }

  protected override void OnUpdateFrame(FrameEventArgs args)
  {
    base.OnUpdateFrame(args);

  }

  protected override void OnResize(ResizeEventArgs e)
  {
    base.OnResize(e);
    GL.Viewport(0, 0, e.Width, e.Height);
  }

  protected override void OnUnload()
  {
    GL.DeleteVertexArray(vertexArrayObject);
    GL.DeleteBuffer(vertexBufferObject);
    GL.DeleteProgram(programShaderHandle);
    base.OnUnload();
  }

}