using UnityEngine;
using System.Collections;

public class SpawnRhino : MonoBehaviour, Mover {
	public float maxPlayerSpeed = 10;
	private NavMeshAgent rhinoNvAgent;
	private bool moving = false;
	private Transform playerTransform;
	private NavMeshAgent playerNVagent;

	// Use this for initialization
	void Start () {
		rhinoNvAgent = GetComponentInChildren <NavMeshAgent> ();
		playerNVagent = GameObject.Find ("Player").GetComponent<NavMeshAgent> ();
		EventManager.RegisterEvent ("Stop", stopMoving);
	}

	void Update () {
		if (moving) {
			rhinoNvAgent.SetDestination (playerTransform.position);
			if (playerNVagent.speed < maxPlayerSpeed) {
				rhinoNvAgent.speed += 0.1f;
				playerNVagent.speed += 0.5f;
			}
		}
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
