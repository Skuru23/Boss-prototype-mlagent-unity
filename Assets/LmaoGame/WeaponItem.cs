using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{   
    [CreateAssetMenu(menuName = "Items/Weapon")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("One Hand Attack")]
        public string OH_lightAttack_1;
    }
}