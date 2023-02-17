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
}
