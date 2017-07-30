using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strafe : Enemy {
	override public void MoveEnemy (){
		//float angle = Vector3.Angle(this.transform.position, FindPlayer());
		//Debug.Log("The angle is " + angle);
		//this.transform.position = Mathf.MoveTowardsAngle(angle, angle + 90f, Time.deltaTime * this.moveSpeed); 
		float hide_range = 3f;
		Vector3 rangeOffset = new Vector3(hide_range,hide_range, 0f);
		this.transform.position = Vector3.MoveTowards(this.transform.position, FindPlayer() + rangeOffset, Time.deltaTime * this.moveSpeed);
	}
}
