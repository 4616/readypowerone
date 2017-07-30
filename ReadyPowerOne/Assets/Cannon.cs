using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public static Queue<Bullet> pool = new Queue<Bullet>();
    public Bullet bulletPrefab;

    public float bulletSpeed = 20f;
    public float spawnTime = 5f;
    public float spawnCooldown = 0f;
    public Vector3 startOffset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        spawnCooldown -= Time.deltaTime;

        if (spawnCooldown <= 0f && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt))) {
            Shoot();
            spawnCooldown = spawnTime;
        }
    }

    public void Shoot() {
        Bullet b = Cannon.pool.Dequeue();
        if (b == null) b = Instantiate<Bullet>(bulletPrefab);
        b.transform.position = transform.position + startOffset;
        b.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
    }
}
