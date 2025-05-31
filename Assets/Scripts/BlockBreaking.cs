using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BlockBreaking : MonoBehaviour
{
    public Camera PlayerCamera;
    public LayerMask layer;
    public float ReachDistance = 20.0f;
    public float drappWith = 1;
    public float drappHeit = 1;
    private Vector3 OriginalPosition;
    private Vector3 PlacementPosition;
    public float BlockPlaceDelay;
    private float timer;
    void BreakBlock()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, ReachDistance, layer))
        {
            /*Vector3 OriginalPosition = hitInfo.point;
            Vector3 normal = hitInfo.normal;
            PlacementPosition = OriginalPosition + normal * 1f;
            PlacementPosition = new Vector3(Mathf.Floor(OriginalPosition.x + 0.5f), Mathf.Floor(OriginalPosition.y + 0.5f), Mathf.Floor(OriginalPosition.z + 0.5f));
            Debug.Log(PlacementPosition);*/

            Vector3 BreakPosition = BreakingCalculation(hitInfo);
            if (hitInfo.transform.parent.name == "Blocks")
            {
                GetComponent<BlockPlacement>().BlockLocations.Remove(hitInfo.transform.position);
                Destroy(hitInfo.transform.gameObject);
            }
        }
    }
    Vector3 BreakingCalculation(RaycastHit hitInfo)
    {
        Vector3 insidepoint = hitInfo.point - hitInfo.normal * 0.01f;
        float gridSize = 1.0f;
        float halfGrid = gridSize / 2.0f;

        float cx = Mathf.Floor(insidepoint.x / gridSize) * gridSize + halfGrid;
        float cy = Mathf.Floor(insidepoint.y / gridSize) * gridSize + halfGrid;
        float cz = Mathf.Floor(insidepoint.z / gridSize) * gridSize + halfGrid;

        return new Vector3(cx, cy, cz) + hitInfo.normal * gridSize;
    }

    void OnDrawGizmos()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * ReachDistance);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, ReachDistance, layer))
        {
            Vector3 placementPos = BreakingCalculation(hitInfo);
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(placementPos, Vector3.one);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(1) && timer >= BlockPlaceDelay)
        {
            timer = 0;
            BreakBlock();
            print("proper button");
        }
    }
}
