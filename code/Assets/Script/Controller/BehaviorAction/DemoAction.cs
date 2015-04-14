using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
				[TaskCategory("Basic/Custom")]
				public class DemoAction : Action
				{
								public GameObject SourceTarget;

								public override TaskStatus OnUpdate()
								{
												Debug.Log("1111");

												//												if (!SourceTarget)
												//												{
												//																return TaskStatus.Failure;
												//												}
												return TaskStatus.Success;
								}
				}

}