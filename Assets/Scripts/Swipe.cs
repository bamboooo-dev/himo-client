using UnityEngine;
using System.Collections;

public class Swipe : MonoBehaviour
{
    public float StartPos;
    public float EndPos;
    Camera mainCamera;

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPos = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        }
        if (Input.GetMouseButtonUp(0))
        {
            EndPos = mainCamera.ScreenToWorldPoint(Input.mousePosition).x;
            if (StartPos > EndPos)
            {
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + 1000, mainCamera.transform.position.y, mainCamera.transform.position.z);
            }
            else if (StartPos < EndPos)
            {
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x - 1000, mainCamera.transform.position.y, mainCamera.transform.position.z);
            }
            StartPos = 0;
            EndPos = 0;
        }
    }
}