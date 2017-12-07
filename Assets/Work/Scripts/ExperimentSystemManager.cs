using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SystemUtil;

public class ExperimentSystemManager : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject HMD;
    public GameObject leftHand;
    public GameObject rightHand;

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

    [Space(10), Header("GUI Settings")]
    public GameObject GUI;
    public Slider blendRateLSlider;
    public Slider blendRateRSlider;
    public GraphPointScript graphPoint;

    [Space(10), Header("Beat Settings")]
    public AudioSource beat;
    public int BPM = 120;
    private bool isPlayingBeat;

    // Use this for initialization
    void Start()
    {
        positionRecenter = new PositionRecenter(cameraRig, HMD, centerPos, lookAngleY);

        synchronizeRightLeft = new SynchronizeRightLeft(HMD, mirrorHandLeft, rightHand, basePos);
        synchronizeLeftRight = new SynchronizeLeftRight(HMD, mirrorHandRight, leftHand, basePos);

        blendHandLeft.GetComponent<BlendMove>().Setup(leftHand, mirrorHandLeft);
        blendHandRight.GetComponent<BlendMove>().Setup(rightHand, mirrorHandRight);

        ChangeBlendMode();
    }

    // Update is called once per frame
    void Update()
    {
        synchronizeRightLeft.Update();
        synchronizeLeftRight.Update();

        blendRateLeft = blendRateLSlider.value * Const.EXPERIMENT_BLENDRATE;
        blendRateRight = blendRateRSlider.value * Const.EXPERIMENT_BLENDRATE;
        graphPoint.UpdateBlendRate(blendRateLeft, blendRateRight);

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

        // MirrorHand and TrackerHand switch active
        if (Input.GetKeyDown("1"))
        {
            leftHand.transform.GetChild(0).gameObject.SetActive(!leftHand.transform.GetChild(0).gameObject.activeSelf);
            rightHand.transform.GetChild(0).gameObject.SetActive(!rightHand.transform.GetChild(0).gameObject.activeSelf);
            mirrorHandLeft.transform.GetChild(0).gameObject.SetActive(!mirrorHandLeft.transform.GetChild(0).gameObject.activeSelf);
            mirrorHandRight.transform.GetChild(0).gameObject.SetActive(!mirrorHandRight.transform.GetChild(0).gameObject.activeSelf);
        }

        // Show or hide GUI
        if (Input.GetKeyDown("2"))
        {
            GUI.SetActive(!GUI.active);
        }
    }

    private void ResetHandActive()
    {
        leftHand.transform.GetChild(1).gameObject.SetActive(true);
        rightHand.transform.GetChild(1).gameObject.SetActive(true);
        mirrorHandLeft.transform.GetChild(1).gameObject.SetActive(true);
        mirrorHandRight.transform.GetChild(1).gameObject.SetActive(true);
        blendHandLeft.transform.GetChild(0).gameObject.SetActive(true);
        blendHandRight.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ChangeBlendMode()
    {
        Debug.Log("Mode: Blend");
        ResetHandActive();
        leftHand.transform.GetChild(1).gameObject.SetActive(false);
        rightHand.transform.GetChild(1).gameObject.SetActive(false);
        mirrorHandLeft.transform.GetChild(1).gameObject.SetActive(false);
        mirrorHandRight.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void StartBeat()
    {
		if (isPlayingBeat) return;

        isPlayingBeat = true;
        StartCoroutine(BeatCoroutine());
    }

    public void StopBeat()
    {
        isPlayingBeat = false;
    }

    private IEnumerator BeatCoroutine()
    {
        while (isPlayingBeat)
        {
            beat.Play();
            yield return new WaitForSeconds(1.0f / (BPM / 60.0f));
        }
    }
}
