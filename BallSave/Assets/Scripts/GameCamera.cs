using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField]
    private GameManager manager;
    [SerializeField]
    private GameObject okObject;
    [SerializeField]
    private GameObject notOkObject;

    private float signYPos;

    // Start is called before the first frame update
    void Start()
    {
        signYPos = okObject.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.IsPlayerTurn())
            return;

        RaycastHit hit;
        int layerMask = 1 << 6;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
        {
            TileData data = hit.collider.gameObject.GetComponent<TileData>();
            if (data != null)
            {
                Vector3 tilePos = hit.collider.gameObject.transform.position;
                PlayerPiece piece = manager.SetOverPiece(data.row, data.col);
                if (piece != null)
                {
                    notOkObject.SetActive(false);
                    okObject.SetActive(false);
                    if (Input.GetMouseButtonDown(0))
                        manager.SetSelected(piece);
                }
                else
                {
                    if (manager.CanMoveTo(data.row, data.col))
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            manager.MovePiece(data.row, data.col);
                        }
                        else
                        {
                            okObject.transform.position = new Vector3(tilePos.x, signYPos, tilePos.z);
                            okObject.SetActive(true);
                            notOkObject.SetActive(false);
                        }
                    }
                    else
                    {
                        notOkObject.transform.position = new Vector3(tilePos.x, signYPos, tilePos.z);
                        notOkObject.SetActive(true);
                        okObject.SetActive(false);
                    }
                }
            }
            else
            {
                okObject.SetActive(false);
            }
        }
    }
}
