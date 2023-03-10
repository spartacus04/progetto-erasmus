using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;

public class Rotator : MonoBehaviour
{
	public ClickableChair[] chairs;

	private int current = 0;

	private Animator animator;

	private static Rotator instance;
    void Start()
    {
        animator = GetComponent<Animator>();
		instance = this;
    }

	public static void Rotate(int id) {
		// check if id is next or prev
		int prev = instance.current - 1;

		if(prev < 0) prev = 3;

		if(id == prev) {
			instance.animator.SetTrigger("URotate" + ((prev + 1) % 4));
		} else {
			instance.animator.SetTrigger("Rotate" + id);
		}

		instance.current = id;

		foreach(ClickableChair chair in instance.chairs) {
			chair.gameObject.SetActive(false);
		}

		setTimeout(() => {
			int prev = instance.current - 1;
			int next = instance.current + 1;

			if(prev < 0) prev = 3;
			if(next > 3) next = 0;

			instance.chairs[prev].gameObject.SetActive(true);
			instance.chairs[next].gameObject.SetActive(true);
		}, 0.5f);
	}
}
