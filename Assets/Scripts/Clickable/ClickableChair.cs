using UnityEngine;
using System;

public class ClickableChair : MonoBehaviour, IClickable {
	public int id;

	public void OnClick() {
		print("Clicked chair " + id);
		Rotator.Rotate(id);
	}
}