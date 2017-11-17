using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphPointScript : MonoBehaviour {

    private float blendRateL = 0;
    private float blendRateR = 0;
    private Vector3 ZERO_POINT = new Vector3(-100, -200, 0);
    private float WIDTH = 200;
    private float HEIGHT = 200;
    private bool isPush;

    public void UpdateBlendRate(float _blendRateL, float _blendRateR)
    {
        blendRateL = _blendRateL;
        blendRateR = _blendRateR;

    }

	// Use this for initialization
	void Start () {
        isPush = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isPush)
        {
            float x = WIDTH * blendRateR + Input.GetAxis("Mouse X") * 12.0f;
            float y = HEIGHT * blendRateL + Input.GetAxis("Mouse Y") * 12.0f;
            if (x > WIDTH)
            {
                x = WIDTH;
            }
            if (x < 0)
            {
                x = 0;
            }
            if (y > HEIGHT)
            {
                y = HEIGHT;
            }
            if (y < 0)
            {
                y = 0;
            }
            gameObject.transform.localPosition = new Vector3(x, y, 0) + ZERO_POINT;
            blendRateR = x / WIDTH;
            blendRateL = y / HEIGHT;
        } else
        {
            float x = WIDTH * blendRateR;
            float y = HEIGHT * blendRateL;
            gameObject.transform.localPosition = new Vector3(x, y, 0) + ZERO_POINT;
        }
	}

    public void PushDown()
    {
        isPush = true;
    }

    public void PushUp()
    {
        isPush = false;
    }

    public bool GetPush()
    {
        return isPush;
    }

    public float GetBlendRateL()
    {
        return blendRateL;
    }

    public float GetBlendRateR()
    {
        return blendRateR;
    }
}
