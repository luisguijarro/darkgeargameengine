using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

using dge.G2D;

using dgtk.OpenGL;

namespace dge.G2D
{
    public static partial class Tools
    {
        public static glFont LoadGLFont(string filepath)
        {
            FileStream fs  = File.Open(filepath, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            byte[] FormatHeader = new byte[] {79, 71, 76, 70};
            byte[] FileHeaderID = br.ReadBytes(4);
            for (int i=0;i<4;i++)
            {
                if (FileHeaderID[i] != FormatHeader[i])
                {
                    throw new Exception("glFontLoader error: The file " + filepath + " is not a valid OpenGL Font file.");
                }
            }

            int namelong = br.ReadByte(); // obtenemos la longitud de chars del nombre de la fuente.
            string FontName = new string(br.ReadChars(namelong)); // Obtenemos el nombre de la fuente.
            float maxSize = br.ReadSingle(); // Leemos 4 Bytes del float que contiene el tamaño máximo de fuente.
            float maxBorder = br.ReadSingle(); // Leemos 4 Bytes del float que contiene el grosor del borde de la fuente.
            int CharNumber = br.ReadInt32(); // Leemos 4 bytes correspondientes al número de caracteres incluidos.
            char[] chars = br.ReadChars(CharNumber); // Leemos los caracteres incluidos.
            glCharacter[] glChars = new glCharacter[chars.Length];
            for (int i=0;i<chars.Length;i++) // Leemos Datos de cada Char.
            {
                char chr = br.ReadChar(); // Leemos el caracter
                if (chr != chars[i])
                {
                    throw new Exception("glFontLoader error: The Order of characters is not correct");
                }
                float chr_width = br.ReadSingle(); // Leemos el ancho del caracter.
                float[] chr_Texcoords = new float[4];
                for (int c=0;c<4;c++)
                {
                    chr_Texcoords[c] = br.ReadSingle(); // Obtenemos las cooredanas de textura del caracter.
                }
                // ADD CHARACTER TO Array:
                glChars[i] = new glCharacter(chr, chr_Texcoords[0], chr_Texcoords[0], chr_Texcoords[0], chr_Texcoords[0], chr_width);
            }
            // TOCA LEER LOS BITMAPS


            glFont ret = new glFont();

            return ret;
        }

        public static void SaveGLFont(glFont font, string outFilePath)
        {
            FileStream fs  = File.Open(outFilePath, FileMode.Open);
            BinaryWriter bw = new BinaryWriter(fs);
            byte[] FileHeaderID = new byte[] {79, 71, 76, 70}; // Los primeros 4 bytes serán 4 caracteres en ASCII que definirán el formato (OGLF) (79, 71, 76, 70).
            bw.Write(FileHeaderID); // Escribimos ID de Formato.
            byte NameLong = (byte)font.Name.Length; // The long of Font Name in number of chars.
            bw.Write(NameLong); // Escribimos longitud del nombre de la fuente
            bw.Write(font.s_Name.ToCharArray()); // Escribimos nombre de la fuente
            bw.Write(font.f_MaxFontSize); // Escribimos 4 bytes correspondientes al Maximo tamaño Original de la fuente.
            bw.Write(font.f_borderWidth); // Escribimos 4 bytes correspondientes al grosor del borde de letra.
            bw.Write(font.d_characters.Count); // Escribimos 4 bytes del número de Caracteres incluidos en la fuente.
            char[] chars = new char[font.d_characters.Count];
            font.d_characters.Keys.CopyTo(chars, 0);
            bw.Write(chars); // Escribimos lista de caracteres incluidos.
            for (int i=0;i<chars.Length;i++) // Controlamos el orden de escritura de los caracteres.
            {
                char key = chars[i];
                bw.Write(key); // Escribimos char
                bw.Write(font.d_characters[key].f_ancho); // Escribimos el ancho del caracter.
                bw.Write(font.d_characters[key].f_x0); // Escribimos Coordenada X0 correspondiente a la Textura.
                bw.Write(font.d_characters[key].f_y0); // Escribimos Coordenada Y0 correspondiente a la Textura.
                bw.Write(font.d_characters[key].f_x1); // Escribimos Coordenada X1 correspondiente a la Textura.
                bw.Write(font.d_characters[key].f_y1); // Escribimos Coordenada Y1 correspondiente a la Textura.
            }
            bw.Write(23); // 0001 0111 - 23 -  Fin del bloque de transmisión.
            
            // TOCA ESCRIBIR LOS BITMAPS.
            bw.Write(font.TBO_Scan0.Width); // Escribimos 4 bytes de tamaño horizontal de la textura.
            bw.Write(font.TBO_Scan0.Height); // Escribimos 4 bytes de tamaño vertical de la textura.
            IntPtr Scan0 = IntPtr.Zero;
            GL.glGetTexImage(TextureTarget.GL_TEXTURE_2D, 0, PixelFormat.GL_BGRA, PixelType.GL_UNSIGNED_BYTE, Scan0);
            byte[] Scan0bytes = new byte[(font.TBO_Scan0.Width * font.TBO_Scan0.Height) * 2 ];

            //COMPRESION SCAN0
            Stream streamScan0 = new MemoryStream();
            using (DeflateStream dfs = new DeflateStream(streamScan0, CompressionMode.Compress))
            {
                dfs.Write(Scan0bytes, 0, Scan0bytes.Length); //Escribimos los bytes y lo comprimimos.
            }
            byte[] bytescompresScan0 = new byte[streamScan0.Length];
            streamScan0.Read(bytescompresScan0, 0, bytescompresScan0.Length);
            //bw.Write(Scan0bytes);
            
            bw.Write(23); // 0001 0111 - 23 -  Fin del bloque de transmisión.

            bw.Write(font.TBO_Scan1.Width); // Escribimos 4 bytes de tamaño horizontal de la textura.
            bw.Write(font.TBO_Scan1.Height); // Escribimos 4 bytes de tamaño vertical de la textura.
            IntPtr Scan1 = IntPtr.Zero;
            GL.glGetTexImage(TextureTarget.GL_TEXTURE_2D, 0, PixelFormat.GL_RGBA, PixelType.GL_UNSIGNED_BYTE, Scan1);
            byte[] Scan1bytes = new byte[(font.TBO_Scan1.Width * font.TBO_Scan1.Height) * 2 ];
            
            //COMPRESION SCAN1
            Stream streamScan1 = new MemoryStream();
            using (DeflateStream dfs = new DeflateStream(streamScan1, CompressionMode.Compress))
            {
                dfs.Write(Scan0bytes, 0, Scan0bytes.Length); //Escribimos los bytes y lo comprimimos.
            }
            byte[] bytescompresScan1 = new byte[streamScan1.Length];
            streamScan1.Read(bytescompresScan1, 0, bytescompresScan1.Length);
            //bw.Write(Scan1bytes);
            
            bw.Write(23); // 0001 0111 - 23 -  Fin del bloque de transmisión.
        }

        private static void WriteBitmapsBytes(BinaryWriter bw, IntPtr Scan)
        {
            
        } 

        private static IntPtr ReadBitmapsBytes(BinaryReader bw)
        {
            return IntPtr.Zero;
        } 
    }
}