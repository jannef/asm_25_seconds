using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleaner : MonoBehaviour
{
    private ParticleSystem _particles;

    private void Awake()
    {
        _particles = GetComponent<ParticleSystem>();
        if (_particles == null)
        {
            Debug.LogWarning("ParticleCleaner attached to a game object with no particle system. Removing the ParticleCleaner now!");
            DestroyImmediate(this);
        }
    }

    private void Update()
    {
        if (!_particles.IsAlive())
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
