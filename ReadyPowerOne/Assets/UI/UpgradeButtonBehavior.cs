using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonBehavior : MonoBehaviour {

	public GameObject textAnimation;
	public Button upgradeButton;

	// Use this for initialization
	void Start () {
		Button btn = upgradeButton.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}


	void TaskOnClick(){
		Debug.Log ("Click");
		Vector3 randvec = new Vector3 (Random.Range (-10, 10), 
										Random.Range (-10, 10), 
										0);
		GameObject floatText = Instantiate (textAnimation,upgradeButton.transform);
		floatText.transform.Translate (randvec);
		
	}
}
