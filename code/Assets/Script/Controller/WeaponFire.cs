using UnityEngine;
using System.Collections;
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
	/// 是否长按
	/// </summary>
	private Boolean isPressed = false;


	// Use this for initialization
	void Start () {
		weaponLauncher = Weapon.GetComponent<WeaponLauncher>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	void OnPress(bool isPressed)
	{
//		Debug.Log("OnPress:"+isPressed);
		this.isPressed = isPressed;

		if (isPressed)
		{
			StartCoroutine(fireAsync());
		}

	}

	private IEnumerator fireAsync()
	{

		while (this.isPressed)
		{
			yield return new WaitForSeconds(0.1f);
			weaponLauncher.Shoot();
		}

	}

	

	void OnClick()
	{			
		//weaponLauncher.Shoot();
	}
}
