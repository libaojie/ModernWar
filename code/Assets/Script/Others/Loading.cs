using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 场景加载
/// </summary>
public class Loading : MonoBehaviour {
	
	/// <summary>
	/// 异步加载操作
	/// </summary>
	private AsyncOperation async;

	/// <summary>
	/// 进度条UI
	/// </summary>
	private UISlider uiSlider = null;


	// Use this for initialization
	void Start () 
	{
		uiSlider = this.gameObject.GetComponent<UISlider>();

		StartCoroutine(loadingAsync());
	}
	
	// Update is called once per frame
	void Update () 
	{
		// 更新加载进度
		uiSlider.value = async.progress;	
	}

	/// <summary>
	/// 加载的异步函数
	/// </summary>
	/// <returns>The async.</returns>
	private IEnumerator loadingAsync()
	{
		async = Application.LoadLevelAsync(Global.ScenceName);

		yield return async;
	}

}
