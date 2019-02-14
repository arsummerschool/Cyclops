using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserController : MonoBehaviour
{
    public bool IsFiring 
        { 
            get { return _isFiring; }
            set { SetFiring(value); }
        }
    private bool _isFiring;
    public Transform OriginLocation;
    public Transform DestinationLocation;

    public Transform OriginObject;
    public Transform DestinationObject;

    public ParticleSystem[] Particles;
    public Light[] Lights;

    // Start is called before the first frame update
    void Start()
    {
        IsFiring = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (IsFiring)
        {
            OriginObject.position = OriginLocation.position;
            DestinationObject.position = DestinationLocation.position;
        }
    }

    private void SetFiring(bool firing)
    {
        _isFiring = firing;
        foreach (ParticleSystem p in Particles)
        {
            if (_isFiring)
            {
                p.Play();
            }
            else 
            {
                p.Stop();
            }
        }

        foreach (Light l in Lights)
        {
            l.enabled = _isFiring;
        }
    }
}
