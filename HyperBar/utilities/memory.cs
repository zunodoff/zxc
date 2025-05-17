using System;
using System.Runtime.InteropServices;

namespace HyperBar.utilities
{
	public class memory
	{
		[DllImport("kernel32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AllocConsole();

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool AttachConsole(uint dwProcessId);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool FreeConsole();

		[DllImport("kernel32.dll")]
		public static extern bool SetConsoleTitle(string lpConsoleTitle);

		[DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetConsoleTextAttribute(IntPtr hConsoleOutput, ushort wAttributes);

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetConsoleWindow();

		public unsafe static void abs_jump(IntPtr pSite, IntPtr pTarget)
		{
			byte* ptr = (byte*)pSite.ToPointer();
			*ptr = 73;
			ptr[1] = 187;
			*(long*)(ptr + 2) = pTarget.ToInt64();
			ptr[10] = 65;
			ptr[11] = byte.MaxValue;
			ptr[12] = 227;
		}
	}
}
