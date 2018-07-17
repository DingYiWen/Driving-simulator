//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(RCC_CarControllerV3)), CanEditMultipleObjects]
public class RCC_Editor : Editor {

	RCC_CarControllerV3 carScript;

	static bool firstInit = false;
	
	Texture2D wheelIcon;
	Texture2D steerIcon;
	Texture2D suspensionIcon;
	Texture2D configIcon;
	Texture2D lightIcon;
	Texture2D soundIcon;
	Texture2D damageIcon;
	Texture2D stabilityIcon;
	
	bool WheelSettings;
	bool SteerSettings;
	bool SuspensionSettings;
	bool FrontSuspension;
	bool RearSuspension;
	bool Configurations;
	bool LightSettings;
	bool SoundSettings;
	bool DamageSettings;
	bool StabilitySettings;
	
	Color defBackgroundColor;

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Add Main Controller To Vehicle", false, 1)]
	static void CreateBehavior(){

		if(!Selection.activeGameObject.GetComponent<RCC_CarControllerV3>()){

			GameObject pivot = new GameObject (Selection.activeGameObject.name);
			pivot.transform.position = RCC_GetBounds.GetBoundsCenter (Selection.activeGameObject.transform);
			pivot.transform.rotation = Selection.activeGameObject.transform.rotation;

			pivot.AddComponent<RCC_CarControllerV3>();

			pivot.GetComponent<Rigidbody>().mass = 1350f;
			pivot.GetComponent<Rigidbody>().drag = .05f;
			pivot.GetComponent<Rigidbody>().angularDrag = .5f;
			pivot.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;

			Selection.activeGameObject.transform.SetParent (pivot.transform);
			Selection.activeGameObject = pivot;

			if(EditorUtility.DisplayDialog("Build Your Car With Default Prefabs and Settings", "Would You Like To Build Your Car With Default Prefabs and Settings?", "Yes", "No"))
				firstInit = true;

			EditorUtility.DisplayDialog("RCC Initialized", "Drag and drop all your wheel models in to ''Wheel Models'' from hierarchy.", "Ok");

		}else{

			EditorUtility.DisplayDialog("Your Gameobject Already Has Realistic Car ControllerV2", "Your Gameobject Already Has Realistic Car ControllerV2", "Ok");

		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Add Main Controller To Vehicle", true)]
	static bool CheckCreateBehavior() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}
	
	void Awake(){

		wheelIcon = Resources.Load("Editor/WheelIcon", typeof(Texture2D)) as Texture2D;
		steerIcon = Resources.Load("Editor/SteerIcon", typeof(Texture2D)) as Texture2D;
		suspensionIcon = Resources.Load("Editor/SuspensionIcon", typeof(Texture2D)) as Texture2D;
		configIcon = Resources.Load("Editor/ConfigIcon", typeof(Texture2D)) as Texture2D;
		lightIcon = Resources.Load("Editor/LightIcon", typeof(Texture2D)) as Texture2D;
		soundIcon = Resources.Load("Editor/SoundIcon", typeof(Texture2D)) as Texture2D;
		damageIcon = Resources.Load("Editor/DamageIcon", typeof(Texture2D)) as Texture2D;
		stabilityIcon = Resources.Load("Editor/StabilityIcon", typeof(Texture2D)) as Texture2D;
		
	}
	
	public override void OnInspectorGUI () {

		serializedObject.Update();

		carScript = (RCC_CarControllerV3)target;
		defBackgroundColor = GUI.backgroundColor;

		if(!carScript.GetComponent<RCC_AICarController>())
			carScript.AIController = false;

		if(firstInit){
			SetDefaultSettings();
			SetDefaultPrefabs();
		}
		
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		
		if(WheelSettings)
			GUI.backgroundColor = Color.gray;
		else GUI.backgroundColor = defBackgroundColor;
		
		if(GUILayout.Button(wheelIcon))
			WheelSettings = EnableCategory();
		
		if(SteerSettings)
			GUI.backgroundColor = Color.gray;
		else GUI.backgroundColor = defBackgroundColor;
		
		if(GUILayout.Button(steerIcon))
			SteerSettings = EnableCategory();

		if(SuspensionSettings)
			GUI.backgroundColor = Color.gray;
		else GUI.backgroundColor = defBackgroundColor;
		
		if(GUILayout.Button(suspensionIcon))
			SuspensionSettings = EnableCategory();
		
		if(Configurations)
			GUI.backgroundColor = Color.gray;
		else GUI.backgroundColor = defBackgroundColor;
		
		if(GUILayout.Button(configIcon))
			Configurations = EnableCategory();

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();

		if(StabilitySettings)
			GUI.backgroundColor = Color.gray;
		else GUI.backgroundColor = defBackgroundColor;

		if(GUILayout.Button(stabilityIcon))
			StabilitySettings = EnableCategory();

		if(LightSettings)
			GUI.backgroundColor = Color.gray;
		else GUI.backgroundColor = defBackgroundColor;

		if(GUILayout.Button(lightIcon))
			LightSettings = EnableCategory();
		
		if(SoundSettings)
			GUI.backgroundColor = Color.gray;
		else GUI.backgroundColor = defBackgroundColor;
		
		if(GUILayout.Button(soundIcon))
			SoundSettings = EnableCategory();

		if(DamageSettings)
			GUI.backgroundColor = Color.gray;
		else GUI.backgroundColor = defBackgroundColor;

		if(GUILayout.Button(damageIcon))
			DamageSettings = EnableCategory();

		GUI.backgroundColor = defBackgroundColor;
		EditorGUILayout.EndHorizontal();
		
		if(WheelSettings){
			
			EditorGUILayout.Space();
			GUI.color = Color.cyan;
			EditorGUILayout.HelpBox("Wheel Settings", MessageType.None);
			GUI.color = defBackgroundColor;
			EditorGUILayout.Space();

			EditorGUILayout.Space();
			if(GUILayout.Button("Create Necessary Gameobject Groups")){
				
				Transform[] objects = carScript.gameObject.GetComponentsInChildren<Transform>();
				bool didWeHaveThisObject = false;
				
				foreach(Transform g in objects){
					if (g.name == "Chassis") {
						didWeHaveThisObject = true;
					}
				}
				
				if(!didWeHaveThisObject){
					
					GameObject chassis = new GameObject("Chassis");
					chassis.transform.parent = carScript.transform;
					chassis.transform.localPosition = Vector3.zero;
					chassis.transform.localScale = Vector3.one;
					chassis.transform.rotation = carScript.transform.rotation;
					carScript.chassis = chassis;
					GameObject wheelModels = new GameObject("Wheel Models");
					wheelModels.transform.parent = chassis.transform;
					wheelModels.transform.localPosition = Vector3.zero;
					wheelModels.transform.localScale = Vector3.one;
					wheelModels.transform.rotation = carScript.transform.rotation;
					GameObject COM = new GameObject("COM");
					COM.transform.parent = carScript.transform;
					COM.transform.localPosition = Vector3.zero;
					COM.transform.localScale = Vector3.one;
					COM.transform.rotation = carScript.transform.rotation;
					carScript.COM = COM.transform;
					
				}else{
					
					EditorUtility.DisplayDialog("Vehicle has these groups already!", "Vehicle has these groups already!", "Ok");
					
				}
				
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("FrontLeftWheelTransform"), new GUIContent("Front Left Wheel Model", "Select front left wheel of your car."), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("FrontRightWheelTransform"), new GUIContent("Front Right Wheel Model", "Select front right wheel of your car."), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("RearLeftWheelTransform"), new GUIContent("Rear Left Wheel Model", "Select rear left wheel of your car."), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("RearRightWheelTransform"), new GUIContent("Rear Right Wheel Model", "Select rear right wheel of your car."), false);
			EditorGUILayout.Space();
			
			if(GUILayout.Button("Create Wheel Colliders")){
				
				WheelCollider[] wheelColliders = carScript.gameObject.GetComponentsInChildren<WheelCollider>();
				
				if(wheelColliders.Length >= 1)
					EditorUtility.DisplayDialog("Vehicle has Wheel Colliders already!", "Vehicle has Wheel Colliders already!", "Ok");
				else
					carScript.CreateWheelColliders();
				
			}
			
			if(carScript.FrontLeftWheelTransform == null || carScript.FrontRightWheelTransform == null || carScript.RearLeftWheelTransform == null || carScript.RearRightWheelTransform == null  )
				EditorGUILayout.HelpBox("Select all of your Wheel Models before creating Wheel Colliders", MessageType.Error);
			
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("FrontLeftWheelCollider"), new GUIContent("Front Left WheelCollider", "WheelColliders are generated when you click ''Create WheelColliders'' button. But if you want to create your WheelCollider yourself, select corresponding WheelCollider for each wheel after you created."), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("FrontRightWheelCollider"), new GUIContent("Front Right WheelCollider", "WheelColliders are generated when you click ''Create WheelColliders'' button. But if you want to create your WheelCollider yourself, select corresponding WheelCollider for each wheel after you created."), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("RearLeftWheelCollider"), new GUIContent("Rear Left WheelCollider", "WheelColliders are generated when you click ''Create WheelColliders'' button. But if you want to create your WheelCollider yourself, select corresponding WheelCollider for each wheel after you created."), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("RearRightWheelCollider"), new GUIContent("Rear Right WheelCollider", "WheelColliders are generated when you click ''Create WheelColliders'' button. But if you want to create your WheelCollider yourself, select corresponding WheelCollider for each wheel after you created."), false);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ExtraRearWheelsTransform"), new GUIContent("Extra Rear Wheel Models", "In case of if your vehicle has extra wheels."), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ExtraRearWheelsCollider"), new GUIContent("Extra Rear Wheel Colliders", "In case of if your vehicle has extra wheels."), true);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("applyEngineTorqueToExtraRearWheelColliders"), new GUIContent("Apply Engine Torque To Extra Rear Wheels", "Applies Engine Torque To Extra Rear Wheels."), false);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("SteeringWheel"), new GUIContent("Interior Steering Wheel Model", "In case of if your vehicle has individual steering wheel model in interior."), false);
			EditorGUILayout.Space();

		}
		
		if(SteerSettings){
			
			EditorGUILayout.Space();
			GUI.color = Color.cyan;
			EditorGUILayout.HelpBox("Steer Settings", MessageType.None);
			GUI.color = defBackgroundColor;
			EditorGUILayout.Space();

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("steerAngle"), new GUIContent("Maximum Steer Angle", "Maximum steer angle for your vehicle."), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("highspeedsteerAngle"), new GUIContent("Maximum Steer Angle At ''X'' Speed", "Maximum steer angle at highest speed."), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("highspeedsteerAngleAtspeed"), new GUIContent("''X'' Speed", "Steer Angle At Highest Speed."), false);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("antiRollFrontHorizontal"), new GUIContent("Anti Roll Front Horizontal", "Anti Roll Force for prevents flip overs."));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("antiRollRearHorizontal"), new GUIContent("Anti Roll Rear Horizontal", "Anti Roll Force for prevents flip overs."));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("antiRollVertical"), new GUIContent("Anti Roll Forward", "Anti Roll Force for preventing flip overs."));
			
		}

		if(SuspensionSettings){
			
			EditorGUILayout.Space();
			GUI.color = Color.cyan;
			EditorGUILayout.HelpBox("Suspension Settings", MessageType.None);
			GUI.color = defBackgroundColor;
			EditorGUILayout.Space();

			if(!carScript.FrontLeftWheelCollider || !carScript.FrontRightWheelCollider || !carScript.RearLeftWheelCollider || !carScript.RearRightWheelCollider){
				EditorGUILayout.HelpBox("Vehicle Missing Wheel Colliders. Be Sure You Have Created Wheel Colliders Before Adjusting Suspensions", MessageType.Error);
				return;
			}

			if(Selection.gameObjects.Length > 1){
				EditorGUILayout.HelpBox("Multiple Editing Suspensions Is Not Allowed", MessageType.Error);
				return;
			}

			JointSpring frontSspring = carScript.FrontLeftWheelCollider.wheelCollider.suspensionSpring;
			JointSpring rearSpring = carScript.RearLeftWheelCollider.wheelCollider.suspensionSpring;

			GUILayout.BeginHorizontal();

			if(FrontSuspension)
				GUI.backgroundColor = Color.gray;
			else
				GUI.backgroundColor = defBackgroundColor;

			if(GUILayout.Button("Front Suspensions")){
				FrontSuspension = true;
				RearSuspension = false;
			}

			if(RearSuspension)
				GUI.backgroundColor = Color.gray;
			else
				GUI.backgroundColor = defBackgroundColor;

			if(GUILayout.Button("Rear Suspensions")){
				FrontSuspension = false;
				RearSuspension = true;
			}

			GUI.backgroundColor = defBackgroundColor;

			GUILayout.EndHorizontal();

			if(FrontSuspension){
				EditorGUILayout.Space();
				//carScript.FrontLeftWheelCollider.wheelCollider.mass = carScript.FrontRightWheelCollider.wheelCollider.mass = EditorGUILayout.FloatField("Front Wheels Mass", carScript.FrontLeftWheelCollider.wheelCollider.mass);
				carScript.FrontLeftWheelCollider.wheelCollider.suspensionDistance = carScript.FrontRightWheelCollider.wheelCollider.suspensionDistance = EditorGUILayout.FloatField("Front Suspensions Distance", carScript.FrontLeftWheelCollider.wheelCollider.suspensionDistance);
				carScript.FrontLeftWheelCollider.wheelCollider.forceAppPointDistance = carScript.FrontRightWheelCollider.wheelCollider.forceAppPointDistance = EditorGUILayout.FloatField("Front Force App Distance", carScript.FrontLeftWheelCollider.wheelCollider.forceAppPointDistance);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("frontCamber"), new GUIContent("Front Camber Angle"));
				EditorGUILayout.Space();
				frontSspring.spring = EditorGUILayout.FloatField("Front Suspensions Spring", frontSspring.spring);
				frontSspring.damper = EditorGUILayout.FloatField("Front Suspensions Damping", frontSspring.damper);
				frontSspring.targetPosition = EditorGUILayout.FloatField("Front Suspensions Target Position", frontSspring.targetPosition);
				EditorGUILayout.Space();
			}

			if(RearSuspension){
				EditorGUILayout.Space();
				//carScript.RearLeftWheelCollider.wheelCollider.mass = carScript.RearRightWheelCollider.wheelCollider.mass = EditorGUILayout.FloatField("Rear Wheels Mass", carScript.RearLeftWheelCollider.wheelCollider.mass);
				carScript.RearLeftWheelCollider.wheelCollider.suspensionDistance = carScript.RearRightWheelCollider.wheelCollider.suspensionDistance = EditorGUILayout.FloatField("Rear Suspensions Distance", carScript.RearLeftWheelCollider.wheelCollider.suspensionDistance);
				carScript.RearLeftWheelCollider.wheelCollider.forceAppPointDistance = carScript.RearRightWheelCollider.wheelCollider.forceAppPointDistance = EditorGUILayout.FloatField("Rear Force App Distance", carScript.RearLeftWheelCollider.wheelCollider.forceAppPointDistance);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("rearCamber"), new GUIContent("Rear Camber Angle"));
				EditorGUILayout.Space();
				rearSpring.spring = EditorGUILayout.FloatField("Rear Suspensions Spring", rearSpring.spring);
				rearSpring.damper = EditorGUILayout.FloatField("Rear Suspensions Damping", rearSpring.damper);
				rearSpring.targetPosition = EditorGUILayout.FloatField("Rear Suspensions Target Position", rearSpring.targetPosition);
				EditorGUILayout.Space();
			}

			carScript.FrontLeftWheelCollider.wheelCollider.suspensionSpring = frontSspring;
			carScript.FrontRightWheelCollider.wheelCollider.suspensionSpring = frontSspring;
			carScript.RearLeftWheelCollider.wheelCollider.suspensionSpring = rearSpring;
			carScript.RearRightWheelCollider.wheelCollider.suspensionSpring = rearSpring;

			EditorGUILayout.Space();
			
		}
		
		if(Configurations){
			
			EditorGUILayout.Space();
			GUI.color = Color.cyan;
			EditorGUILayout.HelpBox("Configurations", MessageType.None);
			GUI.color = defBackgroundColor;
			EditorGUILayout.Space();

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("canControl"), new GUIContent("Can Be Controllable Now", "Enables/Disables controlling the vehicle."));
			EditorGUILayout.Space();
//			EditorGUILayout.PropertyField(serializedObject.FindProperty("runEngineAtAwake"), new GUIContent("Engine Is Running At Awake", "Engine is running defaultly at start of the game."));
			EditorGUILayout.LabelField("Engine Is Running Now", carScript.engineRunning.ToString());
//			EditorGUILayout.PropertyField(serializedObject.FindProperty("canEngineStall"), new GUIContent("Engine Can Be Stalled?", "Stalled Engine due to low RPM."));
			EditorGUILayout.Space();
//			EditorGUILayout.PropertyField(serializedObject.FindProperty("autoReverse"), new GUIContent("Auto Reverse", "Enables/Disables auto reversing when player press brake button. Useful for if you are making parking style game."));
			//EditorGUILayout.PropertyField(serializedObject.FindProperty("automaticGear"), new GUIContent("Automatic Gear Shifting", "Enables/Disables automatic gear shifting of the vehicle."));

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_wheelTypeChoise"));

			if(carScript._wheelTypeChoise == RCC_CarControllerV3.WheelType.BIASED){
				EditorGUI.indentLevel ++;
				EditorGUILayout.PropertyField(serializedObject.FindProperty("biasedWheelTorque"), new GUIContent("Front / Rear"));
				//EditorGUILayout.BeginHorizontal();
				EditorGUILayout.FloatField("Front", 100 -carScript.biasedWheelTorque);
				EditorGUILayout.FloatField("Rear", carScript.biasedWheelTorque);
				//EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel --;
			}
			
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("COM"), new GUIContent("Center Of Mass (''COM'')", "Center of Mass of the vehicle. Usually, COM is below around front driver seat."));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("totalGears"), new GUIContent("Total Gears Count", "How Many Gears Your Vehicle Has?"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("gearShiftingDelay"), new GUIContent("Gear Shifting Delay", "Gear Shifting Delay"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("currentGear"), new GUIContent("Current Gear"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("engineTorque"), new GUIContent("Maximum Engine Torque"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("brakeTorque"), new GUIContent("Maximum Brake Torque"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxspeed"), new GUIContent("Maximum Speed"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("downForce"), new GUIContent("DownForce"), false);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("minEngineRPM"), new GUIContent("Lowest Engine RPM"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxEngineRPM"), new GUIContent("Highest Engine RPM"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("engineInertia"), new GUIContent("Engine Inertia"), false);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useRevLimiter"), new GUIContent("Rev Limiter"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useNOS"), new GUIContent("Use NOS"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useTurbo"), new GUIContent("Use Turbo"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useExhaustFlame"), new GUIContent("Use Exhaust Flame"), false);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("chassis"), new GUIContent("Chassis", "Script will simulate chassis movement based on vehicle rigidbody situation."), false);

			if (RCC_Settings.Instance.dontUseChassisJoint) {
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("chassisVerticalLean"), new GUIContent ("Chassis Vertical Sensitivity", "Script will simulate chassis movement based on vehicle rigidbody situation."), false);
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("chassisHorizontalLean"), new GUIContent ("Chassis Horizontal Sensitivity", "Script will simulate chassis movement based on vehicle rigidbody situation."), false);
			}

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("autoGenerateGearCurves"), new GUIContent("Generate All Gear Torque Curves"), false);

			if(!carScript.autoGenerateGearCurves)
				EditorGUILayout.PropertyField(serializedObject.FindProperty("engineTorqueCurve"), new GUIContent("All Gear Torque Curves"), true);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("autoGenerateTargetSpeedsForChangingGear"), new GUIContent("Generate All Target Speeds For Changing Gears"), false);

			if(!carScript.autoGenerateTargetSpeedsForChangingGear)
				EditorGUILayout.PropertyField(serializedObject.FindProperty("gearSpeed"), new GUIContent("Target Speeds For Changing Gears"), true);

			EditorGUILayout.Space();

		}

		if(StabilitySettings){

			EditorGUILayout.Space();
			GUI.color = Color.cyan;
			EditorGUILayout.HelpBox("Stability System Settings", MessageType.None);
			GUI.color = defBackgroundColor;
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("ABS"), new GUIContent("ABS"), false);

			if(carScript.ABS)
				EditorGUILayout.PropertyField(serializedObject.FindProperty("ABSThreshold"), new GUIContent("ABS Threshold"), false);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("TCS"), new GUIContent("TCS"), false);

			if(carScript.TCS){
				EditorGUILayout.PropertyField(serializedObject.FindProperty("TCSThreshold"), new GUIContent("TCS Threshold"), false);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("TCSStrength"), new GUIContent("TCS Strength"), false);
			}

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("ESP"), new GUIContent("ESP"), false);

			if(carScript.ESP){
				EditorGUILayout.PropertyField(serializedObject.FindProperty("ESPThreshold"), new GUIContent("ESP Threshold"), false);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("ESPStrength"), new GUIContent("ESP Strength"), false);
			}

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("steeringHelper"), new GUIContent("Steering Helper"), false);

			if (carScript.steeringHelper) {
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("steerHelperLinearVelStrength"), new GUIContent ("Steering Helper Linear Velocity Strength"), false);
				EditorGUILayout.PropertyField (serializedObject.FindProperty ("steerHelperAngularVelStrength"), new GUIContent ("Steering Helper Angular Velocity Strength"), false);
			}

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("tractionHelper"), new GUIContent("Traction Helper"), false);

			if(carScript.tractionHelper)
				EditorGUILayout.PropertyField(serializedObject.FindProperty("tractionHelperStrength"), new GUIContent("Traction Helper Strength"), false);

			EditorGUILayout.Space();

		}
		
		if(SoundSettings){
			
			EditorGUILayout.Space();
			GUI.color = Color.cyan;
			EditorGUILayout.HelpBox("Sound Settings", MessageType.None);
			GUI.color = defBackgroundColor;

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("engineClipOn"), new GUIContent("Engine Sound On"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("engineClipOff"), new GUIContent("Engine Sound Off"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("engineClipIdle"), new GUIContent("Engine Sound Idle"), false);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("engineStartClip"), new GUIContent("Engine Starting Sound", "Optional"), false);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("minEngineSoundPitch"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxEngineSoundPitch"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("minEngineSoundVolume"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxEngineSoundVolume"));
			EditorGUILayout.Space();

		}
		
		if(LightSettings){
			
			EditorGUILayout.Space();
			GUI.color = Color.cyan;
			EditorGUILayout.HelpBox("Light Settings", MessageType.None);
			GUI.color = defBackgroundColor;
			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("lowBeamHeadLightsOn"), new GUIContent("Head Lights On"));
			EditorGUILayout.Space();

			RCC_Light[] lights = carScript.GetComponentsInChildren<RCC_Light>();

			EditorGUILayout.LabelField("Head Lights", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			EditorGUI.indentLevel ++;

			for (int i = 0; i < lights.Length; i++) {

				EditorGUILayout.BeginHorizontal();
				if(lights[i].lightType == RCC_Light.LightType.HeadLight){
					EditorGUILayout.ObjectField("Head Light", lights[i].GetComponent<Light>(), typeof(Light), true);
					GUI.color = Color.red;
					if(GUILayout.Button("X", GUILayout.Width(25f)))
						DestroyImmediate(lights[i].gameObject);
					GUI.color = defBackgroundColor;
				}
				EditorGUILayout.EndHorizontal();

			}

			EditorGUILayout.Space();
			EditorGUI.indentLevel --;
			EditorGUILayout.LabelField("Brake Lights", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			EditorGUI.indentLevel ++;

			for (int i = 0; i < lights.Length; i++) {

				EditorGUILayout.BeginHorizontal();
				if(lights[i].lightType == RCC_Light.LightType.BrakeLight){
					EditorGUILayout.ObjectField("Brake Light", lights[i].GetComponent<Light>(), typeof(Light), true);
					GUI.color = Color.red;
					if(GUILayout.Button("X", GUILayout.Width(25f)))
						DestroyImmediate(lights[i].gameObject);
					GUI.color = defBackgroundColor;
				}
				EditorGUILayout.EndHorizontal();

			}

			EditorGUILayout.Space();
			EditorGUI.indentLevel --;
			EditorGUILayout.LabelField("Reverse Lights", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			EditorGUI.indentLevel ++;

			for (int i = 0; i < lights.Length; i++) {

				EditorGUILayout.BeginHorizontal();
				if(lights[i].lightType == RCC_Light.LightType.ReverseLight){
					EditorGUILayout.ObjectField("Reverse Light", lights[i].GetComponent<Light>(), typeof(Light), true);
					GUI.color = Color.red;
					if(GUILayout.Button("X", GUILayout.Width(25f)))
						DestroyImmediate(lights[i].gameObject);
					GUI.color = defBackgroundColor;
				}
				EditorGUILayout.EndHorizontal();

			}

			EditorGUILayout.Space();
			EditorGUI.indentLevel --;
			EditorGUILayout.LabelField("Indicator Lights", EditorStyles.boldLabel);
			EditorGUILayout.Space();
			EditorGUI.indentLevel ++;

			for (int i = 0; i < lights.Length; i++) {

				EditorGUILayout.BeginHorizontal();
				if(lights[i].lightType == RCC_Light.LightType.Indicator){
					EditorGUILayout.ObjectField("Indicator Light", lights[i].GetComponent<Light>(), typeof(Light), true);
					GUI.color = Color.red;
					if(GUILayout.Button("X", GUILayout.Width(25f)))
						DestroyImmediate(lights[i].gameObject);
					GUI.color = defBackgroundColor;
				}
				EditorGUILayout.EndHorizontal();

			}

			EditorGUI.indentLevel --;
			EditorGUILayout.Space();

			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("+ Head Light"))
				RCC_EditorWindows.CreateHeadLight();
			if(GUILayout.Button("+ Brake Light"))
				RCC_EditorWindows.CreateBrakeLight();
			if(GUILayout.Button("+ Reverse Light"))
				RCC_EditorWindows.CreateReverseLight();
			if(GUILayout.Button("+ Indicator Light"))
				RCC_EditorWindows.CreateIndicatorLight();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Space();
			
		}

		if(DamageSettings){
			
			EditorGUILayout.Space();
			GUI.color = Color.cyan;
			EditorGUILayout.HelpBox("Damage Settings", MessageType.None);
			GUI.color = defBackgroundColor;
			EditorGUILayout.Space();

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useDamage"), new GUIContent("Use Damage"), false);
			EditorGUILayout.Space();

			if(carScript.useDamage){

				EditorGUILayout.PropertyField(serializedObject.FindProperty("deformableMeshFilters"), new GUIContent("Deformable Mesh Filters", "If no mesh filters selected, will collect all mesh filters in children except wheel models."), true);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("randomizeVertices"), new GUIContent("Randomize Vertices", "Randomizes vertices movement angle."), false);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("damageRadius"), new GUIContent("Damage Radius on Contact", "Damage radius on contact."), false);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumDamage"), new GUIContent("Maximum Damage", "Maximum deformable damage."), false);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("damageMultiplier"), new GUIContent("Damage Multiplier", "Damage mutliplier."), false);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("maximumContactSparkle"), new GUIContent("Maximum Contact Sparkle Count", "Script is re-using existing collision particles. 3 is good for mobile, 5 and above is good for PC."), false);

				if(carScript.repaired){
					GUI.color = Color.gray;
					GUILayout.Button("Repaired");
				}else{
					GUI.color = Color.green;
					if(GUILayout.Button("Repair Now"))
						carScript.repairNow = true;
				}

			}

			EditorGUILayout.Space();
			
		}

		if(carScript.GetComponent<RCC_AICarController>()){

			EditorGUILayout.Space();
			EditorGUILayout.HelpBox("This Vehicle Is Controlling By AI. Therefore, All Player Controllers Are Disabled For This Vehicle.", MessageType.Info);
			EditorGUILayout.Space();

			if(GUILayout.Button("Remove AI Controller From Vehicle")){
				carScript.AIController = false;
				DestroyImmediate(carScript.GetComponent<RCC_AICarController>());
			}

		}

		EditorGUILayout.Space();
		GUI.color = Color.cyan;
		EditorGUILayout.HelpBox("System Overall Check", MessageType.None);
		GUI.color = defBackgroundColor;
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		
		if(carScript.FrontLeftWheelCollider == null || carScript.FrontRightWheelCollider == null || carScript.RearLeftWheelCollider == null || carScript.RearRightWheelCollider == null)
			EditorGUILayout.HelpBox("Wheel Colliders = NOT OK", MessageType.Error);
		
		if(carScript.FrontLeftWheelTransform == null || carScript.FrontRightWheelTransform == null || carScript.RearLeftWheelTransform == null || carScript.RearRightWheelTransform == null)
			EditorGUILayout.HelpBox("Wheel Models = NOT OK", MessageType.Error);
		
		if(carScript.COM == null)
			EditorGUILayout.HelpBox("COM = NOT OK", MessageType.Error);

		Collider[] cols = carScript.gameObject.GetComponentsInChildren<Collider>();
		int totalCountsOfWheelColliders = carScript.GetComponentsInChildren<WheelCollider>().Length;

		if(cols.Length - totalCountsOfWheelColliders <= 0)
			EditorGUILayout.HelpBox("Your vehicle MUST have any type of body Collider.", MessageType.Error);
		
		EditorGUILayout.EndHorizontal();

		if(carScript.COM){
			
			if(Mathf.Approximately(carScript.COM.transform.localPosition.y, 0f))
				EditorGUILayout.HelpBox("You haven't changed COM position of the vehicle yet. Keep in that your mind, COM is most extremely important for realistic behavior.", MessageType.Warning);
			
		}else{
			
			EditorGUILayout.HelpBox("You haven't created COM of the vehicle yet. Just hit ''Create Necessary Gameobject Groups'' under ''Wheel'' tab for creating this too.", MessageType.Error);
			
		}

		if (carScript.chassis != null) {
			if (!carScript.chassis.GetComponent<RCC_Chassis> ())
				carScript.chassis.AddComponent<RCC_Chassis> ();
		}
		
		serializedObject.ApplyModifiedProperties();

		if(GUI.changed && !EditorApplication.isPlaying){
			SetLayerMask();
			carScript.defMaxSpeed = 0f;
			if(carScript.autoGenerateGearCurves)
				carScript.TorqueCurve();
		}
		
	}

	void SetDefaultSettings(){
		
		Transform[] objects = carScript.gameObject.GetComponentsInChildren<Transform>();
		bool didWeHaveThisObject = false;
		
		foreach(Transform g in objects){
			if(g.name == "Chassis")
				didWeHaveThisObject = true;
		}
		
		if(!didWeHaveThisObject){
			
			GameObject chassis = new GameObject("Chassis");
			chassis.transform.parent = carScript.transform;
			chassis.transform.localPosition = Vector3.zero;
			chassis.transform.localScale = Vector3.one;
			chassis.transform.rotation = carScript.transform.rotation;
			carScript.chassis = chassis;

			foreach(Transform g in objects){
				if(g.parent == carScript.transform){
					g.transform.parent = carScript.chassis.transform;
				}
			}

			GameObject wheelModels = new GameObject("Wheel Models");
			wheelModels.transform.parent = carScript.transform;
			wheelModels.transform.localPosition = Vector3.zero;
			wheelModels.transform.localScale = Vector3.one;
			wheelModels.transform.rotation = carScript.transform.rotation;

			GameObject COM = new GameObject("COM");
			COM.transform.parent = carScript.transform;
			COM.transform.localPosition = Vector3.zero;
			COM.transform.localScale = Vector3.one;
			COM.transform.rotation = carScript.transform.rotation;
			carScript.COM = COM.transform;
			
		}

	}

	bool EnableCategory(){

		WheelSettings = false;
		SteerSettings = false;
		SuspensionSettings = false;
		FrontSuspension = false;
		RearSuspension = false;
		Configurations = false;
		StabilitySettings = false;
		LightSettings = false;
		SoundSettings = false;
		DamageSettings = false;

		return true;

	}

	void SetDefaultPrefabs(){

		if(!firstInit)
			return;

		firstInit = false;
		
	}

	void SetLayerMask(){

		if (string.IsNullOrEmpty (RCC_Settings.Instance.RCCLayer)) {
			Debug.LogError ("RCC Layer is missing in RCC Settings. Go to Tools --> BoneCracker Games --> RCC --> Edit Settings, and set the layer of RCC.");
			return;
		}

		if (string.IsNullOrEmpty (RCC_Settings.Instance.RCCTag)) {
			Debug.LogError ("RCC Tag is missing in RCC Settings. Go to Tools --> BoneCracker Games --> RCC --> Edit Settings, and set the tag of RCC.");
			return;
		}

		Transform[] allTransforms = carScript.GetComponentsInChildren<Transform>();

		foreach (Transform t in allTransforms) {
			t.gameObject.layer = LayerMask.NameToLayer (RCC_Settings.Instance.RCCLayer);
			if (!carScript.AIController) {
				if(RCC_Settings.Instance.tagAllChildrenGameobjects)
					t.gameObject.transform.tag = RCC_Settings.Instance.RCCTag;
				else
					carScript.transform.gameObject.tag = RCC_Settings.Instance.RCCTag;
			} else {
				t.gameObject.transform.tag = "Untagged";
			}
		}

	}
	
}
