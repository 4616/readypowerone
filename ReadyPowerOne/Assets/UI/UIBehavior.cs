using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehavior : MonoBehaviour {

	bool Paused;
	public GameObject pausePanel;
	public GameObject resourcePanel;


	void Start () {
		//Instantiate (pausePanel, this.transform);
		Instantiate (resourcePanel, this.transform);

		//pausePanel.SetActive(false);
	}

	void Update() {
		if (!Paused && Input.GetKeyDown(KeyCode.P) ) {
			Pause();
		}
		else if (Paused && Input.GetKeyDown(KeyCode.P) ) {
			unPause();
		}
	}
	
	void Pause(){
		Paused = true;
		//pausePanel.SetActive (true);

		Time.timeScale = 0f;
		Debug.Log (pausePanel.activeInHierarchy);
	}

	void unPause(){
		Paused = false;
		//pausePanel.SetActive (false);

		Time.timeScale = 1f;
		Debug.Log (pausePanel.activeInHierarchy);
	}
}
