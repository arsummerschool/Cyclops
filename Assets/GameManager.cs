using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeap;
using UnityEngine.XR.MagicLeap;

public class GameManager : MonoBehaviour
{

    public GameObject targetPrefab;
    public GameObject mainMLCamera;
    public GameObject laser;

    public TextMesh scoreUI;
    public TextMesh powerUI;

    //Number of targets that can be on screen at one time
    public int targetAmount = 1;

    public float spawnRate = 10f;

    //Private
    private ArrayList[] _targets;
    private float _timeLeft;
    private int _score;

    // Use this for initialization
    void Start()
    {
        _targets = new ArrayList[targetAmount];

        _timeLeft = spawnRate;

        _score = 0;


    }

    // Update is called once per frame
    void Update()
    {
        //Basic spawn
        _timeLeft -= Time.deltaTime;
        if (_timeLeft < 0 && _targets.Length <= targetAmount)
        {
            Spawn();
            _timeLeft = spawnRate;
        }

        //if (MLHands.IsStarted)
        //{
        //    if (MLHands.Right.KeyPose == MLHandKeyPose.Fist)
        //    {
        //        FireLaser();
        //        //laser.GetComponent<laserController>().IsFiring = true;
        //    }

        //}
    }

    public void OnLaserHit(GameObject target)
    {
        if (target.tag == "Target")
        {
            Destroy(target);

            //Update score/UI
            _score++;
            scoreUI.text = "Score: " + _score;
        }
    }

    void Spawn()
    {
        //Works out a random spot near the player's start pos for now

        float locX = Random.Range(0.1f, 1.5f);
        float locY = Random.Range(2.0f, 2.5f);
        float locZ = Random.Range(1.1f, 3.5f);
        locX = locX + mainMLCamera.transform.position.x;
        locY = locY + mainMLCamera.transform.position.y;
        locZ = locZ + mainMLCamera.transform.position.z;

        Vector3 spawnPos = new Vector3(locX, locY, locZ);

        GameObject newTarget = Instantiate(targetPrefab);
        newTarget.transform.position = spawnPos;
        var lookAtLocation = mainMLCamera.transform.position;
        newTarget.transform.LookAt(lookAtLocation);
    }
}
