using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlintController : MonoBehaviour {
	// Public
	public List<Transform> Trackables;
	public bool DisplayGlints;
	public ParticleSystem Glint;

	// Private
	// While visibleTrackables is true, show no glints
	private bool _visibleTrackables = false;
	private float _maximumAngle = 60f;

	// Use this for initialization
	void Start () {
		Trackables = new List<Transform>();
		DisplayGlints = true;
	}
	
	// Update is called once per frame
	void Update () {

		if (DisplayGlints)
		{
			checkTrackables();
			
		}
		
	}

	private void checkTrackables()
	{
        _visibleTrackables = false;
        float smallestDifference = 180;

        //Remove all null entries
        Trackables.RemoveAll(item => item == null);

		foreach (Transform trans in Trackables)
		{

            Vector3 d = trans.position - transform.position;
            float angleDifference = Vector3.SignedAngle(d, transform.forward, Vector3.up);
            if (angleDifference < _maximumAngle && angleDifference > -_maximumAngle)
            {
                _visibleTrackables = true;
            }
            if (Mathf.Abs(angleDifference) < smallestDifference)
            {
                smallestDifference = angleDifference;
            }
        
		}
        //Debug.Log(smallestDifference);
        // If none of the trackable objects are visible in the scene, display a glint
        if (!_visibleTrackables && Trackables.Count > 0)
        {
            if (smallestDifference > 0)
            {
                Glint.transform.localEulerAngles = Vector3.down * 90;
            }
            else
            {
                Glint.transform.localEulerAngles = Vector3.up * 90;
            }

            if (Glint.isStopped) { Glint.Play(); }
        }
        else
        {
            Glint.Stop();
        }
        
    }
}
