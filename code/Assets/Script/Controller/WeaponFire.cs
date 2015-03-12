using UnityEngine;
using System.Collections;
using System.Threading;
using System;

/// <summary>
/// 开火
/// </summary>
public class WeaponFire : MonoBehaviour {

	/// <summary>
	/// 武器类
	/// </summary>
	public GameObject Weapon;

	/// <summary>
	/// 武器组件
	/// </summary>
	private WeaponLauncher weaponLauncher;

	/// <summary>
	/// 是否连击
	/// </summary>
	private bool isConn = false;

	// Use this for initialization
	void Start () {
		weaponLauncher = Weapon.GetComponent<WeaponLauncher>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		
	void OnClick()
	{			
		if(!this.isConn)
		{
			lock (Global.FireLock)
			{
				weaponLauncher.Shoot();
			}
		}
		else
		{
			this.isConn = false;
		}

	}

	void OnDoubleClick()
	{
		if (isConn)
		{
			isConn = false;
			return;
		}

		isConn = true;

		StartCoroutine(fireAsync());

	}

	private IEnumerator fireAsync()
	{
		int fireCount = 0;

		while (isConn && fireCount < 10)
		{
			yield return new WaitForSeconds(0.1f);
			lock (Global.FireLock)
			{
				weaponLauncher.Shoot();
			}
			fireCount++;
		}

	}

}
