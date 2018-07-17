using UnityEngine;
using System.Collections;

public class TishiManager : MonoBehaviour
{
    public string Info;
    public bool isDoubleFace=false;
    public GameObject managerObj;
    private BoxCollider _boxCollider;
    public Rigidbody _rig;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	 
	
	}

    public void OnTriggerEnter(Collider col)
    {
      
       
            if (col.gameObject.name == "Collider(Clone)") //如果是汽车进入
            {

                _rig = GameObject.Find("E36(Clone)").GetComponent<Rigidbody>();
                if (isDoubleFace)   //isDoubleFace用于设置是否为双向通行时进行提示
               {
                  managerObj.SendMessage("ShowInfo", Info);
                  return;
                }

                //以下代码用于判断汽车的行驶方向和提示牌提示的方向是否对应，对应则提示，否则不提示
                if (Mathf.Abs(transform.forward.y) > 0.8f)
                {
                    if(transform.forward.y* _rig.velocity.y> 0)
                    managerObj.SendMessage("ShowInfo", Info);
                 }

                if (Mathf.Abs(transform.forward.x) > 0.8f)
                {
                if (transform.forward.x * _rig.velocity.x > 0)
                    managerObj.SendMessage("ShowInfo", Info);
                }

                if (Mathf.Abs(transform.forward.z) > 0.8f)
                {
                if (transform.forward.z * _rig.velocity.z > 0)
                    managerObj.SendMessage("ShowInfo", Info);

                 }
            }
        
    }


    

}
