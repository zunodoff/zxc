using System;
using System.Collections.Generic;
using UnityEngine;

namespace HyperBar.utilities
{
	internal class gameUtils
	{
		public static List<PlayerStats> getPlayers()
		{
			List<PlayerStats> list = new List<PlayerStats>();
			foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
			{
				if (gameObject.GetComponent<PlayerStats>() != null && !list.Contains(gameObject.GetComponent<PlayerStats>()))
				{
					list.Add(gameObject.GetComponent<PlayerStats>());
				}
			}
			return list;
		}
	}
}
