using UnityEngine;
using System;

//         _______ _______ ______ _   _ ___________ ____  _   _ ______
//      /\|__   __|__   __|  ____| \ | |___  /_   _/ __ \| \ | |  ____|
//     /  \  | |     | |  | |__  |  \| |  / /  | || |  | |  \| | |__
//    / /\ \ | |     | |  |  __| | . ` | / /   | || |  | | . ` |  __|
//   / ____ \| |     | |  | |____| |\  |/ /__ _| || |__| | |\  | |____
//  /_/    \_\_|     |_|  |______|_| \_/_____|_____\____/|_| \_|______|
//
// Questo file contiene un fracco di utilità che possono essere usate ovunque
// NON MODIFICARE QUESTO FILE PER NESSUN MOTIVO OLTRE CHE AGGIUNGERE ALTRE UTILITÀ

public class Utils : MonoBehaviour
{
	// Questa struttura è detta singleton, è una classe che contiene tantissimi metodi di utilità statici
	private static Utils instance = null;

	private void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}
}


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
}
