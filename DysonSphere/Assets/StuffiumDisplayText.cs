using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StuffiumDisplayText : MonoBehaviour
{
    Text _resourceDisplay;

    void Awake()
    {
        _resourceDisplay = GetComponent<Text>();
        GameState.Instance.player.OnResourceChange += UpdateText;
    }

    void UpdateText()
    {
        _resourceDisplay.text = "x<b>" + GameState.Instance.player.Resources + "</b>";
    }
}
