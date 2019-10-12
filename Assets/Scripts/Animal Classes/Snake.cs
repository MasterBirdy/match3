﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour, AnimalClass
{
    [SerializeField] GameObject findMatchesObject;
    [SerializeField] GameObject dataTrackerObject;
    [SerializeField] GameObject boardObject;
    FindMatches findMatches;
    DataTracker dataTracker;
    Board board;
    [SerializeField] Sprite sprite;
    void Start()
    {

    }
    public void ActivatePower()
    {
        if (board == null)
            board = FindObjectOfType<Board>();
        if (findMatches == null)
            findMatches = FindObjectOfType<FindMatches>();
        int j = Random.Range(0, 8);
        for (int i = 0; i < board.width; i++)
        {
            GameObject testGameObject = board.allAnimals[i, j];
            testGameObject.GetComponent<AnimalTile>().isMatched = true;
            findMatches.currentMatches.Add(testGameObject);
        }
        board.StartDestroyAllNow();
    }

    public bool HasTimeExtension()
    {
        return true;
    }

    public Sprite ReturnSprite()
    {
        return sprite;
    }

    // Update is called once per frame

    public string ReturnName()
    {
        return "Snake";
    }



}
