using System.Collections;
using UnityEngine;

//Code borrowed and modified from Mister Taft Creates https://www.youtube.com/playlist?list=PL4vbr3u7UKWrxEz75MqmTDd899cYAvQ_B

public class DotBehaviour : MonoBehaviour

{
    public Camera cam;
    
    public Vector3 firstTouchPosition;
    public Vector3 finalTouchPosition;
    public Vector3 tempPosition;
    
    public float swipeAngle;
    public float swipeResist = 1f;
    public float SwitchSpeed = .2f;
    
    public int column;
    public int row;

    public int previousColumn;
    public int previousRow;
    
    public int targetX;
    public int targetY;
    
    public bool isMatched = false;
    
    public bool isAdjacentBomb;
    public bool isColumnBomb;
    public bool isRowBomb;
    public bool isColorBomb;

    public GameObject rowBomb;
    public GameObject columnBomb;   
    public GameObject colorBomb;
    public GameObject comparedDot;
    public GameObject adjacentBomb;
    
    private BoardBehaviour board;
    private MatchingBehaviour findMatches;
    private SoundManager soundManager;

    
    private void Start()
    {
        //Testing Function
        isColumnBomb = false;
        isRowBomb = false;
        isAdjacentBomb = false;
        isColorBomb = false;
        //Testing Function
        
        cam = Camera.main;
        
        board = FindObjectOfType<BoardBehaviour>();
        findMatches = FindObjectOfType<MatchingBehaviour>();
        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
        //row = targetY;
        //column = targetX;
        //previousRow = row;
        //previousColumn = column;
    }

    //Testing Function
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isColorBomb = true;
            GameObject marker = Instantiate(colorBomb, transform.position, Quaternion.identity);
            marker.transform.parent = this.transform;
        }
    }
    //Testing Function
    
    private void Update()
    {
        /*
        if (isMatched)
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            sprite.color = new Color(0f, 0f, 0f, .2f);
        }
        */
        targetX = column;
        targetY = row;
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //move toward target
            tempPosition = new Vector3(targetX, transform.position.y);
            transform.position = Vector3.Lerp(transform.position, tempPosition, SwitchSpeed);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            //directly set position
            tempPosition = new Vector3(targetX, transform.position.y);
            transform.position = tempPosition;
        }
        
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //move toward target
            tempPosition = new Vector3(transform.position.x, targetY);
            transform.position = Vector3.Lerp(transform.position, tempPosition, SwitchSpeed);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatches.FindAllMatches();
        }
        else
        {
            //directly set position
            tempPosition = new Vector3(transform.position.x, targetY);
            transform.position = tempPosition;
        }
    }

    public IEnumerator CheckMove()
    {
        if (isColorBomb)
        {
            findMatches.MatchColorPieces(comparedDot.tag);
            isMatched = true;
        }
        else if (comparedDot.GetComponent<DotBehaviour>().isColumnBomb)
        {
            findMatches.MatchColorPieces(this.gameObject.tag);
            comparedDot.GetComponent<DotBehaviour>().isMatched = true;
        }

        yield return new WaitForSeconds(.3f);
        if (comparedDot != null)
        {
            if (!isMatched && !comparedDot.GetComponent<DotBehaviour>().isMatched)
            {
                comparedDot.GetComponent<DotBehaviour>().row = row;
                comparedDot.GetComponent<DotBehaviour>().column = column;
                row = previousRow;
                column = previousColumn;
                yield return new WaitForSeconds(.25f);
                board.currentDot= null;
                board.currentState = GamesState.move;
            }
            else
            {
                board.DestroyMatches();
                board.currentState = GamesState.move;
            }
            //comparedDot = null;
        }
    }

    public void OnMouseDown()
    {
        if (board.currentState == GamesState.move)
        { 
            firstTouchPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
        }
    }
    
    public void OnMouseUp()
    {
        if (board.currentState == GamesState.move)
        {
            finalTouchPosition = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
            CalcuateAngle();
        }
        else
        {
            board.currentState = GamesState.move;
        }
    }
    
     void CalcuateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist);
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MoveObjects();
            board.currentState = GamesState.wait;
            board.currentDot = this;
        }
    }

    void MoveObjects()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //Right swipe
            comparedDot = board.allDots[column + 1, row];
            previousRow = row;
            previousColumn = column;
            comparedDot.GetComponent<DotBehaviour>().column -= 1;
            column += 1;
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            //Up swipe
            comparedDot = board.allDots[column, row + 1];
            previousRow = row;
            previousColumn = column;
            comparedDot.GetComponent<DotBehaviour>().row -= 1;
            row += 1;
        }
        else if ((swipeAngle > 135  || swipeAngle <= -135) && column > 0)
        {
            //Left swipe
            comparedDot = board.allDots[column - 1, row];
            previousRow = row;
            previousColumn = column;
            comparedDot.GetComponent<DotBehaviour>().column += 1;
            column -= 1;
        }   
        else if ((swipeAngle < -45 && swipeAngle >= -135) && row > 0)
        {
            //Down swipe
            comparedDot = board.allDots[column, row - 1];
            previousRow = row;
            previousColumn = column;
            comparedDot.GetComponent<DotBehaviour>().row += 1;
            row -= 1;
        }
        StartCoroutine(CheckMove());
    }

    public void CheckMatch()
    {
        if (column > 0 && column < board.width - 1)
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];

            if (leftDot1 != null && rightDot1 != null)
            {
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<DotBehaviour>().isMatched = true;
                    rightDot1.GetComponent<DotBehaviour>().isMatched = true;
                    isMatched = true;
                }
            }
        }
        
        if (row > 0 && row < board.height - 1)
        {
            GameObject upDot1 = board.allDots[column, row + 1];
            GameObject downDot1 = board.allDots[column, row - 1];
            if (upDot1 != null && downDot1 != null)
            {
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<DotBehaviour>().isMatched = true;
                    downDot1.GetComponent<DotBehaviour>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
    
    public void MakeRowBomb()
    {
        {
            isRowBomb = true;
            GameObject bomb = Instantiate(rowBomb, transform.position, Quaternion.identity);
            bomb.transform.parent = this.transform;
            //this.gameObject.tag = "RowBomb";
        }
    }
    
    public void MakeColumnBomb()
    {
        {
            isColumnBomb = true;
            GameObject bomb = Instantiate(columnBomb, transform.position, Quaternion.identity);
            bomb.transform.parent = this.transform;
            //this.gameObject.tag = "ColumnBomb";
        }
    }
}
