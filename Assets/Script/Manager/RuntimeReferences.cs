using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne 
{
    [CreateAssetMenu(menuName = "Tundayne/Single Instances/Runtime References", fileName = "RuntimeReferences")]
    public class RuntimeReferences : ScriptableObject
    {
        public List<RuntimeWeapon> runtime_weapons = new List<RuntimeWeapon>();

        public void Init()
        {
            runtime_weapons.Clear();
        }

        public RuntimeWeapon WeaponToRuntimeWeapon(Weapon w)
        {
            RuntimeWeapon rw = new RuntimeWeapon();
            rw.w_actual = w;
            rw.curAmmo = w.magizineAmmo;
            rw.curCrrying = w.maxAmmo;

            runtime_weapons.Add(rw);

            return rw;
        }

        public void RemoveRuntimeWeapon(RuntimeWeapon rw)
        {
            if (rw.m_instance)
            {
                Destroy(rw.m_instance);
            }

            if (runtime_weapons.Contains(rw))
            {
                runtime_weapons.Remove(rw);
            }
        }
    }

    [System.Serializable]
    public class RuntimeWeapon
    {
        public int curAmmo;
        public int curCrrying;
        public GameObject m_instance;
        public WeaponHook w_hook;
        public Weapon w_actual;
    }
}
