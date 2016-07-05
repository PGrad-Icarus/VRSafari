using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour {
	public GameObject target;
	public float speed;
	private Vector3 motion;
	void Start() {
		if (gameObject.name == "ElephantFamily")
			motion = new Vector3 (-speed / (Mathf.Sqrt (2) * 4), 0, -speed / (Mathf.Sqrt (2) * 4));
		else
			motion = new Vector3 (0, 0, speed);
	}
	void Update () {
		if (gameObject.name != "GvrMain")
			transform.Translate (motion);
		else
			GetComponent<NavMeshAgent> ().SetDestination (target.transform.position);
		//foreach(Transform child in transform)
			//child.Translate (motion);
	}
}
