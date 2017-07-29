﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, ICombat {

    public static Player player_ = null;

    public float moveSpeed = 1f;
    public float rotationSpeed = 45f;
    public float energy = 100f;
    public float health = 100f;

    public static Player GetPlayer() {
        return player_;
    }

    public void Start() {
        player_ = this;
    }

    public void Update() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            transform.position += transform.up * Time.deltaTime * moveSpeed;
            energy -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            transform.position -= transform.up * Time.deltaTime * moveSpeed;
            energy -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            transform.Rotate(Vector3.forward, Time.deltaTime * rotationSpeed);
            energy -= Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            transform.Rotate(Vector3.forward, -1f * Time.deltaTime * rotationSpeed);
            energy -= Time.deltaTime;
        }

        if (energy <= 0f) {
            Debug.Log("Out of Energy!");
            Time.timeScale = 0f;
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0f) {
            Debug.Log("Player is dead");
            Time.timeScale = 0f;
        }
    }

    void OnCollisionStay2D(Collision2D coll) {
        if (coll.gameObject.tag == "RechargePoint") {
            energy = Mathf.Min(energy + 1f * Time.deltaTime, 100f);
        }
    }
}
