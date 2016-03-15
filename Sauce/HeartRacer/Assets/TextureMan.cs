using UnityEngine;
using System.Collections;

public class TextureMan : MonoBehaviour {

    Material mat;
    Renderer rend;
	// Use this for initialization
	void Start () {
        rend = GetComponent<Renderer>();
        mat = rend.material;
	}
	
	// Update is called once per frame
	void Update () {
        mat.color = new Color(1, 1, 1, mat.color.a - 0.02f);
        if (mat.color.a < 0)
            mat.color = new Color(1, 1, 1, 0);
	}

    public void setTrans()
    {
        mat.color = new Color(1, 1, 1, 1);
    }
}
