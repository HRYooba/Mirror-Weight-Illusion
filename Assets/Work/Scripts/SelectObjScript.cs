using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SystemUtil;

public class SelectObjScript : MonoBehaviour
{
    private List<GameObject> child = new List<GameObject>();
    private int selectNumbuffer;


    private void Awake()
    {
        foreach (Transform childTransform in transform)
        {
            child.Add(childTransform.gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        if (child.Count != Const.VR_OBJECT_COUNT)
        {
            Debug.LogError("Error: child.Count != VR_OBJECT_COUNT");
        }
        SelectObj(VRObject.GetNum());
    }

    // Update is called once per frame
    void Update()
    {
        if (selectNumbuffer != VRObject.GetNum())
        {
            SelectObj(VRObject.GetNum());
        }
    }

    private void InitObj()
    {
        foreach (GameObject obj in child) obj.SetActive(false);
    }

    private void SelectObj(int num)
    {
        InitObj();
        child[num].SetActive(true);
        selectNumbuffer = num;
    }
}
