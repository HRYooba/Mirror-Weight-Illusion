using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRecenter : MonoBehaviour
{
    private GameObject cameraRig;
    private GameObject HMD;
    private Vector3 centerPos = new Vector3(0, 0, 0);
    private float lookAngleY = 0;

    private Transform transformBuffer;

    public PositionRecenter(GameObject _cameraRig, GameObject _HMD, Vector3 _centerPos, float _lookAngleY)
    {
        cameraRig = _cameraRig;
        HMD = _HMD;
        centerPos = _centerPos;
        lookAngleY = _lookAngleY;
    }

    public void Start()
    {

    }

    public void Update()
    {

    }

    public void SetCenterPos()
    {
        ResetPos();

        transformBuffer = HMD.transform;

        Vector3 tempAngle = new Vector3(0, transformBuffer.eulerAngles.y - lookAngleY, 0) * -1;
        cameraRig.transform.eulerAngles = tempAngle;

        Vector3 tempVector = new Vector3(transformBuffer.position.x - centerPos.x, cameraRig.transform.position.y, transformBuffer.position.z - centerPos.z) * -1;
        cameraRig.transform.position = tempVector;
    }

    public void ResetPos()
    {
        cameraRig.transform.position = new Vector3(0, 0, 0);
        cameraRig.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}