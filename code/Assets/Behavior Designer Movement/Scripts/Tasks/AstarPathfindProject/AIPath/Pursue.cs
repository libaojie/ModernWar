using UnityEngine;
using Pathfinding;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject.AIPath
{
    [TaskDescription("Pursue the target specified using the A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project/AIPath")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=5")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PursueIcon.png")]
    public class Pursue : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed;
        [Tooltip("Turn speed of the agent")]
        public SharedFloat angularSpeed;
        [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
        public SharedFloat targetDistPrediction = 20;
        [Tooltip("Multiplier for predicting the look ahead distance")]
        public SharedFloat targetDistPredictionMult = 20;
        [Tooltip("The transform that the agent is pursuing")]
        public SharedTransform targetTransform;

        // The position of the target at the last frame
        private Vector3 targetPosition;
        // A cache of the AIPath
        private AIPathAgent aiPathAgent;

        public override void OnAwake()
        {
            // cache for quick lookup
            aiPathAgent = gameObject.GetComponent<AIPathAgent>();
        }

        public override void OnStart()
        {
            // set the speed, angular speed, and destination then enable the agent
            aiPathAgent.speed = speed.Value;
            aiPathAgent.turningSpeed = angularSpeed.Value;
            aiPathAgent.target.parent = null;
            aiPathAgent.target.position = Target();
            aiPathAgent.enabled = true;
        }

        // Pursue the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            aiPathAgent.target.position = Target();
            return (aiPathAgent.PathCalculated() && aiPathAgent.TargetReached) ? TaskStatus.Success : TaskStatus.Running;
        }

        // Predict the position of the target
        private Vector3 Target()
        {
            // Calculate the current distance to the target and the current speed
            var distance = (targetTransform.Value.position - transform.position).magnitude;
            var speed = aiPathAgent.Velocity().magnitude;

            float futurePrediction = 0;
            // Set the future prediction to max prediction if the speed is too small to give an accurate prediction
            if (speed <= distance / targetDistPrediction.Value) {
                futurePrediction = targetDistPrediction.Value;
            } else {
                futurePrediction = (distance / speed) * targetDistPredictionMult.Value; // the prediction should be accurate enough
            }

            // Predict the future by taking the velocity of the target and multiply it by the future prediction
            var prevTargetPosition = targetPosition;
            targetPosition = targetTransform.Value.position;

            return targetPosition + (targetPosition - prevTargetPosition) * futurePrediction;
        }

        public override void OnEnd()
        {
            // Disable the AIPath
            aiPathAgent.enabled = false;
        }

        // Reset the public variables
        public override void OnReset()
        {
            speed = 0;
            angularSpeed = 0;
            targetDistPrediction = 20;
            targetDistPredictionMult = 20;
        }
    }
}