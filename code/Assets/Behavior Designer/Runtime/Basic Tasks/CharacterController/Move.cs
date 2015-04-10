using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityCharacterController
{
    [TaskCategory("Basic/CharacterController")]
    [TaskDescription("A more complex move function taking absolute movement deltas. Returns Success.")]
    public class Move : Action
    {
        [Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
        public SharedGameObject targetGameObject;
        [Tooltip("The amount to move")]
        public SharedVector3 motion;

        private CharacterController characterController;

        public override void OnStart()
        {
            characterController = GetDefaultGameObject(targetGameObject.Value).GetComponent<CharacterController>();
        }

        public override TaskStatus OnUpdate()
        {
            if (characterController == null) {
                Debug.LogWarning("CharacterController is null");
                return TaskStatus.Failure;
            }

            characterController.Move(motion.Value);

            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            motion = Vector3.zero;
        }
    }
}