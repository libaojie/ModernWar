using UnityEngine;
using System.Collections;

public class WeaponFirePK : MonoBehaviour {

	/// <summary>
	/// 武器类
	/// </summary>
	public string WeaponName;

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
		StartCoroutine(findingAsync());
	}

	private IEnumerator findingAsync()
	{
		int fireCount = 0;

		while (!PK.IsInstantiated)
		{
			yield return new WaitForSeconds(0.1f);
		}

		weaponLauncher = GameObject.Find(WeaponName).GetComponent<WeaponLauncher>();
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