using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SystemUtil;

public class SliderValueOnText : MonoBehaviour {
    public Slider slider;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.GetComponent<Text>().text = (slider.value * Const.EXPERIMENT_BLENDRATE).ToString();
	}
}
