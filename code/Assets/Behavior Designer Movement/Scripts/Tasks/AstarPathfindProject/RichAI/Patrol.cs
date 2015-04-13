using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject.RichAI
{
    [TaskDescription("Patrol around the specified waypoints using A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project/RichAI")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=7")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PatrolIcon.png")]
    public class Patrol : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed;
        [Tooltip("Turn speed of the agent")]
        public SharedFloat angularSpeed;
        [Tooltip("Should the agent patrol the waypoints randomly?")]
        public SharedBool randomPatrol = false;
        [Tooltip("The waypoints to move to")]
        public SharedTransformList waypoints;

        // The current index that we are heading towards within the waypoints array
        private int waypointIndex;
        // A cache of the RichAI
        private RichAIAgent richAIAgent;
        // A cache of the RVOController (if used)
        private RVOController rvoController;

        public override void OnAwake()
        {
            // cache for quick lookup
            richAIAgent = gameObject.GetComponent<RichAIAgent>();
            rvoController = gameObject.GetComponent<RVOController>();

            // initially move towards the closest waypoint
            float distance = Mathf.Infinity;
            float localDistance;
            for (int i = 0; i < waypoints.Value.Count; ++i) {
                if ((localDistance = Vector3.Magnitude(transform.position - waypoints.Value[i].position)) < distance) {
                    distance = localDistance;
                    waypointIndex = i;
                }
            }
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

        // Patrol around the different waypoints specified in the waypoint array. Always return a task status of running. 
        public override TaskStatus OnUpdate()
        {
            if (richAIAgent.PathCalculated() && richAIAgent.TargetReached) {
                if (randomPatrol.Value) {
                    waypointIndex = Random.Range(0, waypoints.Value.Count);
                } else {
                    waypointIndex = (waypointIndex + 1) % waypoints.Value.Count;
                }
                richAIAgent.target.position = Target();
                richAIAgent.UpdatePath();
            }
            return TaskStatus.Running;
        }

        // Return the current waypoint index position
        private Vector3 Target()
        {
            return waypoints.Value[waypointIndex].position;
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
            waypoints = null;
        }

        // Draw a gizmo indicating a patrol 
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (waypoints == null) {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            for (int i = 0; i < waypoints.Value.Count; ++i) {
                UnityEditor.Handles.SphereCap(0, waypoints.Value[i].position, waypoints.Value[i].rotation, 1);
            }
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}