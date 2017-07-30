using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Enemy {

    public static Queue<Bullet> pool = new Queue<Bullet>();
    public Bullet bulletPrefab;
    public float bulletSpeed;
    public Transform turret;

    void Start () {
		this.moveSpeed = 0f;
	}

    protected override void Update() {
        float angle = AngleBetweenPoints(transform.position, Player.GetPlayer().transform.position);
        turret.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90f));
        base.Update();
    }

    float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }


    protected override void AttackPlayer() {
        //Player.GetPlayer().TakeDamage(this.damage);
        attackCoolDown = attackSpeed;

        Bullet b;
        if (Cannon.pool.Count > 0) {
            b = Cannon.pool.Dequeue();
            b.gameObject.SetActive(true);
        } else {
            b = Instantiate<Bullet>(bulletPrefab);
        }
        b.damage = damage;

        Vector3 path = Player.GetPlayer().transform.position - transform.position;


        b.transform.position = transform.position + path.normalized;
        b.GetComponent<Rigidbody2D>().velocity = path.normalized * bulletSpeed;
    }
}