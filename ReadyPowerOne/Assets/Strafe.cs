using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strafe : Enemy {
	float hide_range = 3f;
	float hide_range_x;
	float hide_range_y;
	Vector3 rangeOffset;

    public float rotationSpeed = 180f;


    void Start () {
		hide_range_x = hide_range * PositiveNegative();
		hide_range_y = hide_range * PositiveNegative();
		Vector3 rangeOffset = new Vector3(hide_range_x,hide_range_y, 0f);
		//Debug.Log(rangeOffset);



	}

	override public void MoveEnemy (){
        //float angle = Vector3.Angle(this.transform.position, FindPlayer());
        //Debug.Log("The angle is " + angle);
        //this.transform.position = Mathf.MoveTowardsAngle(angle, angle + 90f, Time.deltaTime * this.moveSpeed); 

        float angle = AngleBetweenPoints(transform.position, Player.GetPlayer().transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, 0f, angle + 90f)), Time.deltaTime * rotationSpeed);


        this.transform.position = Vector3.MoveTowards(this.transform.position, FindPlayer() + rangeOffset, Time.deltaTime * this.moveSpeed);
	}

    float AngleBetweenPoints(Vector2 a, Vector2 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }



    float PositiveNegative(){
		float cutpoint = Random.Range(0f,1f);
		//Debug.Log(cutpoint);
		if(cutpoint >= .5){
			return 1f;
		}

		else{
			return -1f;
		}
		
	}
}

