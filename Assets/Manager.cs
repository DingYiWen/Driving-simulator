using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    public Material[] Materials;
    public Text showInfo;
    public GameObject alertImageObj;
    public GameObject E36,OriPosition;
    public RCC_Camera rccCam;
    private GameObject E36Clone;
    private float timer=0;
    private float showInfoTimer = 0;
    private bool isShowing = false;
    private bool isStart = false;
    private bool isNeedIndicator = true;
    private bool acRed, bdRed;
    private int leftIndicator = 0;
    private int rightIndicator = 0;
    private float speedLimitTimer = 0;
    private string Info;
    private string[] speedInfo;
    private float speed;
    public Text speedLabel, ScoreLabel;
    public Text StartOrStopText;
    public Button LeftIndicatorButton, RightIndicatorButton;
    public AudioClip[] AudioClips;
    private AudioClip oldClip;
    private AudioSource audioSource;
    private int startCount = 0,lastMinusScore=0;
    private float scoreTimer=0;
    private bool canMinus=false;
    public Button StartOrStopButton;
    // Use this for initialization
    void Start ()
    {
        audioSource = this.GetComponent<AudioSource>();
        ResetPosition();//初始化汽车（在原点生成汽车）
        timer = 9;
        RightIndicatorButton.enabled = false;
        LeftIndicatorButton.enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    timer += Time.deltaTime; //红绿灯变换的计时器计时
        scoreTimer += Time.deltaTime;
        if (scoreTimer>4)
        {
            canMinus = true;
        }
	    
	    if (isShowing)           //判断此时是否到达提示区，到达提示区则提示信息显示的时间开始计时
	    {
            showInfoTimer += Time.deltaTime;
        }
	    if (showInfoTimer > 2.5f) //如果提示信息显示的时间大于2.5秒，则隐藏提示信息，计时器归零
	    {
	        showInfoTimer = 0;
	        isShowing = false;
	        showInfo.color=new Color(1,0,0,0);
            alertImageObj.SetActive(false);
	    }
        //A位为绿灯，B红灯
        if (timer>10)                  //如果红绿灯计时器计时达到10秒，则切换红绿灯
        {
	        foreach (var mat in Materials)
	        {
	            mat.color = Color.gray;
	        }

	        Materials[2].color = Color.green;
	        Materials[3].color = Color.red;
	   

            bdRed = true;
            acRed = false;
        }
        //跳转黄灯
	    if (timer>20)
	    {
            foreach (var mat in Materials)
            {
                mat.color = Color.gray;
            }
	        Materials[1].color = Color.yellow;
            Materials[4].color= Color.yellow;
           

            bdRed = false;
            acRed = false;
        }
        //B为绿灯，A红灯
	    if (timer>23)
	    {
            foreach (var mat in Materials)
            {
                mat.color = Color.gray;
            }
            Materials[5].color = Color.green;
            Materials[0].color = Color.red;
          

            bdRed = false;
            acRed = true;

        }
        //跳转黄灯
        if (timer>33)
        {
            foreach (var mat in Materials)
            {
                mat.color = Color.gray;
            }
            Materials[1].color = Color.yellow;
            Materials[4].color = Color.yellow;
           

            timer = 7;

            bdRed = false;
            acRed = false;

        }

	    if (Input.GetAxis("Vertical") > 0.1f)
	    {
	        isNeedIndicator = true;
	    }else if (Input.GetAxis("Vertical") <-0.1f)
	    {
            isNeedIndicator = false;
        }

        if (Input.GetAxis("Horizontal") > 0.9f && rightIndicator % 2 == 0 && E36Clone != null)
        {
            if (E36Clone.GetComponent<Rigidbody>().velocity.magnitude > 0.3f)
            {
                if (isNeedIndicator)
                {
                   
                    SetScore(1);
                }
                
            }
           
        }
        if (Input.GetAxis("Horizontal") < -0.9f && leftIndicator % 2 == 0 && E36Clone != null)
        {
            if (E36Clone.GetComponent<Rigidbody>().velocity.magnitude > 0.3f)
            {
                if (isNeedIndicator)
                {
                    
                    SetScore(1);
                }
                   
            }
               
        }

       

           //超速扣分提醒
            speedInfo = speedLabel.text.Split('\n');
            speed = float.Parse(speedInfo[0]);
            if (speed > 40)
            {
                
                SetScore(3);

            }




        if (Info == "前方减速慢行" || Info == "前方人行横道" || Info == "前方施工") //同上
        {
            speedInfo = speedLabel.text.Split('\n');
            speed = float.Parse(speedInfo[0]);
            if (speed > 30)
            {
                ShowInfo("请减速慢行！");
             

            }


        }

        if (Info == "禁止长时间停车")  //如果提示区信息为"禁止长时间停车"，则当车速低于15km/h时开始计时，时间大于15秒则认定为长时间停车
        {
            speedInfo = speedLabel.text.Split('\n');
            speed = float.Parse(speedInfo[0]);
	        if (speed < 15)
	        {
	            speedLimitTimer += Time.deltaTime;
	        }
	        if (speedLimitTimer > 15)
	        {
	            ShowInfo("您已长时间停车！ \r\n -2分");
                SetScore(2);
                speedLimitTimer = 0;
	        }
        }

	    if (Input.GetKeyDown(KeyCode.Q)) //点击Q,E键同样会设置左右转向灯的值
	    {
	        SetLeftIndicator();
	    }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SetRightIndicator();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }



    }


   

    public void ShowInfo(string info)  //用于显示提示信息的函数，当车体到达提示区域后会调用此函数
    {
        Info=info;


        if (Info == "前方人行横道")
        {
            PlaySound(AudioClips[2]);//人行横道减速慢行

        }

        if (Info == "前方环形路口" || Info == "前方有转弯")
        {

            PlaySound(AudioClips[3]);//转弯打方向灯
        }


      
        if (info == "1")
        {
            if (acRed)
            {
                showInfo.text = "闯红灯行为 \r\n -6分";
                SetScore(6);
            }
            else
            {
                return;
            }
            
        }
        else if (info == "2")
        {
            if (bdRed)
            {
                showInfo.text = "闯红灯行为 \r\n -6分";
                SetScore(6);
            }
            else
            {
                return;
            }
        }
        else if (info == "22")
        {
            if (bdRed)
            {
                PlaySound(AudioClips[4]); //前方红灯，请停车

            }
            else
            {
                return;
            }
        }else if (info == "11")
        {
            if (acRed)
            {

                PlaySound(AudioClips[4]); //前方红灯，请停车
               
            }
            else
            {
                return;
            }

        }
        else 
        {
          
            showInfoTimer = 0;
            showInfo.text = info;
        }
        showInfo.color = new Color(1, 0, 0, 1);
        alertImageObj.SetActive(true);
        isShowing = true;
    }

    public void SetLeftIndicator()  
    {
        leftIndicator++;
        if (rightIndicator%2 == 1)
        {
            rightIndicator++;
        }
    }
    public void SetRightIndicator()
    {
        if (leftIndicator%2 == 1)
        {
            leftIndicator++;
        }
        
        rightIndicator++;
    }


    public void ResetPosition()  //重置函数，调用后游戏重新开始
    {
       GameObject.Destroy(E36Clone);
       E36Clone= GameObject.Instantiate(E36, OriPosition.transform.position, OriPosition.transform.rotation) as GameObject;
       isStart = false;
       StartOrStopText.text = "启   动";
       rccCam.playerCar = E36Clone.transform;
       rccCam.wheelCam = E36Clone.transform.FindChild("Chassis/Cube").gameObject;
        ScoreLabel.text = "Score:12";
       showInfoTimer = 3.0f;
       startCount = 0;
        StartOrStopButton.enabled = true;
    }

    private void SetScore(int MinusScore)
    {
        //防止短时间内重复扣分
        if (MinusScore == lastMinusScore && !canMinus)
        {
           
            return;
        
        }

        lastMinusScore = MinusScore;
        if (MinusScore == 3)
        {
            ShowInfo("超速行驶！ \r\n -3分");
            PlaySound(AudioClips[0]);
        }
        if (MinusScore == 1)
        {
            ShowInfo("未开转向灯 \r\n -1分");
        }
        string[] strs = ScoreLabel.text.Split(':');
        int oldScore = int.Parse(strs[1]);
        int currentScore = 0;
        if (oldScore - MinusScore > 0)
        {
            currentScore = oldScore - MinusScore;
        }
        else
        {
            StartOrStopButton.enabled = false;
            currentScore = 0;
            ShowInfo("您的分数已扣完");
            isShowing = false;
            //分数扣完禁止操作
            E36Clone.GetComponent<RCC_CarControllerV3>().isRuning = false;
           // E36Clone.GetComponent<RCC_CarControllerV3>().KillOrStartEngine();
            E36Clone.GetComponent<RCC_CarControllerV3>().engineSoundIdle.Stop();
            E36Clone.GetComponent<Rigidbody>().drag=5;
            RightIndicatorButton.enabled = false;
            LeftIndicatorButton.enabled = false;
           
        }
        ScoreLabel.text = "Score:" + currentScore;
        canMinus = false;
        scoreTimer = 0;
    }

    public void OnStartOrStop()
    {
        if (E36Clone != null)
        {
            if (startCount == 0)
            {
                PlaySound(AudioClips[1]); //上路前准备提醒，只在第一次启动时提醒

            }
            startCount++;

            if (!isStart)
            {
                isStart = true;
                StartOrStopText.text = "停   车";
                E36Clone.GetComponent<RCC_CarControllerV3>().isRuning = true;
                E36Clone.GetComponent<RCC_CarControllerV3>().KillOrStartEngine();
                E36Clone.GetComponent<RCC_CarControllerV3>().engineSoundIdle.Play();
                RightIndicatorButton.enabled = true;
                LeftIndicatorButton.enabled = true;
            }
            else
            {
                isStart = false;
                StartOrStopText.text = "启   动";
                E36Clone.GetComponent<RCC_CarControllerV3>().isRuning = false;
                E36Clone.GetComponent<RCC_CarControllerV3>().KillOrStartEngine();
                E36Clone.GetComponent<RCC_CarControllerV3>().engineSoundIdle.Stop();
                RightIndicatorButton.enabled = false;
                LeftIndicatorButton.enabled = false;
            }
        }
    }


    private void PlaySound(AudioClip _clip)
    {

      //  audioSource.Stop();
        if (oldClip == _clip)
        {
            audioSource.Play();
            return;
        }
       
            
            audioSource.clip = _clip;
            oldClip = _clip;
            audioSource.Play();
           



    }
}
