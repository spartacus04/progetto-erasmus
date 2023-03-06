using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnMachine : MonoBehaviour
{
	public Transform respawnPoint;

	private void OnCollisionEnter(Collision other) {
		if(other.gameObject.GetComponent<Machine>() != null) { 
			other.gameObject.transform.position = respawnPoint.position;
		}		
	}
}
