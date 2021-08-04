using  System;

namespace dge.GUI
{
    public class KeyBoardTextEventArgs : EventArgs
    {
        private char ch_character;
        public KeyBoardTextEventArgs(char character)
        {
            this.ch_character = character;
        }
        public char Character
        {
            get { return this.ch_character; }
        }
    }
}
