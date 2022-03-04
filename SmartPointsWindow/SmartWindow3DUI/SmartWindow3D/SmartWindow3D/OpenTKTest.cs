using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;

namespace SmartWindow3D
{
    public class gw:GameWindow
    {
        private double _time;
        private Vector2 StartPos;
        private Vector2 MovePos;
        private float rx;
        private float ry;
        private float rz;
        public float[] _vertices = 
        {
             0.5f,  0.5f, 0.0f, 0.1f,0.3f,0.2f,  // top right
             0.5f, -0.5f, 0.0f, 0.2f,0.1f,0.1f, // bottom right
            -0.5f, -0.5f, 0.0f, 0.1f,0.2f,0.1f, // bottom left
            -0.5f,  0.5f, 0.0f, 0.2f,0.1f,0.3f // top left
        };
        uint[] indices = {  // note that we start from 0!
            0, 1, 3,   // first triangle
            1, 2, 3    // second triangle
        };
        private int _VectorofBufferObject;
        private int _VectorofArrayObject;
        private int ElementBufferObject;
        public gw(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }
        private Shader _shader;private Camera camera;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            KeyboardState nowkeyboardStae = Keyboard.GetState();
            MouseState nowmouseStae = Mouse.GetState();
            if (nowkeyboardStae.IsKeyDown(Key.Escape))
            {
                Exit();
            }
            const float cameraSpeed = 1.5f;

            if (nowkeyboardStae.IsKeyDown(Key.W))
            {
                camera.Position += camera.Front * cameraSpeed * (float)e.Time; // Forward
            }
            if (nowkeyboardStae.IsKeyDown(Key.S))
            {
                camera.Position -= camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (nowkeyboardStae.IsKeyDown(Key.A))
            {
                camera.Position -= camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (nowkeyboardStae.IsKeyDown(Key.D))
            {
                camera.Position += camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (nowkeyboardStae.IsKeyDown(Key.Space))
            {
                camera.Position += camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (nowkeyboardStae.IsKeyDown(Key.ShiftLeft))
            {
                camera.Position -= camera.Up * cameraSpeed * (float)e.Time; // Down
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera = new Camera(Vector3.UnitZ * 15, (float)Width / Height);

            GL.ClearColor(0.004f, 0.302f, 0.404f, 1.0f);
            // GL.Enable(EnableCap.DepthTest);
            //codes

            _VectorofArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_VectorofArrayObject);

            _VectorofBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _VectorofBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            //ElementBufferObject = GL.GenBuffer();
            //GL.BindBuffer(BufferTarget.ElementArrayBuffer, ElementBufferObject);
            //GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

            _shader = new Shader("shader.vert", "shader.frag");
            _shader.Use();


            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 12);

        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            _time +=10.0* e.Time;
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _VectorofBufferObject);


            GL.BindVertexArray(_VectorofArrayObject);
            _shader.Use();
            //codes
            Matrix4 mat4 = camera.GetModelMatrix(rz,ry,rx);
            _shader.SetMatrix4("model", mat4);
            mat4 = camera.GetViewMatrix();
            _shader.SetMatrix4("view", mat4);
            mat4 = camera.GetProjectionMatrix();
            _shader.SetMatrix4("projection",mat4 );

            GL.DrawArrays(PrimitiveType.Points, 0, _vertices.Length/6);
            //GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            //

            SwapBuffers();

        }
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }
        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);
            GL.DeleteBuffer(_VectorofBufferObject);
            _shader.Dispose();
            base.OnUnload(e);
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            camera.Position += camera.Front * (float)e.Delta; // Backwards

        }
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            const float sensitivity = 0.1f;
            base.OnMouseMove(e);
            this.Title = e.Position.ToString();
            if (e.Mouse.LeftButton== ButtonState.Pressed&&!(e.Mouse.RightButton == ButtonState.Pressed))
            {
                MovePos= new Vector2(e.Position.X, e.Position.Y);
                rx += (MovePos.X - StartPos.X)*sensitivity;
                ry += (MovePos.Y - StartPos.Y)*sensitivity;
                //       Calculate the offset of the mouse position
                //       var deltaX = e.Position.X - StartPos.X;
                //       var deltaY = e.Position.Y - StartPos.Y;
                //       Apply the camera pitch and yaw(we clamp the pitch in the camera class)
                //       camera.Yaw += deltaX* sensitivity;
                //       camera.Pitch -= deltaY* sensitivity; // Reversed since y-coordinates range from bottom to top
            }
            if (!(e.Mouse.LeftButton == ButtonState.Pressed)&&e.Mouse.RightButton == ButtonState.Pressed)
            {
                MovePos = new Vector2(e.Position.X, e.Position.Y);
                camera.Position -= camera.Right * sensitivity *0.01f* (float)(MovePos.X - StartPos.X);
                camera.Position += camera.Up * sensitivity*0.01f * (float)(MovePos.Y - StartPos.Y);
            }
            if (e.Mouse.RightButton == ButtonState.Pressed&& e.Mouse.LeftButton == ButtonState.Pressed)
            {
                MovePos = new Vector2(e.Position.X, e.Position.Y);
                rz += (MovePos.X - StartPos.X) * sensitivity;

            }
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            StartPos =new Vector2(e.Position.X,e.Position.Y);
            if (e.Mouse.MiddleButton== ButtonState.Pressed)
            {
                rx = 0;ry = 0;rz = 0;
                camera.Position = Vector3.UnitZ * 15;
            }
        }
    }
    public class Shader
    {
        public readonly Dictionary<string, int> _uniformLocations;
        private readonly int Handle;
        public Shader(string vertPath, string fragPath)
        {
            // There are several different types of shaders, but the only two you need for basic rendering are the vertex and fragment shaders.
            // The vertex shader is responsible for moving around vertices, and uploading that data to the fragment shader.
            //   The vertex shader won't be too important here, but they'll be more important later.
            // The fragment shader is responsible for then converting the vertices to "fragments", which represent all the data OpenGL needs to draw a pixel.
            //   The fragment shader is what we'll be using the most here.

            // Load vertex shader and compile
            var shaderSource = File.ReadAllText(vertPath);

            // GL.CreateShader will create an empty shader (obviously). The ShaderType enum denotes which type of shader will be created.
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);

            // Now, bind the GLSL source code
            GL.ShaderSource(vertexShader, shaderSource);

            // And then compile
            CompileShader(vertexShader);

            // We do the same for the fragment shader.
            shaderSource = File.ReadAllText(fragPath);
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, shaderSource);
            CompileShader(fragmentShader);

            // These two shaders must then be merged into a shader program, which can then be used by OpenGL.
            // To do this, create a program...
            Handle = GL.CreateProgram();

            // Attach both shaders...
            GL.AttachShader(Handle, vertexShader);
            GL.AttachShader(Handle, fragmentShader);

            // And then link them together.
            LinkProgram(Handle);

            // When the shader program is linked, it no longer needs the individual shaders attached to it; the compiled code is copied into the shader program.
            // Detach them, and then delete them.
            GL.DetachShader(Handle, vertexShader);
            GL.DetachShader(Handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            // The shader is now ready to go, but first, we're going to cache all the shader uniform locations.
            // Querying this from the shader is very slow, so we do it once on initialization and reuse those values
            // later.

            // First, we have to get the number of active uniforms in the shader.
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

            // Next, allocate the dictionary to hold the locations.
            _uniformLocations = new Dictionary<string, int>();

            // Loop over all the uniforms,
            for (var i = 0; i < numberOfUniforms; i++)
            {
                // get the name of this uniform,
                var key = GL.GetActiveUniform(Handle, i, out _, out _);

                // get the location,
                var location = GL.GetUniformLocation(Handle, key);

                // and then add it to the dictionary.
                _uniformLocations.Add(key, location);
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
            
        }
        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);
            GL.UniformMatrix4(_uniformLocations[name], true, ref data);
        }
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(Handle);

                disposedValue = true;
            }
        }
        ~Shader()
        {
            GL.DeleteProgram(Handle);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private static void CompileShader(int shader)
        {
            // Try to compile the shader
            GL.CompileShader(shader);

            // Check for compilation errors
            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetShaderInfoLog(shader)` to get information about the error.
                var infoLog = GL.GetShaderInfoLog(shader);
                throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
            }
        }
        private static void LinkProgram(int program)
        {
            // We link the program
            GL.LinkProgram(program);

            // Check for linking errors
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                // We can use `GL.GetProgramInfoLog(program)` to get information about the error.
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }
        public int GetAttribLocation(string attribName)
        {
            return GL.GetAttribLocation(Handle, attribName);
        }

    }
}
