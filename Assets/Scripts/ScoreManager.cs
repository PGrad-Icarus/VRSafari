using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class ScoreManager : Singleton<ScoreManager> {
	public int score = 0,
			   rolls = 5;
	public Text scoreText,
				rollText,
				loseText,
				winText;
	public Image rollImage,
			     scoreImage;
	private Transform scoreTransform;
	private Vector3 scoreRotation;
	private int multiplier = 1;
	protected ScoreManager() {}

	void Start() {
		rollText.text = string.Format("x{0}",rolls.ToString());
		scoreTransform = scoreImage.transform;
		scoreRotation = scoreTransform.right * -0.5f;
	}

	public static void AddPoints(int hits) {
		Instance.score += hits;
		Instance.scoreText.text = Instance.score.ToString ();
		Instance.scoreTransform.rotation = Quaternion.Euler (Instance.scoreRotation * hits) * Instance.scoreTransform.rotation;
	}

	public static void AddMultiplier (int multiplier) {
		Instance.multiplier += multiplier;
	}

	public static void ApplyMultiplier () {
		int oldScore = Instance.score;
		Instance.score *= Instance.multiplier;
		Instance.scoreText.text = Instance.score.ToString ();
		Instance.scoreTransform.rotation = Quaternion.Euler (Instance.scoreRotation * (Instance.score - oldScore) ) * Instance.scoreTransform.rotation;
		Instance.multiplier = 1;
	}

	public static void RemoveRoll() {
		if (Instance.rolls > 1)
			Instance.rollText.text = string.Format ("x{0}", (--Instance.rolls).ToString ());
		else
			Instance.StartCoroutine (Instance.WaitForShotThenEndGame ());
	}

	private IEnumerator WaitForShotThenEndGame () {
		yield return new WaitForSeconds (0.1f);
		Instance.rollImage.enabled = Instance.rollText.enabled = false;
		Instance.loseText.enabled = true;
	}

	public static void changeScore(HashSet<GameObject> hits) {
		foreach (GameObject go in hits)
			EventManager.TriggerEvent (go);
		ApplyMultiplier ();
		RemoveRoll ();
		CameraReticle.numGOmap.Clear ();
	}
}
