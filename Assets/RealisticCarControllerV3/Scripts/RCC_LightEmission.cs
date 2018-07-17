//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Light/Light Emission")]
public class RCC_LightEmission : MonoBehaviour {

	private Light sharedLight;
	public Renderer lightRenderer;
	public int materialIndex = 0;
	public bool noTexture = false;

	void Start () {

		sharedLight = GetComponent<Light>();
		Material m = lightRenderer.materials[materialIndex];
		m.EnableKeyword("_EMISSION");
	 
	}

	void Update () {

		if(!sharedLight.enabled){
			lightRenderer.materials[materialIndex].SetColor("_EmissionColor", Color.white * 0f);
			return;
		}

		if(!noTexture)
			lightRenderer.materials[materialIndex].SetColor("_EmissionColor", Color.white * sharedLight.intensity);
		else
			lightRenderer.materials[materialIndex].SetColor("_EmissionColor", Color.red * sharedLight.intensity);
	
	}

}
