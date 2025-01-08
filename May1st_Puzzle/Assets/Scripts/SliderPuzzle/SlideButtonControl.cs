using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SlideButtonControl : MonoBehaviour
{
    [SerializeField]
    public Image[] points;         // �� �̹��� �迭

    public GameObject blockTop;    // ���� ���
    public GameObject blockBottom; // �Ʒ��� ���
    public float moveSpeed = 5f; // �̵� �ӵ�
    public float curveHeight = 2f; // � ���� (Sin �Լ��� ����)

    private Button selectedBlock;  // ���õ� ���
    private Color normalColor = Color.blue; 
    private Color selectedColor = Color.red; // ���õ� ����
    private bool isMoving = false; // ���� �̵� ������ Ȯ��
    private Vector3 targetPosition; // �̵� ��ǥ ��ġ

    void Start()
    {
        // �� ��Ͽ� ��ư �̺�Ʈ ���
        blockTop.GetComponent<Button>().onClick.AddListener(() => SelectBlock(blockTop.GetComponent<Button>()));
        blockBottom.GetComponent<Button>().onClick.AddListener(() => SelectBlock(blockBottom.GetComponent<Button>()));

        // �� ���� Ŭ�� �̺�Ʈ ���
        foreach (Image point in points)
        {
            point.GetComponent<Button>().onClick.AddListener(() => MoveSelectedBlock(point.transform.position));
        }
    }

    // ��� ���� ����
    void SelectBlock(Button blockButton)
    {
        if (selectedBlock != null)
        {
            selectedBlock.GetComponent<Image>().color = normalColor; // ���� ����� ���� �ʱ�ȭ
        }

        selectedBlock = blockButton;
        selectedBlock.GetComponent<Image>().color = selectedColor; // ���õ� ��� ���� ����
    }

    // ���õ� ����� Ư�� ��ġ�� � �̵�
    void MoveSelectedBlock(Vector3 target)
    {
        if (selectedBlock != null && !isMoving)
        {
            StartCoroutine(MoveAlongSineCurve(selectedBlock.gameObject, target));
        }
    }

    // �ڷ�ƾ: ����� Sin ����� �̵�
    IEnumerator MoveAlongSineCurve(GameObject block, Vector3 target)
    {
        isMoving = true;

        Vector3 startPoint = block.transform.position; // ������
        float distance = Vector3.Distance(startPoint, target); // �� �� ���� �Ÿ�
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveSpeed; // �̵� �ӵ� ����
            // ���� ��� ���
            Vector3 linearPosition = Vector3.Lerp(startPoint, target, t);
            // � ���� �߰� (Sin �Լ� ���)
            float sineOffset = Mathf.Sin(t * Mathf.PI) * curveHeight; // Sin ����� ���� ����
            linearPosition.y += sineOffset; // y�࿡ � ������ �߰�
            // ��� ��ġ ������Ʈ
            block.transform.position = linearPosition;

            yield return null;
        }

        block.transform.position = target; // ���� ��ġ ����
        isMoving = false;
        DeselectBlock(); // ��� ���� �� ���� �ʱ�ȭ
    }

    // ��� ���� �ʱ�ȭ
    void DeselectBlock()
    {
        if (selectedBlock != null)
        {
            selectedBlock.GetComponent<Image>().color = normalColor; // ���õ� ����� ���� �ʱ�ȭ
            selectedBlock = null; // ���õ� ��� �ʱ�ȭ
        }
    }
}
