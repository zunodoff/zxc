using System;
using HyperBar.utilities;
using UnityEngine;

namespace HyperBar.core
{
	[linked]
	public class Setup : MonoBehaviour
	{
		public void Start()
		{
		}

		public void FixedUpdate()
		{
		}
		public void OnGUI()
		{
			if (this.welcome)
			{
				this.welcomeWindowRect = GUI.Window(0, this.welcomeWindowRect, new GUI.WindowFunction(this.WelcomeWindow), "Welcome!");
			}
		}

		private void WelcomeWindow(int wId)
		{
			GUI.Label(new Rect((float)(Setup.windowX / 2 - 150), 20f, 300f, (float)Setup.windowY), "Welcome to the Cheaters Bar!!!!\n\n To open the menu Press F1, this menu auto updates to what game mode you are playing.");
			if (GUI.Button(new Rect((float)(Setup.windowX / 2 - 50), (float)(Setup.windowY - 25), 100f, 20f), "Close"))
			{
				this.welcome = false;
			}
			GUI.DragWindow(new Rect(0f, 0f, 10000f, 10000f));
		}

		private static int windowX = 400;
		private static int windowY = 200;
		private Rect welcomeWindowRect = new Rect((float)(Screen.width / 2 - Setup.windowX / 2), (float)(Screen.height / 2 - Setup.windowY / 2), (float)Setup.windowX, (float)Setup.windowY);
		private bool welcome = true;
	}
}
