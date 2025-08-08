using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Tundayne/Single Instancee/Resources Manager", fileName = "ResourcesManager")]
    public class ResourcesManager : ScriptableObject
    {
        public RuntimeReferences runtime;
        public Weapon[] all_weapons;
        Dictionary<string, int> W_dict = new Dictionary<string, int>();

        public void Init()
        {
            for (int i = 0; i < all_weapons.Length; i++)
            {
                if (W_dict.ContainsKey(all_weapons[i].id))
                {

                }
                else
                {
                    W_dict.Add(all_weapons[i].id, i);
                }
            }
        }

        public Weapon GetWeapon(string id)
        {
            Weapon retVal = null;
            int index = -1;
            if (W_dict.TryGetValue(id, out index))
            {
                retVal = all_weapons[index];
            }

            return retVal;
        }
    }
}
