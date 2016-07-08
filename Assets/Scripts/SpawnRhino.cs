using UnityEngine;
using System.Collections;

public class SpawnRhino : MonoBehaviour, Mover {
	private NavMeshAgent nvAgent;
	private bool moving = false;
	private Transform playerTransform;
	// Use this for initialization
	void Start () {
		nvAgent = GetComponentInChildren <NavMeshAgent> ();
		EventManager.RegisterEvent ("Stop", stopMoving);
	}

	void Update () {
		if (moving)
			nvAgent.SetDestination (playerTransform.position);
	}
		
	public void getMoving () {
		moving = true;
	}

	public void stopMoving () {
		moving = false;
	}

	void OnTriggerEnter (Collider other) {
		if (!moving && other.name == "Player") {
			playerTransform = other.transform;
			moving = true;
		}
	}
}
