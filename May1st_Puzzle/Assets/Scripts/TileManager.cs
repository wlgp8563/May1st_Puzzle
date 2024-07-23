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
    public List<GameObject> allTilesList = new List<GameObject>();  //처음 시작 시 타일들 다 받아오기

    
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
        var lastItem = selectedList.Last();  //선택된 타일 그리드 값 리스트 중 최근 거
        //그 최근 타일 그리드 값을 활용해서 그 타일의 인덱스값으로 변환해서 그걸 처음 받은 전체 타일에서 찾아서 색 변환.
        allTilesList[lastItem.GridPosX + 8 * lastItem.GridPosY].GetComponent<Image>().color = Color.white;
        //SetAdjacentTilesLightGray(lastItem);
        CheckPuzzleCompletion();
    }

    public void DrawLineBetweenTiles(Vector3 startPos, Vector3 endPos)
    {

        int newCount = lr.positionCount + 2; // 새로운 선을 그릴 때마다 위치 개수를 2만큼 늘림
        Vector3[] newPositions = new Vector3[newCount];

        // 이전 선의 위치 복사
        for (int i = 0; i < lr.positionCount; i++)
        {
            newPositions[i] = lr.GetPosition(i);
        }

        // 새로운 선의 시작점과 끝점 추가
        newPositions[newCount - 2] = startPos;
        newPositions[newCount - 1] = endPos;

        lr.positionCount = newCount; // 새로운 위치 개수 설정
        lr.SetPositions(newPositions); // 새로운 위치들로 선 그리기
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
        //선택된 타일들의 태그를 저장하는 리스트
        List<string> selectedTileTags = new List<string>();

        //선택된 타일들의 태그를 저장하는 반복문
        foreach (Tile.GridPos pos in selectedList)
        {
            GameObject tileObject = allTilesList[pos.GridPosX + 8 * pos.GridPosY];
            string tileTag = tileObject.tag;
            selectedTileTags.Add(tileTag);
        }

        //순서대로 있는지 확인할 수 있는 정답지 -> 수정 필요함...(아마 1234타일이 중간에 저렇게 순서대로 지나면서 5번 타일이 마지막으로 끝나는 성공 방식이 이거 하나라 오류가 안날거 같음)
        List<string> expectedTileTags = new List<string> { "StartTile", "Tile" , "Tile", "Tile" , "Tile" , "OneTile", "Tile" , "Tile" , "Tile" , "Tile" , "Tile" ,
        "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "TwoTile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile",
        "Tile", "Tile", "Tile", "Tile", "Tile", "ThreeTile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile",
        "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "FourTile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "Tile", "FiveTile"}; // 예상 태그 순서 (StartTile 다음에 Tile 반복)

        if (selectedTileTags.SequenceEqual(expectedTileTags))
        {
            //한 붓 그리기가 완성된 경우
            if (selectedTileTags.Last() == "FiveTile")
            {
                // 퍼즐 완료
                Debug.Log("퍼즐 완료!");
                return true;
            }
            else
            {
                //퍼즐은 완료되지 않았지만 올바른 순서로 선택됨
                Debug.Log("올바른 순서로 선택됨");
                return false;
            }
        }
        else
        {
            //선택된 타일의 순서가 올바르지 않음
            Debug.Log("올바른 순서가 아님");
            return false;
        }
    }

    //이건 선택된 타일 상하좌우의 타일만 밝게 하려고 했었는데... 선이 제대로 나오면서 뺐음.
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
