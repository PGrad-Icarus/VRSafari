using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {
	bool runOnce,
		 dropped;
	int yTranslate;
	GameObject tree;

	void Start() {
		tree = GameObject.Find ("BananaTrees");
		dropped = false;
		runOnce = false;
		yTranslate = 10;
	}

	//Pull an object hidden under the ground into view ("spawning" without the cost of spawning)
	public void popUpOnce() {
		GameObject summoned;
		AudioSource sound;
		if (!runOnce) {
			foreach (string child in DataService.getChildren (this.gameObject.name)) {
				summoned = GameObject.Find (child);
				summoned.transform.Translate (new Vector3 (0, 10, 0));
				sound = summoned.GetComponent<AudioSource> ();
				if (sound != null)
					sound.Play ();
			}	
			runOnce = true;
		}
	}

	//"Spawn" (make visible) a hidden object for a short period of time.
	public void sleight() {
		if (!dropped) {
			dropped = true;
			move ();
			StartCoroutine (Switch ());
		}
	}

	private void move() {
			tree.transform.Translate (new Vector3 (0, yTranslate, 0));
	}

	public IEnumerator Switch() {
		yield return new WaitForSeconds (4.0f);
		yTranslate = -yTranslate;
		move ();
		yTranslate = -yTranslate;
		dropped = false;
	}
}
