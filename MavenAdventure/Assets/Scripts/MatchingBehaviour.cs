using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Code borrowed and modified from Mister Taft Creates https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B

public class MatchingBehaviour : MonoBehaviour
{
    private BoardBehaviour board;

    public List<GameObject> currentMatches = new List<GameObject>();

    private void Start()
    {
        board = FindObjectOfType<BoardBehaviour>();
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private List<GameObject> isAdjacentBomb(DotBehaviour dot1, DotBehaviour dot2, DotBehaviour dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot1.column, dot1.row));
        }

        if (dot2.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot2.column, dot2.row));
        }

        if (dot3.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot3.column, dot3.row));
        }

        return currentDots;
    }

    private List<GameObject> isRowbomb(DotBehaviour dot1, DotBehaviour dot2, DotBehaviour dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot1.row));
        }

        if (dot2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot2.row));
        }

        if (dot3.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot3.row));
        }

        return currentDots;
    }

    private List<GameObject> isColumnbomb(DotBehaviour dot1, DotBehaviour dot2, DotBehaviour dot3)
    {
        List<GameObject> currentDots = new List<GameObject>();
        if (dot1.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot1.column));
        }

        if (dot2.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot2.column));
        }

        if (dot3.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot3.column));
        }

        return currentDots;
    }
    private void AddtoListAndMatch(GameObject dot)
    {
        if (!currentMatches.Contains(dot))
        {
            currentMatches.Add(dot);
        }
        dot.GetComponent<DotBehaviour>().isMatched = true;
    }
    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddtoListAndMatch(dot1);
        AddtoListAndMatch(dot2);
        AddtoListAndMatch(dot3);
    }
    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                GameObject currentDot = board.allDots[i, j];
                if (currentDot != null)
                {
                    DotBehaviour currentDotDot = currentDot.GetComponent<DotBehaviour>();

                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if (leftDot != null && rightDot != null)
                        {
                            DotBehaviour leftDotDot = leftDot.GetComponent<DotBehaviour>();
                            DotBehaviour rightDotDot = rightDot.GetComponent<DotBehaviour>();


                            if (leftDot != null && rightDot != null)
                            {
                                if (leftDot.tag == currentDot.tag && rightDot.tag == currentDot.tag)
                                {
                                    currentMatches.Union(isRowbomb(leftDotDot, currentDotDot, rightDotDot));
                                    currentMatches.Union(isColumnbomb(leftDotDot, currentDotDot, rightDotDot));
                                    currentMatches.Union(isAdjacentBomb(leftDotDot, currentDotDot, rightDotDot));

                                    GetNearbyPieces(leftDot, currentDot, rightDot);
                                }
                            }
                        }
                    }

                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];

                        if (upDot != null && downDot != null)
                        {
                            DotBehaviour UpDotDot = upDot.GetComponent<DotBehaviour>();
                            DotBehaviour DownDotDot = downDot.GetComponent<DotBehaviour>();

                            if (upDot != null && downDot != null)
                            {
                                if (upDot.tag == currentDot.tag && downDot.tag == currentDot.tag)
                                {
                                    currentMatches.Union(isColumnbomb(UpDotDot, currentDotDot, DownDotDot));
                                    currentMatches.Union(isRowbomb(UpDotDot, currentDotDot, DownDotDot));
                                    currentMatches.Union(isAdjacentBomb(UpDotDot, currentDotDot, DownDotDot));
                                    
                                    GetNearbyPieces(upDot, currentDot, downDot);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    
    List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.height; i++)
        {
            if (board.allDots[column, i] != null)
            {
                dots.Add(board.allDots[column, i]);
                board.allDots[column, i].GetComponent<DotBehaviour>().isMatched = true;
            }
        }
        return dots;
    }
    
    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = 0; i < board.width; i++)
        {
            if (board.allDots[i, row] != null)
            {
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<DotBehaviour>().isMatched = true;
            }
        }
        return dots;
    }

    public void CheckBombs()
    {
        if (board.currentDot != null)
        {
            if (board.currentDot.isMatched)
            {
                board.currentDot.isMatched = false;
                int typeOfBomb = Random.Range(0, 100);
                if (typeOfBomb < 50)
                {
                    board.currentDot.MakeRowBomb();
                }
                else if (typeOfBomb >= 50)
                {
                    board.currentDot.MakeColumnBomb();
                }
            }
            else if (board.currentDot.comparedDot != null)
            {
                DotBehaviour otherDot = board.currentDot.comparedDot.GetComponent<DotBehaviour>();
                if (otherDot.isMatched)
                {
                    otherDot.isMatched = false;
                    int typeOfBomb = Random.Range(0, 100);
                    if (typeOfBomb < 50)
                    {
                        otherDot.MakeRowBomb();
                    }
                    else if (typeOfBomb >= 50)
                    {
                        otherDot.MakeColumnBomb();
                    }
                }
            }
        }
    }

    public void MatchColorPieces(string color)
    {
        for (int i = 0; i < board.width; i++)
        {
            for (int j = 0; j < board.height; j++)
            {
                if (board.allDots[i, j] != null)
                {
                    if (board.allDots[i, j].tag == color)
                    {
                        board.allDots[i, j].GetComponent<DotBehaviour>().isMatched = true;
                    }
                }
            }
        }
    }

    List<GameObject> GetAdjacentPieces(int col, int row)
    {
        List<GameObject> dots = new List<GameObject>();
        for (int i = col -1; i <= col + 1; i++)
        {
            for(int j = row -1; j <= row + 1; j++)
            {
                if (i >= 0 && i < board.width && j >= 0 && j < board.height)
                {
                    if (board.allDots[i, j] != null)
                    {
                        dots.Add(board.allDots[i, j]);
                        board.allDots[i, j].GetComponent<DotBehaviour>().isMatched = true;
                    }
                }
            }
        }
        return dots;
    }
}
