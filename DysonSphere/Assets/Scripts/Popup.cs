using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class Popup : MonoBehaviour
{
	public void Close()
	{
		PopupManager.Close(this);
	}
}
