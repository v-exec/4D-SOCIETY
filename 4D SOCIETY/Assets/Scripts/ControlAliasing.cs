using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ControlAliasing : MonoBehaviour {

	public PostProcessLayer pp;
	private float repeater = 0f;
	private float repeaterInc = 1f;

	void Start() {
		pp.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
	}

	void Update() {
		repeater += repeaterInc * Time.deltaTime;
		pp.temporalAntialiasing.sharpness = FourD.remap(Mathf.Sin(repeater), -1, 1, 0, 3);
	}
}