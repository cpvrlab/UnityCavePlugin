using UnityEngine;
using System.Collections;
using Cave;

[System.Serializable]
public class CaveSettings : BasicSettings
{
    [Header("Main")]
    public bool ShowCave = false;
    public bool SetMouseCursor = true;
    public bool ForceShowMouseCursor = false;
    public int BeamerResolutionWidth = 1280;
    public int BeamerResolutionHeight = 1024;
    public string Host = "192.168.0.201";
    public FrustumMode FrustumMode = FrustumMode.CAVEXXL;
    public BasicSettings.Sides GUILocation = BasicSettings.Sides.Front;
    public float EyeDistance = 0.07f;
}
