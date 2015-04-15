using UnityEngine;
using System.Collections;

public class FightButton : MonoBehaviour {

	void OnClick()
	{
		Debug.Log("11111111");
		Global.ScenceName = "Hawaii";
		Invoke("LoadLevel", 0.3f);
	}

	void LoadLevel()
	{
		Application.LoadLevel("Loading");
	}
}
