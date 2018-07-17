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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Truck Trailer")]
[RequireComponent (typeof(Rigidbody))]
public class RCC_TruckTrailer : MonoBehaviour {

	private RCC_CarControllerV3 carController;
	private Rigidbody rigid;
	public Transform COM;

	//Extra Wheels.
	public WheelCollider[] wheelColliders;
	private List<WheelCollider> leftWheelColliders = new List<WheelCollider>();
	private List<WheelCollider> rightWheelColliders = new List<WheelCollider>();

	public float antiRoll = 50000f;

	void Start () {

		rigid = GetComponent<Rigidbody>();
		carController = transform.GetComponentInParent<RCC_CarControllerV3>();

		rigid.interpolation = RigidbodyInterpolation.None;
		rigid.interpolation = RigidbodyInterpolation.Interpolate;


		antiRoll = carController.antiRollFrontHorizontal;

		for (int i = 0; i < wheelColliders.Length; i++) {

			if(wheelColliders[i].transform.localPosition.x < 0f)
				leftWheelColliders.Add(wheelColliders[i]);
			else
				rightWheelColliders.Add(wheelColliders[i]);

		}

		gameObject.SetActive (false);
		gameObject.SetActive (true);

	}

	void FixedUpdate(){
		rigid.centerOfMass = transform.InverseTransformPoint(COM.transform.position);
		AntiRollBars();

		//Applying Small Torque For Preventing Stuck Issue. Unity 5 WheelColliders Are Weird
		foreach(WheelCollider wc in wheelColliders){
			wc.motorTorque = carController._gasInput * (carController.engineTorque / 10f);
		}

	}

	public void AntiRollBars (){

		for (int i = 0; i < leftWheelColliders.Count; i++) {

			WheelHit hit;

			float travelL = 1.0f;
			float travelR = 1.0f;

			bool groundedL= leftWheelColliders[i].GetGroundHit(out hit);

			if (groundedL)
				travelL = (-leftWheelColliders[i].transform.InverseTransformPoint(hit.point).y - leftWheelColliders[i].radius) / leftWheelColliders[i].suspensionDistance;

			bool groundedR= rightWheelColliders[i].GetGroundHit(out hit);

			if (groundedR)
				travelR = (-rightWheelColliders[i].transform.InverseTransformPoint(hit.point).y - rightWheelColliders[i].radius) / rightWheelColliders[i].suspensionDistance;

			float antiRollForce= (travelL - travelR) * antiRoll;

			if (groundedL)
				rigid.AddForceAtPosition(leftWheelColliders[i].transform.up * -antiRollForce, leftWheelColliders[i].transform.position); 
			if (groundedR)
				rigid.AddForceAtPosition(rightWheelColliders[i].transform.up * antiRollForce, rightWheelColliders[i].transform.position); 

		}

	}

}
