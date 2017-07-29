using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour, ICombat {

	/*
	Drills are energy recharge stations.
	*/
	//public enum State
	//{
	//	Active,
	//	Idle
	//}
	//private SpriteRenderer spriteRenderer;
	//public GameObject explosion;

	//public string tag = "Minion";
	public float health = 10f;
	//public State state;
	//public Player owner;
	public float range = 10f;
	public float rechargeRate = 0.05f;

  private int _rechargeTimer;

	// Use this for initialization
	void Start() {
    //state = State.Active;
		//_rechargeTimer = 0;

	}

	// Update is called once per frame
	void Update () {
		if (_rechargeTimer > 0) _rechargeTimer--;

	}

	void FindPlayer () {
		//Player.GetPosition();
	}

	void RechargePlayer () {
		//Player.Recharge();
	}

	//public void CheckDeath(){
	//	if(health < 0){
	//		Destroy(gameObject, 0.05f);
	//	}
	//}

}
