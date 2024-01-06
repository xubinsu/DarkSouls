using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Package : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public GridList grids;
    public int length;
    public int width;
    public int gridSize;

    public bool isUsing = false;

    public RectTransform dragPanel;


    public void OnPointerEnter(PointerEventData eventData)
    {
        isUsing = true;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isUsing = false;
    }

    private void Awake()
    {
        grids = new GridList(length, width, gridSize, this.transform.position);
        dragPanel = transform.Find("DragPanel").transform as RectTransform;
        PackageManager.Instance.Add(this);
    }

    private void OnDestroy()
    {
        PackageManager.Instance.Remove(this);
    }
}
