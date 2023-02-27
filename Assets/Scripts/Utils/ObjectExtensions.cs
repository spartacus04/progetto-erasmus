using System;
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

	public static T[] Empty<T>(this T[] self) {
		return new T[self.Length];
	}

	public static (int x, int y) indexOf<T>(this T[,] self, T item) {
		for(int x = 0; x < self.GetLength(0); x++)
			for(int y = 0; y < self.GetLength(1); y++)
				if(self[x, y].Equals(item))
					return (x, y);

		return (-1, -1);
	}
}
