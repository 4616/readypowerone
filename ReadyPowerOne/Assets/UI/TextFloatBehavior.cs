using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFloatBehavior : MonoBehaviour {

	private IEnumerator KillOnAnimationEnd() {
		yield return new WaitForSeconds (.9f);
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (KillOnAnimationEnd ());
	}
}
