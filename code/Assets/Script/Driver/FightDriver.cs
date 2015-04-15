using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System;
using System.Security.Cryptography;

/// <summary>
/// NPC的生成、管理
/// </summary>
public class FightDriver : MonoBehaviour {

				public GameObject Hero;

				public GameObject NPCPrefab;

				public GameObject[] NPCPoints;

				public int CurrentNPCNum = 0;
				public int CurrentMaxNPC = 5;
				public int NPCSUM = 10;

				private GameObject currentNPC;
				private System.Random ran = new System.Random();

				void Awake()
				{
					for(int i = 0; i < CurrentMaxNPC; i++)
								{
												createNPC();
								}
				}

				void Updata()
				{
					if (CurrentNPCNum < CurrentMaxNPC)
					{
						createNPC();
					}
				}

				void createNPC()
				{
					int currentNPCPoint = UnityEngine.Random.Range(0, CurrentMaxNPC);
				
					currentNPC = (GameObject) Instantiate(	NPCPrefab, 
															NPCPoints[currentNPCPoint].transform.position,
															NPCPoints[currentNPCPoint].transform.rotation);
					CurrentNPCNum++;

				}

}
