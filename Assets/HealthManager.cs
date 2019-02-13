using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    public int health = 100;
	public float timeLimit = 10f;

	void Update(){
		timeLimit -= Time.deltaTime;

		if(timeLimit < 0)
			Destroy(this);
	}
	public int GetHealth(){
		return health;
	}

	public void SetHealth(int health){
		this.health = health;
	}
}
