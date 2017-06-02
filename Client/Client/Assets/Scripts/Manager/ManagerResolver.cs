using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class ManagerResolver {
	
	private static Dictionary<Type, object> TypeDictionary = new Dictionary<Type, object>();
	
	public static void Register<T>(object obj) where T : class
	{
        if (!TypeDictionary.ContainsKey(typeof(T)))
        {
            TypeDictionary.Add(typeof(T), obj);
        }
        else
        {
            TypeDictionary[typeof(T)] = obj;
        }
	}
	
	public static T Resolve<T>() where T : class
	{
		if (!TypeDictionary.ContainsKey (typeof(T)))
			return null;

		return TypeDictionary[typeof(T)] as T;
	}
}
