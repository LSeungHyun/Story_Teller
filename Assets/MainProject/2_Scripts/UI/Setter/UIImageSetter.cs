using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIImageSetter : UIPopUpManager
{
    [SerializeField] private ImageContainer imageContainer;

    public ImageList[] imageList;
    public Sprite[] spriteData;

    public Image defaultSpriteDisplay;
    public Image questSpriteDisplay;

    public override void SetData(string currentObjCode)
    {
        if (imageContainer != null && imageContainer.imageDatas != null)
        {
            var imageData = imageContainer.imageDatas.FirstOrDefault(r => r.objCode == currentObjCode);
            if (imageData != null)
            {
                imageList = imageData.dataList;
                spriteData = SetImage(imageList);
                totalDataPage = spriteData.Length;
            }
        }
        currentDataPage = 1;
        DisplayPage();
    }

    public override void DisplayPage()
    {
        if (spriteData != null && spriteData.Length > 0)
        {
            defaultSpriteDisplay.sprite = spriteData[currentDataPage - 1];
            questSpriteDisplay.sprite = spriteData[currentDataPage - 1];
        }
        StartOnPageChanged(currentDataPage, totalDataPage);
        Debug.Log(currentDataPage + "," + totalDataPage);
    }

    public override void ClearData()
    {
        imageList = new ImageList[0];
        spriteData = new Sprite[0];
        if (defaultSpriteDisplay != null)
            defaultSpriteDisplay.sprite = null;
        if (questSpriteDisplay != null)
            questSpriteDisplay.sprite = null;
    }

    Sprite[] SetImage(ImageList[] imageList)
    {
        Sprite[] sprites = new Sprite[imageList.Length];
        for (int i = 0; i < imageList.Length; i++)
        {
            Sprite sprite = Resources.Load<Sprite>(imageList[i].imageData);
            if (sprite == null)
            {
                Debug.LogWarning("Failed to load sprite: " + imageList[i].imageData);
            }
            sprites[i] = sprite;
        }
        return sprites;
    }
}
