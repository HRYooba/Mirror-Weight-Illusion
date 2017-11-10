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

        Vector3 originalAngle = originalHand.transform.eulerAngles;
        Vector3 mirrorAngle = mirrorHand.transform.eulerAngles;

        Vector3 blendPos = originalPos * inRate + mirrorPos * blendRate;

        float x = BlendAngle(originalAngle.x, mirrorAngle.x);
        float y = BlendAngle(originalAngle.y, mirrorAngle.y);
        float z = BlendAngle(originalAngle.z, mirrorAngle.z);
        Vector3 blendAngle = new Vector3(x, y, z);

        gameObject.transform.position = blendPos;
        gameObject.transform.eulerAngles = blendAngle;
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
}
