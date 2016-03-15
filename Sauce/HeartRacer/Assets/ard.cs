using UnityEngine;
using System.Collections;
using System.IO;
using System.IO.Ports;


public class ard : MonoBehaviour {



    SerialPort test = new SerialPort();
    float timer;
    string teststring;

    public bool hasDevice;
    public bool inGame;

	// Use this for initialization
	void Start () {
        connect();
	}

    public int RequestInputs()
    {
        if (hasDevice)
        {
            test.Write("G");
            return test.ReadByte();
        }
        return 0;
    }

    public void reset()
    {
        test.Write("R");
    }

    void connect() {
        string[] t = SerialPort.GetPortNames();
        test.ReadTimeout = 1000;
        test.BaudRate = 57600;
        foreach (string s in t)
        {
            test.PortName = s;
            test.Open();
            try
            {
                teststring = test.ReadLine();
                if (teststring == "HI")
                {
                    test.Write("G");
                    hasDevice = true;
                    test.ReadTimeout = 3;
                    test.DiscardInBuffer();
                    break;
                }
                else
                {
                    Debug.Log(teststring);
                    hasDevice = false;
                }
            }
            catch
            {
                Debug.Log("Read Timeout");
            }

            test.Close();
        }

        if (!test.IsOpen)
            Debug.Log("No valid COM port found!");
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        if (!inGame && timer > 5 && !hasDevice)
        {

            connect();
            timer = 0;
        }

        if (hasDevice)
            teststring = "Device Found!";
        else
            teststring = "Device not Found!";
	}

    void OnGUI()
    {
        GUI.Label(new Rect(45, 45, 250, 250), teststring);
    }
}
