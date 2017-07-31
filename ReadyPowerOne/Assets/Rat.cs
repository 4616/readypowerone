using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy {

    public float rotationTime = 0.5f;
    public float rotationTimeRandom = 0.25f;
    public float rotationTimer = 0;

    public float rotationSpeed = 90f;

    private float currentRotationDirection = 1f;

    protected override void Update() {
        rotationTimer -= Time.deltaTime;
        if(rotationTimer < 0f) {
            rotationTimer = rotationTime + Random.Range(0f, rotationTimeRandom);
            SetDirection();
        }

        //base.Update();
        MoveEnemy();
    }

    public void SetDirection() {
        float angle = AngleBetweenPoints(transform.position, Player.GetPlayer().transform.position);
        float delta = transform.rotation.eulerAngles.z - angle + 90f;

        currentRotationDirection = delta > 0f && delta < 180f ? 1f : -1f;
    }

    float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public override void MoveEnemy() {
        transform.Rotate(new Vector3(0f, 0f, currentRotationDirection * rotationSpeed * Time.deltaTime));
        transform.position += transform.up * Time.deltaTime * moveSpeed;
    }

    void OnCollisionStay2D(Collision2D coll) {
        float angle = AngleBetweenPoints(transform.position, Player.GetPlayer().transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle + 90f)), Time.deltaTime * rotationSpeed);
    }
}
