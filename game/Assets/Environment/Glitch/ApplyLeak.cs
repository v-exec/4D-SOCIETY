using UnityEngine;

namespace Assets.Scripts.Cam.Effects {
	
	[ExecuteInEditMode]
	[RequireComponent(typeof(UnityEngine.Camera))]
	[AddComponentMenu("Image Effects/Custom/Leak")]

	public class ApplyLeak : MonoBehaviour {
		private Material m_material;
		private Shader shader;
        private RenderTexture accumTexture0;
        private RenderTexture accumTexture1;
        private Material material;

        bool currentTexture = false;

		public void OnRenderImage(RenderTexture source, RenderTexture destination) {

            // create the accumulation textures
            if (accumTexture0 == null || accumTexture0.width != source.width || accumTexture0.height != source.height) {
                DestroyImmediate(accumTexture0);
                accumTexture0 = new RenderTexture(source.width, source.height, 0);
                accumTexture0.hideFlags = HideFlags.HideAndDontSave;
                Graphics.Blit(source, accumTexture0);
            }
            if (accumTexture1 == null || accumTexture1.width != source.width || accumTexture1.height != source.height) {
                DestroyImmediate(accumTexture1);
                accumTexture1 = new RenderTexture(source.width, source.height, 0);
                accumTexture1.hideFlags = HideFlags.HideAndDontSave;
                Graphics.Blit(source, accumTexture1);
            }

            // create the material
            if (material == null) {
                shader = Shader.Find("Leak");
                material = new Material(shader) { hideFlags = HideFlags.DontSave };
                // set the overlaying texture
                material.SetTexture("_SourceTex", source);
            }

            // double buffering the accumulation textures
            if (currentTexture) {
                // distort accumTexture0 and overlay source into accumTexture1
                Graphics.Blit(accumTexture0, accumTexture1, material);
                // send out accumTexture1
                Graphics.Blit(accumTexture1, destination);
            } else {
                // distort accumTexture0 and overlay source into accumTexture1
                Graphics.Blit(accumTexture1, accumTexture0, material);
                // send out accumTexture1
                Graphics.Blit(accumTexture0, destination);
            }
            // update the current texture for the next iteration
            currentTexture = !currentTexture;
        }
    }
}