using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour {

    Rigidbody r;
    CapsuleCollider c;

    TextureMan jumptex;
    TextureMan crouchtex;

    Camera m;

    float gametime;

    ard ardController;
    float camX;
    float camY;

    public GUIStyle style;
    public GUIStyle style2;

    float lastHit;
    int score;
    int consecutiveNotes;

    int HP;

    bool jumping;
    bool hasJumped;

    bool crouching;

    int inputs;

    Vector3[] HPpos;
    GameObject[] objs;

    public bool isMenu;
	// Use this for initialization
	void Start () {
        ardController = GetComponent<ard>();
        if (!isMenu) 
        { 
            objs = new GameObject[] {GameObject.Find("HP1"), GameObject.Find("HP2"), GameObject.Find("HP3"), GameObject.Find("HP4"), GameObject.Find("HP5")};

            HPpos = new Vector3[] { objs[0].transform.localPosition, objs[1].transform.localPosition, objs[2].transform.localPosition, objs[3].transform.localPosition, objs[4].transform.localPosition };
        
            foreach (GameObject o in objs)
            {
                 o.transform.localPosition = new Vector3(-1, -1, -1);
            }
        }
        m = Camera.main;
        r = GetComponent<Rigidbody>();
        c = GetComponent<CapsuleCollider>();

        HP = 5;

        if (!isMenu)
        {
            jumptex = GameObject.Find("JumpMSG").GetComponent<TextureMan>();
            crouchtex = GameObject.Find("CrouchMSG").GetComponent<TextureMan>();
        }
	}

    void LateUpdate()
    {
        try 
        { 
            inputs = ardController.RequestInputs();
            //Debug.Log(inputs.ToString());
        }
        catch
        {
            //Debug.Log("took too long");
        }
    }

	// Update is called once per frame
	void Update () {

        lastHit += Time.deltaTime;
        gametime += Time.deltaTime;

        if (HP <= 0)
        {
            if (ardController.hasDevice)
                ardController.reset();
             Application.LoadLevel(0);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isMenu)
            {
                if (ardController.hasDevice)
                    ardController.reset();
                Application.LoadLevel(1);

            }
        }
        

        if (ardController.hasDevice)
        {
            jumping = (inputs & 1) != 0;

            crouching = (inputs & 2) != 0 && !jumping;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                camX -= 0.5f;
                if (camX < -75f)
                    camX = -75f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                camX += 0.5f;
                if (camX > 75f)
                    camX = 75f;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                camY -= 0.5f;
                if (camY < -35f)
                    camY = -35f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                camY += 0.5f;
                if (camY > 35f)
                    camY = 35f;
            }

            if (!isMenu)
                m.transform.rotation = Quaternion.Euler(camY, camX, 0);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumping = true;
                if (isMenu)
                {
                    if (ardController.hasDevice)
                        ardController.reset();
                    Application.LoadLevel(1);
                }
  
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                jumping = false;
            }



            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                crouching = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                crouching = false;
            }

        }
        
        if (jumping && !hasJumped)
        {
            if (!isMenu)
            {
                r.AddForce(new Vector3(0, 250, 0));
                hasJumped = true;
            } 
        }
        if (!isMenu)
        {
            if (crouching && c.height > 1)
            {
            
                    c.height = c.height - 0.15f;
                    if (c.height < 1)
                        c.height = 1;
            
            }
            if (!crouching && c.height < 2)
            {

                    c.height = c.height + 0.05f;
                    if (c.height > 2)
                        c.height = 2;
    
            }
        }


        
	}

    void FixedUpdate()
    {
        if (!isMenu)
            r.position += new Vector3(0,0,5) * Time.deltaTime;
    }

    void OnGUI()
    {
        if (!isMenu)
        {
            GUI.Label(new Rect(47, 98, 150, 150), "Score: " + score.ToString(), style2);
            GUI.Label(new Rect(43, 102, 150, 150), "Score: " + score.ToString(), style2);
            GUI.Label(new Rect(43, 98, 150, 150), "Score: " + score.ToString(), style2);
            GUI.Label(new Rect(47, 102, 150, 150), "Score: " + score.ToString(), style2);
            GUI.Label(new Rect(45, 100, 150, 150), "Score: " + score.ToString(), style);

            
            GUI.Label(new Rect(43, 123, 150, 150), "Combo: " + consecutiveNotes.ToString() + "x", style2);
            GUI.Label(new Rect(43, 127, 150, 150), "Combo: " + consecutiveNotes.ToString() + "x", style2);
            GUI.Label(new Rect(47, 123, 150, 150), "Combo: " + consecutiveNotes.ToString() + "x", style2);
            GUI.Label(new Rect(47, 127, 150, 150), "Combo: " + consecutiveNotes.ToString() + "x", style2);
            GUI.Label(new Rect(45, 125, 150, 150), "Combo: " + consecutiveNotes.ToString() + "x", style);
        }
    }

    void OnTriggerEnter(Collider obj)
    {
        if (!isMenu)
        {
            if (obj.tag == "JumpTag")
            {
                jumptex.setTrans();
            }

            if (obj.tag == "PassTag" && lastHit > 1f)
            {
                consecutiveNotes++;
                score += 1;//(consecutiveNotes / 5 + 1);
            }

            if (obj.tag == "CrouchTag")
            {
                crouchtex.setTrans();
            }
        }
    }

    void OnCollisionEnter(Collision obj)
    {
        if (!isMenu)
        {
            if (obj.transform.tag == "CollisionObj")
            {

                obj.collider.isTrigger = true;


                if (lastHit > 3)
                {
                    lastHit = 0;
                    HP--;
                    consecutiveNotes = 0;
                    if (HP == 4)
                        objs[0].transform.localPosition = HPpos[0];
                    if (HP == 3)
                        objs[1].transform.localPosition = HPpos[1];
                    if (HP == 2)
                        objs[2].transform.localPosition = HPpos[2];
                    if (HP == 1)
                        objs[3].transform.localPosition = HPpos[3];
                    Debug.Log("HIT OBSTACLE: " + HP.ToString() + "HP Left!");
                }

            }
            else
                hasJumped = false;
            Debug.Log("Collided! Object: " + obj.transform.name);
        }
    }
}
