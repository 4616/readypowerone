using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICombat {

    public static Player player_ = null;

    public static Player GetPlayer()
    {
        return player_;
    }

    public void Start()
    {
        player_ = this;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void TakeDamage(float damage)
    {

    }
}
