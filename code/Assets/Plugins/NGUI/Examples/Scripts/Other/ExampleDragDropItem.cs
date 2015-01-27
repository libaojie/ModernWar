//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Item (Example)")]
public class ExampleDragDropItem : UIDragDropItem
{
	/// <summary>
	/// Prefab object that will be instantiated on the DragDropSurface if it receives the OnDrop event.
	/// </summary>
	public GameObject prefab;
	/// <summary>
	/// Drop a 3D game object onto the surface.
    /// A：一个带“3D物体prefab”的UI
    /// B: 一个带有ExampleDragDropSurface.cs脚本的3D物体
    /// 这个脚本的功能就是把A拖到一个B上，然后把A上的prefab实例化了，然后挂在dds的下面作为儿子存在。
    /// 这里面要注意，NGUITools不仅仅是做平面UI的，而且也是可以做3D方面的动作。NGUITools.AddChild()就挺好
	/// </summary>
	protected override void OnDragDropRelease (GameObject surface)
	{
        Debug.Log(" 3D::OnDragDropRelease(" + (surface ? surface.name : "n/a") + ")");
		if (surface != null)
		{
			ExampleDragDropSurface dds = surface.GetComponent<ExampleDragDropSurface>();

			if (dds != null)
			{
				GameObject child = NGUITools.AddChild(dds.gameObject, prefab);
				child.transform.localScale = dds.transform.localScale;

				Transform trans = child.transform;
				trans.position = UICamera.lastWorldPosition;

				if (dds.rotatePlacedObject)
				{
					trans.rotation = Quaternion.LookRotation(UICamera.lastHit.normal) * Quaternion.Euler(90f, 0f, 0f);
				}
				//Destroy this icon as it's no longer needed
				NGUITools.Destroy(gameObject);
				return;
			}
		}
		base.OnDragDropRelease(surface);
	}
}
