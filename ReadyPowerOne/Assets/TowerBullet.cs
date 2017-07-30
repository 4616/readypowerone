using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBullet : Bullet {

    public override void Die() {
        gameObject.SetActive(false);
        Tower.pool.Enqueue(this);
    }
}
