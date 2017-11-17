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
        Vector3 blendPos = Vector3.Lerp(originalPos, mirrorPos, blendRate);
        gameObject.transform.position = blendPos;

        Quaternion originalRot = originalHand.transform.rotation;
        Quaternion mirrorRot = mirrorHand.transform.rotation;
        Quaternion blendRot = Quaternion.Lerp(originalRot, mirrorRot, blendRate);
        gameObject.transform.rotation = blendRot;
    }

    public void UpdateBlendRate(float rate)
    {
        blendRate = rate;
    }
}
