//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;

public class RCC_EditorWindows : Editor {
	
	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Edit RCC Settings", false, 0)]
	public static void OpenRCCSettings(){
		Selection.activeObject =RCC_Settings.Instance;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Configure Ground Materials", false, 2)]
	public static void OpenGroundMaterialsSettings(){
		Selection.activeObject =RCC_GroundMaterials.Instance;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Hood Camera To Vehicle", false, 3)]
	public static void CreateHoodCamera(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			if(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponentInChildren<RCC_HoodCamera>()){
				EditorUtility.DisplayDialog("Your Vehicle Has Hood Camera Already!", "Your vehicle has hood camera already!", "Ok");
				Selection.activeGameObject = Selection.activeGameObject.GetComponentInChildren<RCC_HoodCamera>().gameObject;
				return;
			}

			GameObject hoodCam = (GameObject)Instantiate(Resources.Load("RCCAssets/HoodCamera", typeof(GameObject)), Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.position, Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.rotation);
			hoodCam.name = Resources.Load ("RCCAssets/HoodCamera", typeof(GameObject)).name;
			hoodCam.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, true);
			hoodCam.GetComponent<ConfigurableJoint>().connectedBody = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponent<Rigidbody>();
			RCC_LabelEditor.SetIcon(hoodCam, RCC_LabelEditor.LabelIcon.Purple);
			Selection.activeGameObject = hoodCam;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Hood Camera To Vehicle", true)]
	public static bool CheckCreateHoodCamera() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Wheel Camera To Vehicle", false, 4)]
	public static void CreateWheelCamera(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			if(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().gameObject.GetComponentInChildren<RCC_WheelCamera>()){
				EditorUtility.DisplayDialog("Your Vehicle Has Wheel Camera Already!", "Your vehicle has wheel camera already!", "Ok");
				Selection.activeGameObject = Selection.activeGameObject.GetComponentInChildren<RCC_WheelCamera>().gameObject;
				return;
			}

			GameObject wheelCam = new GameObject("Wheel Camera");
			wheelCam.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform, false);
			wheelCam.AddComponent<RCC_WheelCamera>();
			RCC_LabelEditor.SetIcon(wheelCam, RCC_LabelEditor.LabelIcon.Purple);
			Selection.activeGameObject = wheelCam;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Wheel Camera To Vehicle", true)]
	public static bool CheckCreateWheelCamera() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Exhaust To Vehicle", false, 5)]
	public static void CreateExhaust(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			GameObject exhaustsMain;

			if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Exhausts")){
				exhaustsMain = new GameObject("Exhausts");
				exhaustsMain.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, false);
			}else{
				exhaustsMain = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Exhausts").gameObject;
			}

			GameObject exhaust = (GameObject)Instantiate(RCC_Settings.Instance.exhaustGas, Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.position, Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.rotation * Quaternion.Euler(0f, 180f, 0f));
			exhaust.name = RCC_Settings.Instance.exhaustGas.name;
			exhaust.transform.SetParent(exhaustsMain.transform);
			exhaust.transform.localPosition = new Vector3(1f, 0f, -2f);
			RCC_LabelEditor.SetIcon(exhaust, RCC_LabelEditor.Icon.DiamondGray);
			Selection.activeGameObject = exhaust;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Exhaust To Vehicle", true)]
	public static bool CheckCreateExhaust() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/HeadLight", false, 10)]
	public static void CreateHeadLight(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			GameObject lightsMain;

			if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights")){
				lightsMain = new GameObject("Lights");
				lightsMain.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, false);
			}else{
				lightsMain = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights").gameObject;
			}

			GameObject headLight = GameObject.Instantiate (RCC_Settings.Instance.headLights, lightsMain.transform.position, lightsMain.transform.rotation) as GameObject;
			headLight.name = RCC_Settings.Instance.headLights.name;
			headLight.transform.SetParent(lightsMain.transform);
			headLight.transform.localRotation = Quaternion.identity;
			headLight.transform.localPosition = new Vector3(0f, 0f, 2f);
			RCC_LabelEditor.SetIcon(headLight, RCC_LabelEditor.Icon.CircleTeal);
			Selection.activeGameObject = headLight;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/HeadLight", true)]
	public static bool CheckHeadLight() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Brake", false, 11)]
	public static void CreateBrakeLight(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			GameObject lightsMain;

			if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights")){
				lightsMain = new GameObject("Lights");
				lightsMain.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, false);
			}else{
				lightsMain = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights").gameObject;
			}

			GameObject brakeLight = GameObject.Instantiate (RCC_Settings.Instance.brakeLights, lightsMain.transform.position, lightsMain.transform.rotation) as GameObject;
			brakeLight.name = RCC_Settings.Instance.brakeLights.name;
			brakeLight.transform.SetParent(lightsMain.transform);
			brakeLight.transform.localRotation = Quaternion.identity;
			brakeLight.transform.localPosition = new Vector3(0f, 0f, -2f);
			RCC_LabelEditor.SetIcon(brakeLight, RCC_LabelEditor.Icon.CircleRed);
			Selection.activeGameObject = brakeLight;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Brake", true)]
	public static bool CheckBrakeLight() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Reverse", false, 12)]
	public static void CreateReverseLight(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			GameObject lightsMain;

			if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights")){
				lightsMain = new GameObject("Lights");
				lightsMain.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, false);
			}else{
				lightsMain = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights").gameObject;
			}

			GameObject reverseLight = GameObject.Instantiate (RCC_Settings.Instance.reverseLights, lightsMain.transform.position, lightsMain.transform.rotation) as GameObject;
			reverseLight.name = RCC_Settings.Instance.reverseLights.name;
			reverseLight.transform.SetParent(lightsMain.transform);
			reverseLight.transform.localRotation = Quaternion.identity;
			reverseLight.transform.localPosition = new Vector3(0f, 0f, -2f);
			RCC_LabelEditor.SetIcon(reverseLight, RCC_LabelEditor.Icon.CircleGray);
			Selection.activeGameObject = reverseLight;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Reverse", true)]
	public static bool CheckReverseLight() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Indicator", false, 13)]
	public static void CreateIndicatorLight(){

		if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>()){

			EditorUtility.DisplayDialog("Select a vehicle controlled by Realistic Car Controller!", "Select a vehicle controlled by Realistic Car Controller!", "Ok");

		}else{

			GameObject lightsMain;

			if(!Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights")){
				lightsMain = new GameObject("Lights");
				lightsMain.transform.SetParent(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.transform, false);
			}else{
				lightsMain = Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().transform.Find(Selection.activeGameObject.GetComponentInParent<RCC_CarControllerV3>().chassis.name+"/Lights").gameObject;
			}

			GameObject indicatorLight = GameObject.Instantiate (RCC_Settings.Instance.indicatorLights, lightsMain.transform.position, lightsMain.transform.rotation) as GameObject;
			indicatorLight.name = RCC_Settings.Instance.indicatorLights.name;
			indicatorLight.transform.SetParent(lightsMain.transform);
			indicatorLight.transform.localRotation = Quaternion.identity;
			indicatorLight.transform.localPosition = new Vector3(0f, 0f, -2f);
			RCC_LabelEditor.SetIcon(indicatorLight, RCC_LabelEditor.Icon.CircleOrange);
			Selection.activeGameObject = indicatorLight;

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Misc/Add Lights To Vehicle/Indicator", true)]
	public static bool CheckIndicatorLight() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}
	
}
