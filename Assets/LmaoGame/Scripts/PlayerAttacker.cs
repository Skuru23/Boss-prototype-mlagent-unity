using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class PlayerAttacker : MonoBehaviour
    {   
        AnimationHandler animationHandler;
        public void Awake()
        {
            animationHandler = GetComponentInChildren<AnimationHandler>();

        }
        public void HandleLightAttack(WeaponItem weap)
        {
            animationHandler.PlayTargetAnim(weap.OH_lightAttack_1, true);
        }
    }
}