using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    public class WeaponHook : MonoBehaviour
    {
        public Transform leftHandIK;
        ParticleSystem[] _particles;

        void OnEnable()
        {
            _particles = transform.GetComponentsInChildren<ParticleSystem>();
        }

        public void Shoot()
        {
            for (int i = 0; i < particles.Length; i++)
            {
                _particles[i].Play();
            }
        }
    }
}

