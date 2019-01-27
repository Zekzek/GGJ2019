using UnityEngine;

public class PlayerState
{
    public Ship Ship { get; set; }
    public int Health { get { return Ship.Health; } set { Ship.Health = value; } }
    public float Resources
    {
        get
        {
            return Ship.TotalResources;
        }
        set
        {
            if (Mathf.Approximately(value, Ship.TotalResources))
                return;

            if (value < Ship.TotalResources)
            {
                Ship.TakeResource(Ship.TotalResources - value);
            }
            else
            {
                Ship.AddRandomResource(value - Ship.TotalResources);
            }
        }
    }
    public int Unrest { get; set; }
    public int GatherDistance { get; set; }
    public int CommunicationDistance { get; set; }
}
