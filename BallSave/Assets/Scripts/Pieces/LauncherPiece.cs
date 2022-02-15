using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherPiece : Piece
{
    public float riseTime = 1.0f;

    private float m_launcherEndHeight;
    private float m_launcherStartPos;
    private bool m_rising = false;
    private float m_appearanceTime;

    // Start is called before the first frame update
    void Awake()
    {
        m_launcherEndHeight = GetComponent<Renderer>().bounds.size.y;
        m_launcherStartPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_rising)
        {
            if (Time.time >= (m_appearanceTime + riseTime))
            {
                transform.position = new Vector3(transform.position.x, m_launcherStartPos + m_launcherEndHeight, transform.position.z);
                manager.SetOpponentComplete();
                m_rising = false;
            }
            else
            {
                float ypos = m_launcherStartPos + m_launcherEndHeight * (Time.time - m_appearanceTime) / riseTime;
                transform.position = new Vector3(transform.position.x, ypos, transform.position.z);
            }
        }
    }

    public void SetPresent(int r, int c)
    {
        m_rising = true;
        m_appearanceTime = Time.time;
        Vector3 underOffset = new Vector3(0, m_launcherEndHeight, 0);
        transform.position = manager.GetBoardPosition(r, c) - underOffset;
        m_launcherStartPos = transform.position.y;
        gameObject.SetActive(true);
        row = r;
        col = c;
    }

    protected override void CompleteMove()
    {
        return;
    }

}
