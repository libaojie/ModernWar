using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject.RichAI
{
    [TaskDescription("Evade the target specified using the A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project/RichAI")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=6")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}EvadeIcon.png")]
    public class Evade : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed;
        [Tooltip("Turn speed of the agent")]
        public SharedFloat angularSpeed;
        [Tooltip("The agent has fleed when the square magnitude is greater than this value")]
        public SharedFloat fleedDistance = 10;
        [Tooltip("The distance to look ahead when fleeing")]
        public SharedFloat lookAheadDistance = 5;
        [Tooltip("How far to predict the distance ahead of the target. Lower values indicate less distance should be predicated")]
        public SharedFloat targetDistPrediction = 20;
        [Tooltip("Multiplier for predicting the look ahead distance")]
        public SharedFloat targetDistPredictionMult = 20;
        [Tooltip("The transform that the agent is evading")]
        public SharedTransform targetTransform;

        // The position of the target at the last frame
        private Vector3 targetPosition;
        // A cache of the RichAI
        private RichAIAgent richAIAgent;
        // A cache of the RVOController (if used)
        private RVOController rvoController;

        public override void OnAwake()
        {
            // cache for quick lookup
            richAIAgent = gameObject.GetComponent<RichAIAgent>();
            rvoController = gameObject.GetComponent<RVOController>();
        }

        public override void OnStart()
        {
            // set the speed, angular speed, and destination then enable the agent
            richAIAgent.maxSpeed = speed.Value;
            richAIAgent.rotationSpeed = angularSpeed.Value;
            richAIAgent.target.parent = null;
            richAIAgent.target.position = Target();
            richAIAgent.enabled = true;
        }

        // Evade from the target. Return success once the agent has fleed the target by moving far enough away from it
        // Return running if the agent is still fleeing
        public override TaskStatus OnUpdate()
        {
            richAIAgent.target.position = Target();
            return (richAIAgent.PathCalculated() && Vector3.SqrMagnitude(transform.position - TargetPosition()) > fleedDistance.Value) ? TaskStatus.Success : TaskStatus.Running;
        }

        private Vector3 TargetPosition()
        {
            // Calculate the current distance to the target and the current speed
            var distance = (targetTransform.Value.position - transform.position).magnitude;
            var speed = richAIAgent.Velocity.magnitude;

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

        // Evade in the opposite direction
        private Vector3 Target()
        {
            return transform.position + (transform.position - TargetPosition()).normalized * lookAheadDistance.Value;
        }

        public override void OnEnd()
        {
            // Disable the RichAI
            richAIAgent.enabled = false;
            // The RVOController has to explicitly be stopped
            if (rvoController != null) {
                rvoController.Move(Vector3.zero);
            }
        }

        // Reset the public variables
        public override void OnReset()
        {
            speed = 0;
            angularSpeed = 0;
            targetTransform = null;
            fleedDistance = 10;
            lookAheadDistance = 5;
            targetDistPrediction = 20;
            targetDistPredictionMult = 20;
        }
    }
}