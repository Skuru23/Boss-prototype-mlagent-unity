using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerInventory : MonoBehaviour
    {   
        WeaponSlotManager weaponSlotManager;
        public WeaponItem rightWeap;
        public WeaponItem leftWeap;

        public void Awake(){
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            weaponSlotManager.LoadWeaponOnSlot(rightWeap, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeap, true);
        }
    }

}