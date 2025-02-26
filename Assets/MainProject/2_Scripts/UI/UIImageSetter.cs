using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle.Manifest;

public class UIImageSetter : UIContentsManager
{
    [SerializeField] private ImageContainer imageContainer;

    public SpriteRenderer spriteDisplay;
    public List<Sprite> spriteData;

    public void SetImageData(string currentObjCode)
    {
        ImageData targetRow = imageContainer.imageDatas.FirstOrDefault(r => r.objCode == currentObjCode);

        spriteData = SetImage(targetRow);
        totalDataPage = (spriteData != null) ? spriteData.Count : 0;
        currentDataPage = 1;
        DisplayPage();
    }

    private List<Sprite> SetImage(ImageData targetRow)
    {
        var sprites = new List<Sprite>();

        foreach (string resource in targetRow.dataList)
        {
            string resourceName = resource.Trim();
            string resourcePath = "ImagePopup/" + (resourceName.Contains("_") ? resourceName.Replace('_', '/') : resourceName);

            Sprite sprite = Resources.Load<Sprite>(resourcePath);
            if (sprite == null)
            {
                Debug.LogWarning("Failed to load sprite: " + resourceName);
            }
            sprites.Add(sprite);
        }

        return sprites;
    }

    public void ClearImageData()
    {
        spriteData = new List<Sprite>();
        if (spriteDisplay != null)
            spriteDisplay.sprite = null;
    }

    public override void DisplayPage()
    {
        if (spriteData != null && spriteData.Count > 0)
        {
            spriteDisplay.sprite = spriteData[currentDataPage - 1];
            UpdateNavigationButtons();
        }
    }
}
