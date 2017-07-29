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

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.FindPlayer() 
	}

	void FindPlayer () {
		Player.GetPosition();
	}

	void DealDamage(){
		Player.TakeDamage(Enemy.damage);
	}

}

