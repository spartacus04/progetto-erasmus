using UnityEngine;
using System;

public class ClickCB : MonoBehaviour, IClickable {
	public Action cb; 

	public void OnClick() {
		if(cb != null) cb();
	}

	public void Apply(Action cb) {
		this.cb = cb;
	}
}