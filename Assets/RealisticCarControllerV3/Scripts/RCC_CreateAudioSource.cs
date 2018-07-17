//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCC_CreateAudioSource : MonoBehaviour {

	public static AudioSource NewAudioSource(GameObject go, string audioName, float minDistance, float maxDistance, float volume, AudioClip audioClip, bool loop, bool playNow, bool destroyAfterFinished){

		GameObject audioSource = new GameObject(audioName);
		audioSource.transform.position = go.transform.position;
		audioSource.transform.rotation = go.transform.rotation;
		audioSource.transform.parent = go.transform;
		audioSource.AddComponent<AudioSource>();
		//audioSource.GetComponent<AudioSource>().priority =1;
		audioSource.GetComponent<AudioSource>().minDistance = minDistance;
		audioSource.GetComponent<AudioSource>().maxDistance = maxDistance;
		audioSource.GetComponent<AudioSource>().volume = volume;
		audioSource.GetComponent<AudioSource>().clip = audioClip;
		audioSource.GetComponent<AudioSource>().loop = loop;

		if(minDistance == 0 && maxDistance == 0)
			audioSource.GetComponent<AudioSource>().spatialBlend = 0f;
		else
			audioSource.GetComponent<AudioSource>().spatialBlend = 1f;

		if (playNow) {
			audioSource.GetComponent<AudioSource> ().playOnAwake = true;
			audioSource.GetComponent<AudioSource> ().Play ();
		} else {
			audioSource.GetComponent<AudioSource> ().playOnAwake = false;
		}

		if(destroyAfterFinished){
			if(audioClip)
				Destroy(audioSource, audioClip.length);
			else
				Destroy(audioSource);
		}

		if (go.transform.FindChild ("All Audio Sources")) {
			audioSource.transform.SetParent (go.transform.FindChild ("All Audio Sources"));
		} else {
			GameObject allAudioSources = new GameObject ("All Audio Sources");
			allAudioSources.transform.SetParent (go.transform, false);
			audioSource.transform.SetParent (allAudioSources.transform, false);
		}

		return audioSource.GetComponent<AudioSource>();

	}

	public static void NewHighPassFilter(AudioSource source, float freq, int level){

		if(source == null)
			return;

		AudioHighPassFilter highFilter = source.gameObject.AddComponent<AudioHighPassFilter>();
		highFilter.cutoffFrequency = freq;
		highFilter.highpassResonanceQ = level;

	}

}
