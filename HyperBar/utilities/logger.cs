using System;
using System.IO;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace HyperBar.utilities
{
	public class logger
	{
		public static bool alloc()
		{
			if (!memory.AllocConsole())
			{
				return false;
			}
			memory.AttachConsole(uint.MaxValue);
			if (!memory.SetConsoleTitle("Cheater's Bar - Cheat Console - By HyperHax"))
			{
				return false;
			}
			Console.SetOut(new StreamWriter(new FileStream(new SafeFileHandle(memory.GetStdHandle(-11), true), FileAccess.Write), Encoding.ASCII)
			{
				AutoFlush = true
			});
			return true;
		}

		public static bool free()
		{
			return memory.FreeConsole();
		}

		public static void print(string szPrint, ushort usColor = 7)
		{
			memory.SetConsoleTextAttribute(memory.GetStdHandle(-11), logger.FOREGROUND_YELLOW);
			memory.SetConsoleTextAttribute(memory.GetStdHandle(-11), usColor);
			Console.WriteLine(szPrint);
		}

        // Basic foreground colors
        public static ushort FOREGROUND_RED = 0x0004;    // Red color bit mask
        public static ushort FOREGROUND_GREEN = 0x0002;  // Green color bit mask
        public static ushort FOREGROUND_BLUE = 0x0001;   // Blue color bit mask
        public static ushort FOREGROUND_BLACK = 0x0000;  // Black color (no bit set)

        // Composite colors using bitwise OR
        public static ushort FOREGROUND_WHITE = (ushort)(FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE); // Red + Green + Blue
        public static ushort FOREGROUND_YELLOW = (ushort)(FOREGROUND_RED | FOREGROUND_GREEN);                  // Red + Green
        public static ushort FOREGROUND_CYAN = (ushort)(FOREGROUND_BLUE | FOREGROUND_GREEN);                   // Blue + Green
        public static ushort FOREGROUND_MAGENTA = (ushort)(FOREGROUND_RED | FOREGROUND_BLUE);                  // Red + Blue
    }
}
