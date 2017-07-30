using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesPanelBehavor : MonoBehaviour {

	public Text voltsText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateVolts(float t){
		Debug.Log ("update to " + t);
		voltsText.text = t.ToString();
		Debug.Log ("updated " + voltsText.text);
	}

}
