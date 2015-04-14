﻿using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Rotates towards the specified rotation ONLY ON XZ. The rotation can either be specified by a transform or rotation. If the transform "+
	                 "is used then the rotation will not be used.")]
	[TaskCategory("Basic/Custom")]

	[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}RotateTowardsIcon.png")]
	public class RotateTowardsOnXZ : Action
	{
		[Tooltip("The agent is done rotating when the square magnitude is less than this value")]
		public float rotationEpsilon = 0.5f;
		[Tooltip("Max rotation delta")]
		public SharedFloat maxLookAtRotationDelta;
		[Tooltip("The transform that the agent is rotating towards")]
		public SharedTransform targetTransform;
		[Tooltip("If target is null then use the target rotation")]
		public SharedVector3 targetRotation;
		
		public override void OnStart()
		{
			if ((targetTransform == null || targetTransform.Value == null) && targetRotation == null) {
				Debug.LogError("Error: A RotateTowards target value is not set.");
				targetRotation = new SharedVector3(); // create a new SharedQuaternion to prevent repeated errors
			}
		}
		
		public override TaskStatus OnUpdate()
		{
			var rotation = Target();
			// Return a task status of success once we are done rotating
			if (Quaternion.Angle(transform.rotation, rotation) < rotationEpsilon) {
				return TaskStatus.Success;
			}
			// We haven't reached the target yet so keep rotating towards it
			transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, maxLookAtRotationDelta.Value);
			return TaskStatus.Running;
		}
		
		// Return targetPosition if targetTransform is null
		private Quaternion Target()
		{
			if (targetTransform == null || targetTransform.Value == null) {
				Vector3 faceTarge = targetRotation.Value;//boat add
				faceTarge.y = transform.position.y;
				return Quaternion.Euler(faceTarge);
			}else{
				Vector3 faceTarget = targetTransform.Value.position;//boat add
				faceTarget.y = transform.position.y;
				return Quaternion.LookRotation(faceTarget - transform.position);
			}
		}
		
		// Reset the public variables
		public override void OnReset()
		{
			rotationEpsilon = 0.5f;
		}
	}
}