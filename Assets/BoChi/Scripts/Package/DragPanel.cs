using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour
{

    private PackageItem dragItem;
    private Package package;
    private Package endPackage;

    private Vector2 resultPos;
    private bool canPut;

    private void Awake()
    {
        package = this.transform.parent.GetComponent<Package>();
    }

    public void OnDraging(PackageItem packageItem)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(package.transform.parent.transform as RectTransform, Input.mousePosition, null, out resultPos);
        resultPos -= new Vector2(packageItem.thisRect.rect.width / 2, packageItem.thisRect.rect.height / 2);
        packageItem.transform.localPosition = resultPos;
    }

    public void OnBeginDraging(PackageItem packageItem)
    {
        packageItem.transform.SetParent(package.transform.parent, false);
        Color color = packageItem.GetComponent<Image>().color;
        color.a = 0.7f;
        packageItem.GetComponent<Image>().color = color;
        packageItem.GetComponent<Image>().raycastTarget = false;
    }

    public void OnEndDraging(PackageItem packageItem)
    {
        Color color = packageItem.GetComponent<Image>().color;
        color.a = 1f;
        packageItem.GetComponent<Image>().color = color;

        endPackage = PackageManager.Instance.FindUsingPackage();
        if (endPackage == null)
        {
            packageItem.transform.SetParent(packageItem.package.dragPanel, true);
            packageItem.transform.position= package.grids.GetWorldPosition(packageItem.originPos.x,packageItem.originPos.y);
            packageItem.GetComponent<Image>().raycastTarget = true;
            return;
        }

      

        packageItem.transform.SetParent(endPackage.dragPanel, true);
        packageItem.GetComponent<Image>().raycastTarget = true;
        Vector2Int lastOrigin = packageItem.originPos;
        Package lastPackage = packageItem.package;
        packageItem.package = endPackage;
        packageItem.dragPanel = packageItem.transform.parent.GetComponent<DragPanel>();
        
        packageItem.RefreshOriginPos();

        for (int i = 0; i < packageItem.length; i++)
        {
            for (int k = 0; k < packageItem.width; k++)
            {
                if (packageItem.package.grids.gridList[i + packageItem.originPos.x, k + packageItem.originPos.y].isUsed == true)
                {
                    canPut = false;
                    packageItem.package = lastPackage;
                    packageItem.originPos = lastOrigin;
                    packageItem.transform.SetParent(packageItem.package.dragPanel, true);
                    packageItem.transform.position = package.grids.GetWorldPosition(packageItem.originPos.x, packageItem.originPos.y);
                    packageItem.GetComponent<Image>().raycastTarget = true;
                    return;
                }
            }
        }

        canPut = true;
        if (canPut)
        {
            packageItem.transform.position = packageItem.package.grids.GetWorldPosition(packageItem.originPos.x, packageItem.originPos.y);

            for (int i = 0; i < packageItem.length; i++)
            {
                for (int k = 0; k < packageItem.width; k++)
                {
                    packageItem.package.grids.gridList[i + lastOrigin.x, k + lastOrigin.y].isUsed = false;
                    packageItem.package.grids.gridList[i + packageItem.originPos.x, k + packageItem.originPos.y].isUsed = true;
                }
            }
        }

        
    }
  

}
