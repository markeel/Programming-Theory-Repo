using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockerPiece : PlayerPiece
{
    public override bool CanMove(int r, int c)
    {
        if ((r == row - 1) || (r == row + 1))
            if (c == col)
                return true;

        if ((c == col - 1) || (c == col + 1))
            if (r == row)
                return true;

        return false;
    }

}

