using UnityEngine;
using Pathfinding;
using Pathfinding.RVO;

namespace BehaviorDesigner.Runtime.Tasks.Movement.AstarPathfindingProject.RichAI
{
    [TaskDescription("Flock around the scene using the A* Pathfinding Project.")]
    [TaskCategory("Movement/A* Pathfinding Project/RichAI")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=13")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}FlockIcon.png")]
    public class Flock : Action
    {
        [Tooltip("Agents less than this distance apart are neighbors")]
        public SharedFloat neighborDistance = 100;
        [Tooltip("How far the agent should look ahead when determine its pathfinding destination")]
        public SharedFloat lookAheadDistance = 5;
        [Tooltip("The greater the alignmentWeight is the more likely it is that the agents will be facing the same direction")]
        public SharedFloat alignmentWeight = 0.4f;
        [Tooltip("The greater the cohesionWeight is the more likely it is that the agents will be moving towards a common position")]
        public SharedFloat cohesionWeight = 0.5f;
        [Tooltip("The greater the separationWeight is the more likely it is that the agents will be separated")]
        public SharedFloat separationWeight = 0.6f;
        [Tooltip("All of the agents that should be following")]
        public RichAIAgent[] agents = null;

        // The corresponding transforms of the agents
        private Transform[] agentTransforms;
        // And the corresponding RVOControllers of the agents (if used)
        private RVOController[] agentRVOControllers;

        public override void OnAwake()
        {
            agentTransforms = new Transform[agents.Length];
            agentRVOControllers = new RVOController[agents.Length];
            // Cache the transform of the agents and set the target parent to null
            for (int i = 0; i < agents.Length; ++i) {
                // create a new target transform if it doesn't already have one
                if (agents[i].target == null) {
                    var go = new GameObject();
                    agents[i].target = go.transform;
                    agents[i].target.name = "Target";
                }
                agents[i].target.parent = null;
                agents[i].enabled = true;
                agentTransforms[i] = agents[i].transform;
                agentRVOControllers[i] = agents[i].GetComponent<RVOController>();
            }
        }

        // The agents will always be flocking so always return running
        public override TaskStatus OnUpdate()
        {
            // Determine a destination for each agent
            for (int i = 0; i < agents.Length; ++i) {
                Vector3 alignment, cohesion, separation;
                // determineFlockAttributes will determine which direction to head, which common position to move toward, and how far apart each agent is from one another,
                determineFlockParameters(i, out alignment, out cohesion, out separation);
                // Weigh each parameter to give one more of an influence than another
                var velocity = alignment * alignmentWeight.Value + cohesion * cohesionWeight.Value + separation * separationWeight.Value;
                // Set the destination based on the velocity multiplied by the look ahead distance
                agents[i].target.position = agentTransforms[i].position + velocity * lookAheadDistance.Value;
            }
            return TaskStatus.Running;
        }

        // Determine the three flock parameters: alignment, cohesion, and separation.
        // Alignment: determines which direction to move
        // Cohesion: Determines a common position to move towards
        // Separation: Determines how far apart the agent is from all other agents
        private void determineFlockParameters(int agentIndex, out Vector3 alignment, out Vector3 cohesion, out Vector3 separation)
        {
            alignment = cohesion = separation = Vector3.zero;
            int neighborCount = 0;
            var agentTransform = agentTransforms[agentIndex];
            // Loop through each agent to determine the alignment, cohesion, and separation
            for (int i = 0; i < agents.Length; ++i) {
                // The agent can't compare against itself
                if (agentIndex != i) {
                    // Only determine the parameters if the other agent is its neighbor
                    if (Vector3.SqrMagnitude(agentTransforms[i].position - agentTransform.position) < neighborDistance.Value) {
                        // This agent is the neighbor of the original agent so add the alignment, cohesion, and separation
                        alignment += agents[i].Velocity;
                        cohesion += agentTransforms[i].position;
                        separation += agentTransforms[i].position - agentTransform.position;
                        neighborCount++;
                    }
                }
            }

            // Don't move if there are no neighbors
            if (neighborCount == 0) {
                return;
            }
            // Normalize all of the values
            alignment = (alignment / neighborCount).normalized;
            cohesion = ((cohesion / neighborCount) - agentTransform.position).normalized;
            separation = ((separation / neighborCount) * -1).normalized;
        }

        // Disable the agents
        public override void OnEnd()
        {
            for (int i = 0; i < agents.Length; ++i) {
                agents[i].enabled = false;
                // The RVOController has to explicitly be stopped
                if (agentRVOControllers[i] != null) {
                    agentRVOControllers[i].Move(Vector3.zero);
                }
            }
        }

        // Reset the public variables
        public override void OnReset()
        {
            neighborDistance = 100;
            lookAheadDistance = 5;
            alignmentWeight = 0.4f;
            cohesionWeight = 0.5f;
            separationWeight = 0.6f;
        }
    }
}