using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {

    public static Queue<Bullet> pool = new Queue<Bullet>();
    public Bullet bulletPrefab;

    public float bulletSpeed = 20f;
    public float spawnTime = 5f;
    public float spawnCooldown = 0f;
    public float energyCost = 1f;
    public Vector3 startOffset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        spawnCooldown -= Time.deltaTime;

        //if (spawnCooldown <= 0f && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt) || Input.GetMouseButton(0))) {
        //    Shoot();
        //    spawnCooldown = spawnTime;
        //}
    }

    public bool CanShoot() {
        return spawnCooldown <= 0f;
    }

    public void Shoot() {
        spawnCooldown = spawnTime;
        Bullet b;
        if (Cannon.pool.Count > 0) {
            //Debug.LogError("Recycle " + pool.Count);
            b = Cannon.pool.Dequeue();
            b.gameObject.SetActive(true);
        } else {
            //Debug.LogError("Creating a new bullet, this should not happen so often " + pool.Count);
            b = Instantiate<Bullet>(bulletPrefab);
        }
        b.transform.position = transform.position + startOffset;
        b.GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;
    }
}
