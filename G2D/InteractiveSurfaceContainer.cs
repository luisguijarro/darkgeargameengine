using System;
using dgtk.Graphics;
using dgtk.OpenGL;

namespace dge.G2D
{
    public class InteractiveSurfaceContainer : dge.G2D.InteractiveSurface
    {
        private uint FrameBuffer;
        private uint DepthRenderBuffer;
        private bool contentUpdate;
        public InteractiveSurfaceContainer(uint width, uint height) : base(width, height)
        {
            this.contentUpdate = false;
            this.FrameBuffer = GL.glGenFramebuffer();
            this.DepthRenderBuffer = GL.glGenRenderbuffer();
            this.UpdateFrameAnbdRenderBuffers();
        }

        private void UpdateFrameAnbdRenderBuffers()
        {
            GL.glBindTexture(TextureTarget.GL_TEXTURE_2D, this.textureBufferObject.ui_ID);

            GL.glTexImage2D(TextureTarget.GL_TEXTURE_2D, 0, InternalFormat.GL_RGBA, (int)this.textureBufferObject.ui_width, (int)this.textureBufferObject.ui_height, 0, PixelFormat.GL_RGBA, PixelType.GL_UNSIGNED_BYTE, new IntPtr(0));

            GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_MAG_FILTER, (int)TextureMagFilter.GL_NEAREST);
            GL.glTexParameteri(TextureTarget.GL_TEXTURE_2D, TextureParameterName.GL_TEXTURE_MIN_FILTER, (int)TextureMagFilter.GL_NEAREST);

            GL.glBindRenderbuffer(RenderbufferTarget.GL_RENDERBUFFER, this.DepthRenderBuffer);
            GL.glRenderbufferStorage(RenderbufferTarget.GL_RENDERBUFFER, InternalFormat.GL_DEPTH_COMPONENT, (int)this.textureBufferObject.ui_width, (int)this.textureBufferObject.ui_height);
            GL.glFramebufferRenderbuffer(FramebufferTarget.GL_FRAMEBUFFER, FramebufferAttachment.GL_DEPTH_ATTACHMENT, RenderbufferTarget.GL_RENDERBUFFER, this.DepthRenderBuffer);

            GL.glFramebufferTexture(FramebufferTarget.GL_FRAMEBUFFER, FramebufferAttachment.GL_COLOR_ATTACHMENT0, this.textureBufferObject.ui_ID, 0);
        }

        protected virtual void DrawContent(Drawer drawer)
        {
            // Dibujar contenido.
        }

        protected virtual void DrawContentIDs()
        {
            // Dibujar IDs de contenido.
        }

        internal override void Draw(Drawer drawer)
        {
            if (this.contentUpdate) 
            {
                GL.glBindFramebuffer(FramebufferTarget.GL_FRAMEBUFFER, this.FrameBuffer);
                GL.glViewport(0, 0, (int)this.textureBufferObject.ui_width, (int)this.textureBufferObject.ui_height);
                DrawContent(drawer);
                GL.glBindFramebuffer(FramebufferTarget.GL_FRAMEBUFFER, 0);
            }
            base.Draw(drawer);
        }

        internal override void DrawID()
        {
            base.DrawID();
            this.DrawContentIDs();
        }

        public override TextureBufferObject TextureBufferObject 
        { 
            get { return base.TextureBufferObject;} 
            set 
            { 
                base.TextureBufferObject = value; 
                this.UpdateFrameAnbdRenderBuffers();
            }
        }

        public bool ContentUpdate
        {
            get { return this.contentUpdate; }
            set { this.contentUpdate = value; }
        }
    }
}