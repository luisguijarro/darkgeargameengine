using System;

namespace dge.GUI
{
    public struct KeyBoard_Status
	{
		public KeyCode KeyCode;
		public PushRelease KeyStatus;
		public KeyBoard_Status(KeyCode keycode, PushRelease keystatus)
		{
			this.KeyCode = keycode;
			this.KeyStatus = keystatus;
		}
	}
}