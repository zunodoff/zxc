using System;
using UnityEngine;

namespace HyperBar.utilities
{
	public class draw
	{
		public static Vector2 world_to_screen(Vector3 vPos)
		{
			Vector3 vector = Camera.main.WorldToScreenPoint(vPos);
			return new Vector3(vector.x, (float)Screen.height - vector.y);
		}

		public static void rect(Vector2 vPos, Vector2 vSize, Color rColor)
		{
			GL.PushMatrix();
			GL.Color(rColor);
			GL.Begin(1);
			GL.Vertex3(vPos.x, vPos.y, 0f);
			GL.Vertex3(vPos.x, vPos.y + vSize.y, 0f);
			GL.Vertex3(vPos.x + vSize.x, vPos.y + vSize.y, 0f);
			GL.Vertex3(vPos.x + vSize.x, vPos.y, 0f);
			GL.Vertex3(vPos.x, vPos.y, 0f);
			GL.End();
			GL.PopMatrix();
		}

		public static void rect(Vector2 vPos, Vector2 vSize)
		{
			draw.rect(vPos, vSize, Color.white);
		}
	}
}
