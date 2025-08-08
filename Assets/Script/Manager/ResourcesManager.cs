using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< Updated upstream

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Tundayne/Single Instancee/Resources Manager", fileName = "ResourcesManager")]
=======
using UnityEngine.UIElements;

namespace Tundayne
{
    [CreateAssetMenu(menuName = "Tundayne/Single Instances/Resources Manager", fileName = "ResourcesManager")]
>>>>>>> Stashed changes
    public class ResourcesManager : ScriptableObject
    {
        public RuntimeReferences runtime;
        public Weapon[] all_weapons;
<<<<<<< Updated upstream
        Dictionary<string, int> W_dict = new Dictionary<string, int>();
=======
        Dictionary<string, int> w_dict = new Dictionary<string, int>();
>>>>>>> Stashed changes

        public void Init()
        {
            for (int i = 0; i < all_weapons.Length; i++)
            {
<<<<<<< Updated upstream
                if (W_dict.ContainsKey(all_weapons[i].id))
=======
                if (w_dict.ContainsKey(all_weapons[i].id))
>>>>>>> Stashed changes
                {

                }
                else
                {
<<<<<<< Updated upstream
                    W_dict.Add(all_weapons[i].id, i);
=======
                    w_dict.Add(all_weapons[i].id, i);
>>>>>>> Stashed changes
                }
            }
        }

        public Weapon GetWeapon(string id)
        {
            Weapon retVal = null;
            int index = -1;
<<<<<<< Updated upstream
            if (W_dict.TryGetValue(id, out index))
=======
            if (w_dict.TryGetValue(id, out index))
>>>>>>> Stashed changes
            {
                retVal = all_weapons[index];
            }

            return retVal;
        }
    }
}
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
