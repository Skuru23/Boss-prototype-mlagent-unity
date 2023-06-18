using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot leftHand;
        WeaponHolderSlot rightHand;
        public bool canDmg;

        DamCollider rightDmgCollider;

        private void Awake()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHand = weaponSlot;
                }
                else if (weaponSlot.isRightHandSlot)
                {
                    rightHand = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                //leftHand.LoadWeapModel(weaponItem);
            }
            else
            {
                rightHand.LoadWeapModel(weaponItem);
                LoadRightWeapDmgCollider();
            }
        }

        #region Handle Weap's Dmg
        private void LoadRightWeapDmgCollider()
        {
            rightDmgCollider = rightHand.currentWeapModel.GetComponentInChildren<DamCollider>();
            if (rightDmgCollider != null)
                canDmg = true;
            else canDmg = false;
        }

        public void OpenRightDmgCollider()
        {
            rightDmgCollider.EnableDmgCollider();
        }

        public void CloseRightDmgCollider()
        {
            rightDmgCollider.DisableDmgCollider();
        }
        #endregion

    }
}