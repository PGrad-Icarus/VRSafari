using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {
	Rigidbody rb;
	bool jumped = false;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter (Collider other) {
		if (!jumped && other.gameObject.name == "Player") {
			rb.AddForce (Vector3.right * 200 + Vector3.up * 500 + Vector3.forward * -200);
			jumped = true;
		}
	}
}
