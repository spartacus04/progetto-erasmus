using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

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

	public static void ApplyMaterialRecursively(GameObject obj, Material mat) {
		if(obj.TryGetComponent<MeshRenderer>(out MeshRenderer component))
			component.materials = new Material[] { mat };

		foreach(Transform child in obj.transform) {
			ApplyMaterialRecursively(child.gameObject, mat);
		}
	}

	public static void setTimeout(Action action, float time) {
		instance.StartCoroutine(Timeout(action, time));
	}

	private static IEnumerator Timeout(Action action, float time) {
		yield return new WaitForSeconds(time);
		action();
	}
}