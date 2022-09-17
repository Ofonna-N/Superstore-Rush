using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTargetPos : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam;

    [SerializeField]
    private LayerMask floorLayer;

    // Update is called once per frame
    void Update()
    {
        MoveToMousePos();
    }

    private void MoveToMousePos()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 50f, floorLayer))
        {
            transform.position = hit.point;
        }

    }
}
