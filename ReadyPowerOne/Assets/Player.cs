using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICombat {

    public LineRenderer phazer;

    public Cannon cannon;

    public static Player player_ = null;

    public float moveSpeed = 1f;
    public float rotationSpeed = 90f;
    public float energy = 100f;
    public float health = 100f;

    public float phaserCost = 2f;

    public static Player GetPlayer() {
        return player_;
    }

    public void Start() {
        player_ = this;
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

    public void Update() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            transform.position += Vector3.up * Time.deltaTime * moveSpeed;
            LoseEnergy(Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            transform.position += Vector3.down * Time.deltaTime * moveSpeed;
            LoseEnergy(Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += Vector3.left * Time.deltaTime * moveSpeed;
            LoseEnergy(Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.position += Vector3.right * Time.deltaTime * moveSpeed;
            LoseEnergy(Time.deltaTime);
        }

        float angle = AngleBetweenPoints(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f));
        //transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + 90f));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle + 90f)), Time.deltaTime * rotationSpeed);
        
        if (Input.GetMouseButton(1)) {
            Phaser();
        } else {
            phazer.gameObject.SetActive(false);
        }
        if (cannon.CanShoot() && (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt) || Input.GetMouseButton(0))) {
            LoseEnergy(cannon.energyCost);
            cannon.Shoot();
        }

        if (energy <= 0f) {
            Debug.Log("Out of Energy!");
            Time.timeScale = 0f;
        }
    }

    float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void LoseEnergy (float amount) {
        energy -= amount;
		UIController.Instance.updateEnergy(energy);
    }

    public void Phaser() {
        LoseEnergy(phaserCost * Time.deltaTime);
        phazer.gameObject.SetActive(true);
        for (int i = 1; i < phazer.positionCount; i++) {
            phazer.SetPosition(i, new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f) + 2f * i, 0f));
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void TakeDamage(float damage) {
		health -= damage;

		UIController.Instance.updateHealth(health);
		UIController.Instance.floatText (damage, this.transform);
        
		if (health <= 0f) {
            Debug.Log("Player is dead");
            Time.timeScale = 0f;
        }
    }

    public void GainEnergy(float amount){
        energy += amount;
        UIController.Instance.updateEnergy(energy);
    }

    void OnCollisionStay2D(Collision2D coll) {
        Drill drill = coll.gameObject.GetComponent<Drill>();
        if (drill != null) {
            energy = Mathf.Min(energy + 1f * Time.deltaTime, 100f);
        }
    }
}
