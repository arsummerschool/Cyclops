using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnDecal : MonoBehaviour {

	// Use this for initialization
    public float timeLimit = 5.0f;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        timeLimit -= Time.deltaTime;
        var mat = gameObject.GetComponent<MeshRenderer>().material;
        Color newColor = mat.color;
        newColor.a -= Time.deltaTime;
        
        //var newScale = transform.localScale - new Vector3();
        //transform.localScale = transform
        mat.color = newColor;
        gameObject.GetComponent<MeshRenderer>().material = mat;
        if (timeLimit < 0)
            Destroy(gameObject);
    }
}
