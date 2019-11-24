using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ControlSaturation : MonoBehaviour {

	public PostProcessProfile pp;
	private ColorGrading cc;
	private float repeater = 0f;
	private float repeaterInc = 1f;

	void Start() {
		ColorGrading temp;
		if (pp.TryGetSettings<ColorGrading>(out temp)) cc = temp;
	}

	void Update() {
		repeater += repeaterInc * Time.deltaTime;
		cc.hueShift.value = FourD.remap(Mathf.Sin(repeater), -1f, 1f, -100f, 100f);
	}
}