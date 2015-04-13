using UnityEngine;
using Pathfinding;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject.AIPath
{
    [TaskDescription("Seek the target specified using the A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project/AIPath")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=3")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class Seek : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed;
        [Tooltip("Turn speed of the agent")]
        public SharedFloat angularSpeed;
        [Tooltip("The transform that the agent is moving towards")]
        public SharedTransform targetTransform;
        [Tooltip("If target is null then use the target position")]
        public SharedVector3 targetPosition;

        // True if the target is a transform
        private bool dynamicTarget;
        // A cache of the AIPath
        private AIPathAgent aiPathAgent;

        public override void OnAwake()
        {
            // cache for quick lookup
            aiPathAgent = gameObject.GetComponent<AIPathAgent>();

            var target = new GameObject();
            target.name = Owner.name + " target";
            aiPathAgent.target = target.transform;
        }

        public override void OnStart()
        {
            // the target is dynamic if the target transform is not null and has a valid
            dynamicTarget = (targetTransform != null && targetTransform.Value != null);
            aiPathAgent.target.position = Target();

            // set the speed, angular speed, and destination then enable the agent
            aiPathAgent.speed = speed.Value;
            aiPathAgent.turningSpeed = angularSpeed.Value;
            aiPathAgent.enabled = true;
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            // Update the target position if the target is a transform because that agent could move
            if (dynamicTarget) {
                aiPathAgent.target.position = Target();
            }
            return (aiPathAgent.PathCalculated() && aiPathAgent.TargetReached) ? TaskStatus.Success : TaskStatus.Running;
        }

        // Return targetPosition if targetTransform is null
        private Vector3 Target()
        {
            if (dynamicTarget) {
                return targetTransform.Value.position;
            }
            return targetPosition.Value;
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
            targetTransform = null;
            targetPosition = Vector3.zero;
        }
    }
}