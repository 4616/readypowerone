using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strafe : Enemy {
	override public void MoveEnemy (){
		float angle = Vector3.Angle(this.transform.position, FindPlayer());
		Debug.Log("The angle is " + angle);
		//Vector3.MoveTowards(, , Time.deltaTime * this.moveSpeed);
	}
}
