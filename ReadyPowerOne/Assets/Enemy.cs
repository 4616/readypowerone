﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICombat {
	public float health = 10f;
	public float energy = 10f;
	public float moveSpeed = 0.1f;
	public float damage = 1f;
	public float attackRange = 1f;
	public float detectRange = 1f;
	public float armor = 0f;
	public float attackSpeed = 1f;
	public float attackCoolDown = 1f;


	// Use this for initialization
	void Start () {
		Debug.Log("Enemy initialized!");
	}
	
	// Update is called once per frame
	void Update () {
		TimeManagment();


		// bool playerDetected = InDetectRange();
		float target_distance = DistanceToPlayer();

		//Debug.Log("in update function");
		Debug.Log(target_distance);		
		if(target_distance <= detectRange){
			//Debug.Log("Player detected! Moving towards player now.");
			this.transform.position = Vector3.MoveTowards(this.transform.position, FindPlayer(), Time.deltaTime * this.moveSpeed);
		}

		if(attackCoolDown <= 0){
			if(target_distance <= attackRange){
				AttackPlayer();
			}
		}
	}

	void TimeManagment (){

		if(attackCoolDown > 0){
			//Debug.Log("Cooling down");
			attackCoolDown -= attackSpeed; 
			attackCoolDown = Mathf.Max(attackCoolDown, 0);
		}
	}

	void AttackPlayer (){
		//Debug.Log("Player takes " + damage + " damage.");
		Player.GetPlayer().TakeDamage(this.damage);
		attackCoolDown = attackSpeed; 
	}

	float DistanceToPlayer (){
		float target_distance = Vector3.Distance(this.transform.position,FindPlayer());
		return target_distance;

	}

	bool InDetectRange () {

		float target_distance = DistanceToPlayer();
		//Debug.Log(this.range);
		bool playerDetected = false;
		if (target_distance < this.detectRange){
			playerDetected = true;
			}
		return playerDetected; 
	}

	public Vector3 GetPosition(){
        return transform.position;
    }



	private Vector3 FindPlayer () {
		return Player.GetPlayer().GetPosition();
	}

// 	void DealDamage(){
// 		Player.GetPlayer().TakeDamage(damage);
// 	}

    public void TakeDamage(float damage) {
        //Debug.Log("Enemy takes " + damage + " damage.");
        this.health -= damage;
        if(this.health <= 0){
        	Die();

        }
    }

    public void Die (){
    	Object.Destroy(this.gameObject);
    }
}

