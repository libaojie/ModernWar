using UnityEngine;
using System.Collections;

public class FightButton : MonoBehaviour {

	void OnClick()
	{
		Debug.Log("11111111");
		Global.ScenceName = "Hawaii";
		Global.IsPK = false;
		Invoke("LoadLevel", 0.3f);
	}

	void LoadLevel()
	{
		Application.LoadLevel("Loading");
	}
}
