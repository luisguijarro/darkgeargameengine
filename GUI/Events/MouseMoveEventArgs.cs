using  System;

namespace dge.GUI
{
    public class MouseMoveEventArgs : EventArgs
    {
        private uint ui_id;
        private int i_posx;
        private int i_posy;
        private int i_spX;
        private int i_spY;
        private int i_lastPosX;
        private int i_lastPosY;
        public MouseMoveEventArgs(int posX, int posY, int lastposx, int lastposy, int posXScreen, int posYScreen, uint id)
        {
            this.i_posx = posX;
            this.i_posy = posY;
            this.i_lastPosX = lastposx;
            this.i_lastPosY = lastposy;
            this.i_spX = posXScreen;
            this.i_spY = posYScreen;
            this.ui_id = id;
        }
        public int X 
        {
            get { return this.i_posx; }
        }
        public int Y
        {
            get { return this.i_posy; }
        }
        public int LastPosX 
        {
            get { return this.i_lastPosX; }
        }
        public int LastPosY 
        {
            get { return this.i_lastPosY;}
        }

        public int X_inScreen
        {
            get { return this.i_spX; }
        }
        public int Y_inScreen
        {
            get { return this.i_spY; }
        }
        public uint ID 
        {
            get { return this.ui_id; }
        }
    }
}
