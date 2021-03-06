﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

	bool Paused;
	public GameObject wallObject;
	public GameObject bossEnemyObject;
	public GameObject enemy1;
	public GameObject enemy2;
	public GameObject spawner;
	public GameObject coal;
	public GameObject destWall;
	public GameObject floor;
	public GameObject exit;

	public int mapWidth = 50;
	public int mapHeight = 120;
	public float blockSize = 2.5f;


	public UpgradePanelBehavior upgradePanelPrefab;
	public ResourcesPanelBehavior resourcePanelPrefab;
    public GameObject startPanelPrefab;
    public GameObject winPanelPrefab;
    public GameObject losePanelPrefab;
	public GameObject floatingTextPrefab;

	private UpgradePanelBehavior upgradePanel;
	private ResourcesPanelBehavior resourcePanel;
    private GameObject startPanel;
    private GameObject winPanel;
    private GameObject losePanel;

	private float lastDraw = 0f;
	private float lastDrawLowEnergy = 0f;
	private float DrawAmount = 0f;

		
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
        startPanel = Instantiate(startPanelPrefab, this.transform);

        Room room = TerrainGenerator.GenerateLevel (mapWidth, mapHeight, 0);


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
				if (t == Terrain.Spawner && this.spawner != null) {
					newObject = Instantiate (this.spawner, this.transform);
				}
				if (t == Terrain.DestWall && this.destWall != null) {
					newObject = Instantiate (this.destWall, this.transform);
				}
				if (t == Terrain.Exit && this.exit != null) {
					newObject = Instantiate (this.exit, this.transform);
				}


				if (t != Terrain.Wall && this.floor != null) {
					GameObject floors = Instantiate (this.floor, this.transform);
					floors.transform.position = new Vector3 (x * blockSize, y * blockSize, 1);
				}
				// if (t == Terrain.Coal && this.coal != null) {
				// 	newObject = Instantiate (this.coal, this.transform); //comment out to remove drills
				// }

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
			openUpgradeMenu();
		}
		else if (Paused && Input.GetKeyDown(KeyCode.P) ) {
			closeUpgradeMenu();
		}

        if(Input.GetKey(KeyCode.Space)) {
            startPanel.SetActive(false);
        }
	}


    public void Win() {
        if (winPanel == null) {
            winPanel = Instantiate(winPanelPrefab, this.transform);
        } else {
            winPanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void Lose() {
        if (losePanel == null) {
            losePanel = Instantiate(losePanelPrefab, this.transform);
        } else {
            losePanel.SetActive(true);
        }
        Time.timeScale = 0f;
    }

    public void openUpgradeMenu(){
		upgradePanel = Instantiate (upgradePanelPrefab, this.transform);
		Paused = true;

		Time.timeScale = 0f;
	}

	public void closeUpgradeMenu(){
		Debug.Log("close");
		Destroy (upgradePanel.gameObject);
		Paused = false;

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

	public void floatTextForText(string s,Transform location){


		if  (Time.frameCount - lastDrawLowEnergy  >= 40f) {
			GameObject ft = Instantiate (floatingTextPrefab, this.transform);

			Vector3 drawlocation = Camera.main.WorldToScreenPoint (location.position);
			Vector3 displacevec = new Vector3 (0, 5, 0);
			ft.GetComponent<TextFloatBehavior>().floatText.text = s;
			ft.transform.position = drawlocation + displacevec;
			lastDrawLowEnergy = Time.frameCount;
			DrawAmount = 0f;
		} 

	}
		

}
