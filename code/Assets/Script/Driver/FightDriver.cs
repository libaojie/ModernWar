using UnityEngine;
using System.Collections;
using System.Collections.Specialized;
using System;

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

				void Awake()
				{
								for(int i = 0; i < 4; i++)
								{
												createNPC(i);
								}
				}

				void Updata()
				{

				}

				void createNPC(int currentNPCPoint)
				{
//								int currentNPCPoint = Random.Range(0, 5); 

								currentNPC = (GameObject) Instantiate(NPCPrefab, 
																																														NPCPoints[currentNPCPoint].transform.position,
																																														NPCPoints[currentNPCPoint].transform.rotation);

				}

}
