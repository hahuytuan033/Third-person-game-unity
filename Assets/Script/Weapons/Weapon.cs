using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Tundayne/Weapons/Weapon", fileName = "Weapon", order =0)]
    public class Weapon : ScriptableObject
    {
        public string id;

        public IKPositions ikPos;
        public GameObject modelPrefab;

        public float fireRate = .1f;
        public int magizineAmmo = 30;
        public int maxAmmo = 160;
        public AnimationCurve recoilY;
        public AnimationCurve recoilZ;

    }
}

