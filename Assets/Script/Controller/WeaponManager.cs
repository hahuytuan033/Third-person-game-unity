using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    [System.Serializable]
    public class WeaponManager
    {
        public string mainWeapon_id;
        public string secWeapon_id;

        public RuntimeWeapon m_weapon;
        public RuntimeWeapon s_weapon; 
    }
}

