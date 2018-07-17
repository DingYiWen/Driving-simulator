//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller/AI/Waypoints Container")]
public class RCC_AIWaypointsContainer : MonoBehaviour {

	public List<Transform> waypoints = new List<Transform>();
	public Transform target;
	
	void OnDrawGizmos() {
		
		for(int i = 0; i < waypoints.Count; i ++){
			
			Gizmos.color = new Color(0.0f, 1.0f, 1.0f, 0.3f);
			Gizmos.DrawSphere (waypoints[i].transform.position, 2);
			Gizmos.DrawWireSphere (waypoints[i].transform.position, 20f);
			
			if(i < waypoints.Count - 1){
				if(waypoints[i] && waypoints[i+1]){
					if (waypoints.Count > 0) {
						Gizmos.color = Color.green;
						if(i < waypoints.Count - 1)
							Gizmos.DrawLine(waypoints[i].position, waypoints[i+1].position); 
						if(i < waypoints.Count - 2)
							Gizmos.DrawLine(waypoints[waypoints.Count - 1].position, waypoints[0].position); 
					}
				}
			}
		}
		
	}
	
}
