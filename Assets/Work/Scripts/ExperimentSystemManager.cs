using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SystemUtil;
using System.IO;

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
    public InputField subjectNameInputField;

    [Space(10), Header("Experiment Settings")]
    public AudioSource beat;
    public int BPM = 120;
    public int moveCount = 10;
    public Text countDisplay;
    public Image countPanel;
    public Slider updownSlider;
    private bool isPlayingBeat;
    private string fileName;
    private bool isRecording;
    private bool isExperimenting;


    // Use this for initialization
    void Start()
    {
        positionRecenter = new PositionRecenter(cameraRig, HMD, centerPos, lookAngleY);

        synchronizeRightLeft = new SynchronizeRightLeft(HMD, mirrorHandLeft, rightHand, basePos);
        synchronizeLeftRight = new SynchronizeLeftRight(HMD, mirrorHandRight, leftHand, basePos);

        blendHandLeft.GetComponent<BlendMove>().Setup(leftHand, mirrorHandLeft);
        blendHandRight.GetComponent<BlendMove>().Setup(rightHand, mirrorHandRight);

        ChangeBlendMode();

        fileName = System.DateTime.Now.ToString("yyyy-MMdd-HHmmss") + ".csv";
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

        if (isExperimenting)
        {
            string writeData = ",Left," + leftHand.transform.position.x + "," + leftHand.transform.position.y + "," + leftHand.transform.position.z
                                + ",Right," + rightHand.transform.position.x + "," + rightHand.transform.position.y + "," + rightHand.transform.position.z;
            logSave(fileName, writeData);
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

        // Calibration HMD position and rotation
        if (Input.GetKeyDown("3"))
        {
            positionRecenter.SetCenterPos();
            Debug.Log("Set Pos Recenter");
        }
        if (Input.GetKeyDown("4"))
        {
            positionRecenter.ResetPos();
            Debug.Log("Reset Pos");
        }
    }

    private void ResetHandActive()
    {
        blendHandLeft.transform.GetChild(0).gameObject.SetActive(true);
        blendHandRight.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void ChangeBlendMode()
    {
        Debug.Log("Mode: Blend");
        ResetHandActive();
    }

    public void StartBeat()
    {
        if (isPlayingBeat) return;

        Debug.Log("Start Beat");
        isPlayingBeat = true;
        StartCoroutine(BeatCoroutine());
    }

    public void StopBeat()
    {
        Debug.Log("Stop Beat");
        isPlayingBeat = false;
    }

    private IEnumerator BeatCoroutine()
    {
        int firstCount = -6;
        int count = firstCount;

        while (isPlayingBeat)
        {
            beat.Play();

            // is recording
            if (isRecording)
            {
                count++;

                // count 1~10 wirte file.csv
                if (count > 0)
                {
                    isExperimenting = true;
                    string writeData = (count).ToString() + ",Left," + leftHand.transform.position.x + "," + leftHand.transform.position.y + "," + leftHand.transform.position.z
                                        + ",Right," + rightHand.transform.position.x + "," + rightHand.transform.position.y + "," + rightHand.transform.position.z;
                    logSave(fileName, writeData);
                }
                // record stop
                if (count == moveCount * 2)
                {
                    isExperimenting = false;
                    isRecording = false;
                }
            }

            // countDisplay
            if (count == firstCount)
            {
                countDisplay.fontSize = 40;
                countDisplay.text = "WAIT";
                countPanel.GetComponent<CanvasGroup>().alpha = 1.0f;
            }
            else if (count > firstCount && count < 0)
            {
                countDisplay.fontSize = 80;
                countDisplay.text = Mathf.Abs(count).ToString();
            }
            else if (count == 0)
            {
                countDisplay.fontSize = 40;
                countDisplay.text = "START";
                //countPanel.GetComponent<CanvasGroup>().DOFade(0, 0.5f).SetEase(Ease.InExpo);
            }
            else if (count <= moveCount * 2)
            {
                countDisplay.fontSize = 80;
                countDisplay.text = count.ToString();
                countPanel.GetComponent<CanvasGroup>().alpha = 0.0f;
            }

            // init count
            if (count == moveCount * 2)
            {
                count = firstCount;
            }

            yield return new WaitForSeconds(1.0f / (BPM / 60.0f));
        }
    }

    public void moveUpdownSlider(float value, float time)
    {
        DOTween.To(() => updownSlider.value,
         (x) => updownSlider.value = x,
         value,
         time
     );
    }

    public void PushRecordButton()
    {
        isRecording = !isRecording;
        if (isRecording)
        {
            Debug.Log("Start Record");
            logSave(fileName, "blendRateL," + blendRateLeft + ",blendRateR," + blendRateRight);
        }
        else
        {
            Debug.Log("Stop Record");
        }
    }

    public void SubmitInputField()
    {
        string name = subjectNameInputField.text;
        logSave(fileName, name);
    }

    public void logSave(string file, string txt)
    {
        StreamWriter sw;
        FileInfo fi;
        fi = new FileInfo(Application.dataPath + "/Work/Output/" + file);
        sw = fi.AppendText();
        sw.WriteLine(txt);
        sw.Flush();
        sw.Close();
    }
}
