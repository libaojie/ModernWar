using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
				[TaskCategory("Basic/Custom")]
				public class IsAttackHero : Action
				{
								public GameObject SourceTarget;

								public override TaskStatus OnUpdate()
								{

												if (!SourceTarget)
												{
																return TaskStatus.Failure;
												}
												return TaskStatus.Success;
								}
				}

}