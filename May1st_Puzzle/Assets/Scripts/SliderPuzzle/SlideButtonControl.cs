using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SlideButtonControl : MonoBehaviour
{
    [SerializeField]
    public Transform[] points;          // �������� ���� (�̹����� ǥ��)
    public Button[] blocks;             // ��� ��ư (�� 2��)

    public Transform centerPoint;       // �߽���
    public Color defaultColor = Color.blue;  // �⺻ ��� ����
    public Color selectedColor = Color.red;  // ���� �� �̵� �� ��� ����
    public float moveSpeed = 2f;        // �̵� �ӵ�
    public float curveRadiusMultiplier = 1.5f; // � ������ ����

    private Button selectedBlock = null; // ���� ���õ� ���
    private bool isMoving = false;       // ��� �̵� �� ����

    void Start()
    {
        foreach (var block in blocks)
        {
            SetBlockColor(block, defaultColor);
            block.onClick.AddListener(() => OnBlockSelected(block));
        }
    }

    public void OnBlockSelected(Button block)
    {
        if (isMoving) return;

        if (selectedBlock != null)
        {
            SetBlockColor(selectedBlock, defaultColor);
        }

        selectedBlock = block;
        SetBlockColor(selectedBlock, selectedColor);
    }

    public void OnPointClicked(Transform targetPoint)
    {
        if (selectedBlock == null || isMoving) return;

        Transform currentPoint = GetCurrentPoint(selectedBlock.transform);
        if (currentPoint != null)
        {
            if (ShouldMoveStraight(currentPoint, targetPoint))
            {
                StartCoroutine(MoveStraight(currentPoint, targetPoint));
            }
            else
            {
                StartCoroutine(MoveAlongCurve(currentPoint, targetPoint));
            }
        }
    }

    private Transform GetCurrentPoint(Transform blockTransform)
    {
        foreach (Transform point in points)
        {
            if (Vector3.Distance(blockTransform.position, point.position) < 0.1f)
            {
                return point;
            }
        }
        return null;
    }

    private bool ShouldMoveStraight(Transform from, Transform to)
    {
        return from == centerPoint || to == centerPoint ||
               Mathf.Approximately(from.position.x, to.position.x) ||
               Mathf.Approximately(from.position.y, to.position.y);
    }

    private void SetBlockColor(Button block, Color color)
    {
        ColorBlock colors = block.colors;
        colors.normalColor = color;
        block.colors = colors;
    }

    IEnumerator MoveStraight(Transform from, Transform to)
    {
        isMoving = true;

        float t = 0f;
        Vector3 startPoint = from.position;
        Vector3 endPoint = to.position;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            selectedBlock.transform.position = Vector3.Lerp(startPoint, endPoint, t);
            yield return null;
        }

        selectedBlock.transform.position = endPoint;
        SetBlockColor(selectedBlock, defaultColor);
        selectedBlock = null;
        isMoving = false;
    }

    IEnumerator MoveAlongCurve(Transform from, Transform to)
    {
        isMoving = true;

        float t = 0f;
        Vector3 startPoint = from.position;
        Vector3 endPoint = to.position;

        // � ������ ���
        Vector3 directionFromCenterToStart = (startPoint - centerPoint.position).normalized;
        Vector3 directionFromCenterToEnd = (endPoint - centerPoint.position).normalized;

        // �߽����� �������� �������� �� �ٱ������� ����
        Vector3 controlPoint = centerPoint.position +
                               (directionFromCenterToStart + directionFromCenterToEnd).normalized *
                               Vector3.Distance(centerPoint.position, startPoint) * curveRadiusMultiplier;

        // � ��� ����� ����
        Debug.DrawLine(startPoint, controlPoint, Color.green, 2f);
        Debug.DrawLine(controlPoint, endPoint, Color.green, 2f);

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;

            Vector3 m1 = Vector3.Lerp(startPoint, controlPoint, t);
            Vector3 m2 = Vector3.Lerp(controlPoint, endPoint, t);
            selectedBlock.transform.position = Vector3.Lerp(m1, m2, t);

            yield return null;
        }

        selectedBlock.transform.position = endPoint;
        SetBlockColor(selectedBlock, defaultColor);
        selectedBlock = null;
        isMoving = false;
    }
}
