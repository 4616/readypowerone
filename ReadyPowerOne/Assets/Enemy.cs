using System.Collections;
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


	// Use this for initialization
	void Start () {
		Debug.Log("Enemy initialized!");
		
	}
	
	// Update is called once per frame
	void Update () {
		bool playerDetected = InDetectRange();

		//Debug.Log(playerDetected);		
		if(playerDetected){
			//Debug.Log("Player detected! Moving towards player now.");
			this.transform.position = Vector3.MoveTowards(this.transform.position, FindPlayer(), this.moveSpeed);
		}

		if(DistanceToPlayer() <= attackRange){
			Player.GetPlayer().TakeDamage(this.damage);
		}



		


        //this.FindPlayer();

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

// 	public Vector3 GetPosition(){
//         return transform.position;
//     }



	private Vector3 FindPlayer () {
		return Player.GetPlayer().GetPosition();
	}

// 	void DealDamage(){
// 		Player.GetPlayer().TakeDamage(damage);
// 	}

}

