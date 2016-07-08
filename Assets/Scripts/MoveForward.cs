using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class MoveForward : MonoBehaviour, Mover {
	public float speed;
	public bool moveOnTrigger = false;
	private Vector3 motion;
	private bool moving = false;
	private Vector3 terrainBounds;

	void Start() {
		if(!moveOnTrigger) {
			EventManager.RegisterEvent ("Move", getMoving);
			EventManager.RegisterEvent ("Stop", stopMoving);
		}
		terrainBounds = GameObject.FindGameObjectWithTag("Level").GetComponent<Terrain> ().terrainData.size;
		terrainBounds.x /= 2;
	}

	void Update () {
		if (moving) {
			transform.Translate (Vector3.forward * speed);
			if (Mathf.Abs (transform.position.x) > terrainBounds.x || transform.position.z < 0 || transform.position.z > terrainBounds.z)
				Destroy (gameObject);
		}
	}

	public void getMoving () {
		moving = true;
	}

	public void stopMoving () {
		moving = false;
	}
}
