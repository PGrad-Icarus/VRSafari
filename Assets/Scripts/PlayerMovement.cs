using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour, Mover {
	public GameObject target;
	private NavMeshAgent nvAgent;
	private bool stopped = false;
	private Vector3 terrainBounds;

	// Use this for initialization
	void Start () {
		nvAgent = GetComponent<NavMeshAgent> ();
		EventManager.RegisterEvent ("StartPlayerMove", startMoving);
		EventManager.RegisterEvent ("Move", getMoving);
		EventManager.RegisterEvent ("Stop", stopMoving);
	}

	public void startMoving () {
		terrainBounds = GameObject.FindGameObjectWithTag ("Level").GetComponent<Terrain> ().terrainData.size;
		terrainBounds.x /= 2;
		getMoving ();
	}

	public void getMoving () {
		if(stopped)
			nvAgent.Resume ();
		else
			nvAgent.SetDestination (target.transform.position);
	}

	public void stopMoving () {
		nvAgent.Stop ();
		stopped = true;
	}
}
