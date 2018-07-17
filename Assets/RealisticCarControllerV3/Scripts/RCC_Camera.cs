//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Camera/Main Camera")]
public class RCC_Camera : MonoBehaviour{
	
	// The target we are following
	public Transform playerCar;
	public Transform _playerCar{get{return playerCar;}set{playerCar = value;	GetPlayerCar();}}
	private Rigidbody playerRigid;

	private Camera cam;
	public GameObject pivot;
	private GameObject boundCenter;

	public CameraMode cameraMode;
	public enum CameraMode{TPS, FPS, WHEEL, FIXED}

	// The distance in the x-z plane to the target
	public float distance = 6f;
	
	// the height we want the camera to be above the target
	public float height = 2f;

	private float heightDamping = 5f;
	private float rotationDamping = 3f;

	public float targetFieldOfView = 60f;
	public float minimumFOV = 55f;
	public float maximumFOV = 70f;
	public float hoodCameraFOV = 70f;
	public float wheelCameraFOV = 60f;
	
	public float maximumTilt = 15f;
	private float tiltAngle = 0f;

	internal int cameraSwitchCount = 0;
	private RCC_HoodCamera hoodCam;
	public GameObject wheelCam;
	private RCC_FixedCamera fixedCam;

	private Vector3 targetPosition = Vector3.zero;

	private float speed = 0f;

	private Vector3 localVector;
	private Vector3 collisionPos;
	private Quaternion collisionRot;

	private float index = 0f;

	void Awake(){

		cam = GetComponentInChildren<Camera>();

	}
	
	void GetPlayerCar(){

		if(!playerCar)
			return;

		cameraMode = CameraMode.TPS;
		playerRigid = playerCar.GetComponent<Rigidbody>();
		hoodCam = playerCar.GetComponentInChildren<RCC_HoodCamera>();
	//	wheelCam = playerCar.GetComponentInChildren<RCC_WheelCamera>();
		fixedCam = GameObject.FindObjectOfType<RCC_FixedCamera>();

		transform.position = playerCar.transform.position;
		transform.rotation = playerCar.transform.rotation * Quaternion.Euler(10f, 0f, 0f);

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

		if(index > 0)
			index -= Time.deltaTime * 5f;

		if(cameraMode == CameraMode.TPS){
			
		}

		cam.fieldOfView = Mathf.Lerp (cam.fieldOfView, targetFieldOfView, Time.deltaTime * 3f);
			
	}
	
	void LateUpdate (){
		
		// Early out if we don't have a target
		if (!playerCar || !playerRigid)
			return;

		if (!playerCar.gameObject.activeSelf)
			return;

		if(Input.GetKeyDown(RCC_Settings.Instance.changeCameraKB)){
			ChangeCamera();
		}

		switch(cameraMode){

		case CameraMode.TPS:
			TPS();
			break;

		case CameraMode.FPS:
			if(hoodCam){
				FPS();
			}else{
				ChangeCamera();
			}
			break;
		
		case CameraMode.WHEEL:
			if(wheelCam){
				WHEEL();
			}else{
				ChangeCamera();
			}
			break;

		case CameraMode.FIXED:
			if(fixedCam){
				FIXED();
			}else{
				ChangeCamera();
			}
			break;

		}

	}

	public void ChangeCamera(){

		cameraSwitchCount ++;
		if(cameraSwitchCount >= 2)
			cameraSwitchCount = 0;
		
		if(fixedCam)
			fixedCam.canTrackNow = false;

		switch(cameraSwitchCount){
		case 0:
			cameraMode = CameraMode.TPS;
			break;
		case 1:
			cameraMode = CameraMode.WHEEL;
			break;
		}

	}

	void FPS(){

		if(transform.parent != hoodCam.transform){
			transform.SetParent(hoodCam.transform, false);
			transform.position = hoodCam.transform.position;
			transform.rotation = hoodCam.transform.rotation;
			targetFieldOfView = hoodCameraFOV;
			hoodCam.FixShake ();
		}

	}

	void WHEEL(){

		if(transform.parent != wheelCam.transform){
			transform.SetParent(wheelCam.transform, false);
			transform.position = wheelCam.transform.position;
			transform.rotation = wheelCam.transform.rotation;
			targetFieldOfView = wheelCameraFOV;
		}

	}

	void TPS(){

		if(transform.parent != null)
			transform.SetParent(null);

		if(targetPosition == Vector3.zero){
			targetPosition = _playerCar.position;
			targetPosition -= transform.rotation * Vector3.forward * distance;
			transform.position = targetPosition;
		}

		speed = (playerRigid.transform.InverseTransformDirection(playerRigid.velocity).z) * 3.6f;
		targetFieldOfView = Mathf.Lerp(minimumFOV, maximumFOV, speed / 150f) + (5f * Mathf.Cos (1f * index));
		tiltAngle = Mathf.Lerp(0f, maximumTilt * (int)Mathf.Clamp(-playerCar.InverseTransformDirection(playerRigid.velocity).x, -1, 1), Mathf.Abs(playerCar.InverseTransformDirection(playerRigid.velocity).x) / 50f);

		// Calculate the current rotation angles.
		float wantedRotationAngle = playerCar.eulerAngles.y;
		float wantedHeight = playerCar.position.y + height;
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;

		rotationDamping = Mathf.Lerp(0f, 3f, (playerRigid.velocity.magnitude * 3f) / 40f);

		if(speed < -10)
			wantedRotationAngle = playerCar.eulerAngles.y + 180;

		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight + Mathf.Lerp(-.5f, 0f, (speed) / 20f), heightDamping * Time.deltaTime);

		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);

		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = playerCar.position;
		transform.position -= currentRotation * Vector3.forward * distance;

		// Set the height of the camera
		transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

		// Always look at the target
		transform.LookAt (new Vector3(playerCar.position.x, playerCar.position.y + 1f, playerCar.position.z));
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y, Mathf.Clamp(tiltAngle, -10f, 10f));

		//pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, (new Vector3(Random.insideUnitSphere.x / 2f, Random.insideUnitSphere.y, Random.insideUnitSphere.z) * speed * maxShakeAmount), Time.deltaTime * 1f);
		collisionPos = Vector3.Lerp(collisionPos, Vector3.zero, Time.deltaTime * 5f);
		collisionRot = Quaternion.Lerp(collisionRot, Quaternion.identity, Time.deltaTime * 5f);
		pivot.transform.localPosition = Vector3.Lerp(pivot.transform.localPosition, collisionPos, Time.deltaTime * 5f);
		pivot.transform.localRotation = Quaternion.Lerp(pivot.transform.localRotation, collisionRot, Time.deltaTime * 5f);

	}

	void FIXED(){

		if(transform.parent != fixedCam.transform){
			transform.SetParent(fixedCam.transform, false);
			transform.position = fixedCam.transform.position;
			transform.rotation = fixedCam.transform.rotation;
			targetFieldOfView = 60;
			fixedCam.currentCar = playerCar;
			fixedCam.canTrackNow = true;
		}

		if(fixedCam.transform.parent != null)
			fixedCam.transform.SetParent(null);

	}

	public void Collision(Collision collision){

		if(!enabled || cameraMode != CameraMode.TPS)
			return;
		
		Vector3 colRelVel = collision.relativeVelocity;
		colRelVel *= 1f - Mathf.Abs(Vector3.Dot(transform.up,collision.contacts[0].normal));

		float cos = Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, colRelVel.normalized));

		if (colRelVel.magnitude * cos >= 5f){

			localVector = transform.InverseTransformDirection(colRelVel) / (30f);

			collisionPos -= localVector * 3f;
			collisionRot = Quaternion.Euler(new Vector3(-localVector.z * 30f, -localVector.y * 30f, -localVector.x * 30f));
			targetFieldOfView = cam.fieldOfView - Mathf.Clamp(collision.relativeVelocity.magnitude, 0f, 15f);
			index = Mathf.Clamp(collision.relativeVelocity.magnitude / 5f, 0f, 10f);

		}

	}

}