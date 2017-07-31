using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICombat {

    public Phazer phazer;

    public Cannon cannon;
    public GameObject drill;

    public static Player player_ = null;

    public float moveSpeed = 1f;
    public float rotationSpeed = 90f;
    public float energy = 100f;
    public float energyMax = 100f;
    public float bolts = 100f;
    public float health = 100f;

	public float moveCost = .1f;
    public float drillCost = 10f;
    public GameObject explosionplayer;
    public GameObject lowenergywarning;
    public static Player GetPlayer() {
        return player_;
    }

    public void Start() {
        //DropDrill();
        player_ = this;

		this.transform.position = new Vector3 (UIController.Instance.mapWidth * UIController.Instance.blockSize / 2, 10, 0);

		Upgrade.upgrades.Add(new Upgrade(
			"Max Battery",
			"Increase max battery capacity by 20",
			() => energyMax += 20f
		));

        Upgrade.upgrades.Add(new Upgrade(
            "Move Speed",
            "Increase move speed 25%",
            () => moveSpeed *= 1.25f
        ));
        Upgrade.upgrades.Add(new Upgrade(
            "Move Efficiency",
            "Deacrease the energy cost of moving by 25%",
            () => moveCost *= 0.75f
        ));
        Upgrade.upgrades.Add(new Upgrade(
            "Rotation Speed",
            "Rotate faster",
            () => rotationSpeed *= 2f
        ));
        Upgrade.upgrades.Add(new Upgrade(
            "Recharge",
            "Refill your battery right now",
            () => GainEnergy(100f - energy)
        ));

        
    }

    //public void Update() {
    //    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
    //        transform.position += transform.up * Time.deltaTime * moveSpeed;
    //        LoseEnergy(Time.deltaTime);
    //    }
    //    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
    //        transform.position -= transform.up * Time.deltaTime * moveSpeed;
    //        LoseEnergy(Time.deltaTime);
    //    }
    //    if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
    //        transform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed);
    //        LoseEnergy(Time.deltaTime);
    //    }
    //    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
    //        transform.Rotate(Vector3.forward, -1f * Time.deltaTime * rotationSpeed);
    //        LoseEnergy(Time.deltaTime);
    //    }

    //    if (Input.GetKey(KeyCode.Space)) {
    //        Phaser();
    //    } else {
    //        phazer.gameObject.SetActive(false);
    //    }

    //    if (energy <= 0f) {
    //        Debug.Log("Out of Energy!");
    //        Time.timeScale = 0f;
    //    }
    //}

//	public void Update() {
//		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
//			transform.position += transform.up * Time.deltaTime * moveSpeed;
//			LoseEnergy(moveCost*Time.deltaTime);
//		}
//		if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
//			transform.position += transform.up * -0.5f * Time.deltaTime * moveSpeed;
//			LoseEnergy(moveCost*Time.deltaTime);
//		}
//		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
//			//transform.position +=     //Vector3.left * Time.deltaTime * moveSpeed; 
//			transform.Rotate(0, 0, 3);
//			LoseEnergy(moveCost*Time.deltaTime);
//		}
//		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
//			transform.Rotate(0, 0, -3);
//			//transform.position +=     //Vector3.right * Time.deltaTime * moveSpeed;
//			LoseEnergy(moveCost*Time.deltaTime);
//		}
//
//		if (Input.GetKeyDown(KeyCode.Q)) {
//			DropDrill();
//		}
//
//		if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space)) {
//			Phaser();
//		} else {
//			phazer.gameObject.SetActive(false);
//		}
//		if (cannon.CanShoot() && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt) || Input.GetMouseButton(0))) {
//			Cannon();
//		}
//
//		if (energy <= 0f) {
//			Debug.Log("Out of Energy!");
//			Time.timeScale = 0f;
//		}
//	}

    public void Update() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            transform.position += Vector3.up * Time.deltaTime * moveSpeed;
			LoseEnergy(moveCost*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            transform.position += Vector3.down * Time.deltaTime * moveSpeed;
			LoseEnergy(moveCost*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += Vector3.left * Time.deltaTime * moveSpeed;
			LoseEnergy(moveCost*Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.position += Vector3.right * Time.deltaTime * moveSpeed;
			LoseEnergy(moveCost*Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            DropDrill();
        }

        float angle = AngleBetweenPoints(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f));
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle + 90f)), Time.deltaTime * rotationSpeed);
        
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space)) {
            LoseEnergy(phazer.energyCost * Time.deltaTime);
            phazer.Fire();
        } else {
            phazer.gameObject.SetActive(false);
        }
        if (cannon.CanShoot() && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt) || Input.GetMouseButton(0))) {
            Cannon();
        }

        if (energy <= 0f) {
            Debug.Log("Out of Energy!");
            Time.timeScale = 0f;
        }

        LowEnergy();
    }

    private void LowEnergy(){
        if(energy <= 30){
            if(energy > 30){
                UIController.Instance.floatTextForText("Low on Energy!", this.transform);
            }
            if(energy < 25){
                UIController.Instance.floatTextForText("Drop a Drill!", this.transform);
            }
            float cutpoint = Random.Range(0f,1f);
            if(cutpoint < ((energy-20)/200)){
                GameObject ex = Instantiate (lowenergywarning, this.transform.position, this.transform.rotation);
                Object.Destroy(ex, 5f);
            }
        }
    }

    float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void LoseEnergy (float amount) {
        energy -= amount;
		UIController.Instance.updateEnergy(energy);
    }

    public void Cannon() {
        LoseEnergy(cannon.energyCost);
        cannon.Shoot();
    }

    //public void Phaser() {
    //    LoseEnergy(phaserCost * Time.deltaTime);
    //    phazer.gameObject.SetActive(true);
    //    for (int i = 1; i < phazer.positionCount; i++) {
    //        phazer.SetPosition(i, new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f) + 2f * i, 0f));
    //    }
    //}

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void TakeDamage(float damage) {
		health -= damage;

		UIController.Instance.updateHealth(health);
		UIController.Instance.floatText (damage, this.transform);
        GameObject ex = Instantiate (explosionplayer, this.transform.position, this.transform.rotation);
        Object.Destroy(ex, 0.5f);

		if (health <= 0f) {
            Debug.Log("Player is dead");
            Time.timeScale = 0f;
        }
    }

    public void GainEnergy(float amount){
        Debug.Log(amount);
        energy += amount;
        UIController.Instance.updateEnergy(energy);
        //Debug.Log(energy);


    }

     public void GainBolts(float amount){
        bolts += amount;
        UIController.Instance.updateBolts(bolts);
    }

     public void GainHealth(float amount){
        health += amount;
        UIController.Instance.updateHealth(health);
    }




    void OnTriggerEnter2D(Collider2D coll){
        Bolts boltsobj = coll.gameObject.GetComponent<Bolts>();
        if (boltsobj != null) {
            GainBolts(boltsobj.bolts);
            Object.Destroy(coll.gameObject);
        }

        Health healthobj = coll.gameObject.GetComponent<Health>();
        if (healthobj != null) {
            GainHealth(healthobj.health);
            Object.Destroy(coll.gameObject);
        }
        UpgradeToken upgradeToken = coll.gameObject.GetComponent<UpgradeToken>();
        if (upgradeToken != null) {
            UIController.Instance.openUpgradeMenu();
            Object.Destroy(coll.gameObject);
        }
    }

    void DropDrill() {
        if(this.bolts >= drillCost){
            Debug.Log("Dropping drill");
            GameObject newObject = Instantiate (this.drill, UIController.Instance.transform);
            newObject.transform.position = this.transform.position;
            this.bolts -= drillCost;
            UIController.Instance.updateBolts(this.bolts);
            GainEnergy(-drillCost);
        }
        else{
            Debug.Log("Not enough bolts to drop drill"); //Implement something for the player to see
        }

    }
}
