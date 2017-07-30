using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesPanelBehavior : MonoBehaviour {

	public Text voltsText;
	public Text boltsText;
	public Text healthText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void updateVolts(float t){
		voltsText.text = t.ToString();
	}

	public void updateBolts(float t){
		boltsText.text = t.ToString();
	}

	public void updateHealth(float t){
		healthText.text = t.ToString();
	}

}
