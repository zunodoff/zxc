using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HyperBar.utilities;
using UnityEngine;

namespace HyperBar.core
{
	public class loader
	{
		public static void load()
		{
			logger.alloc();
			logger.print("console allocated.....", 7);
			loader.h_manager = new manager();
			logger.print("created hooking manager...", 7);
			loader.g_loaded = new GameObject();
			loader.g_loaded.AddComponent<manager>();
			logger.print("loaded gameobject..", 7);
			List<Type> list = Assembly.GetExecutingAssembly().GetTypes().ToList<Type>();
			list.RemoveAll((Type l) => !l.IsDefined(typeof(linked), false));
			list.ForEach(delegate(Type type)
			{
				loader.g_loaded.AddComponent(type);
			});
			logger.print("linked " + list.Count.ToString() + " types in our assembly.", 7);
			UnityEngine.Object.DontDestroyOnLoad(loader.g_loaded);
			logger.print("initialized successfully", 7);
		}

		public static void unload()
		{
			logger.print("uninitializing", 7);
			UnityEngine.Object.Destroy(loader.g_loaded);
			logger.print("uninitialized loaded gameobject", 7);
			logger.free();
		}

		private static GameObject g_loaded;
		public static manager h_manager;
	}
}
