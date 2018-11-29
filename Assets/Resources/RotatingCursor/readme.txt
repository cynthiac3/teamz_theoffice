--

Welcome! 

This asset brings an alternative cursor that rotates in the direction of movement, similar to the one used in Worms 3D.
It includes an example scene, and two preconfigured cursors that can be found in the 'Prefabs' folder.

To use one of the preconfigured cursors, just Drag&Drop the prefab inside your scene.
To show and hide the cursor, just activate or deactivate the GameObject inside your scene. Make sure to deactivate the default cursor by using:
Cursor.visible = false;

To create a custom cursor, create a new GameObject and drop the 'RotatingCursor' script in it.
In the inspector, fill the fields as follows:
- Cursor Texture: The image for your cursor.
- Texture Angle: The angle that your cursor image forms with the vertical axis. For example, 0 if the cursor is pointing up.
- Scale: Changes the image size. Set to 1 to stay at the original image size.
- Pixels Hot Spot: The pixel coordinates in the cursor image where the tip of your cursor is (0,0 for top-left).

- Smoothness: How smoothly the cursor moves.
- Max Turn: Maximum turn in degrees to be performed in a single update, regardless of smoothness.

--

- Zamaroht, February 2016
www.zamaroht.com