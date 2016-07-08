using UnityEngine;
using System.Collections;

public class Turn : MonoBehaviour {
	public float xDegree,
				 yDegree,
				 zDegree;
	private Quaternion appliedRotation;

	// Update is called once per frame
	void Start () {
		appliedRotation = Quaternion.Euler (Vector3.right * xDegree + Vector3.up * yDegree + Vector3.forward * zDegree);
	}

	void Update () {
		transform.rotation = appliedRotation * transform.rotation;
	}
}
