using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizeRightLeft : MonoBehaviour {

    private GameObject HMD;
    private GameObject mirrorHand;
    private GameObject originalHand;
    private Vector3 basePos; // 鏡運動させるときの基準となるポジション
    private bool isMirror;

    public SynchronizeRightLeft (GameObject _HMD, GameObject _mirrorHand, GameObject _originalHand, Vector3 _basePos)
    {
        HMD = _HMD;
        mirrorHand = _mirrorHand;
        originalHand = _originalHand;
        basePos = _basePos;
        isMirror = true;
    }

	// Use this for initialization
	void Start () {

	}

    // Update is called once per frame
    public void Update() {
        Vector3 pos = originalHand.transform.position;
        Quaternion rot = originalHand.transform.rotation;
        Vector3 angle = rot.eulerAngles;

        mirrorHand.transform.position = new Vector3(basePos.x - GetPosXLengthToBasePos(), pos.y, pos.z);
        if (isMirror)
        {
            mirrorHand.transform.eulerAngles = new Vector3(angle.x, -angle.y, - angle.z + 180);
        } else
        {
            mirrorHand.transform.eulerAngles = new Vector3(angle.x, angle.y, angle.z);
        }
    }

    private float GetPosXLengthToBasePos()
    {
        Vector3 pos = originalHand.transform.position;
        return Mathf.Abs(pos.x - basePos.x);
    }

    public void ChangeMirrorMode()
    {
        isMirror = !isMirror;
    }
}
