using UnityEngine;

public class DynamicSortingLayer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [Header("Sorting Layer Names")]
    public string underObjectLayer = "UnderObjectPlayer";
    public string aboveObjectLayer = "AboveObjectPlayer";

    [SerializeField]
    private string currentSortingLayer;

    [SerializeField]
    private Transform objTransform;

    public bool isColliding = false;

    #region Lifecycle Methods
    void Awake()
    {
        currentSortingLayer = spriteRenderer.sortingLayerName;
    }

    void Update()
    {
        if (isColliding)
        {
            UpdateSortingLayer();
        }
    }
    #endregion

    #region
    public void OnTriggerEnter2D(Collider2D collision)
    {
        isColliding = true;
        objTransform = collision.transform;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        isColliding = false;
        objTransform = null;
    }
    #endregion

    #region Sorting Methods

    private void UpdateSortingLayer()
    {
        if (objTransform == null) return;

        string newSortingLayer = objTransform.transform.position.y > this.transform.position.y ? aboveObjectLayer : underObjectLayer;

        if (newSortingLayer != currentSortingLayer)
        {
            currentSortingLayer = newSortingLayer;
            spriteRenderer.sortingLayerName = currentSortingLayer;
        }
    }
    #endregion
}
