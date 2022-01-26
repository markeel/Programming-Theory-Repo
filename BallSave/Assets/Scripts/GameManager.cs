using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject tileWithHolePrefab;

    [SerializeField]
    private Material aMat;
    [SerializeField]
    private Material bMat;

    [SerializeField]
    private float spacingFactor = 1.1f;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = new Vector3(0, 0, 0);
        GameObject tile = Instantiate(tilePrefab, pos, tilePrefab.transform.rotation);
        tile.GetComponent<Renderer>().material = bMat;
        Vector3 size = tile.GetComponentInChildren<MeshRenderer>().bounds.size;
        float xoffset = ((size.x * spacingFactor * 3) + size.x) / 2;
        float xstart = xoffset;
        float zoffset = ((size.z * spacingFactor * 4) + size.x) / 2;
        float zstart = -zoffset;
        pos.x = xstart;
        pos.z = zstart;
        pos.x -= size.x * spacingFactor;
        tile.transform.position = new Vector3(xstart, tile.transform.position.y, zstart);
        int row = 0;
        int col = 1;
        HashSet<int> holes = new HashSet<int>();
        for (int hole = 0; hole < 4; hole++)
            holes.Add(Random.Range(5, 5 * 5));

        while (row < 6)
        {
            while (col < 5)
            {
                bool isHole = holes.Contains(row * 5 + col);

                if (isHole)
                    tile = Instantiate(tileWithHolePrefab, pos, tileWithHolePrefab.transform.rotation);
                else
                    tile = Instantiate(tilePrefab, pos, tilePrefab.transform.rotation);

                if ((((row % 2) == 0) && (col % 2 == 1)) || (((row % 2) == 1) && (col % 2 == 0)))
                    tile.GetComponent<Renderer>().material = aMat;
                else
                    tile.GetComponent<Renderer>().material = bMat;

                col += 1;
                pos.x -= size.x * spacingFactor;
            }
            pos.x = xstart;
            col = 0;
            pos.z += size.z * spacingFactor;
            row += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
