using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class WeaponHook : MonoBehaviour
    {
        public Transform leftHandIK;
<<<<<<< Updated upstream
        ParticleSystem[] _particles;

        void OnEnable()
        {
            _particles = transform.GetComponentsInChildren<ParticleSystem>();
=======
        ParticleSystem[] particles;

        void OnEnable()
        {
            particles = transform.GetComponentsInChildren<ParticleSystem>();
>>>>>>> Stashed changes
        }

        public void Shoot()
        {
            for (int i = 0; i < particles.Length; i++)
            {
<<<<<<< Updated upstream
                _particles[i].Play();
=======
                particles[i].Play();
>>>>>>> Stashed changes
            }
        }
    }
}

