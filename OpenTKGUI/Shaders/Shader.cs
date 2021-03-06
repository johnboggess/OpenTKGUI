﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Reflection;

namespace OpenTKGUI.Shaders
{
    internal class Shader
    {
        public static Shader DefaultShader;
        public static Shader ColoredShader;
        public static Shader FontShader;
        public static Shader TexturedShader;

        public string VertexShaderName { get { return _vertexShaderName; } }
        public string FragmentShaderName { get { return _fragmentShaderName; } }

        int _handle;
        public string _vertexShaderName;
        public string _fragmentShaderName;

        private bool _disposed = false;

        public static void LoadShaders()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            Stream vertex = assembly.GetManifestResourceStream("OpenTKGUI.Shaders.OpenTKGUIDefault.vert");
            Stream frag = assembly.GetManifestResourceStream("OpenTKGUI.Shaders.OpenTKGUIDefault.frag");
            DefaultShader = _loadShader(vertex, "DefaultVertex", frag, "DefaultFrag");

            vertex = assembly.GetManifestResourceStream("OpenTKGUI.Shaders.OpenTKGUIColored.vert");
            frag = assembly.GetManifestResourceStream("OpenTKGUI.Shaders.OpenTKGUIDefault.frag");
            ColoredShader = _loadShader(vertex, "ColoredVertex", frag, "DefaultFrag");

            vertex = assembly.GetManifestResourceStream("OpenTKGUI.Shaders.OpenTKGUIFont.vert");
            frag = assembly.GetManifestResourceStream("OpenTKGUI.Shaders.OpenTKGUIFont.frag");
            FontShader = _loadShader(vertex, "FontVertex", frag, "FontFrag");

            vertex = assembly.GetManifestResourceStream("OpenTKGUI.Shaders.OpenTKGUITextured.vert");
            frag = assembly.GetManifestResourceStream("OpenTKGUI.Shaders.OpenTKGUITextured.frag");
            TexturedShader = _loadShader(vertex, "TexturedVertex", frag, "TexturedFrag");

        }

        private static Shader _loadShader(Stream vertex, string vertexName, Stream frag, string fragName)
        {
            StreamReader reader = new StreamReader(vertex);
            string vertexSource = reader.ReadToEnd();

            reader = new StreamReader(frag);
            string fragSource = reader.ReadToEnd();

            return new Shader(vertexSource, vertexName, fragSource, fragName);
        }

        public Shader(string vertexPath, string fragmentPath)
        {
            string vertexSource = File.ReadAllText(vertexPath);
            string vertexShaderName = vertexPath.Substring(vertexPath.LastIndexOf("/") + 1, vertexPath.Length - vertexPath.LastIndexOf("/") - 1);

            string fragSource = File.ReadAllText(fragmentPath);
            string fragmentShaderName = fragmentPath.Substring(fragmentPath.LastIndexOf("/") + 1, fragmentPath.Length - fragmentPath.LastIndexOf("/") - 1);

            _setUp(vertexSource, vertexShaderName, fragSource, fragmentShaderName);
        }

        public Shader(string vertexSource, string vertexName, string fragSource, string fragName)
        {
            _setUp(vertexSource, vertexName, fragSource, fragName);
        }

        public virtual void Use(params object[] values)
        {
            _Use();
        }

        public void Dispose()
        {
            _disposed = true;
            GL.DeleteProgram(_handle);
        }

        /// <summary>
        /// Set a uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform. Must be a double, float, int, or uint.</param>
        public void SetUniform1(string name, object value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            if (value is double)
                GL.Uniform1(location, (double)value);
            if (value is float)
                GL.Uniform1(location, (float)value);
            if (value is int)
                GL.Uniform1(location, (int)value);
            if (value is uint)
                GL.Uniform1(location, (uint)value);
            if (value is float[])
                GL.Uniform1(location, ((float[])value).Length, (float[])value);
        }

        /// <summary>
        /// Set a vector2 uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform.</param>
        public void SetUniform2(string name, Vector2 value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            GL.Uniform2(location, value);
        }

        /// <summary>
        /// Set a vector3 uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform.</param>
        public void SetUniform3(string name, Vector3 value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            GL.Uniform3(location, value);
        }

        /// <summary>
        /// Set a vector4 uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform.</param>
        public void SetUniform4(string name, Vector4 value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            GL.Uniform4(location, value);
        }

        /// <summary>
        /// Set a quaternion uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform.</param>
        public void SetUniform4(string name, Quaternion value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            GL.Uniform4(location, value);
        }

        /// <summary>
        /// Set a Color4 uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform.</param>
        public void SetUniform4(string name, Color4 value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            GL.Uniform4(location, value);
        }

        /// <summary>
        /// Set a quaternion uniform.
        /// </summary>
        /// <param name="name">The name of the uniform.</param>
        /// <param name="value">The value of a uniform. Must be Matrix2, Matrix3, Matrix4</param>
        public void SetUniformMatrix(string name, bool transpose, object value)
        {
            int location = GL.GetUniformLocation(_handle, name);
            GL.UseProgram(_handle);
            if (value is Matrix2)
            {
                Matrix2 matrix = (Matrix2)value;
                GL.UniformMatrix2(location, transpose, ref matrix);
            }
            else if (value is Matrix3)
            {
                Matrix3 matrix = (Matrix3)value;
                GL.UniformMatrix3(location, transpose, ref matrix);
            }
            else if (value is Matrix4)
            {
                Matrix4 matrix = (Matrix4)value;
                GL.UniformMatrix4(location, transpose, ref matrix);
            }
        }

        internal void _Use()
        {
            GL.UseProgram(_handle);
        }

        private void _setUp(string vertexSource, string vertexName, string fragSource, string fragName)
        {
            _vertexShaderName = vertexName;
            _fragmentShaderName = fragName;

            int vertexHandle;
            int fragHandle;

            vertexHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexHandle, vertexSource);

            fragHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragHandle, fragSource);

            GL.CompileShader(vertexHandle);
            string log = GL.GetShaderInfoLog(vertexHandle);
            if (log != System.String.Empty) { System.Console.WriteLine("Vertex Shader:\n" + log); }

            GL.CompileShader(fragHandle);
            log = GL.GetShaderInfoLog(fragHandle);
            if (log != System.String.Empty) { System.Console.WriteLine("Fragment Shader:\n" + log); }

            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vertexHandle);
            GL.AttachShader(_handle, fragHandle);
            GL.LinkProgram(_handle);

            GL.DetachShader(_handle, vertexHandle);
            GL.DetachShader(_handle, fragHandle);
            GL.DeleteShader(vertexHandle);
            GL.DeleteShader(fragHandle);
        }
    }
}
