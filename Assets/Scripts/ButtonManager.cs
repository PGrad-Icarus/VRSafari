using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonManager : MonoBehaviour {
	public GameObject button,
					  newJungle;
	private GameObject startJungle;
	private 
	void Start () {
		startJungle = GameObject.Find ("Jungle");
	}

	public void startMoving() {
		Destroy (startJungle);
		Instantiate (newJungle);
		Destroy (button);
		GameObject.Find ("CameraReticle").GetComponent<CameraReticle> ().OnGazeExit (null, null);
		GameObject.Find ("GvrMain").GetComponent<Move> ().enabled = true;
	}
}
