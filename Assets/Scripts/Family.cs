using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Family : MonoBehaviour {
	private Dictionary <GameObject, int> countMembers;
	private bool open = true;
	void Awake () {
		GameObject go;
		int numGOs;
		countMembers = new Dictionary <GameObject, int> ();
		for (int child = 0; child < transform.childCount; child++) {
			go = transform.GetChild (child).gameObject;
			if (EventManager.isPhotogenic (go)) {
				if (countMembers.TryGetValue (go, out numGOs))
					countMembers [go] = ++numGOs;
				else
					countMembers.Add (go, 1);
			}
		}
	}

	void Update () {
		if (!open && CameraReticle.registeredHitsByGO.Count == 0) {
			BroadcastMessage ("Open");
			open = true;
		}
	}

	public void searchFamily () {
		int totalHits = 0,
			numHits = 0;
		foreach (var item in countMembers) { 
			if (CameraReticle.registeredHitsByGO.TryGetValue (item.Key, out numHits)) 
				totalHits += numHits;
		}
		ScoreManager.MultiplyScore (totalHits);
		BroadcastMessage ("Searched");
		open = false;
	}
}