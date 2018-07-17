//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RCC_Useless : MonoBehaviour {

	public Useless useless;
	public enum Useless{Controller, Behavior}

	// Use this for initialization
	void Awake () {

		int type = 0;

		if(useless == Useless.Behavior){

			RCC_Settings.BehaviorType behavior = RCC_Settings.Instance.behaviorType;

			switch(behavior){
			case(RCC_Settings.BehaviorType.Simulator):
				type = 0;
				break;
			case(RCC_Settings.BehaviorType.Racing):
				type = 1;
				break;
			case(RCC_Settings.BehaviorType.SemiArcade):
				type = 2;
				break;
			case(RCC_Settings.BehaviorType.Drift):
				type = 3;
				break;
			case(RCC_Settings.BehaviorType.Fun):
				type = 4;
				break;
			case(RCC_Settings.BehaviorType.Custom):
				type = 5;
				break;
			}

		}else{

			if(!RCC_Settings.Instance.useAccelerometerForSteering && !RCC_Settings.Instance.useSteeringWheelForSteering)
				type = 0;
			if(RCC_Settings.Instance.useAccelerometerForSteering)
				type = 1;
			if(RCC_Settings.Instance.useSteeringWheelForSteering)
				type = 2;

		}

		GetComponent<Dropdown>().value = type;
		GetComponent<Dropdown>().RefreshShownValue();
	
	}

}
