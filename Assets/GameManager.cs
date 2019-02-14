using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeap;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap
{
    public class GameManager : MonoBehaviour
    {

        public GameObject targetPrefab;
        public GameObject mainMLCamera;
        public GameObject laserParticles;
        public GameObject laserBeam;

        public TextMesh scoreUI;
        public TextMesh powerUI;

        private MLInputController _controller;


        //Number of targets that can be on screen at one time
        public int targetAmount = 1;

        public float spawnRate = 10f;

        //Private
        public List<GameObject> Targets;
        private float _timeLeft;
        private int _score;

        // Use this for initialization
        void Start()
        {
            Targets = new List<GameObject>(targetAmount);

            _timeLeft = spawnRate;

            _score = 0;

            _controller = MLInput.GetController(MLInput.Hand.Left);



        }

        // Update is called once per frame
        void Update()
        {
            //Basic spawn
            _timeLeft -= Time.deltaTime;
            if (_timeLeft < 0 && Targets.Count <= targetAmount)
            {
                Spawn();
                _timeLeft = spawnRate;
            }
            if (_controller.TriggerValue > 0.2f)
            {
                laserParticles.GetComponent<laserController>().IsFiring = true;
                laserBeam.GetComponent<Renderer>().enabled = true;
            }
            else
            {
                laserParticles.GetComponent<laserController>().IsFiring = false;
                laserBeam.GetComponent<Renderer>().enabled = false;

            }
            Targets.RemoveAll(item => item == null);
            RaycastHit hit;
            if (Physics.Raycast(laserParticles.GetComponent<laserController>().OriginLocation.position, laserParticles.GetComponent<laserController>().DestinationLocation.position - laserParticles.GetComponent<laserController>().OriginLocation.position, out hit))
            {
                switch (hit.transform.gameObject.tag)
                {
                    case "Target":
                        //Output message
                        //print("target detected");
                        Destroy(hit.transform.gameObject);
                        break;
                }
            }
        }

        public void OnRaycastHit(MLWorldRays.MLWorldRaycastResultState state, RaycastHit result, float confidence)
        {
            //Code for damage
            //if (state != MLWorldRays.MLWorldRaycastResultState.RequestFailed && state != MLWorldRays.MLWorldRaycastResultState.NoCollision)
            //{
            //    if (result.transform != null)
            //    {
            //        GameObject hitObject = result.transform.gameObject;
            //        if (hitObject.tag == "Target")
            //        {
            //            Destroy(hitObject);

            //            //Update score/UI
            //            _score++;
            //            scoreUI.text = "Score: " + _score;
            //        }
            //    }
            //}
        }
        void Spawn()
        {
            //Works out a random spot near the player's start pos for now
            float locX = Random.Range(0.1f, 3.5f);
            float locY = Random.Range(0.1f, 2.5f);
            float locZ = Random.Range(0.1f, 3.5f);
            //locX = locX + mainMLCamera.transform.position.x;
            //locY = locY + mainMLCamera.transform.position.y;
            //locZ = locZ + mainMLCamera.transform.position.z;

            Vector3 spawnPos = new Vector3(locX, locY, locZ);


            GameObject newTarget = Instantiate(targetPrefab);
            newTarget.transform.position = mainMLCamera.transform.position + spawnPos;
            var lookAtLocation = mainMLCamera.transform.position;
            newTarget.transform.LookAt(lookAtLocation,mainMLCamera.transform.up);
            Targets.Add(newTarget);
        }
    }
}
