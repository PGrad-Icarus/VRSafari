using UnityEngine;
using System.Collections;

public class IguanaCharacter : MonoBehaviour, Mover {
	Animator iguanaAnimator;
	private bool moving = false;
	
	void Start () {
		moving = true;
		iguanaAnimator = GetComponent<Animator> ();
		EventManager.RegisterEvent ("Move", getMoving);
		EventManager.RegisterEvent ("Stop", stopMoving);
	}

	void Update () {
		if (moving)
			Move (1f, 0f);
		else
			Move (0f, 0f);
	}
	
	public void Attack(){
		iguanaAnimator.SetTrigger("Attack");
	}
	
	public void Hit(){
		iguanaAnimator.SetTrigger("Hit");
	}
	
	/*public void Eat(){
		iguanaAnimator.SetTrigger("Eat");
	}*/

	/*public void Death(){
		iguanaAnimator.SetTrigger("Death");
	}*/

	/*public void Rebirth(){
		iguanaAnimator.SetTrigger("Rebirth");
	}*/

	public void Move(float v,float h){
		iguanaAnimator.SetFloat ("Forward", v);
		iguanaAnimator.SetFloat ("Turn", h);
	}

	public void getMoving () {
		moving = true;
	}

	public void stopMoving () {
		moving = false;
	}
}
