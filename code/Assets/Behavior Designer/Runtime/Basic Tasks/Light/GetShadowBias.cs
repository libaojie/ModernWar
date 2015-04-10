using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLight
{
    [TaskCategory("Basic/Light")]
    [TaskDescription("Stores the shadow bias of the light.")]
    public class GetShadowBias : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [RequiredField]
        [Tooltip("The shadow bias to store")]
        public SharedFloat storeValue;

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

            storeValue = targetLight.shadowBias;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            storeValue = 0;
        }
    }
}