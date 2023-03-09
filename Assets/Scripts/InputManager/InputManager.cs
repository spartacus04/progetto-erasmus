using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR;

public class InputManager : MonoBehaviour
{
	[SerializeField]
	private XRNode xRNode = XRNode.LeftHand;
	private List<InputDevice> devices = new List<InputDevice>();
	private InputDevice device;

	[SerializeField]
	Animator hands;
   
   
    
	void Start()
	{}

	void GetDevice()
	{
		InputDevices.GetDevicesAtXRNode(xRNode, devices);
		device = devices.FirstOrDefault();
	}
	void OnEnable()
	{

		if (!device.isValid)
		{
			GetDevice();
		}

	}

	int cont = 0;

	// Update is called once per frame
	void Update()
	{
		if (!device.isValid)
		{
			GetDevice();
		}
		/*List<InputFeatureUsage> features = new List<InputFeatureUsage>();
		device.TryGetFeatureUsages(features); 
		if(cont>features.Count){
		return;
		}else{
		foreach(var feature in features){
		    if(feature.type== typeof(bool)){
		Debug.Log($"feature{feature.name} type {feature.type}");
		    }
		cont++;
		
		} Logg per riconoscere input*/
		input();
	}

	public bool triggerButtonAction = false;
	public bool gripButton = false;
	void input()
	{




		if (device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonAction) && triggerButtonAction)
		{

			Debug.Log($"Trigger button activated {triggerButtonAction}");
			hands.SetFloat("speed", 1);
			hands.SetTrigger("point");
		} else if (device.TryGetFeatureValue(CommonUsages.gripButton, out gripButton) && gripButton)
		{
			Debug.Log($"Trigger button activated {gripButton}");
			hands.SetFloat("speed", 1);
			hands.SetTrigger("grab");
		}else{
		    hands.SetFloat("speed", -1);

		}

	}




}