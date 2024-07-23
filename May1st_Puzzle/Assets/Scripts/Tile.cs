using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler
{
    public struct GridPos
    {
        public int GridPosX;
        public int GridPosY;
        public void SetPos(int x, int y)
        {
            this.GridPosX = x;
            this.GridPosY = y;
        }
    }

    public int posX;
    public int posY;

    public GridPos gridPos;
    TileManager tileManager;

    private bool isPointDown = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        //gridPos.SetPos(posX, posY);
        tileManager = GameObject.FindWithTag("TileManager").GetComponent<TileManager>();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        if(Input.GetMouseButton(0))
        {
            Debug.Log("Drag");
    
            tileManager.AddTileGridPos(gridPos);
            //tileManager.SetAdjacentTilesLightGray(gridPos);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
        if(Input.GetMouseButtonUp(0))
        {
            Debug.Log("마우스 업");
            
            tileManager.SetAllTileGray();
            tileManager.ClearLine();
            tileManager.ClearSelectedTile();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            tileManager.AddTileGridPos(gridPos);
            //tileManager.SetAdjacentTilesLightGray(gridPos);
        }
    }
}
