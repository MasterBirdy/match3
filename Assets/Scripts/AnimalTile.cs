using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalTile : MonoBehaviour
{
    [Header("Board Variables")]
    public int column;
    public int row;
    public int previousColumn;
    public int previousRow;
    public float targetX;
    public float targetY;
    public float previousTargetX;
    public float previousTargetY;
    public bool isMatched = false;
    private Board board;
    public GameObject otherAnimal;

    [Header("Swipe Variables")]
    public float swipeAngle = 0;
    public float swipeResist = 0f;
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;


    public string[] s;
    private bool mouseDown = false;
    private FindMatches findMatches;

    [Header("Powerups")]
    public bool isColumnBomb;
    public bool isRowBomb;
    [SerializeField] public Sprite columnBomb;
    [SerializeField] public Sprite rowBomb;

    // Start is called before the first frame update
    void Start()
    {
        findMatches = FindObjectOfType<FindMatches>();
        swipeResist = 0.3f;
        board = FindObjectOfType<Board>();
        targetX = transform.position.x;
        targetY = transform.position.y;
        previousRow = row;
        previousColumn = column;
        previousTargetX = targetX;
        previousTargetY = targetY;
        isColumnBomb = false;
        isRowBomb = false;

    }

    //debug features
    /*
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(board.allAnimals[column, row].GetComponent<AnimalTile>().tag +
            " " + board.allAnimals[column, row].GetComponent<AnimalTile>().column + " "
            + board.allAnimals[column, row].GetComponent<AnimalTile>().row);
        }
    }
   
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRowBomb = true;
            GetComponent<SpriteRenderer>().sprite = rowBomb;
            tag = "Bomb";
        }
    }
     */

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            FadeToBlack(mySprite);
        }
        if (isColumnBomb || isRowBomb)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            comeBack(mySprite);
        }
    }

    void FadeToBlack(SpriteRenderer s)
    {

        s.color = new Color(1, 1, 1, .5f);

    }

    private void comeBack(SpriteRenderer s)
    {
        s.color = new Color(1f, 1f, 1f, 1f);
    }

    private void MoveTowardsTarget()
    {
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //move towards the target
            Vector2 tempPosition = new Vector2(targetX, transform.position.y);
            if (Mathf.Abs(targetX - transform.position.x) > 1)
                transform.position = Vector2.Lerp(transform.position, tempPosition, .1f);
            else
                transform.position = Vector2.Lerp(transform.position, tempPosition, .15f);
            //findMatches.FindAllMatches();
        }
        else
        {
            // directly set position
            Vector2 tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allAnimals[column, row] = this.gameObject;

        }

        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //move towards the target
            Vector2 tempPosition = new Vector2(transform.position.x, targetY);
            if (Mathf.Abs(targetY - transform.position.y) > 1)
                transform.position = Vector2.Lerp(transform.position, tempPosition, .1f);
            else
                transform.position = Vector2.Lerp(transform.position, tempPosition, .15f);
            //findMatches.FindAllMatches();
        }
        else
        {
            // directly set position
            Vector2 tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allAnimals[column, row] = this.gameObject;
        }
        findMatches.FindAllMatches();
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.25f);
        if (otherAnimal != null)
        {
            if (isColumnBomb)
            {
                findMatches.ActivateColumnBomb(column);
                board.StartDestroyAllNow();
            }
            else if (isRowBomb)
            {
                findMatches.ActivateRowBomb(row);
                board.StartDestroyAllNow();
            }
            else if (!isMatched && !otherAnimal.GetComponent<AnimalTile>().isMatched)
            {
                otherAnimal.GetComponent<AnimalTile>().row = row;
                otherAnimal.GetComponent<AnimalTile>().column = column;
                otherAnimal.GetComponent<AnimalTile>().targetX = targetX;
                otherAnimal.GetComponent<AnimalTile>().targetY = targetY;
                board.allAnimals[column, row] = otherAnimal;
                row = previousRow;
                column = previousColumn;
                targetX = previousTargetX;
                targetY = previousTargetY;
                board.allAnimals[column, row] = gameObject;
                yield return new WaitForSeconds(.25f);
                board.currentAnimal = null;
                board.currentState = GameState.MOVE;
            }
           else
            {
                board.StartDestroyAllNow();
            }
            otherAnimal = null;
        }
    }

    private void OnMouseDown()
    {
        if (board.currentState == GameState.MOVE)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseDown = true;
        }
    }

    private void OnMouseUp()
    {
        if (board.currentState == GameState.MOVE && mouseDown)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
            mouseDown = false;
        }
    }

    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist ||
            Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
                finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            board.currentState = GameState.WAIT;
            board.currentAnimal = this;
            MovePieces();
        }
       // else
        //{
            // board.currentState = GameState.MOVE;
        //}
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width - 1)
        {
            //right swipe
            otherAnimal = board.allAnimals[column + 1, row];

            board.allAnimals[column, row] = otherAnimal;

            otherAnimal.GetComponent<AnimalTile>().column -= 1;
            previousColumn = column;
            previousRow = row;
            column += 1;

            board.allAnimals[column, row] = gameObject;
            Switcharoo(otherAnimal.GetComponent<AnimalTile>());
        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height - 1)
        {
            // up swipe
            otherAnimal = board.allAnimals[column, row + 1];

            board.allAnimals[column, row] = otherAnimal;

            otherAnimal.GetComponent<AnimalTile>().row -= 1;
            previousColumn = column;
            previousRow = row;
            row += 1;

            board.allAnimals[column, row] = gameObject;
            Switcharoo(otherAnimal.GetComponent<AnimalTile>());
        }
        else if ((swipeAngle > 135 || swipeAngle < -135) && column > 0)
        {    // left swipe
            otherAnimal = board.allAnimals[column - 1, row];

            board.allAnimals[column, row] = otherAnimal;

            otherAnimal.GetComponent<AnimalTile>().column += 1;
            previousColumn = column;
            previousRow = row;
            column -= 1;

            board.allAnimals[column, row] = gameObject;
            Switcharoo(otherAnimal.GetComponent<AnimalTile>());
        }
        else if (swipeAngle < -45 && swipeAngle >= -135 && row > 0)
        {
            // down swipe
            Debug.Log(board.allAnimals[column, row - 1].tag);

            otherAnimal = board.allAnimals[column, row - 1];

            board.allAnimals[column, row] = otherAnimal;

            otherAnimal.GetComponent<AnimalTile>().row += 1;
            previousColumn = column;
            previousRow = row;
            row -= 1;

            board.allAnimals[column, row] = gameObject;
            Switcharoo(otherAnimal.GetComponent<AnimalTile>());
        }
        Debug.Log(column + " " + row + "  vs. " + board.allAnimals[column, row].GetComponent<AnimalTile>().tag +
            " " + board.allAnimals[column, row].GetComponent<AnimalTile>().column + " " 
            + board.allAnimals[column, row].GetComponent<AnimalTile>().row);
        StartCoroutine(CheckMoveCo());
    }

    public void Switcharoo(AnimalTile a) {
        previousTargetX = targetX;
        previousTargetY = targetY;
        float tempX = a.targetX;
        float tempY = a.targetY;
        a.targetX = targetX;
        a.targetY = targetY;
        targetX = tempX;
        targetY = tempY;
    }

    public void MakeRowBomb()
    {
        isRowBomb = true;
        GetComponent<SpriteRenderer>().sprite = rowBomb;
        tag = "Bomb";
    }

    public void MakeColumnBomb()
    {
        isColumnBomb = true;
        GetComponent<SpriteRenderer>().sprite = columnBomb;
        tag = "Bomb";
    }


}
