using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockPlacement : MonoBehaviour
{
    public GameObject blockHighlight;
    public GameObject BreakableBlocks;
    public GameObject BlockPrefab;
    public Camera PlayerCamera;
    public LayerMask layer;
    public float ReachDistance = 20.0f;
    public float drappWith = 1;
    public float drappHeit = 1;
    public List<Vector3> BlockLocations = new List<Vector3>();
    private Vector3 OriginalPosition;
    private Vector3 PlacementPosition;
    public float BlockPlaceDelay;
    private float timer;
    void PlaceBlock()
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

            Vector3 PlacementPosition = placementCalculation(hitInfo);


            if (!BlockLocations.Contains(PlacementPosition))
            {
                BlockLocations.Add(PlacementPosition);
                Instantiate(BlockPrefab, PlacementPosition, Quaternion.identity, BreakableBlocks.transform);
            }
        }
    }
    Vector3 placementCalculation(RaycastHit hitInfo)
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
        if (Physics.Raycast(ray, out hitInfo, ReachDistance, layer)){
            Vector3 placementPos = placementCalculation(hitInfo);
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(placementPos, Vector3.one);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        BlockLocations.Add(new Vector3(9, 9, 111111110));
        /*        Instantiate(BlockPrefab, new Vector3(-261, 3, 428), Quaternion.identity);*/
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timer >= BlockPlaceDelay)
        {
            timer = 0;
            PlaceBlock();
        }
        else if (Input.GetMouseButton(0) && timer >= BlockPlaceDelay * 1.5)
        {
            timer = 0;
            PlaceBlock();
        }
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, ReachDistance, layer))
        {
            blockHighlight.SetActive(true);
            Vector3 placementPos = placementCalculation(hitInfo);
            blockHighlight.transform.position = placementPos;
        }
        else
        {
            blockHighlight.SetActive(false);
        }
        
    }
}
