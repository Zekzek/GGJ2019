using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Planet Preset", menuName = "Planet Presets/Planet Preset DB", order = 2)]
public class PlanetPresetDB : ScriptableObject
{
    public PlanetPreset[] planets;
}
