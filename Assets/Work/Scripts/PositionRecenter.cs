using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRecenter : MonoBehaviour
{
    public Vector3 centerPos = new Vector3(0, 0, 0);
    public float lookAngleY = 0;

    public GameObject HMD;

    private Transform transformBuffer;

    public void Start()
    {

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SetPos();
            Debug.Log("Set Pos Recenter");
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ResetPos();
            Debug.Log("Reset Pos");
        }

    }

    private void SetPos()
    {
        ResetPos();

        transformBuffer = HMD.transform;

        Vector3 tempAngle = new Vector3(0, transformBuffer.eulerAngles.y - lookAngleY, 0) * -1;
        gameObject.transform.eulerAngles = tempAngle;

        Vector3 tempVector = new Vector3(transformBuffer.position.x - centerPos.x, transformBuffer.position.y - centerPos.y, transformBuffer.position.z - centerPos.z) * -1;
        gameObject.transform.position = tempVector;
    }

    private void ResetPos()
    {
        gameObject.transform.position = new Vector3(0, 0, 0);
        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}