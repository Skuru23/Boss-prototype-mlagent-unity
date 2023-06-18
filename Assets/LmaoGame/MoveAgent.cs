using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace SG
{
    public class MoveAgent : Agent
    {
        InputHandler inputHandler;
        Rigidbody rBody;
        public Transform Target;


        public bool isGoal;
        public float time;
        public float dis;

        void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            rBody = GetComponent<Rigidbody>();
            inputHandler.OnDisable();
        }

        public override void OnEpisodeBegin()
        {
            isGoal = false;
            // If the Agent fell, zero its momentum
            if (Mathf.Abs(this.transform.localPosition.x) >= 14|| Mathf.Abs(this.transform.localPosition.z) >= 14)
            {
                this.rBody.angularVelocity = Vector3.zero;
                this.rBody.velocity = Vector3.zero;
                this.transform.localPosition = new Vector3(0, 0, 0);
            }

            // Move the target to a new spot
            Target.localPosition = new Vector3(Random.value * 8 - 4,
                                               0,
                                               Random.value * 8 - 4);
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            // Target and Agent positions
            sensor.AddObservation(Target.localPosition);
            sensor.AddObservation(this.transform.localPosition);

            // Agent velocity
            sensor.AddObservation(rBody.velocity.x);
            sensor.AddObservation(rBody.velocity.z);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            Debug.Log(actions.DiscreteActions[0]);
            inputHandler.movementInput.x = (actions.DiscreteActions[0] - 0.5f) * 2;
            inputHandler.movementInput.y = (actions.DiscreteActions[1] - 0.5f) * 2;

            float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);
            dis = distanceToTarget;
            // Reached target
            if (distanceToTarget < 1.4f)
            {
                //floorMeshRenderer.material = winMaterial;
                isGoal = true;
                SetReward(1.0f);
                EndEpisode();
            }

            // Fell off platform
            else if (Mathf.Abs(this.transform.localPosition.x) >= 14|| Mathf.Abs(this.transform.localPosition.z) >= 14)
            {
                //floorMeshRenderer.material = loseMaterial;
                SetReward(-1.0f);
                EndEpisode();
            }
            else
            {
                SetReward(-0.5f);
            }
        }
        public override void Heuristic(in ActionBuffers actionsOut)
        {   
            //inputHandler.OnEnable();
            var discreteAction = actionsOut.DiscreteActions;
            discreteAction[0] = (int)Input.GetAxis("Horizontal");
            discreteAction[1] = (int)Input.GetAxis("Vertical");
        }
    }
}