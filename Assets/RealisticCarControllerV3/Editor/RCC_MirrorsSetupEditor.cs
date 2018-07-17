//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Reflection;

public class RCC_MirrorsSetupEditor : Editor {

	static GameObject selectedCar;

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Mirrors To Vehicle", false, 6)]
	static void CreateBehavior(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			selectedCar = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject;
			CreateMirrors();

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Mirrors To Vehicle", true)]
	static bool CheckCreateBehavior() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	static void CreateMirrors () {

		if(!selectedCar.transform.GetComponentInChildren<RCC_Mirror>()){
			GameObject mirrors = (GameObject)Instantiate(RCC_Settings.Instance.mirrors, selectedCar.transform.position, selectedCar.transform.rotation);
			mirrors.transform.SetParent(selectedCar.GetComponent<RCC_CarControllerV3>().chassis.transform, true);
			mirrors.name = "Mirrors";
			RCC_LabelEditor.SetIcon(mirrors.transform.GetChild(0).gameObject, RCC_LabelEditor.Icon.DiamondRed);
			RCC_LabelEditor.SetIcon(mirrors.transform.GetChild(1).gameObject, RCC_LabelEditor.Icon.DiamondBlue);
			RCC_LabelEditor.SetIcon(mirrors.transform.GetChild(2).gameObject, RCC_LabelEditor.Icon.DiamondTeal);
			Selection.activeGameObject = mirrors;
			EditorUtility.DisplayDialog("Created Mirrors!", "Created mirrors. Adjust their positions.", "Ok");
		}else{
			EditorUtility.DisplayDialog("Vehicle Has Mirrors Already", "Vehicle has mirrors already!", "Ok");
		}
	
	}

}
