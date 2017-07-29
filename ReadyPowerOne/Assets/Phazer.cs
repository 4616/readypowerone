using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phazer : MonoBehaviour {

    public float damage = 1f;

    void OnCollisionStay2D(Collision2D coll) {
        Enemy e = coll.gameObject.GetComponent<Enemy>();
        if (e != null) {
            e.TakeDamage(damage * Time.deltaTime);
        }
    }
}
