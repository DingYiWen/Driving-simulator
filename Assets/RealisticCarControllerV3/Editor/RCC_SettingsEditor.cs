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
using System.Collections.Generic;

[CustomEditor(typeof(RCC_Settings))]
public class RCC_SettingsEditor : Editor {

//	[MenuItem("Assets/Create/YourClass")]
//	public static void CreateAsset ()
//	{
//		ScriptableObjectUtility.CreateAsset<Vehicles> ();
//	}

	RCC_Settings RCCSettingsAsset;

	Color originalGUIColor;
	Vector2 scrollPos;
	PhysicMaterial[] physicMaterials;

	bool foldGeneralSettings = false;
	bool foldControllerSettings = false;
	bool foldUISettings = false;
	bool foldWheelPhysics = false;
	bool foldSFX = false;
	bool foldOptimization = false;

	void OnEnable(){

		foldGeneralSettings = RCC_Settings.Instance.foldGeneralSettings;
		foldControllerSettings = RCC_Settings.Instance.foldControllerSettings;
		foldUISettings = RCC_Settings.Instance.foldUISettings;
		foldWheelPhysics = RCC_Settings.Instance.foldWheelPhysics;
		foldSFX = RCC_Settings.Instance.foldSFX;
		foldOptimization = RCC_Settings.Instance.foldOptimization;

	}

	void OnDestroy(){

		RCC_Settings.Instance.foldGeneralSettings = foldGeneralSettings;
		RCC_Settings.Instance.foldControllerSettings = foldControllerSettings;
		RCC_Settings.Instance.foldUISettings = foldUISettings;
		RCC_Settings.Instance.foldWheelPhysics = foldWheelPhysics;
		RCC_Settings.Instance.foldSFX = foldSFX;
		RCC_Settings.Instance.foldOptimization = foldOptimization;

	}

	public override void OnInspectorGUI (){

		serializedObject.Update();
		RCCSettingsAsset = (RCC_Settings)target;

		originalGUIColor = GUI.color;
		EditorGUIUtility.labelWidth = 250;
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("RCC Asset Settings Editor Window", EditorStyles.boldLabel);
		GUI.color = new Color(.75f, 1f, .75f);
		EditorGUILayout.LabelField("This editor will keep update necessary .asset files in your project for RCC. Don't change directory of the ''Resources/RCCAssets''.", EditorStyles.helpBox);
		GUI.color = originalGUIColor;
		EditorGUILayout.Space();

		EditorGUI.indentLevel++;

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false );

		EditorGUILayout.Space();

		foldGeneralSettings = EditorGUILayout.Foldout(foldGeneralSettings, "General Settings");

		if(foldGeneralSettings){

			EditorGUILayout.BeginVertical (GUI.skin.box);
			GUILayout.Label("General Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("overrideFixedTimeStep"), new GUIContent("Override FixedTimeStep"));
			if(RCCSettingsAsset.overrideFixedTimeStep)
				EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedTimeStep"), new GUIContent("Fixed Timestep"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAngularVelocity"), new GUIContent("Maximum Angular Velocity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("behaviorType"), new GUIContent("Behavior Type"));
			GUI.color = new Color(.75f, 1f, .75f);
			EditorGUILayout.HelpBox("Using behavior preset will override wheelcollider settings, chassis joint, antirolls, and other stuff. Using ''Custom'' mode will not override anything.", MessageType.Info);
			GUI.color = originalGUIColor;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useFixedWheelColliders"), new GUIContent("Use Fixed WheelColliders"));
			EditorGUILayout.EndVertical ();

		}

		EditorGUILayout.Space();

		foldControllerSettings = EditorGUILayout.Foldout(foldControllerSettings, "Controller Settings");

		if(foldControllerSettings){
			
			List<string> controllerTypeStrings =  new List<string>();
			controllerTypeStrings.Add("Keyboard");	controllerTypeStrings.Add("Mobile");		controllerTypeStrings.Add("Custom");
			EditorGUILayout.BeginVertical (GUI.skin.box);

			GUI.color = new Color(.5f, 1f, 1f, 1f);
			GUILayout.Label("Main Controller Type", EditorStyles.boldLabel);
			RCCSettingsAsset.toolbarSelectedIndex = GUILayout.Toolbar(RCCSettingsAsset.toolbarSelectedIndex, controllerTypeStrings.ToArray());
			GUI.color = originalGUIColor;
			EditorGUILayout.Space();


			if(RCCSettingsAsset.toolbarSelectedIndex == 0){

				RCCSettingsAsset.controllerType = RCC_Settings.ControllerType.Keyboard;

				EditorGUILayout.BeginVertical (GUI.skin.box);

				GUILayout.Label("Keyboard Settings", EditorStyles.boldLabel);

				EditorGUILayout.PropertyField(serializedObject.FindProperty("verticalInput"), new GUIContent("Gas/Reverse Input Axis"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("horizontalInput"), new GUIContent("Steering Input Axis"));
				GUI.color = new Color(.75f, 1f, .75f);
				EditorGUILayout.HelpBox("You can edit your vertical and horizontal input axis in Edit --> Project Settings --> Input.", MessageType.Info);
				GUI.color = originalGUIColor;
				EditorGUILayout.PropertyField(serializedObject.FindProperty("startEngineKB"), new GUIContent("Start/Stop Engine Key"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("lowBeamHeadlightsKB"), new GUIContent("Low Beam Headlights"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("highBeamHeadlightsKB"), new GUIContent("High Beam Headlights"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("changeCameraKB"), new GUIContent("Change Camera"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("rightIndicatorKB"), new GUIContent("Indicator Right"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("leftIndicatorKB"), new GUIContent("Indicator Left"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("hazardIndicatorKB"), new GUIContent("Indicator Hazard"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("shiftGearUp"), new GUIContent("Gear Shift Up"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("shiftGearDown"), new GUIContent("Gear Shift Down"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("boostKB"), new GUIContent("Boost"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("enterExitVehicleKB"), new GUIContent("Get In & Get Out Of The Vehicle"));
				EditorGUILayout.Space();

				EditorGUILayout.EndVertical ();

		}
				
		if(RCCSettingsAsset.toolbarSelectedIndex == 1){

			EditorGUILayout.BeginVertical (GUI.skin.box);

			RCCSettingsAsset.controllerType = RCC_Settings.ControllerType.Mobile;

			GUILayout.Label("Mobile Settings", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("uiType"), new GUIContent("UI Type"));

			GUI.color = new Color(.75f, 1f, .75f);
			EditorGUILayout.HelpBox("All UI/NGUI buttons will feed the vehicles at runtime.", MessageType.Info);
			GUI.color = originalGUIColor;

			EditorGUILayout.PropertyField(serializedObject.FindProperty("UIButtonSensitivity"), new GUIContent("UI Button Sensitivity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("UIButtonGravity"), new GUIContent("UI Button Gravity"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("gyroSensitivity"), new GUIContent("Gyro Sensitivity"));

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useAccelerometerForSteering"), new GUIContent("Use Accelerometer For Steering"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useSteeringWheelForSteering"), new GUIContent("Use Steering Wheel For Steering"));
			
			GUI.color = new Color(.75f, 1f, .75f);
			EditorGUILayout.HelpBox("You can enable/disable Accelerometer in your game by just calling ''RCCAssetSettings.Instance.useAccelerometerForSteering = true/false;''.", MessageType.Info);
			EditorGUILayout.HelpBox("You can enable/disable Steering Wheel Controlling in your game by just calling ''RCCAssetSettings.Instance.useSteeringWheelForSteering = true/false;''.", MessageType.Info);
			GUI.color = originalGUIColor;
			EditorGUILayout.Space();

			EditorGUILayout.EndVertical ();

		}

		if(RCCSettingsAsset.toolbarSelectedIndex == 2){

				EditorGUILayout.BeginVertical (GUI.skin.box);

			RCCSettingsAsset.controllerType = RCC_Settings.ControllerType.Custom;

				GUILayout.Label("Custom Input Settings", EditorStyles.boldLabel);

				GUI.color = new Color(.75f, 1f, .75f);
				EditorGUILayout.HelpBox("In this mode, car controller won't receive these inputs from keyboard or UI buttons. You need to feed these inputs in your own script.", MessageType.Info);
				EditorGUILayout.Space();
				EditorGUILayout.HelpBox("Car controller uses these inputs; \n  \n    gasInput = Clamped 0f - 1f.  \n    brakeInput = Clamped 0f - 1f.  \n    steerInput = Clamped -1f - 1f. \n    clutchInput = Clamped 0f - 1f. \n    handbrakeInput = Clamped 0f - 1f. \n    boostInput = Clamped 0f - 1f.", MessageType.Info);
				EditorGUILayout.Space();
				GUI.color = originalGUIColor;

				EditorGUILayout.EndVertical ();
			
		}

			EditorGUILayout.BeginVertical(GUI.skin.box);

			GUILayout.Label("Main Controller Settings", EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("units"), new GUIContent("Units"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useAutomaticGear"), new GUIContent("Use Automatic Gear"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("runEngineAtAwake"), new GUIContent("Engines Are Running At Awake"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("keepEnginesAlive"), new GUIContent("Keep Engines Alive When Player Get In-Out Vehicles"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("autoReverse"), new GUIContent("Auto Reverse"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("contactParticles"), new GUIContent("Contact Particles On Collision"));

			EditorGUILayout.EndVertical ();

		EditorGUILayout.EndVertical ();




		}

		EditorGUILayout.Space();

		foldUISettings = EditorGUILayout.Foldout(foldUISettings, "UI Settings");

		if(foldUISettings){
			
			EditorGUILayout.BeginVertical (GUI.skin.box);
			GUILayout.Label("UI Dashboard Settings", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("uiType"), new GUIContent("UI Type"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useTelemetry"), new GUIContent("Use Telemetry"));
			EditorGUILayout.Space();
			EditorGUILayout.EndVertical ();

		}

		EditorGUILayout.Space();

		foldWheelPhysics = EditorGUILayout.Foldout(foldWheelPhysics, "Wheel Physics Settings");

		if(foldWheelPhysics){

			if(RCC_GroundMaterials.Instance.frictions != null && RCC_GroundMaterials.Instance.frictions.Length > 0){

					EditorGUILayout.BeginVertical (GUI.skin.box);
					GUILayout.Label("Ground Physic Materials", EditorStyles.boldLabel);

					physicMaterials = new PhysicMaterial[RCC_GroundMaterials.Instance.frictions.Length];
					
					for (int i = 0; i < physicMaterials.Length; i++) {
						physicMaterials[i] = RCC_GroundMaterials.Instance.frictions[i].groundMaterial;
						EditorGUILayout.BeginVertical(GUI.skin.box);
						EditorGUILayout.ObjectField("Ground Physic Materials " + i, physicMaterials[i], typeof(PhysicMaterial), false);
						EditorGUILayout.EndVertical();
					}

					EditorGUILayout.Space();

			}

			GUI.color = new Color(.5f, 1f, 1f, 1f);
			
			if(GUILayout.Button("Configure Ground Physic Materials")){
				Selection.activeObject = Resources.Load("RCCAssets/RCC_GroundMaterials") as RCC_GroundMaterials;
			}

			GUI.color = originalGUIColor;

			EditorGUILayout.EndVertical ();

		}

		EditorGUILayout.Space();

		foldSFX = EditorGUILayout.Foldout(foldSFX, "SFX Settings");

		if(foldSFX){

			EditorGUILayout.BeginVertical(GUI.skin.box);

			GUILayout.Label("Sound FX", EditorStyles.boldLabel);

			EditorGUILayout.Space();
			GUI.color = new Color(.5f, 1f, 1f, 1f);
			if(GUILayout.Button("Configure Wheel Slip Sounds")){
				Selection.activeObject = Resources.Load("RCCAssets/RCC_GroundMaterials") as RCC_GroundMaterials;
			}
			GUI.color = originalGUIColor;
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("crashClips"), new GUIContent("Crashing Sounds"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("gearShiftingClips"), new GUIContent("Gear Shifting Sounds"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("indicatorClip"), new GUIContent("Indicator Clip"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("exhaustFlameClips"), new GUIContent("Exhaust Flame Clips"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("NOSClip"), new GUIContent("NOS Clip"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("turboClip"), new GUIContent("Turbo Clip"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("blowoutClip"), new GUIContent("Blowout Clip"), true);
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("reversingClip"), new GUIContent("Reverse Transmission Sound"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("windClip"), new GUIContent("Wind Sound"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("brakeClip"), new GUIContent("Brake Sound"), true);
			EditorGUILayout.Separator();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxGearShiftingSoundVolume"), new GUIContent("Max Gear Shifting Sound Volume"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxCrashSoundVolume"), new GUIContent("Max Crash Sound Volume"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxWindSoundVolume"), new GUIContent("Max Wind Sound Volume"), true);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("maxBrakeSoundVolume"), new GUIContent("Max Brake Sound Volume"), true);

			EditorGUILayout.EndVertical();

		}

		EditorGUILayout.Space();

		foldOptimization = EditorGUILayout.Foldout(foldOptimization, "Optimization");

		if(foldOptimization){

			EditorGUILayout.BeginVertical(GUI.skin.box);

			GUILayout.Label("Optimization", EditorStyles.boldLabel);

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useLightsAsVertexLights"), new GUIContent("Use Lights As Vertex Lights On Vehicles"));
			GUI.color = new Color(.75f, 1f, .75f);
			EditorGUILayout.HelpBox("Always use vertex lights for mobile platform. Even only one pixel light will drop your performance dramaticaly!", MessageType.Info);
			GUI.color = originalGUIColor;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useLightProjectorForLightingEffect"), new GUIContent("Use Light Projector For Lighting Effect"));
			GUI.color = new Color(.75f, .75f, 0f);
			EditorGUILayout.HelpBox("Unity's Projector will be used for lighting effect. Be sure it effects to your road only. Select ignored layers below this section. Don't let projectors hits the vehicle itself. It may increase your drawcalls if it hits unnecessary high numbered materials. It should just hit the road, nothing else.", MessageType.Warning);
			GUI.color = originalGUIColor;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("projectorIgnoreLayer"), new GUIContent("Light Projector Ignore Layer"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("useSharedAudioSources"), new GUIContent("Use Shared Audio Sources", "For ex, 4 Audio Sources will be created for each wheel. This option merges them to only 1 Audio Source."), true);
			GUI.color = new Color(.75f, 1f, .75f);
			EditorGUILayout.HelpBox("For ex, 4 Audio Sources will be created for each wheelslip SFX. This option merges them to only 1 Audio Source.", MessageType.Info);
			GUI.color = originalGUIColor;
			EditorGUILayout.PropertyField(serializedObject.FindProperty("dontUseAnyParticleEffects"), new GUIContent("Do Not Use Any Particle Effects"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("dontUseSkidmarks"), new GUIContent("Do Not Use Skidmarks"));
			EditorGUILayout.PropertyField(serializedObject.FindProperty("dontUseChassisJoint"), new GUIContent("Do Not Use Chassis Joint"));
			GUI.color = new Color(.75f, 1f, .75f);
			if(!RCCSettingsAsset.dontUseChassisJoint)
				EditorGUILayout.HelpBox("Chassis Joint is a main Configurable Joint for realistic body movements. Script is getting all colliders attached to chassis, and moves them outside to joint. It can be trouble if you are making game about interacting objects inside the car. If you don't want to use it, chassis will be simulated based on rigid velocity and angular velocity, like older versions of RCC.", MessageType.Info);
			GUI.color = originalGUIColor;
			EditorGUILayout.Space();

			EditorGUILayout.EndVertical();

		}
			
		EditorGUILayout.PropertyField(serializedObject.FindProperty("RCCLayer"), new GUIContent("Vehicle Layer"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("RCCTag"), new GUIContent("Vehicle Tag"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("tagAllChildrenGameobjects"), new GUIContent("Tag All Children Gameobjects"), false);
		GUI.color = new Color(.75f, 1f, .75f);
		EditorGUILayout.HelpBox("All vehicles powered by Realistic Car Controller are using this layer. What does this layer do? It was used for masking wheel rays, light masks, and projector masks. Just create a new layer for vehicles from Edit --> Project Settings --> Tags & Layers, and select the layer here.", MessageType.Info);
		GUI.color = originalGUIColor;

		EditorGUILayout.PropertyField(serializedObject.FindProperty("headLights"), new GUIContent("Head Lights"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("brakeLights"), new GUIContent("Brake Lights"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("reverseLights"), new GUIContent("Reverse Lights"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("indicatorLights"), new GUIContent("Indicator Lights"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("mirrors"), new GUIContent("Mirrors"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("skidmarksManager"), new GUIContent("Skidmarks Manager"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("projector"), new GUIContent("Light Projector"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("exhaustGas"), new GUIContent("Exhaust Gas"), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("chassisJoint"), new GUIContent("Chassis Joint"), false);

		EditorGUILayout.Space();

		EditorGUILayout.EndScrollView();
		
		EditorGUILayout.Space();

		EditorGUILayout.BeginVertical (GUI.skin.button);

		GUI.color = new Color(.75f, 1f, .75f);

		GUI.color = new Color(.5f, 1f, 1f, 1f);
		
		if(GUILayout.Button("Reset To Defaults")){
			ResetToDefaults();
			Debug.Log("Resetted To Defaults!");
		}
		
		if(GUILayout.Button("Open PDF Documentation")){
			string url = "http://www.bonecrackergames.com/realistic-car-controller";
			Application.OpenURL(url);
		}

		GUI.color = originalGUIColor;
		
		EditorGUILayout.LabelField("Realistic Car Controller V3.0f\nBoneCrackerGames", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(50f));

		EditorGUILayout.LabelField("Created by Buğra Özdoğanlar", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(50f));

		EditorGUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();
		
		if(GUI.changed)
			EditorUtility.SetDirty(RCCSettingsAsset);

	}

	void ResetToDefaults(){

		RCCSettingsAsset.overrideFixedTimeStep = true;
		RCCSettingsAsset.fixedTimeStep = .02f;
		RCCSettingsAsset.maxAngularVelocity = 6f;
		RCCSettingsAsset.behaviorType = RCC_Settings.BehaviorType.Custom;
		RCCSettingsAsset.verticalInput = "Vertical";
		RCCSettingsAsset.horizontalInput = "Horizontal";
		RCCSettingsAsset.handbrakeKB = KeyCode.Space;
		RCCSettingsAsset.startEngineKB = KeyCode.I;
		RCCSettingsAsset.lowBeamHeadlightsKB = KeyCode.L;
		RCCSettingsAsset.highBeamHeadlightsKB = KeyCode.K;
		RCCSettingsAsset.rightIndicatorKB = KeyCode.E;
		RCCSettingsAsset.leftIndicatorKB = KeyCode.Q;
		RCCSettingsAsset.hazardIndicatorKB = KeyCode.Z;
		RCCSettingsAsset.shiftGearUp = KeyCode.LeftShift;
		RCCSettingsAsset.shiftGearDown = KeyCode.LeftControl;
		RCCSettingsAsset.boostKB = KeyCode.F;
		RCCSettingsAsset.changeCameraKB = KeyCode.C;
		RCCSettingsAsset.enterExitVehicleKB = KeyCode.E;
		RCCSettingsAsset.useAutomaticGear = true;
		RCCSettingsAsset.runEngineAtAwake = true;
		RCCSettingsAsset.keepEnginesAlive = true;
		RCCSettingsAsset.autoReverse = true;
		RCCSettingsAsset.units = RCC_Settings.Units.KMH;
		RCCSettingsAsset.uiType = RCC_Settings.UIType.UI;
		RCCSettingsAsset.useTelemetry = false;
		RCCSettingsAsset.useAccelerometerForSteering = false;
		RCCSettingsAsset.useSteeringWheelForSteering = false;
		RCCSettingsAsset.UIButtonSensitivity = 3f;
		RCCSettingsAsset.UIButtonGravity = 5f;
		RCCSettingsAsset.gyroSensitivity = 2f;
		RCCSettingsAsset.useLightsAsVertexLights = true;
		RCCSettingsAsset.useLightProjectorForLightingEffect = false;
		RCCSettingsAsset.RCCLayer = "RCC";
		RCCSettingsAsset.RCCTag = "Player";
		RCCSettingsAsset.tagAllChildrenGameobjects = false;
		RCCSettingsAsset.dontUseAnyParticleEffects = false;
		RCCSettingsAsset.dontUseChassisJoint = false;
		RCCSettingsAsset.dontUseSkidmarks = false;
		RCCSettingsAsset.useSharedAudioSources = true;
		RCCSettingsAsset.maxGearShiftingSoundVolume = .25f;
		RCCSettingsAsset.maxCrashSoundVolume = 1f;
		RCCSettingsAsset.maxWindSoundVolume = .1f;
		RCCSettingsAsset.maxBrakeSoundVolume = .1f;
		RCCSettingsAsset.foldGeneralSettings = false;
		RCCSettingsAsset.foldControllerSettings = false;
		RCCSettingsAsset.foldUISettings = false;
		RCCSettingsAsset.foldWheelPhysics = false;
		RCCSettingsAsset.foldSFX = false;
		RCCSettingsAsset.foldOptimization = false;

	}

}
