using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robocop : Enemy {

    public float rotationSpeed = 90f;

	public override void MoveEnemy (){
		//this.transform.position = Vector3.MoveTowards(this.transform.position, FindPlayer(), Time.deltaTime * this.moveSpeed);

        float angle = AngleBetweenPoints(transform.position, Player.GetPlayer().transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle + 90f)), Time.deltaTime * rotationSpeed);

        transform.position += transform.up * Time.deltaTime * moveSpeed;
    }


    float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
