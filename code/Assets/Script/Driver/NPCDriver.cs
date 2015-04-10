using UnityEngine;
using System.Collections;

/// <summary>
/// NPC的生成、管理
/// </summary>
public class NPCDriver : MonoBehaviour {

	public GameObject Hero;

	public Vector3[] NPCPlots;
	private int NPCPlotsCount; 

	public GameObject[] NPCType;
	private int NPCTypeCount;
	
	private int CurrentNPCCount;

	// Use this for initialization
	void Start () {
//		NPCPlotsCount	= NPCPlots.GetLength();
//		NPCTypeCount	= NPCType.GetLength();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
