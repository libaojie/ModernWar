using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject.RichAI
{
    [TaskDescription("Find a place to hide and move to it.")]
    [TaskCategory("Movement/A* Pathfinding Project/RichAI")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=8")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CoverIcon.png")]
    public class Cover : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed;
        [Tooltip("Turn speed of the agent")]
        public SharedFloat angularSpeed;
        [Tooltip("The distance to search for cover")]
        public SharedFloat maxCoverDistance = 1000;
        [Tooltip("The layermask of the available cover positions")]
        public LayerMask availableLayerCovers;
        [Tooltip("The maximum number of raycasts that should be fired before the agent gives up looking for an agent to find cover behind")]
        public SharedInt maxRaycasts = 100;
        [Tooltip("How large the step should be between raycasts")]
        public SharedFloat rayStep = 1;
        [Tooltip("Once a cover point has been found, multiply this offset by the normal to prevent the agent from hugging the wall")]
        public SharedFloat coverOffset = 2;
        [Tooltip("Should the agent look at the cover point after it has arrived?")]
        public SharedBool lookAtCoverPoint = false;
        [Tooltip("The agent is done rotating to the cover point when the square magnitude is less than this value")]
        public SharedFloat rotationEpsilon = 0.5f;
        [Tooltip("Max rotation delta if lookAtCoverPoint")]
        public SharedFloat maxLookAtRotationDelta;

        // The cover position
        private Vector3 coverPoint;
        // The position to reach, offsetted from coverPoint
        private Vector3 coverTarget;
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
            RaycastHit hit;
            int raycastCount = 0;
            var direction = transform.forward;
            float step = 0;
            // Keep firing a ray until too many rays have been fired
            while (raycastCount < maxRaycasts.Value) {
                var ray = new Ray(transform.position, direction);
                if (Physics.Raycast(ray, out hit, maxCoverDistance.Value, availableLayerCovers)) {
                    // A suitable agent has been found. Find the opposite side of that agent by shooting a ray in the opposite direction from a point far away
                    if (hit.collider.Raycast(new Ray(hit.point - hit.normal * maxCoverDistance.Value, hit.normal), out hit, Mathf.Infinity)) {
                        coverPoint = hit.point;
                        coverTarget = hit.point + hit.normal * coverOffset.Value;
                        break;
                    }
                }
                // Keep sweeiping along the y axis
                step += rayStep.Value;
                direction = Quaternion.Euler(0, transform.eulerAngles.y + step, 0) * Vector3.forward;
                raycastCount++;
            }

            // set the speed, angular speed, and destination then enable the agent
            richAIAgent.maxSpeed = speed.Value;
            richAIAgent.rotationSpeed = angularSpeed.Value;
            richAIAgent.target.parent = null;
            richAIAgent.target.position = Target();
            richAIAgent.enabled = true;
        }

        // Seek to the cover point. Return success as soon as the location is reached or the agent is looking at the cover point
        public override TaskStatus OnUpdate()
        {
            if (richAIAgent.PathCalculated() && richAIAgent.TargetReached) {
                var rotation = Quaternion.LookRotation(coverPoint - transform.position);
                // Return success if the agent isn't going to look at the cover point or it has completely rotated to look at the cover point
                if (!lookAtCoverPoint.Value || Quaternion.Angle(transform.rotation, rotation) < rotationEpsilon.Value) {
                    return TaskStatus.Success;
                } else {
                    // Still needs to rotate towards the target
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxLookAtRotationDelta.Value);
                }
            }

            return TaskStatus.Running;
        }

        // The target has already been determined. Return the value
        private Vector3 Target()
        {
            return coverTarget;
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
            if (speed != null) {
                speed.Value = 0;
            }
            if (angularSpeed != null) {
                angularSpeed.Value = 0;
            }
            maxCoverDistance = 1000;
            maxRaycasts = 100;
            rayStep = 1;
            coverOffset = 2;
            lookAtCoverPoint = false;
            rotationEpsilon = 0.5f;
        }
    }
}