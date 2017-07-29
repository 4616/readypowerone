using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phazer : MonoBehaviour {

    public float damage = 1f;

    void OnCollisionStay2D(Collision2D coll) {
        Debug.Log("Phaser collides with something");
        Enemy e = coll.gameObject.GetComponent<Enemy>();
        if (e != null) {
            Debug.Log("With enemy");
            e.TakeDamage(damage * Time.deltaTime);
        }
    }
}
