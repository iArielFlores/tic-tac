using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    public TMP_Text[] buttonList;
    private string playerSide;

    public GameObject gameOverPanel;
    public TMP_Text gameOverText;

    private int moveCount;

    public GameObject restartButton;

    public bool playerMove;
    private string computerSide;

    public float delay;
    private int value;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        SetGameControllerReference();
        playerSide = "X";
        moveCount = 0;
        restartButton.SetActive(false);
    }

    void SetGameControllerReference()
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);
        }
    }

    public string GetPlayerSide()
    {
        return playerSide;
    }

    public void EndTurn()
    {
        moveCount++;

        if (buttonList[0].text == playerSide && buttonList[1].text == playerSide && buttonList[2].text == playerSide)
        {
            GameOver(playerSide);
        }

        if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
        {
            GameOver(playerSide);
        }

        if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }

        if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }

        if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
        {
            GameOver(playerSide);
        }

        if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }

        if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        {
            GameOver(playerSide);
        }

        if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        {
            GameOver(playerSide);
        }

        if (moveCount >= 9)
        {
            GameOver("draw");
        }

        ChangeSide();
    }

    void GameOver(string winningPlayer)
    {
        SetBoardInteractable(false);

        if (winningPlayer == "draw")
        {
            SetGameOverText("It's a Draw!");
        }
        else
        {
            SetGameOverText(winningPlayer + " Wins!");
        }

        restartButton.SetActive(true);
    }

    void ChangeSide()
    {
        playerSide = (playerSide == "X") ? "O" : "X";
    }

    void SetGameOverText(string value)
    {
        gameOverPanel.SetActive(true);
        gameOverText.text = value;
    }

    public void RestartGame()
    {
        playerSide = "X";
        moveCount = 0;
        gameOverPanel.SetActive(false);

        SetBoardInteractable(true);

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].text = "";
        }

        restartButton.SetActive(false);
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    public void MakeOptimalMove()
    {
        int bestScore = int.MinValue; // Maximizer starts with negative infinity
        int bestMove = -1;

        for (int i = 0; i < buttonList.Length; i++)
        {
            if (buttonList[i].text == "") // Check if the spot is available
            {
                // Make a hypothetical move for the AI
                buttonList[i].text = playerSide;

                // Evaluate the move using Minimax
                int score = Minimax(buttonList, 0, false);

                // Undo the move
                buttonList[i].text = "";

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = i;
                }
            }
        }

        // Perform the best move
        if (bestMove != -1)
        {
            buttonList[bestMove].text = playerSide;
            EndTurn();
        }
    }

    int Minimax(TMP_Text[] board, int depth, bool isMaximizing)
    {
        string result = CheckWinner();
        if (result != null)
        {
            if (result == playerSide)
            {
                return 1; // AI wins
            }
            else if (result == GetOpponentSide())
            {
                return -1; // Player wins
            }
            return 0; // Draw
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i].text == "")
                {
                    board[i].text = playerSide;
                    int score = Minimax(board, depth + 1, false);
                    board[i].text = ""; // Undo move
                    bestScore = Mathf.Max(score, bestScore);
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i].text == "")
                {
                    board[i].text = GetOpponentSide();
                    int score = Minimax(board, depth + 1, true);
                    board[i].text = ""; // Undo move
                    bestScore = Mathf.Min(score, bestScore);
                }
            }
            return bestScore;
        }
    }

    string GetOpponentSide()
    {
        return (playerSide == "X") ? "O" : "X";
    }

    string CheckWinner()
    {
        // Horizontal
        if (buttonList[0].text == buttonList[1].text && buttonList[1].text == buttonList[2].text && buttonList[0].text != "")
            return buttonList[0].text;
        if (buttonList[3].text == buttonList[4].text && buttonList[4].text == buttonList[5].text && buttonList[3].text != "")
            return buttonList[3].text;
        if (buttonList[6].text == buttonList[7].text && buttonList[7].text == buttonList[8].text && buttonList[6].text != "")
            return buttonList[6].text;

        // Vertical
        if (buttonList[0].text == buttonList[3].text && buttonList[3].text == buttonList[6].text && buttonList[0].text != "")
            return buttonList[0].text;
        if (buttonList[1].text == buttonList[4].text && buttonList[4].text == buttonList[7].text && buttonList[1].text != "")
            return buttonList[1].text;
        if (buttonList[2].text == buttonList[5].text && buttonList[5].text == buttonList[8].text && buttonList[2].text != "")
            return buttonList[2].text;

        // Diagonal
        if (buttonList[0].text == buttonList[4].text && buttonList[4].text == buttonList[8].text && buttonList[0].text != "")
            return buttonList[0].text;
        if (buttonList[2].text == buttonList[4].text && buttonList[4].text == buttonList[6].text && buttonList[2].text != "")
            return buttonList[2].text;

        // Check for draw
        bool isDraw = true;
        for (int i = 0; i < buttonList.Length; i++)
        {
            if (buttonList[i].text == "")
            {
                isDraw = false;
                break;
            }
        }
        if (isDraw) return "draw";

        // No winner yet
        return null;
    }
}
