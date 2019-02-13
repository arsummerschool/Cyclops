using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	
	public GameObject targetPrefab;

	public GameObject scoreUI;
	public GameObject powerUI;
	
	//Number of targets that can be on screen at one time
	public int targetAmount = 1;

	public float spawnRate = 10f;

	//Private
	private ArrayList[] _targets;
	private float _timeLeft;
	public int _score;

	// Use this for initialization
	void Start () {
		 _targets = new ArrayList[targetAmount];

		_timeLeft = spawnRate;
	}
	
	// Update is called once per frame
	void Update () {
		//Basic spawn
		_timeLeft -= Time.deltaTime;
     	if (_timeLeft < 0 && _targets.Length <= targetAmount){
			Spawn();
			_timeLeft = spawnRate;
		 }
	}

	void FireLaser(){
		
		RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
             //Need code for setting the start end of PF
			
			 //Code for damage
			 GameObject hitObject = hit.transform.gameObject;
			 if(hitObject.tag == "Target"){
				 Destroy(hitObject);
				 _score++;
			 }
        }
	}

	void Spawn(){
		//Needs some code for working out the position
		
		float locX = Random.Range(0.5f, 6);
		float locY = Random.Range(0.5f, 6);

		Vector3 spawnPos = new Vector3(locX, locY, 1.0f);

		GameObject newTarget = Instantiate(targetPrefab);
		
		newTarget.transform.position = spawnPos;
	}	
}
