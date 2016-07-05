using UnityEngine;
using UnityEngine.Events;
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
			if (countMembers.TryGetValue (go, out numGOs))
				countMembers [go] = ++numGOs;
			else
				countMembers.Add (go, 1);
		}
	}

	void Update () {
		if (!open && CameraReticle.numGOmap.Count == 0) {
			BroadcastMessage ("Open");
			open = true;
		}
	}

	public void searchFamily () {
		int totalHits = 0,
			numHits = 0,
			totalScore = 0;
		foreach (var item in countMembers) { 
			if (CameraReticle.numGOmap.TryGetValue (item.Key, out numHits)) {
				totalHits += numHits;
				totalScore += DataService.getScore (item.Key);
			}
		}
		ScoreManager.AddPoints (totalScore * totalHits);
		BroadcastMessage ("Searched");
		open = false;
	}
}