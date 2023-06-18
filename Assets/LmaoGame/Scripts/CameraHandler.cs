using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform targetTrans;
        public Transform camTrans;
        public Transform camPivotTrans;
        private Transform myTrans;
        private Vector3 camTransPos;
        private LayerMask ignoreLayers;
        private Vector3 cameraFollowVelecity = Vector3.zero;

        public static CameraHandler singleton;

        public float lookSpeed = 0.1f;
        public float followSpeed = 0.1f;
        public float pivotSpeed = 0.1f;

        private float targetPos;
        private float defaultPos;
        private float lookAngle;
        private float pivotAngle;
        private float minPivot = -35;
        private float maxPivot = 35;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minCollisionOffset =0.2f;

        private void Awake(){
            singleton = this;
            myTrans = transform;
            defaultPos = camTrans.localPosition.z;
            ignoreLayers = ~(1 << 8 | 1 << 9 | 1 << 10);
            targetTrans = FindObjectOfType<PlayerManager>().transform;
        }

        public void FollowTarget(float delta){
            Vector3 targetPos = Vector3.SmoothDamp(
                myTrans.position, targetTrans.position, ref cameraFollowVelecity, delta / followSpeed);
            myTrans.position = targetPos;

            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput){
            lookAngle += (mouseXInput * lookSpeed) / delta;
            pivotAngle -= (mouseYInput * pivotSpeed) / delta;
            pivotAngle = Mathf.Clamp(pivotAngle, minPivot, maxPivot);

            Vector3 rotation = Vector3.zero;
            rotation.y = lookAngle;
            Quaternion targetRotation = Quaternion.Euler(rotation);
            myTrans.rotation = targetRotation;

            rotation = Vector3.zero;
            rotation.x = pivotAngle;

            targetRotation = Quaternion.Euler(rotation);
            camPivotTrans.localRotation = targetRotation;
        }

        private void HandleCameraCollisions(float delta){
            targetPos = defaultPos;
            RaycastHit hit;
            Vector3 dir = camTrans.position - camPivotTrans.position;
            dir.Normalize();

            if(Physics.SphereCast(
                camPivotTrans.position, cameraSphereRadius, dir, out hit, Mathf.Abs(targetPos), ignoreLayers
            )){
                float dis = Vector3.Distance(camPivotTrans.position, hit.point);
                targetPos = -(dis - cameraCollisionOffset);
            }

            if(Mathf.Abs(targetPos) < minCollisionOffset){
                targetPos = -minCollisionOffset;
            }

            camTransPos.z = Mathf.Lerp(camTrans.localPosition.z, targetPos, delta / 0.2f);
            camTrans.localPosition = camTransPos;
        }
    }
}