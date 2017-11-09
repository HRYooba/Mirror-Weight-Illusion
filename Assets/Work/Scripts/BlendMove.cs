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

    public void Setup(GameObject _originalHand, GameObject _mirrorHand)
    {
        originalHand = _originalHand;
        mirrorHand = _mirrorHand;
    }

    // Use this for initialization
    void Start()
    {
        blendRate = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (originalHand == null || mirrorHand == null)
        {
            Debug.LogError("originalHand = null && mirrorHnad = null");
            return;
        }

        Vector3 originalPos = originalHand.transform.position;
        Vector3 mirrorPos = mirrorHand.transform.position;

        Vector3 originalAngle = originalHand.transform.eulerAngles;
        Vector3 mirrorAngle = mirrorHand.transform.eulerAngles;

        float inRate = 1.0f - blendRate;

        Vector3 blendPos = originalPos * inRate + mirrorPos * blendRate;

        float angleX = (originalAngle.x + 180.0f) * inRate + (mirrorAngle.x + 180.0f) * blendRate;
        float angleY = (originalAngle.y + 180.0f) * inRate + (mirrorAngle.y + 180.0f) * blendRate;
        float angleZ = (originalAngle.z + 180.0f) * inRate + (mirrorAngle.z + 180.0f) * blendRate;
        Vector3 blendAngle = new Vector3(angleX, angleY, angleZ);

        gameObject.transform.position = blendPos;
        gameObject.transform.eulerAngles = blendAngle;
    }

    public void UpdateBlendRate(float rate)
    {
        blendRate = rate;
    }
}
