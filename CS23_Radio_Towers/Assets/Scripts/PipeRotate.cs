using UnityEngine;
using UnityEngine.EventSystems;

public class PipeRotate : MonoBehaviour
{
    public Transform pipe;
    Vector3 rotation = new Vector3(0,0,90);

    public void Rotate(){
        Debug.Log("hi");
        pipe.Rotate(rotation);
    }


    public void OnPointerClick(PointerEventData eventData){
        Debug.Log("hi");
        pipe.Rotate(rotation);
    }

}
