using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableGeneric : MonoBehaviour, IClickable
{
	public void OnClick()
	{
		Debug.Log("Clicked");
	}
}