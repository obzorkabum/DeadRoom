using UnityEngine;

public class killerCameraController : MonoBehaviour
{
    private Vector2 startPos;
    private Camera cam;
    private void Start()
    {
        cam = GetComponent<Camera>();

    }
    private void Update()
    {
        if(KillerController.CanIKill == false){ 
        if (Input.GetMouseButtonDown(0)) startPos = cam.ScreenToWorldPoint(Input.mousePosition);
        else if (Input.GetMouseButton(0)){
            float posx = cam.ScreenToWorldPoint(Input.mousePosition).x - startPos.x;
            float posy = cam.ScreenToWorldPoint(Input.mousePosition).y - startPos.y;

                   transform.position = new Vector3(transform.position.x - posx, transform.position.y - posy, transform.position.z);
        }
    }
    }
}
//transform.position = new Vector3(Mathf.Clamp( transform.position.x - posx, 16f, 27f), transform.position.y - posy, transform.position.z);