using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelChangeEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private AudioSource _audio;

    public void PlayLevelChange()
    {
        if (_particles != null && !_particles.isPlaying)
        {
            _particles.Play();
            _audio.PlayOneShot(_audio.clip);
        }
    }
}
