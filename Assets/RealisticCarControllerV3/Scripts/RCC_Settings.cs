//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

[System.Serializable]
public class RCC_Settings : ScriptableObject {
	
	#region singleton
	public static RCC_Settings instance;
	public static RCC_Settings Instance{	get{if(instance == null) instance = Resources.Load("RCCAssets/RCC_Settings") as RCC_Settings; return instance;}}
	#endregion

	public int toolbarSelectedIndex;

	public bool overrideFixedTimeStep = true;
	[Range(.005f, .06f)]public float fixedTimeStep = .02f;
	[Range(.5f, 20f)]public float maxAngularVelocity = 6;

	// Behavior Types
	public BehaviorType behaviorType;
	public enum BehaviorType{Simulator, Racing, SemiArcade, Drift, Fun, Custom}
	public bool useFixedWheelColliders = true;

	// Controller Type
	public ControllerType controllerType;
	public enum ControllerType{Keyboard, Mobile, Custom}

	// Keyboard Inputs
	public string verticalInput = "Vertical";
	public string horizontalInput = "Horizontal";
	public KeyCode handbrakeKB = KeyCode.Space;
	public KeyCode startEngineKB = KeyCode.I;
	public KeyCode lowBeamHeadlightsKB = KeyCode.L;
	public KeyCode highBeamHeadlightsKB = KeyCode.K;
	public KeyCode rightIndicatorKB = KeyCode.E;
	public KeyCode leftIndicatorKB = KeyCode.Q;
	public KeyCode hazardIndicatorKB = KeyCode.Z;
	public KeyCode shiftGearUp = KeyCode.LeftShift;
	public KeyCode shiftGearDown = KeyCode.LeftControl;
	public KeyCode boostKB = KeyCode.F;
	public KeyCode changeCameraKB = KeyCode.C;
	public KeyCode enterExitVehicleKB = KeyCode.E;

	// Main Controller Settings
	public bool useAutomaticGear = true;
	public bool runEngineAtAwake = true;
	public bool keepEnginesAlive = true;
	public bool autoReverse = true;
	public GameObject contactParticles;
	public Units units;
	public enum Units {KMH, MPH}

	// UI Dashboard Type
	public UIType uiType;
	public enum UIType{UI, NGUI, None}

	// Information telemetry about current vehicle
	public bool useTelemetry = false;

	// For mobile usement
	public bool useAccelerometerForSteering;
	public bool useSteeringWheelForSteering;

	// Mobile controller buttons and accelerometer sensitivity
	public float UIButtonSensitivity = 3f;
	public float UIButtonGravity = 5f;
	public float gyroSensitivity = 2f;

	// Used for using the lights more efficent and realistic
	public bool useLightsAsVertexLights = true;
	public bool useLightProjectorForLightingEffect = false;

	// Other stuff
	public string RCCLayer;
	public string RCCTag;
	public bool tagAllChildrenGameobjects = false;

	public GameObject chassisJoint;
	public GameObject exhaustGas;
	public RCC_Skidmarks skidmarksManager;
	public GameObject projector;
	public LayerMask projectorIgnoreLayer;

	public GameObject headLights;
	public GameObject brakeLights;
	public GameObject reverseLights;
	public GameObject indicatorLights;
	public GameObject mirrors;

	public bool dontUseAnyParticleEffects = false;
	public bool dontUseChassisJoint = false;
	public bool dontUseSkidmarks = false;

	// Sound FX
	public AudioClip[] gearShiftingClips;
	public AudioClip[] crashClips;
	public AudioClip reversingClip;
	public AudioClip windClip;
	public AudioClip brakeClip;
	public AudioClip indicatorClip;
	public AudioClip NOSClip;
	public AudioClip turboClip;
	public AudioClip[] blowoutClip;
	public AudioClip[] exhaustFlameClips;
	public bool useSharedAudioSources = true;

	[Range(0f, 1f)]public float maxGearShiftingSoundVolume = .25f;
	[Range(0f, 1f)]public float maxCrashSoundVolume = 1f;
	[Range(0f, 1f)]public float maxWindSoundVolume = .1f;
	[Range(0f, 1f)]public float maxBrakeSoundVolume = .1f;

	// Used for folding sections of RCC Settings
	public bool foldGeneralSettings = false;
	public bool foldControllerSettings = false;
	public bool foldUISettings = false;
	public bool foldWheelPhysics = false;
	public bool foldSFX = false;
	public bool foldOptimization = false;

}
