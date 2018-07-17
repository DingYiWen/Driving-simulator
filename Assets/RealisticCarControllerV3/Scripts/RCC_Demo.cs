//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Demo Manager")]
public class RCC_Demo : MonoBehaviour {

	public RCC_CarControllerV3[] selectableVehicles;
	public int selectedCarIndex = 0;
	public int selectedBehaviorIndex = 0;

	public void SelectVehicle (int index) {

		selectedCarIndex = index;
	
	}

	public void Spawn () {

		RCC_CarControllerV3[] activeVehicles = GameObject.FindObjectsOfType<RCC_CarControllerV3>();
		Vector3 lastKnownPos = new Vector3();
		Quaternion lastKnownRot = new Quaternion();
		GameObject newVehicle;

		if(activeVehicles != null && activeVehicles.Length > 0){
			foreach(RCC_CarControllerV3 rcc in activeVehicles){
				if(!rcc.AIController && rcc.canControl){
					lastKnownPos = rcc.transform.position;
					lastKnownRot = rcc.transform.rotation;
					break;
				}
			}
		}

		if(lastKnownPos == Vector3.zero){
			if(	GameObject.FindObjectOfType<RCC_Camera>()){
				lastKnownPos = GameObject.FindObjectOfType<RCC_Camera>().transform.position;
				lastKnownRot = GameObject.FindObjectOfType<RCC_Camera>().transform.rotation;
			}
		}

		lastKnownRot.x = 0f;
		lastKnownRot.z = 0f;

		for (int i = 0; i < activeVehicles.Length; i++) {

			if(activeVehicles[i].canControl && !activeVehicles[i].AIController){
				Destroy(activeVehicles[i].gameObject);
			}
			 
		}

		newVehicle = (GameObject)GameObject.Instantiate(selectableVehicles[selectedCarIndex].gameObject, lastKnownPos + (Vector3.up), lastKnownRot);
		 
		newVehicle.GetComponent<RCC_CarControllerV3>().canControl = true;

		if(	GameObject.FindObjectOfType<RCC_Camera>()){
			GameObject.FindObjectOfType<RCC_Camera>().SetPlayerCar(newVehicle);
		}

	}

	public void SelectBehavior(int index){

		selectedBehaviorIndex = index;

	}

	public void InitBehavior(){

		switch(selectedBehaviorIndex){
		case 0:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Simulator;
			RestartScene();
			break;
		case 1:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Racing;
			RestartScene();
			break;
		case 2:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.SemiArcade;
			RestartScene();
			break;
		case 3:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Drift;
			RestartScene();
			break;
		case 4:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Fun;
			RestartScene();
			break;
		case 5:
			RCC_Settings.Instance.behaviorType = RCC_Settings.BehaviorType.Custom;
			RestartScene();
			break;
		}

	}

	public void RestartScene(){

		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

	}

	public void Quit(){

		Application.Quit();

	}

}
