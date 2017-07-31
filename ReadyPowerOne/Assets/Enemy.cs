using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ICombat {
	public float health = 10f;
	public float energy = 10f;
	public float moveSpeed = 1f;
	public float damage = 1f;
	public float attackRange = 5f;
	public float detectRange = 10f;
	public float armor = 0f;
	public float attackSpeed = 1f;
	public float attackCoolDown = 1f;
	public GameObject bolts;
	public GameObject healthdrop;
	public GameObject explosionenemy;


	// Use this for initialization
	void Start () {
	}

    // Update is called once per frame
    protected virtual void Update () {
		TimeManagment();


		// bool playerDetected = InDetectRange();
		float target_distance = DistanceToPlayer();

		//Debug.Log("in update function");
		//Debug.Log(target_distance);		
		if(target_distance <= detectRange){
			//Debug.Log("Player detected! Moving towards player now.");
			MoveEnemy();
		}

		if(attackCoolDown <= 0){
			if(target_distance <= attackRange){
				AttackPlayer();
			}
		}
	}

	public virtual void MoveEnemy (){
		this.transform.position = Vector3.MoveTowards(this.transform.position, FindPlayer(), Time.deltaTime * this.moveSpeed);
	}

	protected void TimeManagment (){
        //Debug.LogError("TimeManagment");
        if (attackCoolDown > 0){
			//Debug.Log("Cooling down");
			attackCoolDown -= Time.deltaTime; 
			attackCoolDown = Mathf.Max(attackCoolDown, 0);
		}
	}

	protected virtual void AttackPlayer (){
		//Debug.Log("Player takes " + damage + " damage.");
		if(Time.deltaTime>.01){
			Player.GetPlayer().TakeDamage(this.damage);
			attackCoolDown = attackSpeed; 
		}
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



	protected Vector3 FindPlayer () {
		return Player.GetPlayer().GetPosition();
	}

// 	void DealDamage(){
// 		Player.GetPlayer().TakeDamage(damage);
// 	}

    public void TakeDamage(float damage) {
        //Debug.Log("Enemy takes " + damage + " damage.");
        this.health -= damage;

		//spawn damage text
		UIController.Instance.floatText (damage, this.transform);

        GameObject ex = Instantiate (explosionenemy, this.transform.position, this.transform.rotation);
        Object.Destroy(ex, 0.5f);

        if(this.health <= 0){
        	Die();
        }
    }

    public void Die (){
        //Debug.Log("Enemy dead!");
        if (bolts != null) {
            GameObject newObject = Instantiate(this.bolts, UIController.Instance.transform);
            newObject.transform.position = this.transform.position;
        }
    	Object.Destroy(this.gameObject);
    }
}

