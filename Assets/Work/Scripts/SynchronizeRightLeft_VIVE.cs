using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizeRightLeft_VIVE : MonoBehaviour {

    public GameObject HMD;
    public GameObject originalHand;

    private Vector3 basePos; // 鏡運動させるときの基準となるポジション

	// Use this for initialization
	void Start () {
        basePos = new Vector3(0, 0, 0);
	}

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            basePos = HMD.transform.position;
            Debug.Log(basePos);
        }

        Vector3 pos = originalHand.transform.position;
        Quaternion rot = originalHand.transform.rotation;
        Vector3 angle = rot.eulerAngles;

        gameObject.transform.position = new Vector3(basePos.x - GetPosXLengthToBasePos(), pos.y, pos.z);
        gameObject.transform.eulerAngles = new Vector3(angle.x, -angle.y, -angle.z);
    }

    float GetPosXLengthToBasePos()
    {
        Vector3 pos = originalHand.transform.position;
        return Mathf.Abs(pos.x - basePos.x);
    }
}
