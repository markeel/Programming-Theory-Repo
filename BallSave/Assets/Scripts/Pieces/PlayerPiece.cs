using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerPiece : Piece
{
    public void SetHelpTip()
    {
        GameObject go = gameObject.transform.Find("tip").gameObject;
        go.SetActive(true);
        go.GetComponent<Renderer>().material = manager.tipSelectMat;
    }

    public void OverHelpTip()
    {
        GameObject go = gameObject.transform.Find("tip").gameObject;
        go.SetActive(true);
        go.GetComponent<Renderer>().material = manager.tipOverMat;
    }

    public void ClearHelpTip()
    {
        gameObject.transform.Find("tip").gameObject.SetActive(false);
    }

    public abstract bool CanMove(int newRow, int newCol);

    protected override void CompleteMove()
    {
        manager.TestForImpact();
        manager.SetPlayerComplete();
    }

}
