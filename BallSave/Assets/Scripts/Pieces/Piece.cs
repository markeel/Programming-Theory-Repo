using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public GameManager manager;

    protected int row;
    protected int col;
    private int nextRow;
    private int nextCol;

    private bool isMoving;
    private float moveStartTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Vector3 endPos = manager.GetBoardPosition(nextRow, nextCol);
            float elapsed = Time.time - moveStartTime;
            if (elapsed > manager.moveTime)
            {
                transform.position = endPos;
                row = nextRow;
                col = nextCol;
                isMoving = false;
            }
            else
            {
                Vector3 startPos = manager.GetBoardPosition(row, col);
                Vector3 midPos = (endPos - startPos) / 2;
                float fraction = elapsed / manager.moveTime;
                Vector3 height = new Vector3(0, Mathf.Sin(fraction * Mathf.PI), 0);
                transform.position = Vector3.Lerp(startPos, endPos, fraction) + height * manager.moveHeight;
            }
        }
    }

    public void SetPos(int r, int c)
    {
        row = r;
        col = c;
    }

    public bool isAt(int r, int c)
    {
        if ((this.row == r) && (this.col == c))
            return true;

        return false;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void MoveTo(int newRow, int newCol)
    {
        isMoving = true;
        moveStartTime = Time.time;
        nextRow = newRow;
        nextCol = newCol;
    }

    public abstract bool CanMove(int newRow, int newCol);
}
