using System;

namespace dge.GUI
{ 
	public enum DialogResult
	{
		Cancel=0, OK = 1
	}
	
	public enum BorderStyle
	{
		None=0, In=1, Out=2
	}
	
    public enum Orientation
    {
        Horizontal = 0, Vertical = 1
    }

    public enum SliderShape
    {
        Arrow = 0, Square = 1
    }

    public enum SliderOrientation
    {
        UpRigth = 0, BottomLeft = 1
    }

    public enum TextAlign
    {
        Left = 0, Center = 1, Right = 2
    }

	public enum EnterLeave
	{
        Enter, Leave
	}
    
	public enum MouseButtons
	{
		Left = 1, Center = 2, Right = 3
	}

    public enum PushRelease : byte
    {
        Push = 1, 
        Release = 0
    }

	public enum KeyCode 
	{
		Unknown,
		//OTHERS
		PrintScreen, BloqDespl, Pause,
		//FUNKEYS
		F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12,
		//LETRAS:
		a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t, u, v, w, x, y, z, 
		//NÚMEROS:
		number0, number1, number2, number3, number4, 
		number5, number6, number7, number8, number9, 
		//ARROW:
		UP, DOWN, LEFT, RIGHT,
		//BLOQUE NÚMERICO:
		NumLock, 
		numberPad0, numberPad1, numberPad2, numberPad3, numberPad4, 
		numberPad5, numberPad6, numberPad7, numberPad8, numberPad9, 
		//MATH:
		Plus, Minus, Multiply, Divide, Less,
		//SYSTEM:
		Tab, Shift_Left, Shift_Right, Control_Left, Control_Right, Menu, 
		Alt, AltGr, CapsLock, Return, Intro, BackSpace, Space, Del, 
		Pag_up, Pag_Down, Home, End, Insert, ESC, Win_Left, Win_Right, Scroll, 
		Bracket_Left, Braket_Right, BackSlash, Quote, 
		SemiColon, Comma, Period, Slash, Grave_Accent

	}
}