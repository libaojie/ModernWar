using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityRenderer
{
    [TaskCategory("Basic/Renderer")]
    [TaskDescription("Sets the material on the Renderer.")]
    public class SetMaterial : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The material to set")]
        public SharedMaterial material;

        // cache the renderer component
        private Renderer targetRenderer;

        public override void OnStart()
        {
            targetRenderer = GetDefaultGameObject(targetGameObject.Value).GetComponent<Renderer>();
        }

        public override TaskStatus OnUpdate()
        {
            if (targetRenderer == null) {
                Debug.LogWarning("Renderer is null");
                return TaskStatus.Failure;
            }

            targetRenderer.material = material.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            material = null;
        }
    }
}