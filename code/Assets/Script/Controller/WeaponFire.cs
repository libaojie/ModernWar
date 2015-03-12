using UnityEngine;
using System.Collections;

public class WeaponFire : MonoBehaviour {

	public GameObject Weapon;
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick()
	{			
		Weapon.GetComponent<WeaponLauncher>().Shoot();
	}
}
