using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL4;
namespace OpenTKGUI.Buffers
{
    /// <summary>
    /// GPU array representing the vertices for a mesh, attributes of each vertex, and the order the vertices should be rendered in
    /// </summary>
    public class VertexArray : IDisposable
    {
        public static readonly VertexArray TextSquare = CreateTextSquare();

        public bool Disposed { get { return _disposed; } }

        bool _disposed = false;
        int _handle;

        VertexBuffer _vbo;
        ElementBuffer _ebo;

        public VertexArray()
        {
            _handle = GL.GenVertexArray();
        }

        public void Setup(VertexBuffer vbo, ElementBuffer ebo)
        {
            _ebo = ebo;
            _vbo = vbo;
            Bind();

            vbo.Bind();

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 5, 0);
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, sizeof(float) * 5, 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            ebo.Bind();

            UnBind();

        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void UnBind()
        {
            GL.BindVertexArray(0);
        }

        public void Draw()
        {
            Bind();
            GL.DrawElements(PrimitiveType.Triangles, _ebo.Indices.Length, DrawElementsType.UnsignedInt, 0);
            UnBind();
        }

        public void Dispose()
        {
            _disposed = true;
            GL.DeleteBuffer(_handle);
        }

        public static VertexArray CreateTextSquare()
        {
            float[] vertices = new float[4];

            vertices = new float[]
            {
                1,1,0,    1,1,
                0,1,0,    0,1,
                0,0,0,    0,0,
                1,0,0,    1,0
            };

            uint[] indices = new uint[6]
            {
                0,1,3,
                2,3,1
            };

            VertexBuffer vb = new VertexBuffer(vertices);
            ElementBuffer eb = new ElementBuffer();
            eb.SetIndices(indices);

            VertexArray va = new VertexArray();
            va.Setup(vb, eb);

            return va;
        }
    }
}
