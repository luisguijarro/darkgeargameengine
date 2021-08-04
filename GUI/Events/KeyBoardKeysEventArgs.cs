using  System;

namespace dge.GUI
{
    public class KeyBoardKeysEventArgs : EventArgs
    {
		private KeyBoard_Status ks;
        public KeyBoardKeysEventArgs(KeyBoard_Status KS)
        {
            this.ks = KS;
        }
		public KeyBoard_Status KeyStatus 
		{ 
			get { return this.ks; }
		}
    }
}
