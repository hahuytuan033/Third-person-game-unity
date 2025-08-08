using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Tundayne/Weapons/Weapon", fileName = "Weapon", order =0)]
    public class Weapon : ScriptableObject
    {
        public string id;

        public IKPositions m_h_ik;
        public GameObject modelPrefab;

        public float fireRate = 0.1f;
        public int magizineAmmo = 30;
        public int maxAmmo = 160;

        public AnimationCurve recoilY;
        public AnimationCurve recoilX;

    }
}

