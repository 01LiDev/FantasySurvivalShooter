using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChangeSkybox : MonoBehaviour
{
    public Material skybox1;
    public Material skybox2;

    private bool isSkybox1 = true;

    public void ToggleSkybox()
    {
        if (isSkybox1)
        {
            RenderSettings.skybox = skybox2;
            
        }
        else
        {
            RenderSettings.skybox = skybox1;
        }

        isSkybox1 = !isSkybox1;
    }
}
