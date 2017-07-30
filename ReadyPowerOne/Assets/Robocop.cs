using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robocop : Enemy {
	void MoveEnemy (){
		this.transform.position = Vector3.MoveTowards(this.transform.position, FindPlayer(), Time.deltaTime * this.moveSpeed);
	}
}
