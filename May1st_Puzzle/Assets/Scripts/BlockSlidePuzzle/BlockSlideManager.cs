using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSlideManager : MonoBehaviour
{
    [SerializeField] private Transform[] gridPositions; // 모든 그리드 위치
    [SerializeField] private Transform[] blockPositions; // 블록 위치
    [SerializeField] private Transform[] wallPositions; // 벽 위치
    [SerializeField] private Button[] gridButtons; // 목표 위치 버튼
    [SerializeField] private Transform[] goalPositions; // 목표 블록 위치(성공 조건)
    [SerializeField] private GameObject[] forAni;

    public Animator clearAnimator; // 퍼즐 성공 시 재생할 애니메이터

    private Transform selectedBlock = null;

    private void Start()
    {
        // 목표 위치 버튼에 클릭 이벤트 추가
        for (int i = 0; i < gridButtons.Length; i++)
        {
            int index = i;
            gridButtons[i].onClick.AddListener(() => OnGridButtonClicked(gridPositions[index]));
        }
    }

    // 블록 선택
    public void SelectBlock(Transform block)
    {
        if (selectedBlock == block)
        {
            selectedBlock = null;
            Debug.Log("블록 선택 해제");
        }
        else
        {
            selectedBlock = block;
            Debug.Log($"블록 선택됨: {selectedBlock.name}");
        }
    }

    // 목표 위치 클릭 시 실행
    private void OnGridButtonClicked(Transform target)
    {
        MoveBlock(target);
    }

    // 블록 이동
    public void MoveBlock(Transform target)
    {
        if (selectedBlock == null) return;

        // 목표 위치가 유효한지 확인
        if (!IsPositionAvailable(target) || IsBlockedAtPosition(target.position))
        {
            Debug.Log("이동 불가능한 위치!");
            return;
        }

        // 경로가 유효하면 이동
        if (CanMoveToTarget(selectedBlock.position, target.position))
        {
            selectedBlock.position = target.position;
            selectedBlock = null; // 이동 후 선택 해제
            Debug.Log("이동 성공!");

            if (CheckSuccessCondition())
            {
                Debug.Log("퍼즐 클리어!");
                forAni[0].SetActive(false);
                forAni[1].SetActive(false);
                forAni[2].SetActive(true);
                clearAnimator.SetTrigger("Open"); // 애니메이션 실행
            }
        }
        else
        {
            Debug.Log("이동 경로에 장애물이 있음!");
        }
    }

    // 목표 위치가 유효한지 확인
    private bool IsPositionAvailable(Transform target)
    {
        foreach (Transform pos in gridPositions)
        {
            if (ApproximatelyEqual(pos.position, target.position)) return true;
        }
        return false;
    }

    // 벽이 있는지 확인
    private bool CanMoveToTarget(Vector2 start, Vector2 target)
    {
        Vector2 direction = target - start;

        // 이동이 가능한 경우 (가로 또는 세로 이동만 가능, 대각선 이동 불가)
        if (direction.x != 0 && direction.y != 0) return false;

        // 이동 방향 설정 (단위 벡터)
        Vector2 step = direction.normalized;

        // 현재 위치에서 목표 위치까지 한 칸씩 확인
        Vector2 current = start;
        while (!ApproximatelyEqual(current, target))
        {
            current += step;

            // 변경: 직접 벡터 값을 사용해서 벽이 있는지 체크
            if (IsBlockedAtPosition(current))
            {
                return false;
            }
        }
        return true;
    }

    // 벽이 특정 위치에 있는지 확인
    private bool IsBlockedAtPosition(Vector2 position)
    {
        foreach (Transform wall in wallPositions)
        {
            if (ApproximatelyEqual(wall.position, position)) return true;
        }
        return false;
    }


    // 좌표 비교 (정확도 보정)
    private bool ApproximatelyEqual(Vector2 a, Vector2 b, float tolerance = 0.1f)
    {
        return Mathf.Abs(a.x - b.x) < tolerance && Mathf.Abs(a.y - b.y) < tolerance;
    }

    private bool CheckSuccessCondition()
    {
        for (int i = 0; i < goalPositions.Length; i++)
        {
            if (blockPositions[i].position != goalPositions[i].position)
            {
                return false;
            }
        }
        return true;
    }
}
