using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
	public XRNode xRNode = XRNode.LeftHand;
	private List<InputDevice> devices = new List<InputDevice>();
	private InputDevice device;

	public Animator hand;
	public Transform raycastOrigin;

	private bool firstPress = true;

	private bool triggerButtonAction = false;
	private bool gripButton = false;

	void Start()
	{
		if (!device.isValid)
		{
			GetDevice();
		}
	}

	void GetDevice()
	{
		InputDevices.GetDevicesAtXRNode(xRNode, devices);
		device = devices.FirstOrDefault();
	}

	void Update()
	{
		if (!device.isValid)
		{
			GetDevice();
		}

		if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonAction))
		{
			if (triggerButtonAction)
			{
				if (firstPress)
				{
					firstPress = false;

					// raycast in direction of raycastOrigin if hit object is IClickable fire onclick
					RaycastHit hit;
					if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit))
					{
						if (hit.collider.gameObject.GetComponent<IClickable>() != null)
						{
							hit.collider.gameObject.GetComponent<IClickable>().OnClick();
						}
					}
				}

				hand.SetFloat("speed", 1);
				hand.SetTrigger("point");

				return;
			}
			else
			{
				firstPress = true;
			}
		}

		if (device.TryGetFeatureValue(CommonUsages.gripButton, out gripButton) && gripButton)
		{
			hand.SetFloat("speed", 1);
			hand.SetTrigger("grab");
		}
		else
		{
			hand.SetFloat("speed", -1);
		}
	}
}