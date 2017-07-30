using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonBehavior : MonoBehaviour {

	public Text buttonTitle;
	public Text buttonDesc;

	public Upgrade upgrade;


	// Use this for initialization
	void Start () {
		Button btn = this.gameObject.GetComponent<Button> ();
		btn.onClick.AddListener (TaskOnClick);
	}


	void TaskOnClick(){
		Debug.Log ("Click");
	}

	public void makeButton(Upgrade u){
		upgrade = u;

		buttonTitle.text = u.title;
		buttonDesc.text = u.description;

	}
}
