using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SG
{
    public class PlayerLocomotion : MonoBehaviour
    {
        PlayerManager playerManager;
        Transform cameraObj;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTrans;


        public AnimationHandler aniHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground & Air Detection Stats")]
        [SerializeField]
        float groundDetecRayStartPoint = 0.5f;
        [SerializeField]
        float minDisToFall = 1f;
        [SerializeField]
        float groundDirectRayDis = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField]
        float moveSpeed = 5f;
        [SerializeField]
        float rotationSpeed = 10;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float fallingSpeed = 45;
        [SerializeField]
        float walkingSpeed = 3;

        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            aniHandler = GetComponentInChildren<AnimationHandler>();
            cameraObj = Camera.main.transform;
            myTrans = transform;
            aniHandler.Initialize();

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);
        }


        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        private void HandlerRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float moveOverride = inputHandler.moveAmount;

            targetDir = cameraObj.forward * inputHandler.vectical;
            targetDir += cameraObj.right * inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = myTrans.forward;
            }

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRo = Quaternion.Slerp(myTrans.rotation, tr, rs * delta);

            myTrans.rotation = targetRo;
        }
        #endregion

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
                return;
            
            if(playerManager.isInteracting)
                return;
            moveDirection = cameraObj.forward * inputHandler.vectical;
            moveDirection += cameraObj.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = moveSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
            }
            else
            {   
                if(inputHandler.moveAmount < 0.5){
                moveDirection *= walkingSpeed;
                playerManager.isSprinting = false;
                }
                else 
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false; 
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            aniHandler.UpdateAnimatorValue(inputHandler.moveAmount, 0, playerManager.isSprinting);

            if (aniHandler.canRotate)
            {
                HandlerRotation(delta);
            }
        }

        public void HandleRollAndSprint(float delta)
        {
            if (aniHandler.anim.GetBool("isInteracting"))
                return;

            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObj.forward * inputHandler.vectical;
                moveDirection += cameraObj.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    inputHandler.rollFlag = false;
                    aniHandler.PlayTargetAnim("RollForward", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTrans.rotation = rollRotation;

                }
                else
                {
                    inputHandler.rollFlag = false;
                    aniHandler.PlayTargetAnim("RollBackward", true);

                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirect)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTrans.position;
            origin.y += groundDetecRayStartPoint;

            if(Physics.Raycast(origin, myTrans.forward, out hit, 0.4f))
            {
                moveDirect = Vector3.zero;
            }

            if(playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirect * fallingSpeed / 10f);
            }

            Vector3 dir = moveDirect;
            dir.Normalize();
            origin += dir*groundDirectRayDis;

            targetPosition = myTrans.position;

            Debug.DrawRay(origin, -Vector3.up * minDisToFall, Color.red, 0.1f, false);
            if(Physics.Raycast(origin, -Vector3.up, out hit, minDisToFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if(playerManager.isInAir)
                {
                    if(inAirTimer > 0.5f)
                    {
                        Debug.Log("You are in air for" + inAirTimer);
                        aniHandler.PlayTargetAnim("Land", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        aniHandler.PlayTargetAnim("Empty", false);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if(playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if(!playerManager.isInAir)
                {
                    if(!playerManager.isInteracting)
                    {
                        aniHandler.PlayTargetAnim("Falling", true);
                    }

                    Vector3 ve1 = rigidbody.velocity;
                    ve1.Normalize();
                    rigidbody.velocity = ve1*( moveSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if(playerManager.isGrounded){
                if(playerManager.isInteracting || inputHandler.moveAmount == 0)
                {
                    myTrans.position = Vector3.Lerp(myTrans.position, targetPosition, Time.deltaTime);

                }
                else
                {
                    myTrans.position = targetPosition;
                }
            }
        }
    }

}