using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SlideButtonControl : MonoBehaviour
{
    [SerializeField]
    public Transform[] points;          // 동서남북 점들 (이미지로 표시)
    public Button[] blocks;             // 블록 버튼 (총 2개)

    public Transform centerPoint;       // 중심점
    public Color defaultColor = Color.blue;  // 기본 블록 색상
    public Color selectedColor = Color.red;  // 선택 및 이동 중 블록 색상
    public float moveSpeed = 2f;        // 이동 속도
    public float curveRadiusMultiplier = 1.5f; // 곡선 반지름 배율

    private Button selectedBlock = null; // 현재 선택된 블록
    private bool isMoving = false;       // 블록 이동 중 여부

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

        // 곡선 제어점 계산
        Vector3 directionFromCenterToStart = (startPoint - centerPoint.position).normalized;
        Vector3 directionFromCenterToEnd = (endPoint - centerPoint.position).normalized;

        // 중심점을 기준으로 제어점을 더 바깥쪽으로 설정
        Vector3 controlPoint = centerPoint.position +
                               (directionFromCenterToStart + directionFromCenterToEnd).normalized *
                               Vector3.Distance(centerPoint.position, startPoint) * curveRadiusMultiplier;

        // 곡선 경로 디버깅 라인
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
