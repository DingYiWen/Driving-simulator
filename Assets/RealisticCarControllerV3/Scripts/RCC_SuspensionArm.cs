//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Misc/Suspension Distance Based Axle")]
public class RCC_SuspensionArm : MonoBehaviour {

	public WheelCollider wheelcollider;

	public Axis axis;
	public enum Axis {X, Y, Z}

	private Vector3 orgRot;
	private float totalSuspensionDistance = 0;

	public float offsetAngle = 30;
	public float angleFactor = 150;
	
	void Start () {
		
		orgRot = transform.localEulerAngles;
		totalSuspensionDistance = GetSuspensionDistance ();

	}

	void FixedUpdate () {
		
		float suspensionCourse = GetSuspensionDistance () - totalSuspensionDistance;
		transform.localEulerAngles = orgRot;

		switch(axis){

		case Axis.X:
			transform.Rotate (Vector3.right, suspensionCourse * angleFactor - offsetAngle, Space.Self);
			break;
		case Axis.Y:
			transform.Rotate (Vector3.up, suspensionCourse * angleFactor - offsetAngle, Space.Self);
			break;
		case Axis.Z:
			transform.Rotate (Vector3.forward, suspensionCourse * angleFactor - offsetAngle, Space.Self);
			break;

		}

	}
		
	private float GetSuspensionDistance() {
		
		Quaternion quat;
		Vector3 position;
		wheelcollider.GetWorldPose(out position, out quat);
		Vector3 local = wheelcollider.transform.InverseTransformPoint (position);
		return local.y;

	}

}
