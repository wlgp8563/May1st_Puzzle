using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SlideButtonControl : MonoBehaviour
{
    [SerializeField]
    public Image[] points;         // 점 이미지 배열

    public GameObject blockTop;    // 위쪽 블록
    public GameObject blockBottom; // 아래쪽 블록
    public float moveSpeed = 5f; // 이동 속도
    public float curveHeight = 2f; // 곡선 높이 (Sin 함수로 조정)

    private Button selectedBlock;  // 선택된 블록
    private Color normalColor = Color.blue; 
    private Color selectedColor = Color.red; // 선택된 색상
    private bool isMoving = false; // 현재 이동 중인지 확인
    private Vector3 targetPosition; // 이동 목표 위치

    void Start()
    {
        // 각 블록에 버튼 이벤트 등록
        blockTop.GetComponent<Button>().onClick.AddListener(() => SelectBlock(blockTop.GetComponent<Button>()));
        blockBottom.GetComponent<Button>().onClick.AddListener(() => SelectBlock(blockBottom.GetComponent<Button>()));

        // 각 점에 클릭 이벤트 등록
        foreach (Image point in points)
        {
            point.GetComponent<Button>().onClick.AddListener(() => MoveSelectedBlock(point.transform.position));
        }
    }

    // 블록 선택 로직
    void SelectBlock(Button blockButton)
    {
        if (selectedBlock != null)
        {
            selectedBlock.GetComponent<Image>().color = normalColor; // 이전 블록의 색상 초기화
        }

        selectedBlock = blockButton;
        selectedBlock.GetComponent<Image>().color = selectedColor; // 선택된 블록 색상 변경
    }

    // 선택된 블록을 특정 위치로 곡선 이동
    void MoveSelectedBlock(Vector3 target)
    {
        if (selectedBlock != null && !isMoving)
        {
            StartCoroutine(MoveAlongSineCurve(selectedBlock.gameObject, target));
        }
    }

    // 코루틴: 블록을 Sin 곡선으로 이동
    IEnumerator MoveAlongSineCurve(GameObject block, Vector3 target)
    {
        isMoving = true;

        Vector3 startPoint = block.transform.position; // 시작점
        float distance = Vector3.Distance(startPoint, target); // 두 점 사이 거리
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveSpeed; // 이동 속도 조정
            // 직선 경로 계산
            Vector3 linearPosition = Vector3.Lerp(startPoint, target, t);
            // 곡선 높이 추가 (Sin 함수 기반)
            float sineOffset = Mathf.Sin(t * Mathf.PI) * curveHeight; // Sin 곡선으로 높이 조정
            linearPosition.y += sineOffset; // y축에 곡선 오프셋 추가
            // 블록 위치 업데이트
            block.transform.position = linearPosition;

            yield return null;
        }

        block.transform.position = target; // 최종 위치 보정
        isMoving = false;
        DeselectBlock(); // 블록 도착 후 선택 초기화
    }

    // 블록 선택 초기화
    void DeselectBlock()
    {
        if (selectedBlock != null)
        {
            selectedBlock.GetComponent<Image>().color = normalColor; // 선택된 블록의 색상 초기화
            selectedBlock = null; // 선택된 블록 초기화
        }
    }
}
