using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float damage = 5f;

    public void Die() {
        gameObject.SetActive(false);
        Cannon.pool.Enqueue(this);
        Debug.LogError("Bullet dead, pool has " + Cannon.pool.Count);
    }

    void OnCollisionEnter2D(Collision2D coll) {
        ICombat e = coll.gameObject.GetComponent<ICombat>();
        if (e != null) {
            e.TakeDamage(damage);
        }
        Die();
    }
}
