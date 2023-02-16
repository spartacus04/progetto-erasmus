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