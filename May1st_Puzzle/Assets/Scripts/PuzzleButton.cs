using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzleButton : MonoBehaviour, IPointerClickHandler
{
    private Vector3 originalPosition;
    private PuzzleManager puzzleManager;

    private bool isInteractable = true; // ��ư�� ��ȣ�ۿ� ���θ� �����ϴ� ����

    private void Start()
    {
        originalPosition = transform.position;
        puzzleManager = FindObjectOfType<PuzzleManager>();

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        puzzleManager.SelectButton(this);
    }

    public void ResetPosition()
    {
        transform.position = originalPosition;
    }

    public void SetInteractable(bool interactable)
    {
        isInteractable = interactable;
        this.gameObject.GetComponent<Button>().interactable = isInteractable;
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }
}
