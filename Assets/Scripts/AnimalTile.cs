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
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    public float swipeAngle = 0;
    public float swipeResist = 0f;
    public string[] s;
    // Start is called before the first frame update
    void Start()
    {
        swipeResist = 0.3f;
        board = FindObjectOfType<Board>();
        targetX = transform.position.x;
        targetY = transform.position.y;
        previousRow = row;
        previousColumn = column;
        previousTargetX = targetX;
        previousTargetY = targetY;

    }

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();
        FindMatches();
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0f, 0f, 0f);
        }
    }

    private void MoveTowardsTarget()
    {
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //move towards the target
            Vector2 tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = Vector2.Lerp(transform.position, tempPosition, .2f);
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
            transform.position = Vector2.Lerp(transform.position, tempPosition, .2f);
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
        yield return new WaitForSeconds(.3f);
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
            }
           else
            {
                board.DestroyMatches();
            }
            otherAnimal = null;
        }
    }

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(firstTouchPosition);
    }

    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist ||
            Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y,
                finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            Debug.Log(swipeAngle);
            MovePieces();
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
            GameObject leftAnimal1 = board.allAnimals[column - 1, row];
            GameObject rightAnimal1 = board.allAnimals[column + 1, row];
            if (leftAnimal1.tag == this.gameObject.tag && 
                rightAnimal1.tag == this.gameObject.tag && 
                leftAnimal1.tag == rightAnimal1.tag)
            {
                leftAnimal1.GetComponent<AnimalTile>().isMatched = true;
                rightAnimal1.GetComponent <AnimalTile>().isMatched = true;
                isMatched = true;
            }
        }

        if (row > 0 && row < board.height - 1)
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
