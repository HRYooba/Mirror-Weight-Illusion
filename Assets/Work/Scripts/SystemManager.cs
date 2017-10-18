using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SystemUtil;

public class SystemManager : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject HMD;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject centerPanel;
    public CanvasGroup blackOutPanel;

    [Space(10), Header("PositionRecenter Settings")]
    public GameObject cameraRig;
    public Vector3 centerPos = new Vector3(0, 0, 0);
    public float lookAngleY = 0;
    private PositionRecenter positionRecenter;

    [Space(10), Header("Mirror Hand Settings")]
    public GameObject mirrorHand;
    public Vector3 basePos = new Vector3(0, 0, 0); // Mirror point
    private SynchronizeRightLeft synchronizeRightLeft;

    [Space(10), Header("VR Objects Settings"), Range(0, Const.VR_OBJECT_COUNT - 1)]
    public int selectNum = 0;
    [SerializeField]
    private int objCount = Const.VR_OBJECT_COUNT;

    // Use this for initialization
    void Start()
    {
        leftHand.SetActive(false);

        positionRecenter = new PositionRecenter(cameraRig, HMD, centerPos, lookAngleY);
        synchronizeRightLeft = new SynchronizeRightLeft(HMD, mirrorHand, rightHand, basePos);

        blackOutPanel.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        synchronizeRightLeft.Update();

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
        }

        // Show or hide centerPanel
        if (Input.GetKeyDown("2"))
        {
            centerPanel.SetActive(!centerPanel.active);
        }

        // Start BlackOut
        if (Input.GetKeyDown("3"))
        {
            StartCoroutine(BlackOut());
        }

        // Switch active mirrorhand and lefthand
        if (Input.GetKeyDown("4"))
        {
            mirrorHand.SetActive(!mirrorHand.active);
            leftHand.SetActive(!leftHand.active);
        }
    }

    IEnumerator BlackOut()
    {
        blackOutPanel.DOFade(1.0f, 0.5f).SetEase(Ease.InSine).OnComplete(() => {
            mirrorHand.SetActive(!mirrorHand.active);
            leftHand.SetActive(!leftHand.active);
        });
        yield return new WaitForSeconds(2.0f);
        blackOutPanel.DOFade(0.0f, 0.3f).SetEase(Ease.InSine);
    }
}
