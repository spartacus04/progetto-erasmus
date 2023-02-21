using UnityEngine;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
	private static GameManager instance = null;

	private void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	public static List<Fluid> fluids = new List<Fluid>();
}