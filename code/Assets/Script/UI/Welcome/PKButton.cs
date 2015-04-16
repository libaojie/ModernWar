using UnityEngine;
using System.Collections;

public class PKButton : MonoBehaviour {

	void OnClick()
	{
		Global.ScenceName = "PK";
		Global.IsPK = true;
		Invoke("LoadLevel", 0.3f);
	}

	void LoadLevel()
	{
		Application.LoadLevel("Loading");
	}
}
