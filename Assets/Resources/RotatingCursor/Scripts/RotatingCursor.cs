using UnityEngine;
using System.Collections;

public class RotatingCursor : MonoBehaviour 
{
	[Header("Cursor graphics")]
	[Tooltip("The cursor texture.")]
	public Texture2D cursorTexture;
	[Range(0, 360)]
	[Tooltip("The angle at which the cursor is pointing in the original texture.")]
	public float textureAngle;
	[Tooltip("Texture scaling.")]
	public float scale = 1;
	[Tooltip("Coordinate in pixels where the cursor clicks.")]
	public Vector2 pixelsHotSpot;

	[Header("Cursor behaviour")]
	[Range(0, 1)]
	[Tooltip("How smoothly the cursor moves.")]
	public float smoothness = 0.8f;
	[Range(0, 180)]
	[Tooltip("Maximum turn in degrees to be performed in a single update, regardless of smoothness.")]
	public float maxTurn = 15f;

	Vector2 pos;
	Rect rect;
	float angle;

	Vector2 prev_position;

	void Start()
	{
		Cursor.visible = false;
		prev_position = Input.mousePosition;
	}

	void OnGUI()
	{
		updateTexture();

		Matrix4x4 back = GUI.matrix;
		GUIUtility.RotateAroundPivot(textureAngle + 90 + angle, pos);
		GUI.DrawTexture(rect, cursorTexture);
		GUI.matrix = back;
	}

	void updateTexture()
	{
		pos = Input.mousePosition;
		pos.y = Screen.height - pos.y;
		// pos -= pixelsHotSpot;

		if(pos != prev_position)
		{
			float last_angle = angle;
			float new_angle = Mathf.Atan2(pos.y - prev_position.y, pos.x - prev_position.x) * Mathf.Rad2Deg;
			angle = Mathf.MoveTowardsAngle(
				last_angle, 
				Mathf.LerpAngle(last_angle, new_angle, 1f - smoothness), 
				10f
				);
		}
		
		prev_position = pos;

		rect = new Rect(pos.x - pixelsHotSpot.x, pos.y - pixelsHotSpot.y, cursorTexture.width * scale, cursorTexture.height * scale);
	}

}
