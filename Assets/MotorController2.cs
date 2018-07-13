using UnityEngine;
using System;
using System.Diagnostics;
using System.Timers;
using System.Threading;

class MotorController2 : MonoBehaviour {
    private SerialHandler serialHandler;
    public float standard_speed = 5.0f; //モータが動く規定速度
    private int n = 50;
    private int prev = 0;
    private float scaleFac = 1;

    private bool motorEnabled = true;

    private System.Timers.Timer stopTimer;
    private System.Timers.Timer keepMotorTimer;
    private System.Timers.Timer restartMotorTimer;

    private Stopwatch stopwatch; // don't send data when sending interval < this.interval
    private int interval = 100; //ms

    private bool stretchDirectionFlag = true;
    private bool stretchEndFlag = true;

    private string sendGravityData;

    public int mechanismMode = 0;// 0:both, 1:only gravity moving, 2:only skin stretch, 3:none
    public float stretchTime = 0.9f;
    public float keepTime = 0.05f;
    private string previousSendStretchData;
    private string previousSendGravityData;

    void Start() {

        try {
            this.serialHandler = new SerialHandler();
            serialHandler.Write("2000");
        } catch (Exception e) {
            UnityEngine.Debug.LogWarning(e);
            //			this.motorEnabled = false;
        }

        //this.stopTimer = new System.Timers.Timer(500); // motor will stop after 1500ms
        float stopTimerMS = stretchTime * 1000;
        if(stopTimerMS > 400) {
            stopTimerMS -= 400f;
        }
        this.stopTimer = new System.Timers.Timer(stopTimerMS);

        stopTimer.Elapsed += stopEvent;
        stopTimer.AutoReset = false;

        this.stopwatch = new Stopwatch();
        this.stopwatch.Start();

        this.keepMotorTimer = new System.Timers.Timer(keepTime * 1000f);
        keepMotorTimer.Elapsed += keepMotorEvent;
        keepMotorTimer.AutoReset = false;

        this.restartMotorTimer = new System.Timers.Timer(keepTime * 1000f);
        restartMotorTimer.Elapsed += restartMotorEvent;
        keepMotorTimer.AutoReset = false;
    }

    public void onStretchStart(Vector3 origin, Vector3 direction) {
        RaycastHit hit;

        //		if (Physics.Raycast (origin, direction, out hit)) { 
        //			gravityTimer.Interval = 600;
        //			UnityEngine.Debug.Log ("stretch start in MotorController" + hit.collider);
        //		} else {
        //			gravityTimer.Interval = 200;
        UnityEngine.Debug.Log("stretch start in MotorController2");
        SendVelocityMagnitude(255);
        //SendVelocityMagnitude(makeSendStretchMagnitude(stretchTime));
    }

    public float makeSendStretchMagnitude(float stretchTime) {
        float magMax = 255f;
        float stretchTimeMin = 0.9f;
        float scale = 1f;
        float mag = magMax;
        scale = stretchTimeMin / stretchTime;
        mag = magMax * scale;
        return mag;
    }

    public void keepMotorEvent(object sender, EventArgs e) {
        serialHandler.Write("0600");
        serialHandler.Write("1600");
        //restartMotorTimer.Start();
    }

    public void restartMotorEvent(object sender, EventArgs e) {
        serialHandler.Write(previousSendGravityData);
        serialHandler.Write(previousSendStretchData);
        //keepMotorTimer.Start();
    }

    public void onStretchEnd() {

    }

    public void onShrinkStart() {
        UnityEngine.Debug.Log("shrink start in MotorController2");
        SendVelocityMagnitude(-255);
    }

    public void onShrinkEnd() {

    }

    private void SendVelocityMagnitude(float mag) {

        if (this.motorEnabled == false)
            return;


        if (stopwatch.ElapsedMilliseconds < interval) {
            return;
        }


        if (mag == 0) {
            return;
        }


        // scale mag to 0-100
        mag = mag / scaleFac;
        mag = mag > 255f ? 255f : mag;
        mag = mag < -256f ? -256f : mag;


        if (mechanismMode == 2 || mechanismMode == 3) {
            sendGravityData = makeSendGravityData(0);
        } else {
            sendGravityData = makeSendGravityData(mag);
        }

        string sendStretchData;
        if (mechanismMode == 1 || mechanismMode == 3) {
            sendStretchData = makeSendStretchData(0);
        } else {
            if (mag > 0) {
                sendStretchData = makeSendStretchData(mag);
            } else {
                sendStretchData = makeSendStretchData(-mag);
            }
        }

        if (mag < 0) {
            serialHandler.Write("1700");
            serialHandler.Write(sendGravityData);
            stretchEndFlag = false;
        } else {
            serialHandler.Write(sendStretchData);
            serialHandler.Write(sendGravityData);
            stretchEndFlag = true;
        }

        previousSendGravityData = sendGravityData;
        previousSendStretchData = sendStretchData;

        if (stopTimer.Enabled == true) {

            stopTimer.Dispose();
        }
        stopTimer.Start();
       // keepMotorTimer.Enabled = true;
       // restartMotorTimer.Enabled = true;
       // keepMotorTimer.Start();

        stopwatch.Reset();
        stopwatch.Start();

    }

    void stopEvent(object sender, ElapsedEventArgs e) {
        keepMotorTimer.Enabled = false;
        restartMotorTimer.Enabled = false;
        serialHandler.Write("0600");

        if (stretchEndFlag == true) {
            if (mechanismMode == 1 || mechanismMode == 3) {
                serialHandler.Write("1000");
            } else {
                serialHandler.Write("1255");
            }
        } else {
            serialHandler.Write("1700");
        }
        stretchEndFlag = !stretchEndFlag;

    }

    //シリアル通信用に直す
    string makeSendGravityData(float speed) {
        // The data that send to Arduino must be stringfied and a 4th digit number.

        string sendData;

        n = (int)speed;

        // process for opposite rotation
        if (n < 0) {
            n = -n;
            n += 256;
        }

        // process for number of digits of n
        if (n % 10 == n) {
            sendData = "000" + n;
        } else if (n % 100 == n) {
            sendData = "00" + n;
        } else if (n % 1000 == n) {
            sendData = "0" + n;
        } else {
            sendData = n.ToString();
        }

        return sendData;
    }

    string makeSendStretchData(float speed) {

        n = (int)speed;


        string sendData;

        if (n < 0) {
            sendData = "1000";
            return sendData;
        }

        if (n % 10 == n) {
            sendData = "100" + n;
        }
        //nが二桁だったとき
        else if (n % 100 == n) {
            sendData = "10" + n;
        } else if (n % 1000 == n) {
            sendData = "1" + n;
        } else {
            sendData = n.ToString();
        }

        return sendData;


    }

    void OnDisable() {
        stopTimer.Close();
        serialHandler.Write("0700");
        serialHandler.Write("1700");
        this.serialHandler.Close();
    }

}