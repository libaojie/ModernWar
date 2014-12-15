﻿using UnityEngine;
using System.Collections;

public class FlashLight : MonoBehaviour {

	public float LightMult = 2;
	void Update () {
		if(!this.light)
			return;
		
		this.light.intensity -= LightMult*Time.deltaTime;
	}
}
