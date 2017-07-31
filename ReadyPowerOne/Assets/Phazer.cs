using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phazer : MonoBehaviour {

    public float damage = 1f;
    public int range = 4;
    public float energyCost = 4f;
    public float width = 1f;

    public LineRenderer line;
    public BoxCollider2D collider;
    

    void Start() {
        Upgrade.upgrades.Add(new Upgrade(
            "Phazer Damage",
            "Increase Damage per second of the Phazer by 1",
            () => damage += 1f
        ));
        Upgrade.upgrades.Add(new Upgrade(
            "Phazer Range",
            "Increase range of the Phazor by 1 section",
            () => range += 1
        ));
        Upgrade.upgrades.Add(new Upgrade(
            "Phazer Width",
            "It's short, but it's thick",
            () => width += 2
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

    public void Fire() {
        collider.size = new Vector2(width, (range * 2f) - 0.5f);
        collider.offset = new Vector2(0f, range + 0.5f);
        gameObject.SetActive(true);
        line.positionCount = range + 1;
        for (int i = 1; i < line.positionCount; i++) {
            float adjustedWidth = Mathf.Max(((float)i / (float)range) * width / 2f, 0.4f);
            line.SetPosition(i, new Vector3(Random.Range(-adjustedWidth, adjustedWidth), Random.Range(-0.4f, 0.4f) + 2f * i, 0f));
        }
    }
}
