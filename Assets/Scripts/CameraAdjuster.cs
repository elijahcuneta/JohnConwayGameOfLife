using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class CameraAdjuster : MonoBehaviour
{
    private float horizontalResolution;
 
    void Awake ()
    {   
        horizontalResolution = Screen.width;
        float currentAspect = (float) Screen.width / (float) Screen.height;
        Camera.main.orthographicSize = horizontalResolution / currentAspect / 200;
    }
}