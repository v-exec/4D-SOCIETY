using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

	public Camera portalCameraA;
	public Material portalCameraMaterialA;
	public Camera portalCameraB;
	public Material portalCameraMaterialB;

	void Start() {
		if (portalCameraA.targetTexture != null) portalCameraA.targetTexture.Release();
		portalCameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		portalCameraMaterialA.mainTexture = portalCameraA.targetTexture;

		if (portalCameraB.targetTexture != null) portalCameraB.targetTexture.Release();
		portalCameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		portalCameraMaterialB.mainTexture = portalCameraB.targetTexture;
	}
}