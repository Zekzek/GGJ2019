using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet Preset", menuName = "Planet Presets/Planet Preset", order = 1)]
public class PlanetPreset : ScriptableObject
{
    public float windspeed;
    public float spinspeed;
    public Color landDetailColor;
    public Color landColor;
    public Color waterColor;
    public Color cloudColor;
}
