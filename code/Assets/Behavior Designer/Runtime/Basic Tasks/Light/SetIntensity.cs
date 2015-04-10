using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityLight
{
    [TaskCategory("Basic/Light")]
    [TaskDescription("Sets the intensity of the light.")]
    public class SetIntensity : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The intensity to set")]
        public SharedFloat intensity;

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

            targetLight.intensity = intensity.Value;
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            intensity = 0;
        }
    }
}