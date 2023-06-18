using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class Item : ScriptableObject
    {
        [Header("Item Infor")]
        public Sprite itemIcon;
        public string itemName;
    }
}
