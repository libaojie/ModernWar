using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
				[TaskCategory("Basic/Custome")]
				public class NPCFire : Action
				{
								public GameObject SourceTarget;

								public override TaskStatus OnUpdate()
								{
												WeaponLauncher weaponLauncher = gameObject.GetComponentsInChildren<WeaponLauncher>()[0]; 
												weaponLauncher.Shoot();
												return TaskStatus.Success;
								}
				}

}