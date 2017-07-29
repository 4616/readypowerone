using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phazer : MonoBehaviour {

    public float damage = 1f;

    void OnCollisionStay2D(Collision2D coll) {
        ICombat e = coll.gameObject.GetComponent<ICombat>();
        if (e != null) {
            e.TakeDamage(damage * Time.deltaTime);
        }
    }
}
