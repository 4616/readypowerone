using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Button btn = this.gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}


	void TaskOnClick(){
		Debug.Log ("Click");
	}
}
