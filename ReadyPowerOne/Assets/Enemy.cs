using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICombat {
	public float health = 10f;
	public float energy = 10f;
	public float moveSpeed = 0.1f;
	public float damage = 1f;
	public float range = 1.5f;
	public float armor = 0f;
	public 	

	// Use this for initialization
	void Start () {
		Debug.Log("print works");
		
	}
	
	// Update is called once per frame
	void Update () {


        //this.FindPlayer();

	}

	bool InRange() {
		float target_distance = 1f; //Need to add distance calculation 
		bool attackable = false;
		if (target_distance < this.range){
			attackable = true;
			}
		return attackable; 
	}

// 	public Vector3 GetPosition(){
//         return transform.position;
//     }



// 	void FindPlayer () {
// 		Player.GetPlayer().GetPosition();
// 	}

// 	void DealDamage(){
// 		Player.GetPlayer().TakeDamage(damage);
// 	}

}

