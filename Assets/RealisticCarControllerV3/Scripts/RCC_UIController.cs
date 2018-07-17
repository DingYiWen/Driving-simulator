//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/UI/Mobile/Button")]
public class RCC_UIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	internal float input;
	private float sensitivity{get{return RCC_Settings.Instance.UIButtonSensitivity;}}
	private float gravity{get{return RCC_Settings.Instance.UIButtonGravity;}}
	public bool pressing;

	public void OnPointerDown(PointerEventData eventData){

		pressing = true;

	}

	public void OnPointerUp(PointerEventData eventData){
		 
		pressing = false;
		
	}

	void OnPress (bool isPressed){

		if(isPressed)
			pressing = true;
		else
			pressing = false;

	}

	void FixedUpdate(){
		
		if(pressing)
			input += Time.fixedDeltaTime * sensitivity;
		else
			input -= Time.fixedDeltaTime * gravity;
		
		if(input < 0f)
			input = 0f;
		if(input > 1f)
			input = 1f;
		
	}

	void OnDisable(){

		input = 0f;
		pressing = false;

	}

}
