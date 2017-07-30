using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	bool Paused;
	public GameObject wallObject;
	public GameObject bossEnemyObject;
	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject coal;

	public GameObject pausePanel;
	public ResourcesPanelBehavor resourcePanelPrefab;
	public GameObject floatingTextPrefab;


	private ResourcesPanelBehavor resourcePanel;

	private float lastDraw = 0f;
	private float DrawAmount = 0f;
	private float blockSize = 2.5f;
		
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
	
	}

	void Start () {
		//Instantiate (pausePanel, this.transform);
		resourcePanel = Instantiate (resourcePanelPrefab, this.transform);

		Room room = TerrainGenerator.GenerateLevel (30, 120, 0);

		for (int y = 0; y < room.getHeight(); y++) {
			for (int x = 0; x < room.getWidth(); x++) {
				Terrain t = room.getLayout () [y] [x];
				GameObject newObject = null;
				if (t == Terrain.Wall) {
					newObject = Instantiate (this.wallObject, this.transform);
				}
				if (t == Terrain.BossEnemy && this.bossEnemyObject != null) {
					newObject = Instantiate (this.bossEnemyObject, this.transform);
				}
				if (t == Terrain.Enemy1 && this.enemy1 != null) {
					newObject = Instantiate (this.enemy1, this.transform);
				}
				if (t == Terrain.Enemy2 && this.enemy2 != null) {
					newObject = Instantiate (this.enemy2, this.transform);
				}
				if (t == Terrain.Coal && this.coal != null) {
					newObject = Instantiate (this.coal, this.transform); //comment out to remove drills
				}

				if (newObject != null) {
					newObject.transform.position = new Vector3 (x * blockSize, y * blockSize, 0);
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

	public void updateBolts(float bolts){
		resourcePanel.updateBolts (bolts);
	}

	public void updateHealth(float health){
		resourcePanel.updateHealth (health);
	}

	public void floatText(float floatnum,Transform location){
		DrawAmount += floatnum;

		if  (Time.frameCount - lastDraw >= 12f) {
			GameObject ft = Instantiate (floatingTextPrefab, this.transform);

			Vector3 drawlocation = Camera.main.WorldToScreenPoint (location.position);
			Vector3 randvec = new Vector3 (Random.Range (-20, 20), 
				                  Random.Range (-20, 20), 
				                  0);
			ft.GetComponent<TextFloatBehavior>().floatText.text = DrawAmount.ToString();
			ft.transform.position = drawlocation + randvec;
			lastDraw = Time.frameCount;
			DrawAmount = 0f;
		} 

	}

}
