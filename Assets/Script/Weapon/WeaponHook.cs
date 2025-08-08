using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class WeaponHook : MonoBehaviour
    {
        public Transform leftHandIK;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        ParticleSystem[] _particles;

        void OnEnable()
        {
            _particles = transform.GetComponentsInChildren<ParticleSystem>();
=======
=======
>>>>>>> Stashed changes
        ParticleSystem[] particles;

        void OnEnable()
        {
            particles = transform.GetComponentsInChildren<ParticleSystem>();
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        }

        public void Shoot()
        {
            for (int i = 0; i < particles.Length; i++)
            {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                _particles[i].Play();
=======
                particles[i].Play();
>>>>>>> Stashed changes
=======
                particles[i].Play();
>>>>>>> Stashed changes
            }
        }
    }
}

