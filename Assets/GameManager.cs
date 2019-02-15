using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeap;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap
{
    public class GameManager : MonoBehaviour
    {

        public List<GameObject> targetPrefab;
        public GameObject mainMLCamera;
        public GameObject laserParticles;
        public GameObject laserBeam;
        public GameObject _explosion;
        public GameObject _smoke;

        public TextMesh scoreUI;
        public TextMesh powerUI;

        public GlintController glintController;

        private MLInputController _controller;
        public AudioSource _ding;
        public AudioSource _error;

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

            //targetPrefab = new List<GameObject>();

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
            //Remove all null entries
            Targets.RemoveAll(item => item == null);
            if (laserParticles.GetComponent<laserController>().IsFiring)
            {
                RaycastHit hit;
                if (Physics.Raycast(laserParticles.GetComponent<laserController>().OriginLocation.position, laserParticles.GetComponent<laserController>().DestinationLocation.position - laserParticles.GetComponent<laserController>().OriginLocation.position, out hit))
                {
                    switch (hit.transform.gameObject.tag)
                    {
                        case "Target":
                            //Output message
                            //print("target detected");
                            if (hit.transform.gameObject.GetComponent<HealthManager>().GetHealth() < 1)
                            {
                                _score++;
                                scoreUI.text = "Score: " + _score;
                                Targets.Remove(hit.transform.gameObject);
                                var explosion = Instantiate(_explosion);
                                explosion.transform.position = hit.transform.gameObject.transform.position;                                
                                Destroy(hit.transform.gameObject);
                                _ding.Play();
                            }
                            else
                            {
                                hit.transform.gameObject.GetComponent<HealthManager>().SetHealth(hit.transform.gameObject.GetComponent<HealthManager>().GetHealth() - 1);
                                var originalColor = hit.transform.gameObject.GetComponentInChildren<Material>().color;

                                hit.transform.gameObject.GetComponentInChildren<Material>().color = Color.red;
                                hit.transform.gameObject.GetComponentInChildren<Material>().color = originalColor;

                            }

                            break;
                        case "Kiwi":
                            //Output message
                            //print("target detected");
                            if (hit.transform.gameObject.GetComponent<HealthManager>().GetHealth() < 1)
                            {
                                _score--;
                                scoreUI.text = "Score: " + _score;
                                Targets.Remove(hit.transform.gameObject);
                                var explosion = Instantiate(_explosion);
                                explosion.transform.position = hit.transform.gameObject.transform.position;
                                Destroy(hit.transform.gameObject);
                                _error.Play();
                            }
                            else
                            {
                                hit.transform.gameObject.GetComponent<HealthManager>().SetHealth(hit.transform.gameObject.GetComponent<HealthManager>().GetHealth() - 1);
                                var originalColor = hit.transform.gameObject.GetComponentInChildren<Material>().color;

                                hit.transform.gameObject.GetComponentInChildren<Material>().color = Color.red;
                                hit.transform.gameObject.GetComponentInChildren<Material>().color = originalColor;

                            }

                            break;
                    }
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
            float locX = Random.Range(-2.5f, 2.5f);
            float locY = Random.Range(0.1f, 1.5f);
            float locZ = Random.Range(-2.5f, 2.5f);

            Vector3 spawnPos = new Vector3(locX, locY, locZ);

            var targetType = Random.Range(0, 100);
            if (targetType > 50)
            {
                targetType = 0;
            }
            else if (targetType < 30)
            {
                targetType = 1;
            }
            else if (targetType <= 50)
            {
                targetType = 2;
            }


            GameObject newTarget = Instantiate(targetPrefab[targetType]);

            newTarget.transform.position = mainMLCamera.transform.position + spawnPos;
            var lookAtLocation = mainMLCamera.transform.position;
            newTarget.transform.LookAt(lookAtLocation,mainMLCamera.transform.up);
            Targets.Add(newTarget);
            glintController.Trackables.Add(newTarget.transform);
        }
    }
}
