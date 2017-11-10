﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SystemUtil;

public class MirroBlendMoveManager : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject HMD;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject centerPanel;

    [Space(10), Header("PositionRecenter Settings")]
    public GameObject cameraRig;
    public Vector3 centerPos = new Vector3(0, 0, 0);
    public float lookAngleY = 0;
    private PositionRecenter positionRecenter;

    [Space(10), Header("Mirror Hand Settings")]
    public GameObject mirrorHandLeft;
    public GameObject mirrorHandRight;
    public Vector3 basePos = new Vector3(0, 0, 0); // Mirror point
    private SynchronizeRightLeft synchronizeRightLeft;
    private SynchronizeLeftRight synchronizeLeftRight;

    [Space(10), Header("Blend Settings")]
    public GameObject blendHandLeft;
    public GameObject blendHandRight;
    [Range(0, 1)]
    public float blendRateLeft;
    [Range(0, 1)]
    public float blendRateRight;
    public float autoBlendTime = 10.0f;

    [Space(10), Header("VR Objects Settings"), Range(0, Const.VR_OBJECT_COUNT - 1)]
    public int selectNum = 0;
    [SerializeField]
    private int objCount = Const.VR_OBJECT_COUNT;

    // Use this for initialization
    void Start()
    {
        positionRecenter = new PositionRecenter(cameraRig, HMD, centerPos, lookAngleY);

        synchronizeRightLeft = new SynchronizeRightLeft(HMD, mirrorHandLeft, rightHand, basePos);
        synchronizeLeftRight = new SynchronizeLeftRight(HMD, mirrorHandRight, leftHand, basePos);

        blendHandLeft.GetComponent<BlendMove>().Setup(leftHand, mirrorHandLeft);
        blendHandRight.GetComponent<BlendMove>().Setup(rightHand, mirrorHandRight);
    }

    // Update is called once per frame
    void Update()
    {
        synchronizeRightLeft.Update();
        synchronizeLeftRight.Update();

        blendHandLeft.GetComponent<BlendMove>().UpdateBlendRate(blendRateLeft);
        blendHandRight.GetComponent<BlendMove>().UpdateBlendRate(blendRateRight);

        // Calibration HMD position and rotation
        if (Input.GetKeyDown(KeyCode.Return))
        {
            positionRecenter.SetCenterPos();
            Debug.Log("Set Pos Recenter");
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            positionRecenter.ResetPos();
            Debug.Log("Reset Pos");
        }

        // Select VR Object
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectNum++;
            if (selectNum > Const.VR_OBJECT_COUNT - 1)
            {
                selectNum = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectNum--;
            if (selectNum < 0)
            {
                selectNum = Const.VR_OBJECT_COUNT - 1;
            }

        }
        VRObject.SetNum(selectNum);

        // Change MirrorHand angle
        if (Input.GetKeyDown("1"))
        {
            synchronizeRightLeft.ChangeMirrorMode();
            synchronizeLeftRight.ChangeMirrorMode();
        }

        // Show or hide centerPanel
        if (Input.GetKeyDown("2"))
        {
            centerPanel.SetActive(!centerPanel.active);
        }

        // MirrorHand and TrackerHand switch active
        if (Input.GetKeyDown("3"))
        {
            leftHand.transform.GetChild(0).gameObject.SetActive(!leftHand.transform.GetChild(0).gameObject.active);
            rightHand.transform.GetChild(0).gameObject.SetActive(!rightHand.transform.GetChild(0).gameObject.active);
            mirrorHandLeft.transform.GetChild(0).gameObject.SetActive(!mirrorHandLeft.transform.GetChild(0).gameObject.active);
            mirrorHandRight.transform.GetChild(0).gameObject.SetActive(!mirrorHandRight.transform.GetChild(0).gameObject.active);
        }

        // Show only rightHand and mirrorHandLeft
        if (Input.GetKeyDown("4"))
        {
            Debug.Log("Mode: Mirror");
            ResetHandActive();
            leftHand.transform.GetChild(1).gameObject.SetActive(false);
            mirrorHandRight.transform.GetChild(1).gameObject.SetActive(false);
            blendHandLeft.transform.GetChild(0).gameObject.SetActive(false);
            blendHandRight.transform.GetChild(0).gameObject.SetActive(false);
        }

        // Show only blendHands
        if (Input.GetKeyDown("5"))
        {
            Debug.Log("Mode: Blend");
            ResetHandActive();
            leftHand.transform.GetChild(1).gameObject.SetActive(false);
            rightHand.transform.GetChild(1).gameObject.SetActive(false);
            mirrorHandLeft.transform.GetChild(1).gameObject.SetActive(false);
            mirrorHandRight.transform.GetChild(1).gameObject.SetActive(false);
        }

        // Change show or hide only trackerHand
        if (Input.GetKeyDown("6"))
        {
            Debug.Log("Mode: Tracker");
            ResetHandActive();
            mirrorHandLeft.transform.GetChild(1).gameObject.SetActive(false);
            mirrorHandRight.transform.GetChild(1).gameObject.SetActive(false);
            blendHandLeft.transform.GetChild(0).gameObject.SetActive(false);
            blendHandRight.transform.GetChild(0).gameObject.SetActive(false);
        }


        // Blend rate set TrackerHand
        if (Input.GetKeyDown("7"))
        {
            Debug.Log("blendRate: 0");
            blendRateLeft = 0.0f;
            blendRateRight = 0.0f;
        }

        // Blend rate set Average 
        if (Input.GetKeyDown("8"))
        {
            Debug.Log("blendRate: 0.5");
            blendRateLeft = 0.5f;
            blendRateRight = 0.5f;
        }

        // Blend rate set MirrorHand
        if (Input.GetKeyDown("9"))
        {
            Debug.Log("blendRate: 1");
            blendRateLeft = 1.0f;
            blendRateRight = 1.0f;
        }

        // Auto blending
        if (Input.GetKeyDown("0"))
        {
            Debug.Log("Auto blending Start!");
            DOTween.To(
                () => blendRateLeft,
                num => blendRateLeft = num,
                1.0f,
                autoBlendTime
            );
            DOTween.To(
                () => blendRateRight,
                num => blendRateRight = num,
                1.0f,
                autoBlendTime
            );
        }

        if (Input.GetKeyDown("q"))
        {
            Debug.Log("Mode: Rest");
            ResetHandActive();
        }
    }

    private void ResetHandActive ()
    {
        leftHand.transform.GetChild(1).gameObject.SetActive(true);
        rightHand.transform.GetChild(1).gameObject.SetActive(true);
        mirrorHandLeft.transform.GetChild(1).gameObject.SetActive(true);
        mirrorHandRight.transform.GetChild(1).gameObject.SetActive(true);
        blendHandLeft.transform.GetChild(0).gameObject.SetActive(true);
        blendHandRight.transform.GetChild(0).gameObject.SetActive(true);
    }
}
