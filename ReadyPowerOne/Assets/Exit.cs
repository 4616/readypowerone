using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll) {
        Player e = coll.gameObject.GetComponent<Player>();
        if (e != null) {
            // WIN
            UIController.Instance.Win();
        }
    }
}
