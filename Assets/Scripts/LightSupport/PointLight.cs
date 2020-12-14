using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLight : MonoBehaviour
{
    public float Near = 5.0f;
    public float Far = 10.0f;
    public Color LightColor = Color.white;

    public void LoadLightToShader()
    {
        Shader.SetGlobalVector("LightPosition", transform.localPosition);
        Shader.SetGlobalColor("LightColor", LightColor);
        Shader.SetGlobalFloat("LightNear", Near);
        Shader.SetGlobalFloat("LightFar", Far);
    }
}
