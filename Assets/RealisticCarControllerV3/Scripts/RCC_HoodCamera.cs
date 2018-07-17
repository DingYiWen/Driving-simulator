//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Hood Camera")]
public class RCC_HoodCamera : MonoBehaviour {

	void Start () {

		StartCoroutine ("FixShakeDelayed");
	
	}

	public void FixShake(){

		StartCoroutine ("FixShakeDelayed");
		
	}

	IEnumerator FixShakeDelayed(){

		if (!GetComponent<Rigidbody> ())
			yield return null;

		yield return new WaitForFixedUpdate ();
		GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.None;
		yield return new WaitForFixedUpdate ();
		GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.Interpolate;

	}

}

