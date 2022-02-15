using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissilePiece : Piece
{
    int m_moveCount = 0;
    PlayerPiece m_target;

    public void SetTarget(PlayerPiece p)
    {
        m_target = p;
    }

    public void Advance()
    {
        m_moveCount = 2;
        MoveToward();
    }

    private void MoveToward()
    { 
        int nr = row;
        int nc = col;
        if (m_target.row < row)
            nr = row - 1;
        if (m_target.row > row)
            nr = row + 1;
        if (m_target.col < col)
            nc = col - 1;
        if (m_target.col > col)
            nc = col + 1;

        MoveTo(nr, nc);
    }

    public void Explode()
    {
        // TODO - Add particle effect and sound effect
        m_moveCount = 0;
    }

    protected override void CompleteMove()
    {
        manager.TestForImpact();

        if (m_moveCount > 0)
            m_moveCount -= 1;

        if (m_moveCount == 0)
        {
            manager.SetOpponentComplete();
            return;
        }

        MoveToward();
    }
}
