using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float spawnTime = 5f;
    public float spawnCooldown = 0f;
    public Enemy enemyPrefab;
	
	// Update is called once per frame
	void Update () {
        spawnCooldown -= Time.deltaTime;

        if (spawnCooldown <= 0f) {
            Spawn();
            spawnCooldown = spawnTime;
        }
	}

    public void Spawn() {
        GameObject.Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
}
