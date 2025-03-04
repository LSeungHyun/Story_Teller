using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // URP�� ���

public class Blur_Detail_Controller : MonoBehaviour
{
    [Header("Volume")]
    public Volume volume;           // Volume ������Ʈ�� �Ҵ� (Post Process Volume)

    [Header("Blur Detail")]
    public InputField color_InputField; // Color Filter�� ������ �� ����� Hex �Է� �ʵ� (��: "FFFFFF")
    public Text color_Text;
    public Slider weight_Slider;     // Volume Weight ������ �����̴�
    public Text weight_Text;
    public Slider FocalLength_Slider;  // Depth Of Field�� Max Radius ������ �����̴�
    public Text FocalLength_Text;
    

    [Header("Panel BackGround")]
    public Image panel_Image;
    public InputField rgbInputField;
    public Text rgb_Text;
    public Slider alphaSlider;
    public Text alpha_Text;

    void Awake()
    {
        
    }

    // Volume Weight �����̴� ���� �ٲ� �� ȣ�� (OnValueChanged �̺�Ʈ�� ����)
    public void OnWeightSliderChanged()
    {
        if (volume != null)
        {
            volume.weight = weight_Slider.value;
            weight_Text.text = Math.Round(weight_Slider.value, 2).ToString();
            Debug.Log("Volume Weight set to: " + weight_Slider.value);
        }
    }

    // Max Radius �����̴� ���� �ٲ� �� ȣ�� (OnValueChanged �̺�Ʈ�� ����)
    public void OnFocalLengthSliderChanged()
    {
        if (volume != null && volume.profile != null && volume.profile.TryGet<DepthOfField>(out var dof))
        {
            dof.focalLength.Override(FocalLength_Slider.value);
            FocalLength_Text.text = Math.Round(FocalLength_Slider.value, 2).ToString();
            Debug.Log("DepthOfField Max Radius set to: " + FocalLength_Slider.value);
        }
    }

    // Color �Է� �ʵ��� ���� �ٲ� �� ȣ�� (OnEndEdit �̺�Ʈ � ����)
    // �Է� ���� "FFFFFF"�� ���� 6�ڸ� Hex ������ �Է� (���������� '#'�� �ٿ��� ��)
    public void OnColorInputChanged()
    {
        string hexColor = color_InputField.text;
        // �Է� ���� '#'�� ������ �ٿ��ݴϴ�.
        if (!hexColor.StartsWith("#"))
            hexColor = "#" + hexColor;

        if (ColorUtility.TryParseHtmlString(hexColor, out Color newColor))
        {
            if (volume != null && volume.profile != null && volume.profile.TryGet<ColorAdjustments>(out var colorAdj))
            {
                colorAdj.colorFilter.Override(newColor);
                Debug.Log("Color Filter set to: " + hexColor);
            }
        }
        else
        {
            Debug.LogError("Invalid hex color string: " + hexColor);
        }

        color_Text.text = hexColor;
    }

    public void OnRGBInputEndEdit()
    {
        if (panel_Image == null)
        {
            Debug.LogError("panelImage�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        string hexRGB = rgbInputField.text;

        // �Է°��� '#'�� ������ �ٿ��ݴϴ�.
        if (!hexRGB.StartsWith("#"))
            hexRGB = "#" + hexRGB;

        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexRGB, out newColor))
        {
            // ������ Alpha ���� �����մϴ�.
            newColor.a = panel_Image.color.a;
            panel_Image.color = newColor;
            Debug.Log("�г��� RGB�� " + hexRGB + "�� ����Ǿ����ϴ�.");
        }
        else
        {
            Debug.LogError("�߸��� RGB ��: " + hexRGB);
        }

        rgb_Text.text = hexRGB;
    }

    public void OnAlphaSliderChanged()
    {
        if (panel_Image == null)
        {
            Debug.LogError("panelImage�� �Ҵ���� �ʾҽ��ϴ�.");
            return;
        }

        Color current = panel_Image.color;
        float newAlpha = alphaSlider.value;
        current.a = newAlpha;
        panel_Image.color = current;
        alpha_Text.text = Math.Round(newAlpha, 2).ToString();
        Debug.Log("�г��� Alpha�� " + newAlpha + "�� ����Ǿ����ϴ�.");
    }

    public void PopUpColorOn()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out var colorAdj))
        {
            colorAdj.active = true;
        }
    }

    public void PopUpColorOff()
    {
        if (volume.profile.TryGet<ColorAdjustments>(out var colorAdj))
        {
            colorAdj.active = false;
        }
    }
}
