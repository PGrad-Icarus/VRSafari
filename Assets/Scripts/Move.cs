using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public float speed;
	private Vector3 motion;
	void Start() {
		motion = new Vector3 (-speed,0,0);
	}
	void Update () {
		transform.Translate (motion);
		foreach(Transform child in transform)
			child.Translate (motion);
	}
}
