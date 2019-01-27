using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiTool : MonoBehaviour
{
    public List<GameObject> Tools;
    public int SelectedTool
    {
        get { return _selectedTool; }
        set { UpdateSelectedTool(value); }
    }

    private int _selectedTool = 0;

    public Vector3 Target;
    private float _rotSpeed = 2000f;

    private AudioSource _switchSound;
    public float lookSpeed;

    public float toolCooldownPeriod = 0.5f;
    private float toolCooldownRemaining = 0;

    public void Start()
    {
        _switchSound = GetComponent<AudioSource>();
    }

    public void UpdateSelectedTool(int newSelection)
    {
        if (newSelection >= 0 && newSelection < Tools.Count)
        {
            _selectedTool = newSelection;
        }
        else if (newSelection == Tools.Count)
        {
            _selectedTool = 0;
        }

        foreach (var t in Tools)
        {
            t.SetActive(false);
        }

        Tools[_selectedTool].SetActive(true);

        if(Tools[_selectedTool].GetComponent<ToolControl>().PlayerShip())
        {
            _switchSound.Play();
        }
    }

    public GameObject cooldownCircle;
    public void ActivateMultiTool()
    {
        if (toolCooldownRemaining <= 0)
        {
            var tool = Tools[_selectedTool].GetComponent<ToolControl>();
            tool.DoActivate();
            toolCooldownRemaining = toolCooldownPeriod;

            if (tool.PlayerShip())
            {
                cooldownCircle.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
                cooldownCircle.transform.GetChild(1).transform.localScale = Vector3.one;
            }
        }
    }

    void Update()
    {
        Vector3 diff = Target - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * lookSpeed);

        if (toolCooldownRemaining > 0)
        {
            var tool = Tools[_selectedTool].GetComponent<ToolControl>();
            toolCooldownRemaining -= Time.deltaTime;

            if (tool.PlayerShip())
            {
                cooldownCircle.transform.GetChild(0).GetComponent<Image>().fillAmount -= Time.deltaTime * (1 / toolCooldownPeriod);
                cooldownCircle.transform.GetChild(1).transform.localScale -= Vector3.one * (Time.deltaTime * (1 / toolCooldownPeriod));
            }
        }
    }
}
