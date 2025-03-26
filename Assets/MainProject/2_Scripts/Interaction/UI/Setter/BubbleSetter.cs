using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BubbleSetter : UIContentsManager
{
    [SerializeField] public BubbleContainer bubbleContainer;

    public BubbleData targetRow;
    public string textData;
    public float showTime = 2f;
    private GameObject currentBalloon;
    public Vector3 currentObjOffset;
    private Text textDisplay;

    [Header("��ǳ�� ����")]
    public GameObject balloonPrefab;
    public float offsetY;

    [Header("ũ�� ����")]
    public float fixedWidth;
    public float minHeight;
    public float lineHeight;

    private void Start()
    {
        SetSizeValue();
    }

    public void SetSizeValue()
    {
        offsetY = 0.5f; // ������Ʈ �Ӹ� ���� ���� ���� ��ġ
        fixedWidth = 0.2f; // ���� �¿� ����
        minHeight = 0.35f;
        lineHeight = minHeight;
    }
    public override void SetData(string currentObjCode)
    {
        targetRow = bubbleContainer.bubbleDatas.FirstOrDefault(r => r.objCode == currentObjCode);
        if (targetRow == null)
            return;

        textData = targetRow.dataList != null ? string.Join("\n", targetRow.dataList) : "";
        showTime = targetRow.closeTime;
        DisplayPage();
    }

    public override void DisplayPage()
    {
        if (balloonPrefab != null)
        {
            currentBalloon = Instantiate(balloonPrefab, currentObjOffset, Quaternion.identity);
            textDisplay = currentBalloon.GetComponentInChildren<Text>();
            textDisplay.text = textData;

            Vector3 newPosition = currentObjOffset + new Vector3(0, offsetY, 0);
            currentBalloon.transform.position = newPosition;

            AdjustBalloonSize();
            Invoke(nameof(HideBalloon), showTime);
        }
    }

    public override void ClearData()
    {
        textData = "";
        if (textDisplay != null)
            textDisplay.text = "";
        CancelInvoke("HideBalloon");
    }

    private void AdjustBalloonSize()
    {
        if (currentBalloon != null)
        {
            SpriteRenderer balloonRenderer = currentBalloon.GetComponentInChildren<SpriteRenderer>();
            if (balloonRenderer != null && textDisplay != null)
            {
                // �ؽ�Ʈ�� �� ���� ��� (�ٹٲ� ���� ����)
                int lineCount = textData.Split('\n').Length;
                string[] lines = textData.Split('\n');
                int maxLineLength = 0;

                foreach (string line in lines)
                {
                    int length = line.Length;
                    if (length > maxLineLength)
                    {
                        maxLineLength = length;
                    }
                }
                float width = fixedWidth + ( maxLineLength * 0.078f);

                for (int i = 0; i < lineCount-1; i++)
                {
                    if (i % 2 == 0)
                    {
                        lineHeight += 0.1f;
                    }
                    else
                    {
                        lineHeight += 0.15f;
                    }
                }

                float height = Mathf.Max(minHeight, lineHeight);
                
                balloonRenderer.size = new Vector2(width, height);
                lineHeight = minHeight;
                // textDisplay.transform.localPosition = new Vector3(0, height / 2, 0); // ���߿� ��� ���� �� ����
            }
        }
    }
    
    public void HideBalloon()
    {
        if (currentBalloon != null)
        {
            Destroy(currentBalloon);
        }
    }
}
