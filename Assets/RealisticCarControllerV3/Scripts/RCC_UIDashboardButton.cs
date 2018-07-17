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

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Button")]
public class RCC_UIDashboardButton : MonoBehaviour {
	
	public ButtonType _buttonType;
	public enum ButtonType{Start, ABS, ESP, TCS, Headlights, LeftIndicator, RightIndicator, Gear, Low, Med, High, SH};
	private Scrollbar gearSlider;

	private RCC_CarControllerV3[] carControllers;
	private int gearDirection = 0;

	void Start(){

		if(GetComponentInChildren<Scrollbar>()){
			gearSlider = GetComponentInChildren<Scrollbar>();
			gearSlider.onValueChanged.AddListener (delegate {ChangeGear ();});
		}

	}

	void OnEnable(){

		Check();

	}
	
	public void OnClicked () {
		
		carControllers = GameObject.FindObjectsOfType<RCC_CarControllerV3>();
		
		switch(_buttonType){
			
		case ButtonType.Start:
			
			for(int i = 0; i < carControllers.Length; i++){
				
				if(carControllers[i].canControl && !carControllers[i].AIController)
					carControllers[i].KillOrStartEngine();
				
			}
			
			break;
			
		case ButtonType.ABS:
			
			for(int i = 0; i < carControllers.Length; i++){
				
				if(carControllers[i].canControl && !carControllers[i].AIController)
					carControllers[i].ABS = !carControllers[i].ABS;
				
			}
			
			break;
			
		case ButtonType.ESP:
			
			for(int i = 0; i < carControllers.Length; i++){
				
				if(carControllers[i].canControl && !carControllers[i].AIController)
					carControllers[i].ESP = !carControllers[i].ESP;
				
			}
			
			break;
			
		case ButtonType.TCS:
			
			for(int i = 0; i < carControllers.Length; i++){
				
				if(carControllers[i].canControl && !carControllers[i].AIController)
					carControllers[i].TCS = !carControllers[i].TCS;
				
			}
			
			break;

		case ButtonType.SH:

			for(int i = 0; i < carControllers.Length; i++){

				if(carControllers[i].canControl && !carControllers[i].AIController)
					carControllers[i].steeringHelper = !carControllers[i].steeringHelper;

			}

			break;
			
		case ButtonType.Headlights:
			
			for(int i = 0; i < carControllers.Length; i++){
				
				if(carControllers[i].canControl && !carControllers[i].AIController){
					if(!carControllers[i].highBeamHeadLightsOn && carControllers[i].lowBeamHeadLightsOn){
						carControllers[i].highBeamHeadLightsOn = true;
						carControllers[i].lowBeamHeadLightsOn = true;
						break;
					}
					if(!carControllers[i].lowBeamHeadLightsOn)
						carControllers[i].lowBeamHeadLightsOn = true;
					if(carControllers[i].highBeamHeadLightsOn){
						carControllers[i].lowBeamHeadLightsOn = false;
						carControllers[i].highBeamHeadLightsOn = false;
					}
				}
				
			}
			
			break;

		case ButtonType.LeftIndicator:

			for(int i = 0; i < carControllers.Length; i++){

				if(carControllers[i].canControl && !carControllers[i].AIController){
					if(carControllers[i].indicatorsOn != RCC_CarControllerV3.IndicatorsOn.Left)
						carControllers[i].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Left;
					else
						carControllers[i].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
				}

			}

			break;

		case ButtonType.RightIndicator:

			for(int i = 0; i < carControllers.Length; i++){

				if(carControllers[i].canControl && !carControllers[i].AIController){
					if(carControllers[i].indicatorsOn != RCC_CarControllerV3.IndicatorsOn.Right)
						carControllers[i].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Right;
					else
						carControllers[i].indicatorsOn = RCC_CarControllerV3.IndicatorsOn.Off;
				}

			}

			break;

		case ButtonType.Low:

			QualitySettings.SetQualityLevel (1);

			break;

		case ButtonType.Med:

			QualitySettings.SetQualityLevel (3);

			break;

		case ButtonType.High:

			QualitySettings.SetQualityLevel (5);

			break;
			
		}
		
		Check();
		
	}
	
	public void Check(){
		
		carControllers = GameObject.FindObjectsOfType<RCC_CarControllerV3>();

		if (!GetComponent<Image> ())
			return;
		
		switch(_buttonType){
			
		case ButtonType.ABS:
			
			for(int i = 0; i < carControllers.Length; i++){
				
				if(!carControllers[i].AIController && carControllers[i].canControl && carControllers[i].ABS)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(!carControllers[i].AIController && carControllers[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;
			
		case ButtonType.ESP:
			
			for(int i = 0; i < carControllers.Length; i++){
				
				if(!carControllers[i].AIController && carControllers[i].canControl && carControllers[i].ESP)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(!carControllers[i].AIController && carControllers[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;
			
		case ButtonType.TCS:
			
			for(int i = 0; i < carControllers.Length; i++){
				
				if(!carControllers[i].AIController && carControllers[i].canControl && carControllers[i].TCS)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(!carControllers[i].AIController && carControllers[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;

		case ButtonType.SH:

			for(int i = 0; i < carControllers.Length; i++){

				if(!carControllers[i].AIController && carControllers[i].canControl && carControllers[i].steeringHelper)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(!carControllers[i].AIController && carControllers[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);

			}

			break;
			
		case ButtonType.Headlights:
			
			for(int i = 0; i < carControllers.Length; i++){
				
				if(!carControllers[i].AIController && carControllers[i].canControl && carControllers[i].lowBeamHeadLightsOn || carControllers[i].highBeamHeadLightsOn)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(!carControllers[i].AIController && carControllers[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;
			
		}
		
	}

	public void ChangeGear(){

		if(gearDirection == (int)gearSlider.value)
			return;

		gearDirection = (int)gearSlider.value;

		for(int i = 0; i < carControllers.Length; i++){

			if(!carControllers[i].AIController && carControllers[i].canControl){
				
				carControllers[i].semiAutomaticGear = true;

				if(gearDirection == 1)
					carControllers[i].StartCoroutine("ChangingGear", -1);
				else
					carControllers[i].StartCoroutine("ChangingGear", 0);
				
			}

		}

	}

	void OnDisable(){

		if(_buttonType == ButtonType.Gear){

			carControllers = GameObject.FindObjectsOfType<RCC_CarControllerV3>();

			foreach(RCC_CarControllerV3 rcc in carControllers){

				if(!rcc.AIController && rcc.canControl)
					rcc.semiAutomaticGear = false;

			}

		}

	}
	
}
