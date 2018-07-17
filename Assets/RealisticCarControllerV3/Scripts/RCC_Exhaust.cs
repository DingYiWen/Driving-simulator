//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Exhaust")]
public class RCC_Exhaust : MonoBehaviour {

	private RCC_CarControllerV3 carController;
	private ParticleSystem particle;
	private ParticleSystem.EmissionModule emission;
	private ParticleSystem.MinMaxCurve emissionRate;
	public ParticleSystem flame;
	private ParticleSystem.EmissionModule subEmission;
	private ParticleSystem.MinMaxCurve subEmissionRate;
	private Light flameLight;

	public float flameTime = 0f;
	private AudioSource flameSource;

	public Color flameColor = Color.red;
	public Color boostFlameColor = Color.blue;

	void Start () {

		if (RCC_Settings.Instance.dontUseAnyParticleEffects) {
			Destroy (gameObject);
			return;
		}

		carController = GetComponentInParent<RCC_CarControllerV3>();
		particle = GetComponent<ParticleSystem>();
		emission = particle.emission;

		if(flame){
			
			subEmission = flame.emission;
			flameLight = flame.GetComponentInChildren<Light>();
			flameSource = RCC_CreateAudioSource.NewAudioSource(gameObject, "Exhaust Flame AudioSource", 10f, 50f, 10f, RCC_Settings.Instance.exhaustFlameClips[0], false, false, false);
			flameLight.renderMode = RCC_Settings.Instance.useLightsAsVertexLights ? LightRenderMode.ForceVertex : LightRenderMode.ForcePixel;

		}
	
	}

	void Update () {

		if(!carController || !particle)
			return;

		if(carController.engineRunning){

			if(carController.speed < 150){
				if(!emission.enabled)
					emission.enabled = true;
			if(carController._gasInput > .05f){
				emissionRate.constantMax = 50f;
				emission.rate = emissionRate;
				particle.startSpeed = 5f;
				particle.startSize = 8;
			}else{
				emissionRate.constantMax = 5;
				emission.rate = emissionRate;
				particle.startSpeed = .5f;
				particle.startSize = 4;
				}
			}else{
				if(emission.enabled)
					emission.enabled = false;
			}

			if(carController._gasInput >= .25f)
				flameTime = 0f;

			if((carController.useExhaustFlame && carController.engineRPM >= 5000 && carController.engineRPM <= 5500 && carController._gasInput <= .25f && flameTime <= .5f) || carController._boostInput >= 1.5f){
				
				flameTime += Time.deltaTime;
				subEmission.enabled = true;

				if(flameLight)
					flameLight.intensity = flameSource.pitch * 3f * Random.Range(.25f, 1f) ;
				
				if(carController._boostInput >= 1.5f && flame){
					flame.startColor = boostFlameColor;
					flameLight.color = flame.startColor;
				}else{
					flame.startColor = flameColor;
					flameLight.color = flame.startColor;
				}

				if(!flameSource.isPlaying){
					flameSource.clip = RCC_Settings.Instance.exhaustFlameClips[Random.Range(0, RCC_Settings.Instance.exhaustFlameClips.Length)];
				//	flameSource.Play();
				}

			}else{
				
				subEmission.enabled = false;

				if(flameLight)
					flameLight.intensity = 0f;
				if(flameSource.isPlaying)
					flameSource.Stop();
				
			}
				
		}else{

			if(emission.enabled)
				emission.enabled = false;

			subEmission.enabled = false;

			if(flameLight)
				flameLight.intensity = 0f;
			if(flameSource.isPlaying)
				flameSource.Stop();
			
		}

	}

}
