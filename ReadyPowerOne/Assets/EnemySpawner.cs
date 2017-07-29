using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour, ICombat {

    public float spawnTime = 5f;
    public float spawnCooldown = 0f;
    public float health = 200f;
    public Enemy enemyPrefab;
	
	// Update is called once per frame
	void Update () {
        spawnCooldown -= Time.deltaTime;

        if (spawnCooldown <= 0f && Vector3.Distance(this.transform.position, Player.GetPlayer().GetPosition()) < 10f) {
            Spawn();
            spawnCooldown = spawnTime;
        }
	}

    public void Spawn() {
        GameObject.Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    public void TakeDamage(float damage) {
        this.health -= damage;
        if (this.health <= 0) {
            Die();
        }
    }

    public void Die() {
        Object.Destroy(this.gameObject);
    }
}
