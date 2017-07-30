using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phazer : MonoBehaviour {

    public float damage = 1f;
    public float range = 4f;
    public float energyCost = 4f;

    void Start() {
        Upgrade.upgrades.Add(new Upgrade(
            "Phazer Damage",
            "Increase Damage per second of the Phazer by 1",
            () => damage += 1f
        ));
        Upgrade.upgrades.Add(new Upgrade(
            "Phazer Range",
            "Increase range of the Phazor by 1 tile (but not actually)",
            () => range += 1f
        ));
        Upgrade.upgrades.Add(new Upgrade(
            "Phazer Efficiency",
            "Deacrease the energy cost of the Phazer by one third",
            () => energyCost *= 0.67f
        ));
    }

    void OnCollisionStay2D(Collision2D coll) {
        ICombat e = coll.gameObject.GetComponent<ICombat>();
        if (e != null) {
            e.TakeDamage(damage * Time.deltaTime);
            Rigidbody2D r = coll.gameObject.GetComponent<Rigidbody2D>();
            if (r != null) {
                r.AddForce(transform.up * 50f);
            }
        }
    }
}
