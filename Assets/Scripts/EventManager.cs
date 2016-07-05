//Adapted from here: https://unity3d.com/learn/tutorials/topics/scripting/events-creating-simple-messaging-system
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : Singleton <EventManager> {
	private Dictionary <GameObject, UnityEvent> objToEvent;

	void Awake () {
		objToEvent = new Dictionary<GameObject, UnityEvent> ();
	}
	public static void StartListening (GameObject obj, UnityAction listener) {
		UnityEvent thisEvent = null;
		if (Instance.objToEvent.TryGetValue (obj, out thisEvent))
			thisEvent.AddListener (listener);
		else {
			thisEvent = new UnityEvent ();
			thisEvent.AddListener (listener);
			Instance.objToEvent.Add (obj, thisEvent);
		}
	}

	public static void StopListening (GameObject obj, UnityAction listener) {
		if (Instance == null) return;
		UnityEvent thisEvent = null;
		if (Instance.objToEvent.TryGetValue (obj, out thisEvent))
			thisEvent.RemoveListener (listener);
	}

	public static void TriggerEvent (GameObject obj) {
		UnityEvent thisEvent = null;
		if (Instance.objToEvent.TryGetValue (obj, out thisEvent)) 
			thisEvent.Invoke ();
	}
}
