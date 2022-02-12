using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherePiece : PlayerPiece
{
    public override bool CanMove(int r, int c)
    {
        if ((r == row - 1) || (r == row + 1))
            if ((c == col - 1) || (c == col) || (c == col + 1))
                return true;

        if (r == row)
            if ((c == col - 1) || (c == col + 1))
                return true;

        return false;
    }

}
