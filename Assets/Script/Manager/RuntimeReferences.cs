using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Tundayne/Single Instancee/Runtime References", fileName = "RuntimeReferences")]
    public class RuntimeReferences : ScriptableObject
    {
        public List<RuntimeWeapon> runtimeWeapons = new List<RuntimeWeapon>();

        public void Init()
        {
            runtimeWeapons.Clear();

        }

        public RuntimeWeapon WeaponToRuntimeWeapon(Weapon weapon)
        {
            RuntimeWeapon rw = new RuntimeWeapon();
            rw.weaponActual = weapon;
            rw.curAmmo = weapon.magizineAmmo;
            rw.curCarrying = weapon.maxAmmo;
            runtimeWeapons.Add(rw);
            return rw;
        }

        public void RemoveRuntimeWeapon(RuntimeWeapon rw)
        {
            if (rw.m_instance)
            {
                Destroy(rw.m_instance);
            }
            if (runtimeWeapons.Contains(rw))
            {
                runtimeWeapons.Remove(rw);
            }
        }
    }

    [System.Serializable]
    public class RuntimeWeapon
    {
        public int curAmmo;
        public int curCarrying;
        public GameObject m_instance;
        public WeaponHook weaponHook;
        public Weapon weaponActual;
    }
}

