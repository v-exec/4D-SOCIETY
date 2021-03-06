﻿using UnityEngine;

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

        bool currentTextureEye0 = false;

        public int antiAliasing = 1;
        public float scaleDistortion = 8.0f;
        public float scaleFlow = 2.0f;
        public float scaleTurbulence = 60.0f;
        public float speedDistortion = 0.5f;
        public float speedFlow = 0.2f;
        public float speedTurbulence = 5.8f;
        public float amountDistortion = 0.1f;
        public float amountFlow = 2.5f;
        public float amountTurbulence = 0.01f;

        private void SanitizeShaderParameters() {
            antiAliasing =      Mathf.Clamp(antiAliasing, 1, 2048);
            scaleDistortion =   Mathf.Clamp(scaleDistortion, 0.0f, 100.0f);
            scaleFlow =         Mathf.Clamp(scaleFlow, 0.0f, 100.0f);
            scaleTurbulence =   Mathf.Clamp(scaleTurbulence, 0.0f, 100.0f);
            speedDistortion =   Mathf.Clamp(speedDistortion, 0.0f, 100.0f);
            speedFlow =         Mathf.Clamp(speedFlow, 0.0f, 100.0f);
            speedTurbulence =   Mathf.Clamp(speedTurbulence, 0.0f, 100.0f);
            amountDistortion =  Mathf.Clamp(amountDistortion, 0.0f, 100.0f);
            amountFlow =        Mathf.Clamp(amountFlow, 0.0f, 100.0f);
            amountTurbulence =  Mathf.Clamp(amountTurbulence, 0.0f, 100.0f);
        }

        private void SendShaderParameters() {
            material.SetInt("antiAliasing", antiAliasing);
            material.SetFloat("scaleDistortion", scaleDistortion);
            material.SetFloat("scaleFlow", scaleFlow);
            material.SetFloat("scaleTurbulence", scaleTurbulence);
            material.SetFloat("speedDistortion", speedDistortion);
            material.SetFloat("speedFlow", speedFlow);
            material.SetFloat("speedTurbulence", speedTurbulence);
            material.SetFloat("amountDistortion", amountDistortion);
            material.SetFloat("amountFlow", amountFlow);
            material.SetFloat("amountTurbulence", amountTurbulence);
        }

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

            // sanitize & send shader parameters
            SanitizeShaderParameters();
            SendShaderParameters();

            material.SetTexture("_SourceTex", source);
            // double buffering the accumulation textures
            if (currentTextureEye0) {
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
            currentTextureEye0 = !currentTextureEye0;
        }
    }
}