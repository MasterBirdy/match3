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
    private GameObject otherAnimal;

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

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isRowBomb = true;
            GetComponent<SpriteRenderer>().sprite = rowBomb;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
        //FindMatches();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            StartCoroutine(FadeToBlack(mySprite));
        }
    }

    IEnumerator FadeToBlack(SpriteRenderer s)
    {
        for (float i = 1f; i >= 0; i -= Time.deltaTime * 1.4f)
        {
            s.color = new Color(1, 1, 1, i);
            yield return null;
        }
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
            findMatches.FindAllMatches();
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
            findMatches.FindAllMatches();
        }
        else
        {
            // directly set position
            Vector2 tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allAnimals[column, row] = this.gameObject;
        }
    }

    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.25f);
        if (otherAnimal != null)
        {
            if (!isMatched && !otherAnimal.GetComponent<AnimalTile>().isMatched)
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
            MovePieces();
        }
        else
        {
            // board.currentState = GameState.MOVE;
        }
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
            previousRow = row;
            row -= 1;

            board.allAnimals[column, row] = gameObject;
            Switcharoo(otherAnimal.GetComponent<AnimalTile>());
        }
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

    void FindMatches()
    {
        if (column > 0 && column < board.width - 1)
        {
            if (board.allAnimals[column - 1, row] != null && board.allAnimals[column + 1, row] != null && board.allAnimals[column, row] != null)
            {
                GameObject leftAnimal1 = board.allAnimals[column - 1, row];
                GameObject rightAnimal1 = board.allAnimals[column + 1, row];
                if (leftAnimal1.tag == this.gameObject.tag &&
                    rightAnimal1.tag == this.gameObject.tag &&
                    leftAnimal1.tag == rightAnimal1.tag)
                {
                    leftAnimal1.GetComponent<AnimalTile>().isMatched = true;
                    rightAnimal1.GetComponent<AnimalTile>().isMatched = true;
                    isMatched = true;
                }
            }

        }

        if (row > 0 && row < board.height - 1)
        {
            if (board.allAnimals[column , row-1] != null && board.allAnimals[column, row] != null && board.allAnimals[column, row +1] != null)
            {
                GameObject downAnimal1 = board.allAnimals[column, row - 1];
                GameObject upAnimal1 = board.allAnimals[column, row + 1];
                if (upAnimal1.tag == this.gameObject.tag &&
                    downAnimal1.tag == this.gameObject.tag &&
                    upAnimal1.tag == downAnimal1.tag)
                {
                    upAnimal1.GetComponent<AnimalTile>().isMatched = true;
                    downAnimal1.GetComponent<AnimalTile>().isMatched = true;
                    isMatched = true;
                }
            }
        }

    }

}
