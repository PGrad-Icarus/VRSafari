using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
public class ScoreManager : Singleton<ScoreManager> {
	public int score = 0,
			   rolls = 5,
			   maxScore = 100;
	public Text scoreText,
				rollText,
				winText;
	public Image rollImage,
			     scoreImage;
	public GameObject HUDPanel, 
					  loseObject;
	private Transform scoreTransform;
	private Vector3 scoreRotation;
	private int multiplier = 1,
				picScore = 0,
				scoreToGetRoll = 10,
				rollCost = 20;
	private Queue<HashSet<GameObject>> lastFivePics;
	private HashSet<GameObject> newPicSet;
	protected ScoreManager() {}

	void Awake () {
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		lastFivePics = new Queue<HashSet<GameObject>> ();
		newPicSet = new HashSet<GameObject> ();
		scoreText.text = string.Format ("{0}/{1}", 0, Instance.maxScore);
		rollText.text = string.Format("x{0}",rolls.ToString());
		scoreTransform = scoreImage.transform;
	}

	public static void AddBonusRoll () {
		Instance.rolls += 1;
	}

	public static void AddRollWithCost () {
		if (Instance.score >= Instance.rollCost) {
			Instance.rolls++;
			Instance.score -= Instance.rollCost;
		}
		ShowNewScore ();
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
		Instance.rollText.text = string.Format ("x{0}", (--Instance.rolls).ToString ());
		if (Instance.rolls < 1)
			Instance.StartCoroutine (Instance.WaitForShotThenEndGame ());
	}

	private IEnumerator WaitForShotThenEndGame () {
		yield return new WaitForSeconds (0.1f);
		Instance.loseObject.SetActive (true);
		EventManager.TriggerEvent ("Stop");
		//Image loseImage = Instance.losePanel.GetComponent<Image> ();
		//loseImage.CrossFadeAlpha (255f, 1, false);
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

	public static void ShowNewScore () {
		Instance.rollText.text = string.Format ("x{0}", Instance.rolls.ToString());
		Instance.scoreText.text = string.Format ("{0}/{1}", Instance.score.ToString(), Instance.maxScore);
	}
		
	public static bool CanGetBonus (HashSet<GameObject> newPicSet) {
		foreach (var pic in Instance.lastFivePics) {
			if (pic.SetEquals (newPicSet))
				return false;
		}
		return true;
	}

	public static void changeScore(HashSet<GameObject> hits) {
		int oldScore = Instance.score,
			objScore = 0;
		bool notOld = CanGetBonus (hits);
		if (notOld) {
			foreach (GameObject go in hits) {
				if (!DataService.getScore (go, out objScore))
					throw new UnityException ("Object not registered with database, cannot get score!");
				AddPoints (objScore);
				Instance.newPicSet.Add (go);
				if (CameraReticle.mapGOtoFacing[go])
					EventManager.TriggerGameobject (go);
			}
		} else {
			foreach (GameObject go in hits) {
				if (!DataService.getScore (go, out objScore))
					throw new UnityException ("Object not registered with database, cannot get score!");
				AddPoints (objScore);
				Instance.newPicSet.Add (go);
			}
		}
		ApplyMultiplier ();
		if (Instance.picScore >= Instance.scoreToGetRoll && notOld) 
			AddBonusRoll ();
		else
			RemoveRoll ();
		if (Instance.lastFivePics.Count >= 5) 
			Instance.lastFivePics.Dequeue ();
		Instance.lastFivePics.Enqueue (Instance.newPicSet);
		CumulateScore (oldScore);
		ResetPicScore ();
		ShowNewScore ();
		CameraReticle.mapGOtoFacing.Clear ();
	}
}
