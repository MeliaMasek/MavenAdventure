using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//Code borrowed and modified from Mister Taft Creates https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B

public enum GamesState
{
    wait,
    move
}

public enum TileKind
{
    Breakable,
    Blank,
    Normal
}

[System.Serializable]
public class TileType
{
    public int x;
    public int y;
    public TileKind tileKind;
}
public class BoardBehaviour : MonoBehaviour
{
    public int width;
    public int height;
    public int offset;

    public IntData scoreData;

    private bool[,] blankSpaces;
    
    public GameObject tilePrefab;
    public GameObject[] dots;
    public GameObject[,] allDots;
    
    public GamesState currentState = GamesState.move;
    
    private MatchingBehaviour findMatches;

    private SoundManager soundManager;
    
    public DotBehaviour currentDot;
    
    public TileType[] boardLayout;  
    
    public GameObject shuffleMenu;
    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        findMatches = FindObjectOfType<MatchingBehaviour>();
        blankSpaces = new bool[width, height];
        allDots = new GameObject[width, height];
        SetUp();
    }

    public void GenerateBlankspaces()
    {
        for (int i = 0; i < boardLayout.Length; i++)
        {
            if (boardLayout[i].tileKind == TileKind.Blank)
            {
                blankSpaces[boardLayout[i].x, boardLayout[i].y] = true;
            }
        }   
    }
    
    public void SetUp()
    {
        GenerateBlankspaces();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (!blankSpaces[i, j])
                {
                    Vector3 tempPosition = new Vector3(i, j + offset, 0);
                    GameObject backgroundTile =
                        Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                    backgroundTile.transform.parent = this.transform;
                    backgroundTile.name = "(" + i + "," + j + ")";

                    int usableDots = Random.Range(0, dots.Length);
                    int maxIterations = 0;

                    while (MatchesAt(i, j, dots[usableDots]) && maxIterations < 100)
                    {
                        usableDots = Random.Range(0, dots.Length);
                        maxIterations++;
                    }

                    maxIterations = 0;

                    GameObject dot = Instantiate(dots[usableDots], tempPosition, Quaternion.identity);
                    dot.GetComponent<DotBehaviour>().row = j;
                    dot.GetComponent<DotBehaviour>().column = i;
                    dot.transform.parent = this.transform;
                    dot.name = "(" + i + "," + j + ")";
                    allDots[i, j] = dot;
                }
            }
        }
    }

    public void ResetBoard()
    {
        // Clear all dots on the board
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    Destroy(allDots[i, j]);
                    allDots[i, j] = null;
                }
            }
        }
    }

    private bool MatchesAt(int column, int row, GameObject obj)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
            {
                if (allDots[column - 1, row].tag == obj.tag && allDots[column - 2, row].tag == obj.tag)
                {
                    return true;
                }
            }

            if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
            {
                if (allDots[column, row - 1].tag == obj.tag && allDots[column, row - 2].tag == obj.tag)
                {
                    return true;
                }
            }
        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1] != null && allDots[column, row - 2] != null)
                {
                    if (allDots[column, row - 1].tag == obj.tag && allDots[column, row - 2].tag == obj.tag)
                    {
                        return true;
                    }
                }
            }

            if (column > 1)
            {
                if (allDots[column - 1, row] != null && allDots[column - 2, row] != null)
                {
                    if (allDots[column - 1, row].tag == obj.tag && allDots[column - 2, row].tag == obj.tag)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<DotBehaviour>().isMatched)
        {
            if (findMatches.currentMatches.Count == 4 || findMatches.currentMatches.Count == 7)
            {
                findMatches.CheckBombs();
            }
            
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
        soundManager.PlayMatchingSound();
    }

    public void DestroyMatches()
    {
        int scoreToAdd = 1;

        bool scoreAdded = false;
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                    
                    if (!scoreAdded) // Add score only once per match.
                    {
                        scoreData.value += scoreToAdd; // Increment the score.
                        scoreAdded = true;
                    }
                }
            }
        }
        findMatches.currentMatches.Clear();
        StartCoroutine(DecreaseRow());
    }

    private IEnumerator DecreaseRow()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    allDots[i, j].GetComponent<DotBehaviour>().row -= nullCount;
                    allDots[i, j] = null;
                }
            }

            nullCount = 0;
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(FillBoard());
    }

    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] == null)
                {
                    Vector3 tempPosition = new Vector3(i, j + offset, 0);
                    int usableDots = Random.Range(0, dots.Length);
                    GameObject dot = Instantiate(dots[usableDots], tempPosition, Quaternion.identity);
                    allDots[i, j] = dot;
                    dot.GetComponent<DotBehaviour>().row = j;
                    dot.GetComponent<DotBehaviour>().column = i;

                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (allDots[i, j].GetComponent<DotBehaviour>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private IEnumerator FillBoard()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        findMatches.currentMatches.Clear();
        currentDot = null;
        yield return new WaitForSeconds(.25f);

        if (IsDeadlocked())
        {
            //Shuffle();
            shuffleMenu.SetActive(true);
            Debug.Log("Deadlocked");
        }
        currentState = GamesState.move;
    }

    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;
        allDots[column + (int) direction.x, row + (int) direction.y] = allDots[column, row];
        allDots[column, row] = holder;
    }

    public bool CheckforMatches()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < width - 2)
                    {
                        if (allDots[i + 1, j] != null && allDots[i + 2, j] != null)
                        {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag &&
                                allDots[i + 2, j].tag == allDots[i, j].tag)
                            {
                                //right matches
                                return true;
                            }
                        }
                    }

                    if (j < height - 2)
                    {
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null)
                        {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag &&
                                allDots[i, j + 2].tag == allDots[i, j].tag)
                            {
                                //up matches
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    
    private bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        SwitchPieces(column, row, direction);
        if (CheckforMatches())
        {
            SwitchPieces(column, row, direction);
            return true;
        }
        SwitchPieces(column, row, direction);
        return false;
    }
    
    public bool IsDeadlocked()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            { 
                if (allDots[i, j] != null)
                {
                    if (i < width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }

                    if (j < height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public void Shuffle()
    {
        List<GameObject> newBoard = new List<GameObject>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    newBoard.Add(allDots[i, j]);
                }
            }
        }
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (blankSpaces[i,j])
                {
                    int pieceToUse = Random.Range(0, newBoard.Count);
                    int maxIterations = 0;

                    while (MatchesAt(i, j, newBoard[pieceToUse]) && maxIterations < 100)
                    {
                        pieceToUse = Random.Range(0, newBoard.Count);
                        maxIterations++;
                        Debug.Log(maxIterations);
                    }
                   
                    DotBehaviour piece = newBoard[pieceToUse].GetComponent<DotBehaviour>();
                    maxIterations = 0;
                    piece.column = i;
                    piece.row = j;
                    allDots[i,j] = newBoard[pieceToUse];
                    newBoard.Remove(newBoard[pieceToUse]);
                }
            }
        }

        //if (IsDeadlocked())
        //{
            //Shuffle();
        //}
    }
}