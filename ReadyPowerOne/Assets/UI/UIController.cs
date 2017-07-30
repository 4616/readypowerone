using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	bool Paused;
	public GameObject pausePanel;

	public GameObject wallObject;

	public ResourcesPanelBehavor resourcePanelPrefab;
	public GameObject floatingText;

	private ResourcesPanelBehavor resourcePanel;

	private float lastDraw;

	public static UIController Instance { get; private set; }

	void Awake() {
		
		// First we check if there are any other instances conflicting
		if(Instance != null && Instance != this)
		{
			// If that is the case, we destroy other instances
			Destroy(gameObject);
		}

		// Here we save our singleton instance
		Instance = this;

		//Add terrain


	
	}

	private float blockSize = 2.5f;

	void Start () {
		//Instantiate (pausePanel, this.transform);
		resourcePanel = Instantiate (resourcePanelPrefab, this.transform);

		Room room = TerrainGenerator.GenerateLevel (100, 100, 0);

		for (int y = 0; y < room.getHeight(); y++) {
			for (int x = 0; x < room.getWidth(); x++) {
				Terrain t = room.getLayout () [y] [x];
				if (t == Terrain.Wall) {
					GameObject newWall = Instantiate (this.wallObject, this.transform);
					newWall.transform.position = new Vector3 (x * blockSize, y * blockSize, 0);
				}
			}
		}

//		GameObject potato = Instantiate (this.wallObject, this.transform);
//		potato.transform.position = new Vector3 (5, 5, 0);



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
	}

	void unPause(){
		Paused = false;
		//pausePanel.SetActive (false);

		Time.timeScale = 1f;
	}

	public void updateEnergy(float energy){
		resourcePanel.updateVolts (energy);
	}

	public void floatText(float floatnum,Transform location){
		
		GameObject ft = Instantiate (floatingText, this.transform);

		Vector3 drawlocation = Camera.main.WorldToScreenPoint (location.position);
		Vector3 randvec = new Vector3 (Random.Range (-20, 20), 
			                 Random.Range (-20, 20), 
			                 0);
		
		ft.transform.position = drawlocation + randvec;

	}

}
