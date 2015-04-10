using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLight
{
    [TaskCategory("Basic/Light")]
    [TaskDescription("Sets the shadow bias of the light.")]
    public class SetShadowBias : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The shadow bias to set")]
        public SharedFloat shadowBias;

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

            targetLight.shadowBias = shadowBias.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            shadowBias = 0;
        }
    }
}