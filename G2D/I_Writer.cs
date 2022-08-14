using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using dgtk.OpenGL;
using dgtk.Math;
using dge.GLSL;

namespace dge.G2D
{    
    /// <summary>
	/// Class used for writing Text on screen with OpenGL.
	/// It can contain different fonts with which to write the desired text.
	/// The fonts must be in GLFont format (* .glf).
	/// </summary>
    interface I_Writer
    {
        /// <sumary>
        /// Method use to define de 4x4 Matrix of View to use in Draw Methods.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="mat">Matrix 4x4 with de projectión information.</param>
        void DefineViewMatrix(dgtk.Math.Mat4 mat);

        /// <sumary>
        /// Method use to turn On or Turn Off Antialiasing in text.
        /// </sumary>
        /// <remarks>Turn On or Turn Off Antialiasing in text</remarks>
        /// <param name="bool">Turn On?</param>
        void AA_OnOff(bool SetOn);

        /// <sumary>
        /// Method use to define de 4x4 Matrix of Perspective to use in Draw Methods.
        /// </sumary>
        /// <remarks>Define Transparent color. Green (0, 255, 0) by default.</remarks>
        /// <param name="mat">Matrix 4x4 with de perspective information.</param>
        void DefinePerspectiveMatrix(float x, float y, float with, float height, bool invert_y);

        void DefinePerspectiveMatrix(dgtk.Math.Mat4 m4);

		void WriteCharGL(uint tboId, dgtk.Graphics.Color4 color, float posx, float posy, float f_width, float f_height, float f_x0, float f_y0, float f_x1, float f_y1);
		dgtk.Math.Mat4 M4P { get; }
    }
}