//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Top Camera")]
public class RCC_TopCamera : MonoBehaviour{
	
	// The target we are following
	public Transform playerCar;
	public Transform _playerCar{get{return playerCar;}set{playerCar = value;	GetPlayerCar();}}
	private Rigidbody playerRigid;

	private Camera cam;
	public GameObject pivot;

	// The distance in the x-z plane to the target
	public float distance = 20f;
	private float distanceOffset = 0f;
	public float maximumDistanceOffset = 10f;

	private float targetFieldOfView = 60f;
	public float minimumOrtSize =7.5f;
	public float maximumOrtSize = 12.5f;

	private Vector3 targetPosition, pastFollowerPosition = Vector3.zero;
	private Vector3 pastTargetPosition = Vector3.zero;

	private float speed = 0f;

	void Awake(){

		cam = GetComponentInChildren<Camera>();

	}
	
	void GetPlayerCar(){

		if(!playerCar)
			return;

		playerRigid = playerCar.GetComponent<Rigidbody>();

		transform.position = playerCar.transform.position;

		if(playerCar.GetComponent<RCC_CameraConfig>())
			playerCar.GetComponent<RCC_CameraConfig>().SetCameraSettings();

	}

	public void SetPlayerCar(GameObject player){

		_playerCar = player.transform;

	}
	
	void Update(){
		
		// Early out if we don't have a player
		if (!playerCar || !playerRigid){
			GetPlayerCar();
			return;
		}

		// Speed of the vehicle.
		speed = Mathf.Lerp(speed, playerRigid.velocity.magnitude * 3.6f, Time.deltaTime * .5f);

		distanceOffset = Mathf.Lerp (0f, maximumDistanceOffset, speed / 100f);
		targetFieldOfView = Mathf.Lerp (minimumOrtSize, maximumOrtSize, speed / 100f);
		cam.orthographicSize = targetFieldOfView;

		targetPosition = playerCar.position;
		targetPosition += playerCar.rotation * Vector3.forward * distanceOffset;

		transform.position = SmoothApproach(pastFollowerPosition, pastTargetPosition, targetPosition, Mathf.Clamp(.5f, speed, Mathf.Infinity));

		pastFollowerPosition = transform.position;
		pastTargetPosition = targetPosition;

		pivot.transform.localPosition = new Vector3 (0f, 0f, -distance);

	}

	private Vector3 SmoothApproach( Vector3 pastPosition, Vector3 pastTargetPosition, Vector3 targetPosition, float delta){

		if(float.IsNaN(delta) || float.IsInfinity(delta) || pastPosition == Vector3.zero || pastTargetPosition == Vector3.zero || targetPosition == Vector3.zero)
			return transform.position;

		float t = Time.deltaTime * delta;
		Vector3 v = ( targetPosition - pastTargetPosition ) / t;
		Vector3 f = pastPosition - pastTargetPosition + v;
		return targetPosition - v + f * Mathf.Exp( -t );

	}

}