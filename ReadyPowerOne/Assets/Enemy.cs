using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICombat {
	public float health = 10f;
	public float energy = 10f;
	public float moveSpeed = 0.1f;
	public float damage = 1f;
	public float range = 1f;
	public float armor = 0f;


	// Use this for initialization
	void Start () {
		Debug.Log("Enemy initialized!");
		
	}
	
	// Update is called once per frame
	void Update () {
		bool attackable = InRange();
		//Debug.Log(attackable);


        //this.FindPlayer();

	}

	bool InRange() {

		float target_distance = Vector3.Distance(this.transform.position,FindPlayer());
		//Debug.Log(this.range);
		bool attackable = false;
		if (target_distance < this.range){
			attackable = true;
			}
		return attackable; 
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

