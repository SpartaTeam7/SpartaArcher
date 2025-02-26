using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkParticleController : MonoBehaviour
{
    [SerializeField] private bool createEffOnWalk = true;
    [SerializeField] private ParticleSystem walkParticleSystem;

    public void CreateDustParticels()
    {
        if (createEffOnWalk)
        {
            walkParticleSystem.Stop();
            walkParticleSystem.Play();
        }
    }
}
