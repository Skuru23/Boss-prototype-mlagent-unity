using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class AnimationHandler : MonoBehaviour
    {   
        PlayerManager playerManager;
        public Animator anim;
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;
        int vertical;
        int horizontal;
        public int delay;
        public bool canRotate;

        public void Initialize(){
            playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValue(float verticalMovement, float horizontalMovement, bool isSprinting){
            #region Vertical
            float v = 0;
            
            if(verticalMovement > 0 && verticalMovement < 0.55f){
                v = 0.5f;
            }

            else if(verticalMovement > 0.55f){
                v = 1;
            }

            else if(verticalMovement < 0 && verticalMovement > -0.55f){
                v = -0.5f;
            }

            else if(verticalMovement < -0.55){
                v = -1;
            }
            else v = 0;
            #endregion

            #region Horizontal
            float h = 0; 
            if(horizontalMovement > 0 && horizontalMovement < 0.55f){
                h = 0.5f;
            }

            else if(horizontalMovement > 0.55f){
                h = 1;
            }

            else if(horizontalMovement < 0 && horizontalMovement > -0.55f){
                h = -0.5f;
            }

            else if(horizontalMovement < -0.55){
                h = -1;
            }
            else h = 0;
            #endregion

            if(isSprinting){
                v=2;
                h=horizontalMovement;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void PlayTargetAnim(string targetAnim, bool isInteracting){
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.CrossFade(targetAnim, 0.2f); 
        }
        public void CanRotate(){
            canRotate = true;
        }

        public void StopRotate(){
            canRotate = false;
        }

        public void OnAnimatorMove(){
            if(playerManager.isInteracting == false)
            return; 
            
            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = delay;
            Vector3 deltaPos = anim.deltaPosition;
            deltaPos.y = 0;
            Vector3 velocity = deltaPos / delta;
            playerLocomotion.rigidbody.velocity = velocity;
        }
    }
}

