using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityAudioSource
{
    [TaskCategory("Basic/AudioSource")]
    [TaskDescription("Changes the time at which a sound that has already been scheduled to play will start. Returns Success.")]
    public class SetScheduledStartTime : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("Time in seconds")]
        float time = 0;

        private AudioSource audioSource;

        public override void OnStart()
        {
            audioSource = GetDefaultGameObject(targetGameObject.Value).GetComponent<AudioSource>();
        }

        public override TaskStatus OnUpdate()
        {
            if (audioSource == null) {
                Debug.LogWarning("AudioSource is null");
                return TaskStatus.Failure;
            }

            audioSource.SetScheduledStartTime(time);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            time = 0;
        }
    }
}
