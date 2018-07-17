//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Fixed Camera")]
public class RCC_FixedCamera : MonoBehaviour {

	public Transform currentCar;
	private RCC_Camera rccCamera;

	public float maxDistance = 50f;
	private float distance;

	public float minimumFOV = 20f;
	public float maximumFOV = 60f;
	public bool canTrackNow = false;

	private float timer = 0f;
	public float updateInSeconds = .05f;

	void Start(){

		rccCamera = GameObject.FindObjectOfType<RCC_Camera> ();

	}

	void Update(){

		if (!canTrackNow)
			return;

		if (!rccCamera) {
			rccCamera = GameObject.FindObjectOfType<RCC_Camera> ();
			return;
		}

		if (!currentCar) {
			currentCar = rccCamera.playerCar;
			return;
		}

		CheckCulling ();
			
		distance = Vector3.Distance (transform.position, currentCar.position);
		rccCamera.targetFieldOfView = Mathf.Lerp (maximumFOV, minimumFOV, distance / maxDistance);
		transform.LookAt (currentCar.position);

	}

	void CheckCulling(){

		timer += Time.deltaTime;

		if (timer < updateInSeconds) {
			return;
		} else {
			timer = 0f;
		}
			
		RaycastHit hit;

		if ((Physics.Linecast (currentCar.position, transform.position, out hit) && !hit.transform.IsChildOf (currentCar) && !hit.collider.isTrigger) || distance >= maxDistance) {
			ChangePosition ();
		}

	}

	void ChangePosition(){

		float randomizedAngle = Random.Range (-15f, 15f);
		RaycastHit hit;

		if (Physics.Raycast (currentCar.position, Quaternion.AngleAxis (randomizedAngle, currentCar.up) * currentCar.forward, out hit, maxDistance) && !hit.transform.IsChildOf(currentCar) && !hit.collider.isTrigger) {
			transform.position = hit.point;
			transform.LookAt (currentCar.position + new Vector3(0f, Mathf.Clamp(randomizedAngle, 0f, 10f), 0f));
			transform.position += transform.rotation * Vector3.forward * 3f;
		} else {
			transform.position = currentCar.position + new Vector3(0f, Mathf.Clamp(randomizedAngle, 0f, 5f), 0f);
			transform.position += Quaternion.AngleAxis (randomizedAngle, currentCar.up) * currentCar.forward * (maxDistance * .9f);
		}

	}
	
}
