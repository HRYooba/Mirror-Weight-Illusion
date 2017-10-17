using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SystemUtil;

public class SystemManager : MonoBehaviour
{

    [Header("PositionRecenter Settings")]
    public GameObject cameraRig;
    public GameObject HMD;
    public Vector3 centerPos = new Vector3(0, 0, 0);
    public float lookAngleY = 0;
    private PositionRecenter positionRecenter;

    [Space(10), Header("Mirror Hand Settings")]
    public GameObject mirrorHand;
    public GameObject originalHand;
    public Vector3 basePos = new Vector3(0, 0, 0); // Mirror position
    private SynchronizeRightLeft synchronizeRightLeft;

    [Space(10), Header("VR Objects Settings"), Range(0, Const.VR_OBJECT_COUNT - 1)]
    public int selectNum = 0;
    [SerializeField]
    private int objCount = Const.VR_OBJECT_COUNT;

    // Use this for initialization
    void Start()
    {
        positionRecenter = new PositionRecenter(cameraRig, HMD, centerPos, lookAngleY);
        synchronizeRightLeft = new SynchronizeRightLeft(HMD, mirrorHand, originalHand, basePos);
    }

    // Update is called once per frame
    void Update()
    {
        synchronizeRightLeft.Update();

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
    }
}
