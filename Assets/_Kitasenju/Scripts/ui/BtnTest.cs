using UnityEngine;
using UnityEngine.UI;

public class BtnTest :  Button, ICanvasRaycastFilter{

	public bool IsRaycastLocationValid (Vector2 sp, Camera eventCamera)
	{
		return false;
	}
}