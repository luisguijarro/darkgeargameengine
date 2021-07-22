using System;
using System.IO;
using System.IO.Compression;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using dge.G2D;

using dgtk.OpenGL;

namespace dge.G2D
{
    public static partial class Tools
    {
        public static dgFont LoadDGFont(string filepath)
        {
            if (File.Exists(filepath))
            {
                FileStream fs  = File.Open(filepath, FileMode.Open); // Abrimos fichero
                dgFont ret = LoadDGFont(fs, filepath);
                fs.Close();
                return ret;
            }
            throw new Exception("La fuente no carga");
        }
        
        internal static dgFont LoadDGFont(Stream filestream, string filepath)
        {
            //FileStream fs  = File.Open(filepath, FileMode.Open); // Abrimos fichero
            BinaryReader br = new BinaryReader(filestream); // Asignamos lector Binario.
            byte[] FormatHeader = new byte[] {68, 71, 69, 70}; // Definimos cabecera correcta.
            byte[] FileHeaderID = br.ReadBytes(4); // Leemos los primeros 4 Bytes.
            for (int i=0;i<4;i++)
            {
                if (FileHeaderID[i] != FormatHeader[i]) // Si alguno d elos bytes no se corresponde falla.
                {
                    throw new Exception("DGFontLoader error: The file " + filepath + " is not a valid Dark Gear Font file.");
                }
            }

            int namelong = br.ReadByte(); // obtenemos la longitud de Characters del nombre de la font.
            string FontName = new string(br.ReadChars(namelong)); // Obtenemos el nombre de la font.
            float maxSize = br.ReadSingle(); // Leemos 4 Bytes del float que contiene el tamaño máximo de font.
            float BorderWidth = br.ReadSingle(); // Leemos 4 Bytes del float que contiene el grosor del borde de la font.
            float spaceWidth = br.ReadSingle(); // Leemos 4 Bytes del float que contiene el grosor del character spacio de la font.
            int CharNumber = br.ReadInt32(); // Leemos 4 bytes correspondientes al número de caracteres incluidos.
            char[] Characters = br.ReadChars(CharNumber); // Leemos los caracteres incluidos.
            dgCharacter[] dgCharacters = new dgCharacter[Characters.Length];
            for (int i=0;i<Characters.Length;i++) // Leemos Datos de cada Char.
            {
                char chr = br.ReadChar(); // Leemos el caracter
                if (chr != Characters[i])
                {
                    throw new Exception("DGFontLoader error: The Order of characters is not correct");
                }
                float chr_width = br.ReadSingle(); // Leemos el ancho del caracter.
                float[] chr_Texcoords = new float[4];
                for (int c=0;c<4;c++)
                {
                    chr_Texcoords[c] = br.ReadSingle(); // Obtenemos las cooredanas de textura del caracter.
                }
                ushort s_width = br.ReadUInt16(); // Leemos anchura para superficie de dibujado en pixeles.
                ushort s_height = br.ReadUInt16(); // Leemos altura para superficie de dibujado en pixeles.
                // ADD CHARACTER TO Array:
                dgCharacters[i] = new dgCharacter(chr, chr_Texcoords[0], chr_Texcoords[1], chr_Texcoords[2], chr_Texcoords[3], s_width, s_height, chr_width);
            }

            if (br.ReadByte() != (byte)23) // Si no es fin de Bloque... 0001 0111 - 23
            {
                throw new Exception("DGFontLoader error: End of block byte not found.");
            }

            // TOCA LEER LOS BITMAPS
            uint imgWidth = br.ReadUInt32(); // Leemos el tamaño horizontal de la textura.
            uint imgHeight = br.ReadUInt32();  // Leemos el tamaño vertical de la textura.
            int compresedlength = br.ReadInt32();  // Leemos la longitud de los bytes comprimidos.
            byte[] compresedBytes = br.ReadBytes(compresedlength); // Leemos datos comprimirdos.

            //DECOMPRESION SCAN0
            MemoryStream streamCompresed0 = new MemoryStream(compresedBytes); // Creamos "contenedor" para Bytes comprimidos.
            //streamCompresed0.Write(compresedBytes, 0, compresedlength); // Escribimos datos comprimidos en el "contenedor".

            MemoryStream streamScan0 = new MemoryStream(); // Creamos "contenedor" para Bytes descomprimidos.
            using (DeflateStream dfs = new DeflateStream(streamCompresed0, CompressionMode.Decompress))
            {
                dfs.CopyTo(streamScan0); //leemos los bytes y los descomprimimos.
            }

            byte[] decompresedbytes = streamScan0.ToArray(); // Obtenemos Bytes Descomprimidos.
            IntPtr scan0 = Marshal.AllocHGlobal(decompresedbytes.Length); // Creamos Puntero para Subor a la gráfica.
            Marshal.Copy(decompresedbytes, 0, scan0, decompresedbytes.Length); // Copiamos bytes en puntero.

            TextureBufferObject tbo0 = dge.G2D.Tools.p_LoadImageFromIntPTr(FontName+"_Scan0", (int)imgWidth, (int)imgHeight, scan0); // Creamos tbo de letras.

            Marshal.FreeHGlobal(scan0);

            if (br.ReadByte() != (byte)23) // Si no es fin de Bloque... 0001 0111 - 23
            {
                throw new Exception("DGFontLoader error: End of Scan0 block byte not found.");
            }

            uint img1Width = br.ReadUInt32(); // Leemos el tamaño horizontal de la textura.
            uint img1Height = br.ReadUInt32();  // Leemos el tamaño vertical de la textura.
            int compresed1length = br.ReadInt32();  // Leemos la longitud de los bytes comprimidos.
            byte[] compresed1Bytes = br.ReadBytes(compresed1length); // Leemos datos comprimirdos.

            //DECOMPRESION SCAN0
            MemoryStream streamCompresed1 = new MemoryStream(compresed1Bytes); // Creamos "contenedor" para Bytes comprimidos.
            //streamCompresed1.Write(compresed1Bytes, 0, compresed1length); // Escribimos datos comprimidos en el "contenedor".

            MemoryStream streamScan1 = new MemoryStream(); // Creamos "contenedor" para Bytes descomprimidos.
            using (DeflateStream dfs = new DeflateStream(streamCompresed1, CompressionMode.Decompress))
            {
                dfs.CopyTo(streamScan1); //leemos los bytes y los descomprimimos.
            }

            byte[] decompresed1bytes = streamScan1.ToArray(); // Obtenemos Bytes Descomprimidos.
            IntPtr scan1 = Marshal.AllocHGlobal(decompresed1bytes.Length); // Creamos Puntero para Subor a la gráfica.
            Marshal.Copy(decompresed1bytes, 0, scan1, decompresed1bytes.Length); // Copiamos bytes en puntero.

            TextureBufferObject tbo1 = dge.G2D.Tools.p_LoadImageFromIntPTr(FontName+"_Scan1", (int)img1Width, (int)img1Height, scan1); // Creamos tbo de letras.

            Marshal.FreeHGlobal(scan1);

            if (br.ReadByte() != (byte)23) // Si no es fin de Bloque... 0001 0111 - 23
            {
                throw new Exception("DGFontLoader error: End of Scan1 block byte not found.");
            }

            br.Close();

            dgFont ret = new dgFont(FontName, maxSize, BorderWidth, spaceWidth, Characters, dgCharacters, tbo0, tbo1);

            return ret;
        }

        public static void SaveDGFont(dgFont font, string outFilePath)
        {
            FileStream fs  = File.Open(outFilePath, FileMode.Create); // Creamos Fichero.
            BinaryWriter bw = new BinaryWriter(fs); // Creamos escritor binario para fichero.
            byte[] FileHeaderID = new byte[] {68, 71, 69, 70}; // Los primeros 4 bytes serán 4 caracteres en ASCII que definirán el formato (DGEF) (68, 71, 69, 70).
            bw.Write(FileHeaderID); // Escribimos ID de Formato.
            byte NameLong = (byte)font.Name.Length; // The long of Font Name in number of Characters.
            bw.Write(NameLong); // Escribimos longitud del nombre de la font
            bw.Write(font.s_Name.ToCharArray()); // Escribimos nombre de la font
            bw.Write(font.f_MaxFontSize); // Escribimos 4 bytes correspondientes al Maximo tamaño Original de la font.
            bw.Write(font.f_borderWidth); // Escribimos 4 bytes correspondientes al grosor del borde de letra.
            bw.Write(font.f_spaceWidth); // Escribimos 4 bytes correspondientes al grosor del borde de letra.
            bw.Write(font.d_characters.Count); // Escribimos 4 bytes del número de Caracteres incluidos en la font.
            char[] Characters = new char[font.d_characters.Count];
            font.d_characters.Keys.CopyTo(Characters, 0);
            bw.Write(Characters); // Escribimos lista de caracteres incluidos.
            for (int i=0;i<Characters.Length;i++) // Controlamos el orden de escritura de los caracteres.
            {
                char key = Characters[i];
                bw.Write(key); // Escribimos char
                bw.Write(font.d_characters[key].f_width); // Escribimos el ancho del caracter.
                bw.Write(font.d_characters[key].f_x0); // Escribimos Coordenada X0 correspondiente a la Textura.
                bw.Write(font.d_characters[key].f_y0); // Escribimos Coordenada Y0 correspondiente a la Textura.
                bw.Write(font.d_characters[key].f_x1); // Escribimos Coordenada X1 correspondiente a la Textura.
                bw.Write(font.d_characters[key].f_y1); // Escribimos Coordenada Y1 correspondiente a la Textura.
                bw.Write(font.d_characters[key].ui_width); // Escribimos anchira de la superficie del caracter en pixeles.
                bw.Write(font.d_characters[key].ui_height); // Escribimos altura de la superficie del caracter en pixeles.
            }
            bw.Write((byte)23); // 0001 0111 - 23 -  Fin del bloque de transmisión.
            
            // TOCA ESCRIBIR LOS BITMAPS.
            bw.Write(font.TBO_Scan0.Width); // Escribimos 4 bytes de tamaño horizontal de la textura.
            bw.Write(font.TBO_Scan0.Height); // Escribimos 4 bytes de tamaño vertical de la textura.
            IntPtr Scan0 = Marshal.AllocHGlobal((int)(font.TBO_Scan0.Width * font.TBO_Scan0.Height) * 4); // Definimos puntero para los datos de imagen
            dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            GL.glEnable(EnableCap.GL_TEXTURE_2D);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, font.TBO_Scan0.ui_ID);
            GL.glGetTexImage(TextureTarget.GL_TEXTURE_2D, 0, dgtk.OpenGL.PixelFormat.GL_RGBA, PixelType.GL_UNSIGNED_BYTE, Scan0); // Obtenemos datos de imagen y volcamos en puntero.
            dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
            byte[] Scan0bytes = new byte[(font.TBO_Scan0.Width * font.TBO_Scan0.Height) * 4 ]; // Pasamos los datos de imagen a Bytes.
            Marshal.Copy(Scan0, Scan0bytes, 0, (int)((font.TBO_Scan0.Width * font.TBO_Scan0.Height) * 4));

            //COMPRESION SCAN0
            MemoryStream streamScan0 = new MemoryStream(); // Creamos "contenedor" de Bytes comprimidos.
            
            using (DeflateStream dfs = new DeflateStream(streamScan0, CompressionLevel.Optimal))
            {
                dfs.Write(Scan0bytes, 0, Scan0bytes.Length); //Escribimos los bytes y lo comprimimos.
            }
            byte[] bytescompresScan0 = streamScan0.ToArray();

            bw.Write(bytescompresScan0.Length); // Escribimos longitud de Datos de imagen comprimidos.
            bw.Write(bytescompresScan0); // Escribimos datos de imagen comprimidos.
            
            bw.Write((byte)23); // 0001 0111 - 23 -  Fin del bloque de transmisión.

            bw.Write(font.TBO_Scan1.Width); // Escribimos 4 bytes de tamaño horizontal de la textura.
            bw.Write(font.TBO_Scan1.Height); // Escribimos 4 bytes de tamaño vertical de la textura.
            IntPtr Scan1 = Marshal.AllocHGlobal((int)(font.TBO_Scan1.Width * font.TBO_Scan1.Height) * 4);;
            dgtk.OpenGL.OGL_SharedContext.MakeCurrent();
            GL.glEnable(EnableCap.GL_TEXTURE_2D);
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, font.TBO_Scan1.ui_ID);
            GL.glGetTexImage(TextureTarget.GL_TEXTURE_2D, 0, dgtk.OpenGL.PixelFormat.GL_RGBA, PixelType.GL_UNSIGNED_BYTE, Scan1);
            dgtk.OpenGL.OGL_SharedContext.UnMakeCurrent();
            byte[] Scan1bytes = new byte[(font.TBO_Scan1.Width * font.TBO_Scan1.Height) * 4 ];
            Marshal.Copy(Scan1, Scan1bytes, 0, (int)((font.TBO_Scan1.Width * font.TBO_Scan1.Height) * 4));
            
            //COMPRESION SCAN1
            MemoryStream streamScan1 = new MemoryStream();
            using (DeflateStream dfs = new DeflateStream(streamScan1, CompressionLevel.Optimal))
            {
                dfs.Write(Scan1bytes, 0, Scan1bytes.Length); //Escribimos los bytes y lo comprimimos.
            }
            byte[] bytescompresScan1 = streamScan1.ToArray();
            
            bw.Write(bytescompresScan1.Length); // Escribimos longitud de Datos de imagen comprimidos.
            bw.Write(bytescompresScan1); // Escribimos datos de imagen comprimidos.
            
            bw.Write((byte)23); // 0001 0111 - 23 -  Fin del bloque de transmisión.

            bw.Close();
            fs.Close();
        }

        public static dgFont AutoDGFontGenerator(string filepath, int MaxSizeInPixels, string Characters, float BorderWidth, bool pixeled)
        {
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(filepath);
            Font font = new Font(pfc.Families[0], MaxSizeInPixels);

            List<string>lines = new List<string>();
            
            double cuadrado = Math.Sqrt(Characters.Length);
            int lastposition = 0;
            int maxlines = (((int)(cuadrado)) * ((int)(cuadrado)) < Characters.Length ? (int)(cuadrado+1): (int)(cuadrado));
            for (int i=0;i<maxlines;i++)
            {
                int Cuantas = (int)cuadrado;//+2;//*2;
                if (lastposition+(Cuantas)>Characters.Length)
                {
                    Cuantas = Characters.Length-lastposition;
                }                
                string l_letras = Characters.Substring(lastposition, (int)Cuantas);
                lastposition+=Cuantas;
                lines.Add(l_letras);
            }
            // Obtenemos tamaño del Bitmap:
            Dictionary<int, float> alturas = new Dictionary<int, float>();

            Bitmap bmp0 = new Bitmap(1, 1);
            Graphics g = Graphics.FromImage(bmp0);
            g = Graphics.FromImage(bmp0); // Definimos entorno de dibujo para el texto.
            g.PageUnit = GraphicsUnit.Pixel;
            float maxwidth = 0; // Ancho máximo calculado.
            float maxheight = 0; // Altura máxima calculada.
            StringFormat sf = StringFormat.GenericTypographic;
            sf.FormatFlags = StringFormatFlags.FitBlackBox;

            float spacewidth = g.MeasureString(" ", font, new PointF(0,0), sf).Width; // Definimos ancho del espacio.
            
            for (int i=0;i<lines.Count;i++)
            {
                SizeF tamaño = g.MeasureString(lines[i], font, new PointF(0,0), sf);
                maxwidth = maxwidth > (tamaño.Width+spacewidth) ? maxwidth : (tamaño.Width+spacewidth); // Dejamos espacio al final de la linea.
                maxheight+= tamaño.Height;
                alturas.Add(i, tamaño.Height);
            }
            maxwidth += spacewidth * lines[0].Length; // Añadimos margen izquierdo.
            maxheight += (spacewidth * 2); // Añadimos margen inferior.
            g.Dispose();


            // Creamos Bitmap Final:
            bmp0 = new Bitmap((int)maxwidth, (int)maxheight); // Creamos Bitmap para el Texto.
            g = Graphics.FromImage(bmp0); // Definimos entorno de dibujo para el texto.
            g.PageUnit = GraphicsUnit.Pixel;
            if (!pixeled)
            {
                g.CompositingQuality = CompositingQuality.HighQuality; // Definimos calidad del entorno de dibujo.
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality; // Definimos calidad del entorno de dibujo.
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; // Definimos calidad del entorno de dibujo.
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            }
            else
            {
                g.CompositingQuality = CompositingQuality.AssumeLinear; // Definimos calidad del entorno de dibujo.
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.SmoothingMode = SmoothingMode.None; // Definimos calidad del entorno de dibujo.
                g.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit; // Definimos calidad del entorno de dibujo.
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            }
            g.Clear(Color.Transparent); // Establecemos alpha como color de fondo.

            Bitmap bmpBorde = new Bitmap((int)maxwidth, (int)maxheight); // Creamos Bitmap para el Borde.
            Graphics gb = Graphics.FromImage(bmpBorde); // Definimos entorno de dibujo para el borde.
            gb.PageUnit = GraphicsUnit.Pixel;
            if (!pixeled)
            {
                gb.CompositingQuality = CompositingQuality.HighQuality; // Definimos calidad del entorno de dibujo.
                gb.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gb.SmoothingMode = SmoothingMode.HighQuality; // Definimos calidad del entorno de dibujo.
                gb.TextRenderingHint = TextRenderingHint.AntiAliasGridFit; // Definimos calidad del entorno de dibujo.
                gb.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            }
            else
            {
                gb.CompositingQuality = CompositingQuality.AssumeLinear; // Definimos calidad del entorno de dibujo.
                gb.InterpolationMode = InterpolationMode.NearestNeighbor;
                gb.SmoothingMode = SmoothingMode.None; // Definimos calidad del entorno de dibujo.
                gb.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit; // Definimos calidad del entorno de dibujo.
                gb.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.None;
            }

            gb.Clear(Color.Transparent); // Establecemos alpha como color de fondo.

            float altura = spacewidth; //Margen superior.
            
            Dictionary<char, dgCharacter> dgChars = new Dictionary<char, dgCharacter>();
            float proporcionalWidth = 1f/(float)maxwidth;
            float proporcionalHeight = 1f/(float)maxheight;
            //System.Drawing.Drawing2D.GraphicsPath p = new System.Drawing.Drawing2D.GraphicsPath(); // Creamos ruta.
            for (int i=0;i<lines.Count;i++) // Recorremos lineas.
            {
                System.Drawing.Drawing2D.GraphicsPath p = new System.Drawing.Drawing2D.GraphicsPath(); // Creamos ruta.
                float iniX = spacewidth; //Margen izquierda
                
                for (int c=0;c<lines[i].Length;c++) // Recorremos caracteres de la linea
                {
                    RectangleF rf = new RectangleF(new PointF(iniX, altura), g.MeasureString((lines[i])[c].ToString(), font, new PointF(iniX,altura), sf));
                    
                    //System.Drawing.Drawing2D.GraphicsPath p = new System.Drawing.Drawing2D.GraphicsPath(); // Creamos ruta.
                    p.AddString(lines[i][c].ToString(), font.FontFamily, 0, font.Size, new PointF(iniX+(BorderWidth*2), altura), sf); // Añadimos letra a la ruta
                    //g.FillPath(new SolidBrush(Color.White), p); // Pintamos letra.
                    //gb.DrawPath(new Pen(new SolidBrush(Color.White), BorderWidth), p); // Pintamos Borde

                    iniX += rf.Width+spacewidth; //Siguiente caracter con espacio de margen.
                    dgChars.Add(lines[i][c], new dgCharacter(lines[i][c], proporcionalWidth * rf.X, proporcionalHeight * rf.Y, proporcionalWidth * rf.Right, proporcionalHeight * rf.Bottom, (ushort)rf.Width, (ushort)rf.Height, rf.Width));
                }
                g.FillPath(new SolidBrush(Color.White), p); // Pintamos letra.
                gb.DrawPath(new Pen(new SolidBrush(Color.White), BorderWidth), p); // Pintamos Borde
                altura+= alturas[i];
            }
            

            
            char[] charkeys= new char[dgChars.Count];
            dgChars.Keys.CopyTo(charkeys, 0);
            dgCharacter[] dgcArray = new dgCharacter[dgChars.Count];
            dgChars.Values.CopyTo(dgcArray, 0);

            BitmapData bd = bmp0.LockBits(new Rectangle(0, 0, bmp0.Size.Width, bmp0.Size.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				
            TextureBufferObject tbo0 = dge.G2D.Tools.p_LoadImageFromIntPTr(pfc.Families[0].Name, bmp0.Width, bmp0.Height, bd.Scan0);
            bmp0.UnlockBits(bd);
            bmp0.Save("Letras.png");
			bmp0.Dispose();

            BitmapData bdB = bmpBorde.LockBits(new Rectangle(0, 0, bmpBorde.Size.Width, bmpBorde.Size.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
				
            TextureBufferObject tboB = dge.G2D.Tools.p_LoadImageFromIntPTr(pfc.Families[0].Name+"_Borde", bmpBorde.Width, bmpBorde.Height, bdB.Scan0);
            bmpBorde.UnlockBits(bdB);
            bmpBorde.Save("Borde.png");
			bmpBorde.Dispose();

            /*
            Bitmap FondoTest = new Bitmap((int)maxwidth, (int)maxheight);
            Graphics g_fondo = Graphics.FromImage(FondoTest);
            g_fondo.Clear(Color.Transparent);
            for (int i=0;i<dgcArray.Length;i++)
            {
                g_fondo.FillRectangle(new SolidBrush(Color.White), new RectangleF(dgcArray[i].f_x0*maxwidth, dgcArray[i].f_y0*maxheight, dgcArray[i].ui_width, dgcArray[i].ui_height));
            }
            FondoTest.Save("Fondo.png");
            
			FondoTest.Dispose();*/
            
            
            dgFont dgf_ret = new dgFont(pfc.Families[0].Name, MaxSizeInPixels, BorderWidth, spacewidth, charkeys, dgcArray, tbo0, tboB);
            
            return dgf_ret;
        }
    }
}