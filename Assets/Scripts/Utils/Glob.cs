using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Glob {
	public const float TICK_RATE = 1f/5f;

	public enum InteractionType {
		PUSH,
		PULL,
	}

	public const int DEFAULT_TANK_CAPACITY = 5000;
}
