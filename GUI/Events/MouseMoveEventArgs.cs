using  System;

namespace dge.GUI
{
    public class MouseMoveEventArgs : EventArgs
    {
        private int i_posx;
        private int i_posy;
        private int i_spX;
        private int i_spY;
        public MouseMoveEventArgs(int posX, int posY, int posXScreen, int posYScreen)
        {
            this.i_posx = posX;
            this.i_posy = posY;
            this.i_spX = posXScreen;
            this.i_spY = posYScreen;
        }
        public int X 
        {
            get { return this.i_posx; }
        }
        public int Y
        {
            get { return this.i_posy; }
        }
        public int X_inScreen
        {
            get { return this.i_spX; }
        }
        public int Y_inScreen
        {
            get { return this.i_spY; }
        }
    }
}
