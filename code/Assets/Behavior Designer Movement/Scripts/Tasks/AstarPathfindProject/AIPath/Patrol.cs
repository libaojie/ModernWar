using UnityEngine;
using Pathfinding;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject.AIPath
{
    [TaskDescription("Patrol around the specified waypoints using A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project/AIPath")]
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
        [Tooltip("The arrive distance to the current waypoint")]
        public SharedFloat arriveDistance = 1;

        // The current index that we are heading towards within the waypoints array
        private int waypointIndex;
        // The square value of arriveDistance
        private float arriveDistanceSquared;
        // A cache of the AIPath
        private AIPathAgent aiPathAgent;

        public override void OnAwake()
        {
            // cache for quick lookup
            aiPathAgent = gameObject.GetComponent<AIPathAgent>();
            arriveDistanceSquared = arriveDistance.Value * arriveDistance.Value;

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
            aiPathAgent.speed = speed.Value;
            aiPathAgent.turningSpeed = angularSpeed.Value;
            aiPathAgent.target.parent = null;
            aiPathAgent.target.position = Target();
            aiPathAgent.enabled = true;
        }
        
        // Patrol around the different waypoints specified in the waypoint array. Always return a task status of running. 
        public override TaskStatus OnUpdate()
        {
            if (Vector3.SqrMagnitude(transform.position - waypoints.Value[waypointIndex].position) < arriveDistanceSquared) {
                if (randomPatrol.Value) {
                    waypointIndex = Random.Range(0, waypoints.Value.Count);
                } else {
                    waypointIndex = (waypointIndex + 1) % waypoints.Value.Count;
                }
                aiPathAgent.target.position = Target();
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
            // Disable the AIPath
            aiPathAgent.enabled = false;
        }

        // Reset the public variables
        public override void OnReset()
        {
            speed = 0;
            angularSpeed = 0;
            waypoints = null;
            arriveDistance = 1;
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