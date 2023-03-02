using UnityEngine;

public class Selectable : MonoBehaviour {
	private Color grey = new Color(0.5f, 0.5f, 0.5f, 0.1f);
	private Color orange = new Color(1f, 0.5f, 0f, 0.1f);
	private Color blue = new Color(0f, 0.5f, 1f, 0.1f);

	private bool canClick = false;
	public int mode = 0;

	private GameObject box;

	public void Apply(int m) {
		// create bounding box around object
		box = GameObject.CreatePrimitive(PrimitiveType.Cube);
		box.transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
		box.transform.localScale = new Vector3(0.1f, 0.01f, 0.1f);
		box.transform.parent = transform;

		var clickcb = box.AddComponent<ClickCB>();

		clickcb.Apply(() => {
			if(!canClick) return;

			mode++;
			if(mode > 2) mode = 0;

			ApplyColor();
		});

		this.mode = m;
		ApplyColor();
		canClick = true;
	}

	public void ApplyColor() {
		switch(mode) {
			case 0:
				box.GetComponent<MeshRenderer>().material.color = grey;
				break;
			case 1:
				box.GetComponent<MeshRenderer>().material.color = blue;
				break;
			case 2:
				box.GetComponent<MeshRenderer>().material.color = orange;
				break;
		}
	}

	public void Remove() {
		Destroy(box);
	}
}