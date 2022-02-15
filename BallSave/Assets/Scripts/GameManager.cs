using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float moveHeight = 0.1f;
    public float moveTime = 1.0f;

    public Material tipOverMat = null;
    public Material tipSelectMat = null;

    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject tileWithHolePrefab;
    [SerializeField]
    private GameObject breakerPrefab;
    [SerializeField]
    private GameObject spherePrefab;
    [SerializeField]
    private GameObject blockerPrefab;
    [SerializeField]
    private GameObject launcherPrefab;
    [SerializeField]
    private GameObject missilePrefab;

    [SerializeField]
    private GameObject m_gameOverText;
    [SerializeField]
    private GameObject m_youWonText;
    [SerializeField]
    private GameObject m_replayButton;

    [SerializeField]
    private Material aMat;
    [SerializeField]
    private Material bMat;

    [SerializeField]
    private float spacingFactor = 1.1f;

    private GameObject m_breaker1;
    private GameObject m_breaker2;
    private GameObject m_sphere;
    private GameObject m_blocker;
    private GameObject m_launcher;
    private GameObject m_missile;

    private HashSet<int> m_holes;
    private Vector3 m_size;
    private float m_xstart;
    private float m_zstart;
    private float m_ystart;

    private int numCols = 4;
    private int numRows = 6;

    private PlayerPiece m_selectedPiece;
    private PlayerPiece m_overPiece;
    private bool m_opponentTurn = true;
    private bool m_missleActive = false;
    private bool m_launcherActive = false;
    private bool m_gameOver = false;

    // Start is called before the first frame update
    void Awake()
    {
        LayoutTiles();
        LayoutPieces();
        m_launcher = Instantiate(launcherPrefab, GetBoardPosition(0, 0), launcherPrefab.transform.rotation);
        m_launcher.GetComponent<LauncherPiece>().manager = this;
        m_launcher.gameObject.SetActive(false);
        m_missile = Instantiate(missilePrefab, GetBoardPosition(0, 0), missilePrefab.transform.rotation);
        m_missile.GetComponent<MissilePiece>().manager = this;
        m_missile.GetComponent<MissilePiece>().SetTarget(m_sphere.GetComponent<SpherePiece>());
        m_missile.SetActive(false);

        MakeLauncherAppear();
    }

    public Vector3 GetBoardPosition(int row, int col)
    {
        float x = m_xstart - m_size.x * spacingFactor * col;
        float y = m_ystart;
        float z = m_zstart + m_size.z * spacingFactor * row;
        Vector3 pos = new Vector3(x, y, z);
        return pos;
    }


    private GameObject LayoutPiece(GameObject prefab, int r, int c)
    {
        GameObject go;
        go = Instantiate(prefab, GetBoardPosition(r, c), prefab.transform.rotation);
        go.GetComponent<PlayerPiece>().manager = this;
        go.GetComponent<Piece>().SetPos(r, c);

        return go;
    }

    private void LayoutPieces()
    {
        m_breaker1 = LayoutPiece(breakerPrefab, numRows - 1, numCols / 2 - 2);
        m_sphere = LayoutPiece(spherePrefab, numRows - 1, numCols / 2 - 1);
        m_blocker = LayoutPiece(blockerPrefab, numRows - 1, numCols / 2);
        m_breaker2 = LayoutPiece(breakerPrefab, numRows - 1, numCols / 2 + 1);
    }

    private void LayoutTiles()
    { 
        Vector3 pos = new Vector3(0, 0, 0);
        GameObject tile = Instantiate(tilePrefab, pos, tilePrefab.transform.rotation);
        tile.GetComponent<Renderer>().material = bMat;
        tile.name = $"tile 0, 0";
        tile.layer = 6;
        m_size = tile.GetComponentInChildren<MeshRenderer>().bounds.size;
        float xoffset = ((m_size.x * spacingFactor * (numCols-2)) + m_size.x) / 2;
        m_xstart = xoffset;
        float zoffset = ((m_size.z * spacingFactor * (numRows-2)) + m_size.x) / 2;
        m_zstart = -zoffset;
        pos.x = m_xstart;
        pos.z = m_zstart;
        pos.x -= m_size.x * spacingFactor;
        m_ystart = tile.transform.position.y;
        tile.transform.position = new Vector3(m_xstart, m_ystart, m_zstart);
        int row = 0;
        int col = 1;
        m_holes = new HashSet<int>();
        for (int hole = 0; hole < 3; hole++)
            m_holes.Add(Random.Range((hole+1)*numCols, (hole+1)*numCols+numCols));

        while (row < numRows)
        {
            while (col < numCols)
            {
                bool isHole = m_holes.Contains(row * numCols + col);

                if (isHole)
                    tile = Instantiate(tileWithHolePrefab, pos, tileWithHolePrefab.transform.rotation);
                else
                    tile = Instantiate(tilePrefab, pos, tilePrefab.transform.rotation);

                if ((((row % 2) == 0) && (col % 2 == 1)) || (((row % 2) == 1) && (col % 2 == 0)))
                    tile.GetComponent<Renderer>().material = aMat;
                else
                    tile.GetComponent<Renderer>().material = bMat;
                tile.name = $"tile {row}, {col}";
                tile.layer = 6;
                TileData data = tile.GetComponent<TileData>();
                data.col = col;
                data.row = row;
                col += 1;
                pos.x -= m_size.x * spacingFactor;
            }
            pos.x = m_xstart;
            col = 0;
            pos.z += m_size.z * spacingFactor;
            row += 1;
        }
    }

    public bool IsOccupied(int row, int col)
    {
        if (m_breaker1.GetComponent<Piece>().isAt(row, col))
            return true;
        if (m_breaker2.GetComponent<Piece>().isAt(row, col))
            return true;
        if (m_sphere.GetComponent<Piece>().isAt(row, col))
            return true;
        if (m_blocker.GetComponent<Piece>().isAt(row, col))
            return true;

        return false;
    }

    public PlayerPiece SetOverPiece(int row, int col)
    {
        if (m_breaker1.GetComponent<PlayerPiece>().isAt(row, col))
            return SetOver(m_breaker1.GetComponent<PlayerPiece>()); 
        
        if (m_breaker2.GetComponent<PlayerPiece>().isAt(row, col))
            return SetOver(m_breaker2.GetComponent<PlayerPiece>());

        if (m_sphere.GetComponent<PlayerPiece>().isAt(row, col))
            return SetOver(m_sphere.GetComponent<PlayerPiece>());

        if (m_blocker.GetComponent<PlayerPiece>().isAt(row, col))
            return SetOver(m_blocker.GetComponent<PlayerPiece>());

        return SetOver(null);
    }

    public void SetPlayerComplete()
    {
        if (m_gameOver)
            return;

        if (m_missleActive)
            MoveMissile();
        else if (m_launcherActive)
            LaunchMissile();
        else
            MakeLauncherAppear();
    }

    public void SetOpponentComplete()
    {
        m_opponentTurn = false;
    }

    private void MoveMissile()
    {
        m_opponentTurn = true;
        m_missile.GetComponent<MissilePiece>().Advance();
    }

    private void LaunchMissile()
    {
        m_opponentTurn = true;
        m_missile.transform.position = m_launcher.transform.position;
        m_missile.GetComponent<Piece>().SetPos(m_launcher.GetComponent<LauncherPiece>().row, m_launcher.GetComponent<LauncherPiece>().col);
        m_missile.SetActive(true);
        m_missile.GetComponent<MissilePiece>().Advance();

        m_launcher.SetActive(false);
        m_missleActive = true;
        m_launcherActive = false;
    }

    private void MakeLauncherAppear()
    {
        int col = Random.Range(0, numCols);
        if (IsOccupied(0, col))
            return;

        m_launcher.GetComponent<LauncherPiece>().SetPresent(0, col);
        SetOpponentComplete();
        m_opponentTurn = true;
        m_launcherActive = true;
    }

    private void SetGameOver()
    {
        m_gameOver = true;
        m_gameOverText.SetActive(true);
        m_replayButton.SetActive(true);
    }

    private void SetYouWon()
    {
        m_gameOver = true;
        m_youWonText.SetActive(true);
        m_replayButton.SetActive(true);
    }

    public void TestForImpact()
    {
        if (m_sphere.GetComponent<Piece>().row == 0)
        {
            SetYouWon();
        }

        if (m_missleActive)
        {
            int mrow = m_missile.GetComponent<Piece>().row;
            int mcol = m_missile.GetComponent<Piece>().col;

            if (m_sphere.GetComponent<PlayerPiece>().isAt(mrow, mcol))
            {
                m_missile.SetActive(false);
                m_missile.GetComponent<MissilePiece>().Explode();
                m_sphere.SetActive(false);
                SetGameOver();
            }
            if (m_breaker1.GetComponent<PlayerPiece>().isAt(mrow, mcol))
            {
                m_breaker1.GetComponent<Piece>().SetPos(-1, -1);
                m_breaker1.SetActive(false);
                m_missile.GetComponent<MissilePiece>().Explode();
                m_missile.SetActive(false);
                m_missleActive = false;
            }
            if (m_breaker2.GetComponent<PlayerPiece>().isAt(mrow, mcol))
            {
                m_breaker2.GetComponent<Piece>().SetPos(-1, -1);
                m_breaker2.SetActive(false);
                m_missile.GetComponent<MissilePiece>().Explode();
                m_missile.SetActive(false);
                m_missleActive = false;
            }
            if (m_blocker.GetComponent<PlayerPiece>().isAt(mrow, mcol))
            {
                m_missile.GetComponent<MissilePiece>().Explode();
                m_missile.SetActive(false);
                m_missleActive = false;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public bool IsPlayerTurn()
    {
        return !m_gameOver && !m_opponentTurn;
    }

    public void MovePiece(int row, int col)
    {
        m_selectedPiece.MoveTo(row, col);
    }

    public bool CanMoveTo(int row, int col)
    {
        if (m_selectedPiece != null)
        {
            if (m_selectedPiece.IsMoving())
                return false;

            if (m_selectedPiece.CanMove(row, col))
            {
                if (IsOccupied(row, col))
                    return false;

                if (m_holes.Contains(row * numCols + col))
                    return false;

                return true;
            }

            return false;
        }

        return false;
    }

    public PlayerPiece SetOver(PlayerPiece piece)
    {
        if (m_overPiece != null)
        {
            if (m_overPiece != m_selectedPiece)
                m_overPiece.ClearHelpTip();
        }

        m_overPiece = piece;

        if (piece != null)
            if (m_overPiece != m_selectedPiece)
                m_overPiece.OverHelpTip();

        return piece;
    }

    public void SetSelected(PlayerPiece piece)
    {
        if (m_selectedPiece != null)
        {
            if (m_overPiece == m_selectedPiece)
                m_selectedPiece.OverHelpTip();
            else
                m_selectedPiece.ClearHelpTip();
        }
        m_selectedPiece = piece;
        m_selectedPiece.SetHelpTip();
    }
}
