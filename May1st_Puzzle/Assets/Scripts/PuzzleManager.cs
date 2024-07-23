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
        // �̹� ���õ� ��ư�� �ִ� ���
        if (selectedButton != null)
        {
            // ���� ��ư�� �������� Ŭ���� ��� ���� ����
            if (selectedButton == puzzleButton)
            {
                SetInadjacentBooks(true);
                selectedButton = null;
            }
            // ������ ��ư���� Ȯ�� �� ��ġ ��ȯ
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
            // ���õ� ��ư�� ���� ���
            selectedButton = puzzleButton;
            SetInadjacentBooks(false);
        }
    }

    //�ε����� 1�� ���� ������ Ȯ�� �Լ�
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

    //��ġ + �ε��� ���� �Լ�
    private void SwapButtons(PuzzleButton buttonA, PuzzleButton buttonB)
    {
        int indexA = puzzleButtons.IndexOf(buttonA);
        int indexB = puzzleButtons.IndexOf(buttonB);

        // �ڽ� ������Ʈ ������ �ٲ㼭 ��ġ ��ȯ
        // ��ư���� ��ġ�� ���̾��Ű �ε��� ��ȯ�̹Ƿ� �ڵ����� ����.
        buttonA.transform.SetSiblingIndex(indexB);
        buttonB.transform.SetSiblingIndex(indexA);

        //PuzzleBtton ����Ʈ �� �ε����� ��ȯ.
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
        // ���� ��ư ������ �������� Ȯ��
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
            Debug.Log("���� Ŭ����!");
            IsClear();
        }
    }
}
