using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    private Ship _ship;

    public Image hpBar;
    public Image hpBarDetail;

    public float hpSmoothing, hpDetailSmoothing;

    private void Start()
    {
        _ship = GetComponent<Ship>();
        hpBar.fillAmount = _ship.Health;
        hpBarDetail.fillAmount = _ship.Health;
    }

    private void Update()
    {
        hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, ((float)_ship.Health)/100, Time.deltaTime * hpSmoothing);
        hpBarDetail.fillAmount = Mathf.Lerp(hpBarDetail.fillAmount, ((float)_ship.Health) / 100, Time.deltaTime * hpDetailSmoothing);
    }


}
