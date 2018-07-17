//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------


using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/Main/Wheel Collider")]
[RequireComponent (typeof(WheelCollider))]
public class RCC_WheelCollider : MonoBehaviour {
	
	private RCC_CarControllerV3 carController;
	private Rigidbody rigid;

	private WheelCollider _wheelCollider;
	public WheelCollider wheelCollider{
		get{
			if(_wheelCollider == null)
				_wheelCollider = GetComponent<WheelCollider>();
			return _wheelCollider;
		}set{
			_wheelCollider = value;
		}
	}

	private List <RCC_WheelCollider> allWheelColliders = new List<RCC_WheelCollider>() ;
	public Transform wheelModel;

	private float wheelRotation = 0f;
	private float camber = 0f;
	private PhysicMaterial groundMaterial;

	internal float steerAngle = 0f;
	internal bool isGrounded = false;
	internal float totalSlip = 0f;
	internal float rpm = 0f;
	internal float wheelRPMToSpeed = 0f;
	internal float wheelTemparature = 0f;

	private RCC_GroundMaterials physicsMaterials{get{return RCC_GroundMaterials.Instance;}}
	private RCC_GroundMaterials.GroundMaterialFrictions[] physicsFrictions{get{return RCC_GroundMaterials.Instance.frictions;}}

	private RCC_Skidmarks skidmarks;
	private float startSlipValue = .25f;
	private int lastSkidmark = -1;
	
	private float wheelSlipAmountSideways;
	private float wheelSlipAmountForward;

	private float orgSidewaysStiffness = 1f;
	private float orgForwardStiffness = 1f;

	//WheelFriction Curves and Stiffness.
	public WheelFrictionCurve forwardFrictionCurve;
	public WheelFrictionCurve sidewaysFrictionCurve;

	private AudioSource audioSource;
	private AudioClip audioClip;

	private List<ParticleSystem> allWheelParticles = new List<ParticleSystem>();
	private ParticleSystem.EmissionModule emission;

	internal float tractionHelpedSidewaysStiffness = 1f;

	private float minForwardStiffness = .75f;
	private float maxForwardStiffness  = 1f;

	private float minSidewaysStiffness = .75f;
	private float maxSidewaysStiffness = 1f;
	
	void Awake (){

		carController = GetComponentInParent<RCC_CarControllerV3>();
		rigid = carController.GetComponent<Rigidbody> ();
		wheelCollider = GetComponent<WheelCollider>();

		if (!RCC_Settings.Instance.dontUseSkidmarks) {
			if (FindObjectOfType (typeof(RCC_Skidmarks))) {
				skidmarks = FindObjectOfType (typeof(RCC_Skidmarks)) as RCC_Skidmarks;
			} else {
				Debug.Log ("No skidmarks object found. Creating new one...");
				skidmarks = (RCC_Skidmarks)Instantiate (RCC_Settings.Instance.skidmarksManager, Vector3.zero, Quaternion.identity);
			}
		}

		wheelCollider.mass = rigid.mass / 15f;
		forwardFrictionCurve = wheelCollider.forwardFriction;
		sidewaysFrictionCurve = wheelCollider.sidewaysFriction;

		switch(RCC_Settings.Instance.behaviorType){

		case RCC_Settings.BehaviorType.SemiArcade:
			forwardFrictionCurve = SetFrictionCurves(forwardFrictionCurve, .2f, 2f, 2f, 2f);
			sidewaysFrictionCurve = SetFrictionCurves(sidewaysFrictionCurve, .25f, 2f, 2f, 2f);
			wheelCollider.forceAppPointDistance = Mathf.Clamp(wheelCollider.forceAppPointDistance, .35f, 1f);
			break;

		case RCC_Settings.BehaviorType.Drift:
			forwardFrictionCurve = SetFrictionCurves(forwardFrictionCurve, .25f, 1f, .8f, .5f);
			sidewaysFrictionCurve = SetFrictionCurves(sidewaysFrictionCurve, .4f, 1f, .5f, .75f);
			wheelCollider.forceAppPointDistance = Mathf.Clamp(wheelCollider.forceAppPointDistance, .1f, 1f);
			if(carController._wheelTypeChoise == RCC_CarControllerV3.WheelType.FWD){
				Debug.LogError("Current behavior mode is ''Drift'', but your vehicle named " + carController.name + " was FWD. You have to use RWD, AWD, or BIASED to rear wheels. Setting it to *RWD* now. ");
				carController._wheelTypeChoise = RCC_CarControllerV3.WheelType.RWD;
			}
			break;

		case RCC_Settings.BehaviorType.Fun:
			forwardFrictionCurve = SetFrictionCurves(forwardFrictionCurve, .2f, 2f, 2f, 2f);
			sidewaysFrictionCurve = SetFrictionCurves(sidewaysFrictionCurve, .25f, 2f, 2f, 2f);
			wheelCollider.forceAppPointDistance = Mathf.Clamp(wheelCollider.forceAppPointDistance, .75f, 2f);
			break;

		case RCC_Settings.BehaviorType.Racing:
			forwardFrictionCurve = SetFrictionCurves(forwardFrictionCurve, .2f, 1f, .8f, .75f);
			sidewaysFrictionCurve = SetFrictionCurves(sidewaysFrictionCurve, .3f, 1f, .25f, .75f);
			wheelCollider.forceAppPointDistance = Mathf.Clamp(wheelCollider.forceAppPointDistance, .25f, 1f);
			break;

		case RCC_Settings.BehaviorType.Simulator:
			forwardFrictionCurve = SetFrictionCurves(forwardFrictionCurve, .2f, 1f, .8f, .75f);
			sidewaysFrictionCurve = SetFrictionCurves(sidewaysFrictionCurve, .25f, 1f, .5f, .75f);
			wheelCollider.forceAppPointDistance = Mathf.Clamp(wheelCollider.forceAppPointDistance, .1f, 1f);
			break;

		}

		orgForwardStiffness = forwardFrictionCurve.stiffness;
		orgSidewaysStiffness = sidewaysFrictionCurve.stiffness;
		wheelCollider.forwardFriction = forwardFrictionCurve;
		wheelCollider.sidewaysFriction = sidewaysFrictionCurve;

		if(RCC_Settings.Instance.useSharedAudioSources){
			if(!carController.transform.Find("All Audio Sources/Skid Sound AudioSource"))
				audioSource = RCC_CreateAudioSource.NewAudioSource(carController.gameObject, "Skid Sound AudioSource", 5, 50, 0, audioClip, true, true, false);
			else
				audioSource = carController.transform.Find("All Audio Sources/Skid Sound AudioSource").GetComponent<AudioSource>();
		}else{
			audioSource = RCC_CreateAudioSource.NewAudioSource(carController.gameObject, "Skid Sound AudioSource", 5, 50, 0, audioClip, true, true, false);
			audioSource.transform.position = transform.position;
		}

		if (!RCC_Settings.Instance.dontUseAnyParticleEffects) {

			for (int i = 0; i < RCC_GroundMaterials.Instance.frictions.Length; i++) {

				GameObject ps = (GameObject)Instantiate (RCC_GroundMaterials.Instance.frictions [i].groundParticles, transform.position, transform.rotation) as GameObject;
				emission = ps.GetComponent<ParticleSystem> ().emission;
				emission.enabled = false;
				ps.transform.SetParent (transform, false);
				ps.transform.localPosition = Vector3.zero;
				ps.transform.localRotation = Quaternion.identity;
				allWheelParticles.Add (ps.GetComponent<ParticleSystem> ());

			}

		}
			
	}

	void Start(){

		allWheelColliders = carController.allWheelColliders.ToList();
		allWheelColliders.Remove(this);

	}

	WheelFrictionCurve SetFrictionCurves(WheelFrictionCurve curve, float extremumSlip, float extremumValue, float asymptoteSlip, float asymptoteValue){

		WheelFrictionCurve newCurve = curve;

		newCurve.extremumSlip = extremumSlip;
		newCurve.extremumValue = extremumValue;
		newCurve.asymptoteSlip = asymptoteSlip;
		newCurve.asymptoteValue = asymptoteValue;

		return newCurve;

	}

	void Update(){

		if (!carController.enabled)
			return;

		if(!carController.sleepingRigid){

			WheelAlign();
			WheelCamber();

		}

	}
	
	void  FixedUpdate (){

		if (!carController.enabled)
			return;

		WheelHit hit;
		isGrounded = wheelCollider.GetGroundHit(out hit);

		steerAngle = wheelCollider.steerAngle;
		rpm = wheelCollider.rpm;
		wheelRPMToSpeed = (((wheelCollider.rpm * wheelCollider.radius) / 2.8f) * Mathf.Lerp(1f, .75f, hit.forwardSlip)) * rigid.transform.lossyScale.y;
		camber = this == carController.FrontLeftWheelCollider || this == carController.FrontRightWheelCollider ? carController.frontCamber : carController.rearCamber;

		SkidMarks();
		Frictions();
		Audio();

	}

	public void WheelAlign (){

		if(!wheelModel){
			Debug.LogError(transform.name + " wheel of the " + carController.transform.name + " is missing wheel model. This wheel is disabled");
			enabled = false;
			return;
		}

		RaycastHit hit;
		WheelHit CorrespondingGroundHit;

		Vector3 ColliderCenterPoint = wheelCollider.transform.TransformPoint(wheelCollider.center);
		wheelCollider.GetGroundHit(out CorrespondingGroundHit);

		if(Physics.Raycast(ColliderCenterPoint, -wheelCollider.transform.up, out hit, (wheelCollider.suspensionDistance + wheelCollider.radius) * transform.localScale.y) && !hit.transform.IsChildOf(carController.transform) && !hit.collider.isTrigger){
			wheelModel.transform.position = hit.point + (wheelCollider.transform.up * wheelCollider.radius) * transform.localScale.y;
			float extension = (-wheelCollider.transform.InverseTransformPoint(CorrespondingGroundHit.point).y - wheelCollider.radius) / wheelCollider.suspensionDistance;
			Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point + wheelCollider.transform.up * (CorrespondingGroundHit.force / rigid.mass), extension <= 0.0 ? Color.magenta : Color.white);
			Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - wheelCollider.transform.forward * CorrespondingGroundHit.forwardSlip * 2f, Color.green);
			Debug.DrawLine(CorrespondingGroundHit.point, CorrespondingGroundHit.point - wheelCollider.transform.right * CorrespondingGroundHit.sidewaysSlip * 2f, Color.red);
		}else{
			wheelModel.transform.position = Vector3.Lerp(wheelModel.transform.position, ColliderCenterPoint - (wheelCollider.transform.up * wheelCollider.suspensionDistance) * transform.localScale.y, Time.deltaTime * 10f);
		}

		wheelRotation += wheelCollider.rpm * 6 * Time.deltaTime;
		wheelModel.transform.rotation = wheelCollider.transform.rotation * Quaternion.Euler(wheelRotation, wheelCollider.steerAngle, wheelCollider.transform.rotation.z);

	}

	public void WheelCamber (){

		Vector3 wheelLocalEuler;

		if(wheelCollider.transform.localPosition.x < 0)
			wheelLocalEuler = new Vector3(wheelCollider.transform.localEulerAngles.x, wheelCollider.transform.localEulerAngles.y, (-camber));
		else
			wheelLocalEuler = new Vector3(wheelCollider.transform.localEulerAngles.x, wheelCollider.transform.localEulerAngles.y, (camber));

		Quaternion wheelCamber = Quaternion.Euler(wheelLocalEuler);
		wheelCollider.transform.localRotation = wheelCamber;

	}

	void SkidMarks(){

		WheelHit GroundHit;
		wheelCollider.GetGroundHit(out GroundHit);

		wheelSlipAmountSideways = Mathf.Abs(GroundHit.sidewaysSlip);
		wheelSlipAmountForward = Mathf.Abs(GroundHit.forwardSlip);
		totalSlip = wheelSlipAmountSideways + (wheelSlipAmountForward / 2f);

		if(skidmarks){

			if (wheelSlipAmountSideways > startSlipValue || wheelSlipAmountForward > startSlipValue * 2f){

				Vector3 skidPoint = GroundHit.point + 2f * (rigid.velocity) * Time.deltaTime;

				if(rigid.velocity.magnitude > 1f){
					lastSkidmark = skidmarks.AddSkidMark(skidPoint, GroundHit.normal, (wheelSlipAmountSideways / 2f) + (wheelSlipAmountForward / 2f), lastSkidmark);
					wheelTemparature += ((wheelSlipAmountSideways / 2f) + (wheelSlipAmountForward / 2f)) / ((Time.fixedDeltaTime * 100f) * Mathf.Lerp(1f, 5f, wheelTemparature / 150f));
				}else{
					lastSkidmark = -1;
					wheelTemparature -= Time.fixedDeltaTime * 5f;
				}

			}else{
				
				lastSkidmark = -1;
				wheelTemparature -= Time.fixedDeltaTime * 5f;

			}

			wheelTemparature = Mathf.Clamp(wheelTemparature, 0f, 150f);

		}

	}

	void Frictions(){

		WheelHit GroundHit;
		wheelCollider.GetGroundHit(out GroundHit);
		bool contacted = false;

		for (int i = 0; i < physicsFrictions.Length; i++) {

			if(GroundHit.point != Vector3.zero && GroundHit.collider.sharedMaterial == physicsFrictions[i].groundMaterial){

				contacted = true;
				
				forwardFrictionCurve.stiffness = physicsFrictions[i].forwardStiffness;
				sidewaysFrictionCurve.stiffness = (physicsFrictions[i].sidewaysStiffness * tractionHelpedSidewaysStiffness);

				if(RCC_Settings.Instance.behaviorType == RCC_Settings.BehaviorType.Drift){
					Drift(Mathf.Abs(GroundHit.forwardSlip));
				}

				wheelCollider.forwardFriction = forwardFrictionCurve;
				wheelCollider.sidewaysFriction = sidewaysFrictionCurve;

				wheelCollider.wheelDampingRate = physicsFrictions[i].damp;

				if (!RCC_Settings.Instance.dontUseAnyParticleEffects) 
					emission = allWheelParticles[i].emission;
				
				audioClip = physicsFrictions[i].groundSound;

				if (wheelSlipAmountSideways > physicsFrictions[i].slip || wheelSlipAmountForward > physicsFrictions[i].slip){
					emission.enabled = true;
				}else{
					emission.enabled = false;
				}

			}

		}

		if(!contacted && physicsMaterials.useTerrainSplatMapForGroundFrictions){

			for (int k = 0; k < physicsMaterials.terrainSplatMapIndex.Length; k++) {

				if(GroundHit.point != Vector3.zero && GroundHit.collider.sharedMaterial == physicsMaterials.terrainPhysicMaterial){

					if(TerrainSurface.GetTextureMix(transform.position) != null && TerrainSurface.GetTextureMix(transform.position)[k] > .5f){

						contacted = true;
						
						forwardFrictionCurve.stiffness = physicsFrictions[physicsMaterials.terrainSplatMapIndex[k]].forwardStiffness;
						sidewaysFrictionCurve.stiffness = (physicsFrictions[physicsMaterials.terrainSplatMapIndex[k]].sidewaysStiffness * tractionHelpedSidewaysStiffness);

						if(RCC_Settings.Instance.behaviorType == RCC_Settings.BehaviorType.Drift){
							Drift(Mathf.Abs(GroundHit.forwardSlip));
						}

						wheelCollider.forwardFriction = forwardFrictionCurve;
						wheelCollider.sidewaysFriction = sidewaysFrictionCurve;

						wheelCollider.wheelDampingRate = physicsFrictions[physicsMaterials.terrainSplatMapIndex[k]].damp;

						if (!RCC_Settings.Instance.dontUseAnyParticleEffects)
							emission = allWheelParticles[physicsMaterials.terrainSplatMapIndex[k]].emission;

						audioClip = physicsFrictions[physicsMaterials.terrainSplatMapIndex[k]].groundSound;

						if (wheelSlipAmountSideways > physicsFrictions[physicsMaterials.terrainSplatMapIndex[k]].slip || wheelSlipAmountForward > physicsFrictions[physicsMaterials.terrainSplatMapIndex[k]].slip){
							emission.enabled = true;
						}else{
							emission.enabled = false;
						}
							 
					}

				}
				
			}

		}

		if(!contacted){

			forwardFrictionCurve.stiffness = orgForwardStiffness;
			sidewaysFrictionCurve.stiffness = orgSidewaysStiffness * tractionHelpedSidewaysStiffness;

			if(RCC_Settings.Instance.behaviorType == RCC_Settings.BehaviorType.Drift){
				Drift(Mathf.Abs(GroundHit.forwardSlip));
			}

			wheelCollider.forwardFriction = forwardFrictionCurve;
			wheelCollider.sidewaysFriction = sidewaysFrictionCurve;

			wheelCollider.wheelDampingRate = physicsFrictions[0].damp;

			if (!RCC_Settings.Instance.dontUseAnyParticleEffects)
				emission = allWheelParticles[0].emission;
			
			audioClip = physicsFrictions[0].groundSound;

			if (wheelSlipAmountSideways > physicsFrictions[0].slip || wheelSlipAmountForward > physicsFrictions[0].slip){
				emission.enabled = true;
			}else{
				emission.enabled = false;
			}

		}

		if (!RCC_Settings.Instance.dontUseAnyParticleEffects) {

			for (int i = 0; i < allWheelParticles.Count; i++) {

				if (wheelSlipAmountSideways > startSlipValue || wheelSlipAmountForward > startSlipValue) {
				
				} else {
					emission = allWheelParticles [i].emission;
					emission.enabled = false;
				}
			
			}

		}

	}

	void Drift(float forwardSlip){
		
		Vector3 relativeVelocity = transform.InverseTransformDirection(rigid.velocity);
		float sqrVel = ((relativeVelocity.x * relativeVelocity.x)) / 15f;

		// Forward
		if(wheelCollider == carController.FrontLeftWheelCollider.wheelCollider || wheelCollider == carController.FrontRightWheelCollider.wheelCollider){
			forwardFrictionCurve.extremumValue = Mathf.Clamp(1f - sqrVel, .1f, maxForwardStiffness);
			forwardFrictionCurve.asymptoteValue = Mathf.Clamp(.75f - (sqrVel / 2f), .1f, minForwardStiffness);
		}else{
			forwardFrictionCurve.extremumValue = Mathf.Clamp(1f - sqrVel, .75f, maxForwardStiffness);
			forwardFrictionCurve.asymptoteValue = Mathf.Clamp(.75f - (sqrVel / 2f), .75f,  minForwardStiffness);
		}

		// Sideways
		if(wheelCollider == carController.FrontLeftWheelCollider.wheelCollider || wheelCollider == carController.FrontRightWheelCollider.wheelCollider){
			sidewaysFrictionCurve.extremumValue = Mathf.Clamp(1f - sqrVel / 1f, .6f, maxSidewaysStiffness);
			sidewaysFrictionCurve.asymptoteValue = Mathf.Clamp(.75f - (sqrVel / 2f), .6f, minSidewaysStiffness);
		}else{
			sidewaysFrictionCurve.extremumValue = Mathf.Clamp(1f - sqrVel, .5f, maxSidewaysStiffness);
			sidewaysFrictionCurve.asymptoteValue = Mathf.Clamp(.75f - (sqrVel / 2f), .5f, minSidewaysStiffness);
		}

	}

	void Audio(){

		if(RCC_Settings.Instance.useSharedAudioSources && isSkidding())
			return;

		if(totalSlip > startSlipValue){

			if(audioSource.clip != audioClip)
				audioSource.clip = audioClip;

			if(!audioSource.isPlaying)
				audioSource.Play();

			if(rigid.velocity.magnitude > 1f){
				audioSource.volume = Mathf.Lerp(audioSource.volume, Mathf.Lerp(0f, 1f, totalSlip - startSlipValue), Time.deltaTime * 5f);
				audioSource.pitch = Mathf.Lerp(1f, .8f, audioSource.volume);
			}else{
				audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * 5f);
			}
			
		}else{
			
			audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, Time.deltaTime * 5f);

			if(audioSource.volume <= .05f && audioSource.isPlaying)
				audioSource.Stop();
			
		}

	}

	bool isSkidding(){

		for (int i = 0; i < allWheelColliders.Count; i++) {

			if(allWheelColliders[i].totalSlip > totalSlip)
				return true;

		}

		return false;

	}

}