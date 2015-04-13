using UnityEngine;
using Pathfinding;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject.AIPath
{
    [TaskDescription("Queue in a line with the A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project/AIPath")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=15")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}QueueIcon.png")]
    public class Queue : Action
    {
        [Tooltip("Agents less than this distance apart are neighbors")]
        public SharedFloat neighborDistance = 10;
        [Tooltip("The distance that the agents should be separated")]
        public SharedFloat separationDistance = 2;
        [Tooltip("The distance the the agent should look ahead to see if another agent is in the way")]
        public SharedFloat maxQueueAheadDistance = 2;
        [Tooltip("The radius that the agent should check to see if another agent is in the way")]
        public SharedFloat maxQueueRadius = 20;
        [Tooltip("The multiplier to slow down if an agent is in front of the current agent")]
        public SharedFloat slowDownSpeed = 0.15f;
        [Tooltip("The target to see towards")]
        public SharedTransform seekPosition;
        [Tooltip("All of the agents that should be following")]
        public AIPathAgent[] agents = null;

        // The corresponding transforms of the agents
        private Transform[] agentTransforms;

        public override void OnAwake()
        {
            agentTransforms = new Transform[agents.Length];

            // Cache the transform of the agents and set the target parent to null
            for (int i = 0; i < agents.Length; ++i) {
                // create a new target transform if it doesn't already have one
                if (agents[i].target == null) {
                    var go = new GameObject();
                    agents[i].target = go.transform;
                    agents[i].target.name = "Target";
                }
                agentTransforms[i] = agents[i].transform;
            }
        }

        // The agents will always be flocking so always return running
        public override TaskStatus OnUpdate()
        {
            // Determine a destination for each agent
            for (int i = 0; i < agents.Length; ++i) {
                if (AgentAhead(i)) {
                    agents[i].target.position = agentTransforms[i].position + agentTransforms[i].forward * slowDownSpeed.Value + DetermineSeparation(i);
                } else {
                    agents[i].target.position = seekPosition.Value.position;
                }
            }

            return TaskStatus.Running;
        }

        // Returns the agent that is ahead of the current agent
        private AIPathAgent AgentAhead(int agentIndex)
        {
            // queueAhead is the distance in front of the current agent
            var queueAhead = agents[agentIndex].Velocity() * maxQueueAheadDistance.Value;
            for (int i = 0; i < agents.Length; ++i) {
                // Return the first agent that is ahead of the current agent
                if (agentIndex != i && Vector3.SqrMagnitude(queueAhead - agentTransforms[i].position) < maxQueueRadius.Value) {
                    return agents[i];
                }
            }
            return null;
        }

        // Determine the separation between the current agent and all of the other agents also queuing
        private Vector3 DetermineSeparation(int agentIndex)
        {
            var separation = Vector3.zero;
            int neighborCount = 0;
            var agentTransform = agentTransforms[agentIndex];
            // Loop through each agent to determine the separation
            for (int i = 0; i < agents.Length; ++i) {
                // The agent can't compare against itself
                if (agentIndex != i) {
                    // Only determine the parameters if the other agent is its neighbor
                    if (Vector3.SqrMagnitude(agentTransforms[i].position - agentTransform.position) < neighborDistance.Value) {
                        // This agent is the neighbor of the original agent so add the separation
                        separation += agentTransforms[i].position - agentTransform.position;
                        neighborCount++;
                    }
                }
            }

            // Don't move if there are no neighbors
            if (neighborCount == 0) {
                return Vector3.zero;
            }
            // Normalize the value
            return ((separation / neighborCount) * -1).normalized * separationDistance.Value;
        }

        // Reset the public variables
        public override void OnReset()
        {
            neighborDistance = 10;
            separationDistance = 2;
            maxQueueAheadDistance = 2;
            maxQueueRadius = 20;
            slowDownSpeed = 0.15f;
            agents = null;
        }
    }
}