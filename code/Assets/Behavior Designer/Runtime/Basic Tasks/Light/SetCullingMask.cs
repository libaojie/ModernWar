using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLight
{
    [TaskCategory("Basic/Light")]
    [TaskDescription("Sets the culling mask of the light.")]
    public class SetCullingMask : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The culling mask to set")]
        public LayerMask cullingMask;

        // cache the light component
        private Light targetLight;

        public override void OnStart()
        {
            targetLight = GetDefaultGameObject(targetGameObject.Value).GetComponent<Light>();
        }

        public override TaskStatus OnUpdate()
        {
            if (targetLight == null) {
                Debug.LogWarning("Light is null");
                return TaskStatus.Failure;
            }

            targetLight.cullingMask = cullingMask.value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            cullingMask = -1;
        }
    }
}