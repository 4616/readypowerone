﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drill : MonoBehaviour, ICombat {

	/*
	Drill is a combat-active recharge point for the Player
	with finite health and energy pools (dependent on
	deployment terrain). Player should have a GetEnergy
	method to drain the energy pool of the Drill and
	recharge the Player's energy pool.

	Use OnCollisionStay2D on player to detect collision
	with Drill.
	*/
	public enum State
	{
		Active,
		Inactive
	};

  //public string tag = "Drill";
	public State state;
	public float health = 10f;
	public float energy = 10f;
	public float range = 4f;
	public float armor = 0f;

	public float rechargeRate = 0.05f;
  //private int _rechargeTimer;
	//private bool _playerInRange;

	// Use this for initialization
	void Start() {
    state = State.Active;
		//_rechargeTimer = 0;
		//_playerInRange = true;
		Debug.Log("drill works");
	}

	// Update is called once per frame
	void Update () {

		}

	/*
	void OnCollisionStay2D(Collision2D coll) {
  	if (coll.gameObject.GetComponent<Player>() != null) {
    	state = state.Active;
    }
  }
  */



	public float GetEnergy () {
		float energyTransfer = 0;
		if(energy > 0){
			energyTransfer = Mathf.Min(this.energy, this.rechargeRate * Time.deltaTime);
			this.energy -= energyTransfer;
			return energyTransfer;
		}
		else{
			return energyTransfer;
		}
	}


    void OnCollisionStay2D(Collision2D coll) {
        Player e = coll.gameObject.GetComponent<Player>();
        if (e != null) {
            e.GainEnergy(this.GetEnergy());
        }
    }



  //public void CheckDeath(){
  //	if(health < 0){
  //		Destroy(gameObject, 0.05f);
  //	}
  //}

  public void TakeDamage(float damage) {
      Debug.Log("Drill takes " + damage + " damage.  Not implemented");
  }

}
