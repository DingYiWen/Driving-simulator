//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Inputs")]
public class RCC_DashboardInputs : MonoBehaviour {

	public RCC_CarControllerV3 currentCarController;

	public GameObject RPMNeedle;
	public GameObject KMHNeedle;
	public GameObject turboGauge;
	public GameObject NOSGauge;
	public GameObject BoostNeedle;
	public GameObject NoSNeedle;

	private float RPMNeedleRotation = 0f;
	private float KMHNeedleRotation = 0f;
	private float BoostNeedleRotation = 0f;
	private float NoSNeedleRotation = 0f;

	public float RPM;
	public float KMH;
	internal int direction = 1;
	internal float Gear;
	internal bool NGear = false;

	internal bool ABS = false;
	internal bool ESP = false;
	internal bool Park = false;
	internal bool Headlights = false;
	internal RCC_CarControllerV3.IndicatorsOn indicators;

	void Update(){

		if(RCC_Settings.Instance.uiType == RCC_Settings.UIType.None){
			gameObject.SetActive(false);
			enabled = false;
			return;
		}

		GetValues();

	}
	
	public void GetVehicle(RCC_CarControllerV3 rcc){

		currentCarController = rcc;
		RCC_UIDashboardButton[] buttons = GameObject.FindObjectsOfType<RCC_UIDashboardButton>();

		foreach(RCC_UIDashboardButton button in buttons)
			button.Check();

	}

	void GetValues(){

		if(!currentCarController)
			return;

		if(!currentCarController.canControl || currentCarController.AIController){
			return;
		}

		if(NOSGauge){
			if(currentCarController.useNOS){
				if(!NOSGauge.activeSelf)
					NOSGauge.SetActive(true);
			}else{
				if(NOSGauge.activeSelf)
					NOSGauge.SetActive(false);
			}
		}

		if(turboGauge){
			if(currentCarController.useTurbo){
				if(!turboGauge.activeSelf)
					turboGauge.SetActive(true);
			}else{
				if(turboGauge.activeSelf)
					turboGauge.SetActive(false);
			}
		}
		
		RPM = currentCarController.engineRPM;
		KMH = currentCarController.speed;
		direction = currentCarController.direction;
		Gear = currentCarController.currentGear;

		NGear = currentCarController.changingGear;
		
		ABS = currentCarController.ABSAct;
		ESP = currentCarController.ESPAct;
		Park = currentCarController.handbrakeInput > .1f ? true : false;
		Headlights = currentCarController.lowBeamHeadLightsOn || currentCarController.highBeamHeadLightsOn;
		indicators = currentCarController.indicatorsOn;

		if(RPMNeedle){
			RPMNeedleRotation = (currentCarController.engineRPM / 50f);
			RPMNeedle.transform.eulerAngles = new Vector3(RPMNeedle.transform.eulerAngles.x ,RPMNeedle.transform.eulerAngles.y, -RPMNeedleRotation);
		}
		if(KMHNeedle){
			if(RCC_Settings.Instance.units == RCC_Settings.Units.KMH)
				KMHNeedleRotation = (currentCarController.speed);
			else
				KMHNeedleRotation = (currentCarController.speed * 0.62f);
			KMHNeedle.transform.eulerAngles = new Vector3(KMHNeedle.transform.eulerAngles.x ,KMHNeedle.transform.eulerAngles.y, -KMHNeedleRotation);
		}
		if(BoostNeedle){
			BoostNeedleRotation = (currentCarController.turboBoost / 30f) * 270f;
			BoostNeedle.transform.eulerAngles = new Vector3(BoostNeedle.transform.eulerAngles.x ,BoostNeedle.transform.eulerAngles.y, -BoostNeedleRotation);
		}
		if(NoSNeedle){
			NoSNeedleRotation = (currentCarController.NoS / 100f) * 270f;
			NoSNeedle.transform.eulerAngles = new Vector3(NoSNeedle.transform.eulerAngles.x ,NoSNeedle.transform.eulerAngles.y, -NoSNeedleRotation);
		}
			
	}

}



