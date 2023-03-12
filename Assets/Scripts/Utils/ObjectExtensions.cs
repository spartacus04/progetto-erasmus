using System;
using System.Collections.Generic;
using System.Linq;

// Aggiunge tutti questi medoti statici a ogni oggetto
static class ObjectExtensions 
{
	// Kotlin: fun <T, R> T.let(block: (T) -> R): R
	public static R Let<T, R>(this T self, Func<T, R> block) 
	{
		return block(self);
	}

	// Kotlin: fun <T> T.also(block: (T) -> Unit): T
	public static T also<T>(this T self, Action<T> block)
	{
		block(self);
		return self;
	}

	// Kotlin: fun <T> T.apply(block: T.() -> T): T
	public static T Apply<T>(this T self, Func<T, T> block) 
	{
		return block(self);
	}
	public static (int x, int y) indexOf<T>(this T[,] self, T item) {
		for(int x = 0; x < self.GetLength(0); x++)
			for(int y = 0; y < self.GetLength(1); y++)
				if(self[x, y].Equals(item))
					return (x, y);

		return (-1, -1);
	}

	public static void ForEachIndexed<T>(this T[] self, Action<T, int> block) {
		for(int i = 0; i < self.Length; i++)
			block(self[i], i);
	}

	public static void ForEachIndexed<T>(this List<T> self, Action<T, int> block) {
		for(int i = 0; i < self.Count; i++)
			block(self[i], i);
	}

	public static void ForEach<T>(this T[] self, Action<T> block) {
		for(int i = 0; i < self.Length; i++)
			block(self[i]);
	}
}
