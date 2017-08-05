using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChangeEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;

    public void PlayLevelChange()
    {
        if (_particles != null && !_particles.isPlaying)
        {
            _particles.Play();
        }
    }
}
