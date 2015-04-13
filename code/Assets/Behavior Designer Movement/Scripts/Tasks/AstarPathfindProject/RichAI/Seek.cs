using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject.RichAI
{
    [TaskDescription("Seek the target specified using the A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project/RichAI")]
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
            // the target is dynamic if the target transform is not null and has a valid
            dynamicTarget = (targetTransform != null && targetTransform.Value != null);
            var target = new GameObject();
            target.name = Owner.name + " target";
            richAIAgent.target = target.transform;
            richAIAgent.target.position = Target();

            // set the speed, angular speed, and destination then enable the agent
            richAIAgent.maxSpeed = speed.Value;
            richAIAgent.rotationSpeed = angularSpeed.Value;
            richAIAgent.enabled = true;
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            // Update the target position if the target is a transform because that agent could move
            if (dynamicTarget) {
                richAIAgent.target.position = Target();
            }
            return (richAIAgent.PathCalculated() && richAIAgent.TargetReached) ? TaskStatus.Success : TaskStatus.Running;
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
            // Disable the ai path
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
            targetPosition = Vector3.zero;
        }
    }
}