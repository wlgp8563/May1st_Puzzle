using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class TileManager : MonoBehaviour
{
    public List<Tile.GridPos> selectedList = new List<Tile.GridPos>();
    public LineRenderer lr;

    Image tileImage;
    Color tileColor;
    public List<GameObject> allTilesList = new List<GameObject>();  //ó�� ���� �� Ÿ�ϵ� �� �޾ƿ���

    
    void Start()
    {
        for(int i = 0; i < 64; i++)
        {
            allTilesList.Add(GameObject.Find("Back").transform.GetChild(i).gameObject);
            allTilesList[i].GetComponent<Tile>().gridPos.SetPos((i % 8), (i / 8));
            SetAllTileGray();
        }

        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material.color = Color.blue;
    }
    
    public void AddTileGridPos(Tile.GridPos pos)
    {
        if(!selectedList.Contains(pos))
        {
            selectedList.Add(pos);
        }
        SetSelectTileWhite();
        var lastItem = selectedList.Last();
        int lastIndex = selectedList.IndexOf(lastItem);
        Vector3 lastItemPos = allTilesList[lastItem.GridPosX + 8 * lastItem.GridPosY].transform.position;

        if (selectedList.Count >= 2)
        {
            var lastSecondItem = selectedList[selectedList.Count - 2];
            //int lastSecondIndex = selectedList.IndexOf(lastSecondItem);
            Vector3 lastSecondItemPos = allTilesList[lastSecondItem.GridPosX + 8 * lastSecondItem.GridPosY].transform.position;
            if (Mathf.Abs(lastItem.GridPosX - lastSecondItem.GridPosX) <= 1 && Mathf.Abs(lastItem.GridPosY - lastSecondItem.GridPosY) <= 1)
            {
                DrawLineBetweenTiles(lastSecondItemPos, lastItemPos);
            }
        }
    }

    public void SetAllTileGray()
    {
        selectedList.Clear();

        foreach (var data in allTilesList)
        {
            data.GetComponent<Image>().color = Color.gray;
        }
    }

    public void SetSelectTileWhite()
    {
        var lastItem = selectedList.Last();  //���õ� Ÿ�� �׸��� �� ����Ʈ �� �ֱ� ��
        //�� �ֱ� Ÿ�� �׸��� ���� Ȱ���ؼ� �� Ÿ���� �ε��������� ��ȯ�ؼ� �װ� ó�� ���� ��ü Ÿ�Ͽ��� ã�Ƽ� �� ��ȯ.
        allTilesList[lastItem.GridPosX + 8 * lastItem.GridPosY].GetComponent<Image>().color = Color.white;
        //SetAdjacentTilesLightGray(lastItem);
        CheckPuzzleCompletion();
    }

    public void DrawLineBetweenTiles(Vector3 startPos, Vector3 endPos)
    {

        int newCount = lr.positionCount + 2; // ���ο� ���� �׸� ������ ��ġ ������ 2��ŭ �ø�
        Vector3[] newPositions = new Vector3[newCount];

        // ���� ���� ��ġ ����
        for (int i = 0; i < lr.positionCount; i++)
        {
            newPositions[i] = lr.GetPosition(i);
        }

        // ���ο� ���� �������� ���� �߰�
        newPositions[newCount - 2] = startPos;
        newPositions[newCount - 1] = endPos;

        lr.positionCount = newCount; // ���ο� ��ġ ���� ����
        lr.SetPositions(newPositions); // ���ο� ��ġ��� �� �׸���
    }

    public void ClearLine()
    {
        lr.positionCount = 0;

    }

    public void ClearSelectedTile()
    {
        selectedList.Clear();
        Debug.Log("Tile Emtpy");
    }

    public bool CheckPuzzleCompletion()
    {
        //���õ� Ÿ�ϵ��� �±׸� �����ϴ� ����Ʈ
        List<string> selectedTileTags = new List<string>();

        //���õ� Ÿ�ϵ��� �±׸� �����ϴ� �ݺ���
        foreach (Tile.GridPos pos in selectedList)
        {
            GameObject tileObject = allTilesList[pos.GridPosX + 8 * pos.GridPosY];
            string tileTag = tileObject.tag;
            selectedTileTags.Add(tileTag);
        }

        //������� �ִ��� Ȯ���� �� �ִ� ������ -> ���� �ʿ���...(�Ƹ� 1234Ÿ���� �߰��� ������ ������� �����鼭 5�� Ÿ���� ���������� ������ ���� ����� �̰� �ϳ��� ������ �ȳ��� ����)
        List<string> expectedTileTags = new List<string> { "StartTile", "Tile" , "Tile", "Tile" , "Tile" , "OneTile", "Tile" , "Tile" , "Tile" , "Tile" , "Tile" ,
        "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "TwoTile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile",
        "Tile", "Tile", "Tile", "Tile", "Tile", "ThreeTile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile",
        "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "FourTile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "FiveTile"}; // ���� �±� ���� (StartTile ������ Tile �ݺ�)

        if (selectedTileTags.SequenceEqual(expectedTileTags))
        {
            //�� �� �׸��Ⱑ �ϼ��� ���
            if (selectedTileTags.Last() == "FiveTile")
            {
                // ���� �Ϸ�
                Debug.Log("���� �Ϸ�!");
                return true;
            }
            else
            {
                //������ �Ϸ���� �ʾ����� �ùٸ� ������ ���õ�
                Debug.Log("�ùٸ� ������ ���õ�");
                return false;
            }
        }
        else
        {
            //���õ� Ÿ���� ������ �ùٸ��� ����
            Debug.Log("�ùٸ� ������ �ƴ�");
            return false;
        }
    }

    //�̰� ���õ� Ÿ�� �����¿��� Ÿ�ϸ� ��� �Ϸ��� �߾��µ�... ���� ����� �����鼭 ����.
    /*public void SetAdjacentTilesLightGray(Tile.GridPos pos)
    {
        int[] xOffset = { -1, 1, 0, 0 };
        int[] yOffset = { 0, 0, -1, 1 };

        foreach (Tile.GridPos gridpos in selectedList)
        {
            int x = gridpos.GridPosX;
            int y = gridpos.GridPosY;
            

            for (int i = 0; i < 4; i++)
            {
                int adjX = x + xOffset[i];
                int adjY = y + yOffset[i];

                if (adjX >= 0 && adjX < 8 && adjY >= 0 && adjY < 8)
                {
                    Tile.GridPos adjPos = new Tile.GridPos();
                    adjPos.GridPosX = adjX;
                    adjPos.GridPosY = adjY;

                    if (!selectedList.Contains(adjPos))
                    {
                        SetTileColor(adjPos, new Color(0.7f, 0.7f, 0.7f));
                    }
                }
            }
        }
    }

    
    void SetTileColor(Tile.GridPos pos, Color color)
    {
        int index = pos.GridPosX + 8 * pos.GridPosY;
        allTilesList[index].GetComponent<Image>().color = color;
    }*/

}
