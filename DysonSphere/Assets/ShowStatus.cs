using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStatus : MonoBehaviour
{
    public GameObject angryFace, neutralFace, happyFace;

    void Update()
    {
        var ship = GetComponentInParent<Ship>();
        if (ship.PlayerRelationShip == AIController.RelationshipStatus.Ally)
        {
            angryFace.SetActive(false);

            neutralFace.SetActive(false);

            happyFace.SetActive(true);
        }
        else if (ship.PlayerRelationShip == AIController.RelationshipStatus.Neutral)
        {
            angryFace.SetActive(false);

            neutralFace.SetActive(true);

            happyFace.SetActive(false);
        }
        else if (ship.PlayerRelationShip == AIController.RelationshipStatus.Enemy)
        {
            angryFace.SetActive(true);

            neutralFace.SetActive(false);

            happyFace.SetActive(false);
        }
    }
}
