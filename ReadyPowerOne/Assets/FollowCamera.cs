using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour {

    public Transform target;

	// Update is called once per frame
	void Update () {
		if (target == null) {
            target = Player.GetPlayer().transform;
        }
        if (target != null) {
            transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
        }
    }
}
