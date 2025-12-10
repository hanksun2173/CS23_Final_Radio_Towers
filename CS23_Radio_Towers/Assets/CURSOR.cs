using UnityEngine;

public class CURSOR : MonoBehaviour
{
   [SerializeField] private Texture2D cursorTexture;

   private Vector2 cursorHotspot; 

//Start is called before the first frame update 

void Start ()
    {
        cursorHotspot = new Vector2 (cursorTexture.width / 2, cursorTexture.height / 2); 
        Cursor.SetCursor (cursorTexture, cursorHotspot, CursorMode.Auto); 

    }
}
  