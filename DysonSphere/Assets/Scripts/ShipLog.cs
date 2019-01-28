using System.Collections.Generic;

public class ShipLog
{
	public AIController.Personality Temperment { get; set; }
	public AIController.RelationshipStatus PlayerRelationship { get; set; }
	public bool Dead { get; set; }
	public bool KilledByPlayer { get; set; }
}
