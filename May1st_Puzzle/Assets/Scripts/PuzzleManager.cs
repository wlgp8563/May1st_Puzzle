using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleManager : MonoBehaviour
{
    private PuzzleButton selectedButton;
    private List<PuzzleButton> puzzleButtons;

    void Start()
    {
        puzzleButtons = new List<PuzzleButton>(GetComponentsInChildren<PuzzleButton>());
    }

    public void SelectButton(PuzzleButton puzzleButton)
    {
        // 이미 선택된 버튼이 있는 경우
        if (selectedButton != null)
        {
            // 같은 버튼을 연속으로 클릭한 경우 선택 해제
            if (selectedButton == puzzleButton)
            {
                SetInadjacentBooks(true);
                selectedButton = null;
            }
            // 인접한 버튼인지 확인 후 위치 교환
            else if (IsAdjacent(selectedButton, puzzleButton))
            {
                SetInadjacentBooks(true);
                SwapButtons(selectedButton, puzzleButton);
                selectedButton = null;

                CheckForClear();
            }
        }
        else
        {
            // 선택된 버튼이 없는 경우
            selectedButton = puzzleButton;
            SetInadjacentBooks(false);
        }
    }

    //인덱스가 1개 차이 나는지 확인 함수
    private bool IsAdjacent(PuzzleButton buttonA, PuzzleButton buttonB)
    {
        int indexA = puzzleButtons.IndexOf(buttonA);
        int indexB = puzzleButtons.IndexOf(buttonB);

        if (Mathf.Abs(indexA - indexB) == 1)
        {
            return true;
        }
        else
            return false;
    }

    //위치 + 인덱스 스왑 함수
    private void SwapButtons(PuzzleButton buttonA, PuzzleButton buttonB)
    {
        int indexA = puzzleButtons.IndexOf(buttonA);
        int indexB = puzzleButtons.IndexOf(buttonB);

        // 자식 오브젝트 순서를 바꿔서 위치 교환
        // 버튼들의 위치가 하이어라키 인덱스 교환이므로 자동으로 조정.
        buttonA.transform.SetSiblingIndex(indexB);
        buttonB.transform.SetSiblingIndex(indexA);

        //PuzzleBtton 리스트 내 인덱스도 교환.
        PuzzleButton temp = puzzleButtons[indexA];
        puzzleButtons[indexA] = puzzleButtons[indexB];
        puzzleButtons[indexB] = temp;
    }

    private void SetInadjacentBooks(bool value)
    {
        int selectedIndex = puzzleButtons.IndexOf(selectedButton);
        foreach (var books in puzzleButtons)
        {
            int targetIndex = puzzleButtons.IndexOf(books);

            if(selectedIndex - 1 > targetIndex || selectedIndex + 1 < targetIndex)
            {
                books.SetInteractable(value);
            }
        }
    }

    private void IsClear()
    {
        foreach (var books in puzzleButtons)
        {
            books.SetInteractable(false);
        }
    }

    private void CheckForClear()
    {
        // 현재 버튼 순서가 정답인지 확인
        bool isClear = true;
        string[] answer = { "Book2", "Book4", "Book5", "Book6", "Book1", "Book7", "Book3" };

        for (int i = 0; i < puzzleButtons.Count; i++)
        {
            if (puzzleButtons[i].name != answer[i])
            {
                isClear = false;
                break;
            }
        }

        if (isClear)
        {
            Debug.Log("퍼즐 클리어!");
            IsClear();
        }
    }
}
