using UnityEngine;

public class CURSOR : MonoBehaviour
{
   [SerializeField] private Texture2D cursorTexture;

   private Vector2 cursorHotspot; 

//Start is called before the first frame update 

  void Start()
    {
        // Fingertip hotspot for your 89x128 hand image
        cursorHotspot = new Vector2(34, 18);

        Cursor.SetCursor(
            cursorTexture,
            cursorHotspot,
            CursorMode.Auto
        );
    }
}
  