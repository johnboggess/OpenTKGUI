using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL4;
namespace OpenTKGUI.Buffers
{
    /// <summary>
    /// Represents a buffer of vertices on the GPU.
    /// </summary>
    public class VertexBuffer : GPUBuffer
    {
        /// <summary>
        /// The vertices stored in the buffer.
        /// </summary>
        public float[] Vertices { get { return _vertices; } }

        private float[] _vertices;

        /// <summary>
        /// Populates the GPU buffer with a set of vertices.
        /// </summary>
        /// <param name="vertices"></param>
        public VertexBuffer(float[] vertices) : base(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw)
        {
            _vertices = vertices;
            Bind();
            GL.BufferData(bufferTarget, vertices.Length * sizeof(float), _vertices, bufferUsageHint);
            UnBind();
        }
    }
}
