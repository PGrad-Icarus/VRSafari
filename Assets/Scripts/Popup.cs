using UnityEngine;
using System.Collections;

public class Popup : MonoBehaviour {
	bool runOnce,
		 dropped;
	public string[] stuff = {"who, what, when"};
	int yTranslate;
	GameObject[] trees;
	DataService ds;

	void Start() {
		ds = new DataService ("safari.db");
		trees = GameObject.FindGameObjectsWithTag ("BananaTree");
		dropped = false;
		runOnce = false;
		yTranslate = 10;
	}

	public void popUpOnce() {
		GameObject summoned;
		AudioSource sound;
		if (!runOnce) {
			foreach (string child in ds.getChildren (this.gameObject.name)) {
				summoned = GameObject.Find (child);
				summoned.transform.Translate (new Vector3 (0, 10, 0));
				sound = summoned.GetComponent<AudioSource> ();
				if (sound != null)
					sound.Play ();
			}	
			runOnce = true;
		}
	}

	public void sleight() {
		if (!dropped) {
			dropped = true;
			move ();
			StartCoroutine (Switch ());
		}
	}

	public void move() {
		for (int tree = 0; tree < trees.Length; tree++)
			trees [tree].transform.Translate (new Vector3 (0, yTranslate, 0));
	}

	public IEnumerator Switch() {
		yield return new WaitForSeconds (2.0f);
		yTranslate = -yTranslate;
		move ();
		yTranslate = -yTranslate;
		dropped = false;
	}
}
