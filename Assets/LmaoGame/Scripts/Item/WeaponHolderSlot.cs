using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public GameObject currentWeapModel;

        public void UnloadWeap()
        {
            if (currentWeapModel != null)
            {
                currentWeapModel.SetActive(false);
            }
        }

        public void UnloadWeapAndDestroy()
        {
            if (currentWeapModel != null)
            {
                Destroy(currentWeapModel);
            }
        }
        public void LoadWeapModel(WeaponItem weaponItem)
        {
            UnloadWeapAndDestroy();

            if (weaponItem == null)
            {
                UnloadWeap();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
            if (model != null)
            {
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                }
                else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }
            currentWeapModel = model;
        }
    }
}