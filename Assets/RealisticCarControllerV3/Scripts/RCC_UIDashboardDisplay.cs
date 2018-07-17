//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Dashboard Displayer")]
[RequireComponent (typeof(RCC_DashboardInputs))]
public class RCC_UIDashboardDisplay : MonoBehaviour {

	private RCC_DashboardInputs inputs;
	
	public Text RPMLabel;
	public Text KMHLabel;
	public Text GearLabel;

	public Image ABS;
	public Image ESP;
	public Image Park;
	public Image Headlights;
	public Image leftIndicator;
	public Image rightIndicator;
	
	void Start () {
		
		inputs = GetComponent<RCC_DashboardInputs>();
		StartCoroutine("LateDisplay");
		
	}

	void OnEnable(){

		StopAllCoroutines();
		StartCoroutine("LateDisplay");

	}
	
	IEnumerator LateDisplay () {

		while(true){

			yield return new WaitForSeconds(.04f);
		
			if(RPMLabel){
				RPMLabel.text = inputs.RPM.ToString("0");
			}
			
			if(KMHLabel){
				if(RCC_Settings.Instance.units == RCC_Settings.Units.KMH)
					KMHLabel.text = inputs.KMH.ToString("0") + "\nKMH";
				else
					KMHLabel.text = (inputs.KMH * 0.62f).ToString("0") + "\nMPH";
			}

			if(GearLabel){
				if(!inputs.NGear)
					GearLabel.text = inputs.direction == 1 ? (inputs.Gear + 1).ToString("0") : "R";
				else
					GearLabel.text = "N";
			}

			if(ABS)
				ABS.color = inputs.ABS == true ? Color.red : Color.white;
			if(ESP)
				ESP.color = inputs.ESP == true ? Color.red : Color.white;
			if(Park)
				Park.color = inputs.Park == true ? Color.red : Color.white;
			if(Headlights)
				Headlights.color = inputs.Headlights == true ? Color.green : Color.white;

			if(leftIndicator && rightIndicator){

				switch(inputs.indicators){

				case RCC_CarControllerV3.IndicatorsOn.Left:
					leftIndicator.color = new Color(1f, .5f, 0f);
					rightIndicator.color = new Color(.5f, .25f, 0f);
					break;
				case RCC_CarControllerV3.IndicatorsOn.Right:
					leftIndicator.color = new Color(.5f, .25f, 0f);
					rightIndicator.color = new Color(1f, .5f, 0f);
					break;
				case RCC_CarControllerV3.IndicatorsOn.All:
					leftIndicator.color = new Color(1f, .5f, 0f);
					rightIndicator.color = new Color(1f, .5f, 0f);
					break;
				case RCC_CarControllerV3.IndicatorsOn.Off:
					leftIndicator.color = new Color(.5f, .25f, 0f);
					rightIndicator.color = new Color(.5f, .25f, 0f);
					break;
				}

			}

		}

	}

}
