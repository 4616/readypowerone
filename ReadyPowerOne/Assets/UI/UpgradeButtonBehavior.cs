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

	}


	public void ApplyUpgrade(){
		Debug.Log ("click");
		upgrade.apply();
		UIController.Instance.closeUpgradeMenu();
	}

	public void makeButton(Upgrade u){
		upgrade = u;

		buttonTitle.text = u.title;
		buttonDesc.text = u.description;

	}
}
