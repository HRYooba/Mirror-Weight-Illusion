using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemUtil;
using DG.Tweening;

public class BlendMove : MonoBehaviour
{

    private GameObject originalHand;
    private GameObject mirrorHand;
    private float blendRate;
    private float inRate; // Inverse blendRate
    private Vector3 bufferOriginalAngle;
    private Vector3 bufferMirrorAngle;

    public void Setup(GameObject _originalHand, GameObject _mirrorHand)
    {
        originalHand = _originalHand;
        mirrorHand = _mirrorHand;
    }

    // Use this for initialization
    void Start()
    {
        blendRate = 0.0f;
        inRate = 1.0f - blendRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (originalHand == null || mirrorHand == null)
        {
            Debug.LogError("originalHand = null && mirrorHnad = null");
            return;
        }

        inRate = 1.0f - blendRate;

        Vector3 originalPos = originalHand.transform.position;
        Vector3 mirrorPos = mirrorHand.transform.position;
        Vector3 blendPos = originalPos * inRate + mirrorPos * blendRate;
        gameObject.transform.position = blendPos;

        Vector3 originalAngle = originalHand.transform.eulerAngles;
        Vector3 mirrorAngle = mirrorHand.transform.eulerAngles;
        //float x = BlendAngle(originalAngle.x, mirrorAngle.x);
        //float y = BlendAngle(originalAngle.y, mirrorAngle.y);
        //float z = BlendAngle(originalAngle.z, mirrorAngle.z);
        //Vector3 blendAngle = new Vector3(x, y, z);
        //float x = ConfirmBlendAngle(originalAngle.x, mirrorAngle.x, bufferOriginalAngle.x, bufferMirrorAngle.x);
        //float y = ConfirmBlendAngle(originalAngle.y, mirrorAngle.y, bufferOriginalAngle.y, bufferMirrorAngle.y);
        //float z = ConfirmBlendAngle(originalAngle.z, mirrorAngle.z, bufferOriginalAngle.z, bufferMirrorAngle.z);
        //Vector3 blendAngle = new Vector3(x, y, z);
        Vector3 blendAngle = originalAngle * inRate + mirrorAngle * blendRate;
        gameObject.transform.eulerAngles = blendAngle;
        bufferOriginalAngle = originalAngle;
        bufferMirrorAngle = mirrorAngle;
    }

    public void UpdateBlendRate(float rate)
    {
        blendRate = rate;
    }

    public float BlendAngle(float angle1, float angle2)
    {

        float sin1 = Mathf.Sin(angle1 / 180.0f * Mathf.PI);
        float cos1 = Mathf.Cos(angle1 / 180.0f * Mathf.PI);
        float sin2 = Mathf.Sin(angle2 / 180.0f * Mathf.PI);
        float cos2 = Mathf.Cos(angle2 / 180.0f * Mathf.PI);


        float blendSin = sin1 * inRate + sin2 * blendRate;
        float blendCos = cos1 * inRate + cos2 * blendRate;


        Vector2 blendVector = new Vector2(blendCos, blendSin);
        float blendAngle = Mathf.Atan2(blendSin, blendCos) * 180.0f / Mathf.PI;


        return blendAngle;
    }

    public float ConfirmBlendAngle(float angle1, float angle2, float angle1Buff, float angle2Buff)
    {
        if (angle1 >= 0 && angle1 <= 90 && angle1Buff >= 270 && angle1Buff <= 360 && angle2 >= 270 && angle2 <= 360)
        {
            angle1 = angle1 + 360;
        }
        if (angle2 >= 0 && angle2 < 90 && angle2Buff >= 270 && angle2Buff <= 360 && angle1 >= 270 && angle1 <= 360)
        {
            angle2 = angle2 + 360;
        }
        if (angle1 >= 270 && angle1 <= 360 && angle1Buff >= 0 && angle1Buff <= 90 && angle2 >= 0 && angle2 <= 90)
        {
            angle1 = -angle1;
        }
        if (angle2 >= 270 && angle2 <= 360 && angle2Buff >= 0 && angle2Buff <= 90 && angle1 >= 0 && angle1 <= 90)
        {
            angle2 = -angle2;
        }

        return angle1 * inRate + angle2 * blendRate;
    }
}
