using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTool : MonoBehaviour
{
    public List<GameObject> Tools;
    public int SelectedTool
    {
        get { return _selectedTool; }
        set { UpdateSelectedTool(value);}
    }

    private int _selectedTool = 0;

    public Vector3 Target;
    private float _rotSpeed = 2000f;
    public float lookSpeed;

    void UpdateSelectedTool(int newSelection)
    {
        if (newSelection >= 0 && newSelection < Tools.Count)
        {
            _selectedTool = newSelection;
        }
        else if (newSelection == Tools.Count)
        {
            _selectedTool = 0;
        }

        foreach(var t in Tools)
        {
            t.SetActive(false);
        }

        Tools[_selectedTool].SetActive(true);
    }

    void Update()
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Target) - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rot_z - 90), Time.deltaTime * lookSpeed);
    }
}
