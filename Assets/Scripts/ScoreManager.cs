using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class ScoreManager : Singleton<ScoreManager> {
	public int score = 0,
			   rolls = 10,
			   maxScore = 100;
	public Text scoreText,
				rollText,
				winText;
	public Image rollImage,
			     scoreImage;
	public GameObject HUDPanel, 
					  losePanel;
	private Transform scoreTransform;
	private Vector3 scoreRotation;
	private int multiplier = 1,
				picScore = 0;
	protected ScoreManager() {}

	void Start() {
		scoreText.text = string.Format ("{0}/{1}", 0, Instance.maxScore);
		rollText.text = string.Format("x{0}",rolls.ToString());
		scoreTransform = scoreImage.transform;
	}

	void OnEnable () {
		GvrViewer.Instance.OnTilt += AddRoll;
	}

	public static void AddRoll () {
		if (Instance.score >= 20) {
			Instance.rolls += 1;
			Instance.rollText.text = string.Format ("x{0}", (++Instance.rolls).ToString());
			Instance.score -= 20;
		}
	}

	public static void AddPoints(int hits) {
		Instance.picScore += hits;
	}

	public static void MultiplyScore (int multiplier) {
		Instance.multiplier *= multiplier;
	}

	private static void ApplyMultiplier () {
		Instance.picScore *= Instance.multiplier;
		Instance.multiplier = 1;
	}

	private static void RemoveRoll() {
		if (Instance.rolls > 1)
			Instance.rollText.text = string.Format ("x{0}", (--Instance.rolls).ToString ());
		else
			Instance.StartCoroutine (Instance.WaitForShotThenEndGame ());
	}

	private IEnumerator WaitForShotThenEndGame () {
		yield return new WaitForSeconds (0.1f);
		Instance.rollImage.enabled = Instance.rollText.enabled = false;
		Instance.HUDPanel.SetActive (false);
		Instance.losePanel.SetActive (true);
		EventManager.TriggerEvent ("Stop");
		Image loseImage = Instance.losePanel.GetComponent<Image> ();
		loseImage.CrossFadeAlpha (255f, 1, false);
		CameraReticle.shotsEnabled = false;
	}

	private static void ResetPicScore () {
		Instance.picScore = 0;
	}

	private static void CumulateScore (int oldScore) {
		Instance.score += Instance.picScore;
		Instance.scoreText.text = string.Format ("{0}/{1}", Instance.score.ToString (), Instance.maxScore);
		Instance.scoreTransform.rotation = Quaternion.Euler (Instance.scoreTransform.right * -0.05f * (Instance.score - oldScore) ) * Instance.scoreTransform.rotation;
	}

	public static void changeScore(HashSet<GameObject> hits) {
		int oldScore = Instance.score,
			objScore = 0;
		foreach (GameObject go in hits) {
			if (!DataService.getScore (go, out objScore))
				throw new UnityException("Object not registered with database, cannot get score!");
			AddPoints (objScore);
			EventManager.TriggerGameobject (go);
		}
		ApplyMultiplier ();
		CumulateScore (oldScore);
		ResetPicScore ();
		RemoveRoll ();
		CameraReticle.registeredHitsByGO.Clear ();
	}
}
