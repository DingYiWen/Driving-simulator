using UnityEngine;
using System.Collections;

//此脚本用于在汽车转向时旋转方向盘
public class RotateSteeringWheel : MonoBehaviour
{
    public GameObject SteerWheel;
    private float currentRotationX, currentRotationY, currentRotationZ, currentRotationW;
    private float oriRotationX, oriRotationY, oriRotationZ, oriRotationW;
    // Use this for initialization
    void Start ()
    {
        oriRotationW = SteerWheel.transform.localRotation.w;
        oriRotationX = SteerWheel.transform.localRotation.x;
        oriRotationY = SteerWheel.transform.localRotation.y;
        oriRotationZ = SteerWheel.transform.localRotation.z;
    }
	
	// Update is called once per frame
	void Update () {

	    if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.05f)
	    {
	        SteerWheel.transform.Rotate(Vector3.forward*Input.GetAxis("Horizontal"));
	        currentRotationZ = SteerWheel.transform.localRotation.z;
            currentRotationX = SteerWheel.transform.localRotation.x;
            currentRotationY = SteerWheel.transform.localRotation.y;
            currentRotationW = SteerWheel.transform.localRotation.w;
        }
	    else
	    {
	        SteerWheel.transform.localRotation = Quaternion.Slerp(new Quaternion(currentRotationX,currentRotationY,currentRotationZ,currentRotationW), new Quaternion(oriRotationX,oriRotationY,oriRotationZ,oriRotationW), 1f);
	        
	    }


	}
}
