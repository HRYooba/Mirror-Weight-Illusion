using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjScript : MonoBehaviour
{
    private List<GameObject> child = new List<GameObject>();
    private int selectNum;

    private void Awake()
    {
        int count = 0;
        foreach (Transform childTransform in transform)
        {
            child.Add(childTransform.gameObject);
            count++;
            //Debug.Log(count);
        }
    }

    // Use this for initialization
    void Start()
    {
        selectNum = 0;
        SelectObj(selectNum);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectNum++;
            if (selectNum > child.Count - 1)
            {
                selectNum = 0;
            }
            SelectObj(selectNum);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectNum--;
            if (selectNum < 0)
            {
                selectNum = child.Count - 1;
            }
            SelectObj(selectNum);
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
        //Debug.Log(num);
    }
}
