using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PackageItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform thisRect;
    public DragPanel dragPanel;
    public Package package;

    public Vector2Int originPos;

    public int length;
    public int width;

    private void Awake()
    {
        thisRect = this.GetComponent<RectTransform>();
        dragPanel = this.transform.parent.GetComponent<DragPanel>();
        package = dragPanel.transform.parent.GetComponent<Package>();
        
    }
    
    public void RefreshOriginPos()
    {
        Vector2Int gridPos = package.grids.GetPosXY(this.transform.position);
        Vector2Int targetPos = new Vector2Int(gridPos.x,gridPos.y);
        if(targetPos.x < 0 || targetPos.y < 0||targetPos.x>package.grids.length||targetPos.y>package.grids.width)
        {
            return;
        }
        originPos = targetPos;
    }
    private void Start()
    {
        this.transform.position = package.grids.GetWorldPosition(originPos.x, originPos.y);
        for (int i = 0; i < length; i++)
        {
            for (int k = 0; k < width; k++)
            {
                package.grids.gridList[i + originPos.x, k + originPos.y].isUsed = true;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragPanel.OnBeginDraging(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragPanel.OnDraging(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragPanel.OnEndDraging(this);
    }
}
