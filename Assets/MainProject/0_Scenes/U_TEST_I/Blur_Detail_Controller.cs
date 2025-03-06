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
    public Slider weight_Slider;     // Volume Weight ������ �����̴�
    public Text weight_Text;
    public Slider FocalLength_Slider;  // Depth Of Field�� Max Radius ������ �����̴�
    public Text FocalLength_Text;

    [Header("Panel BackGround")]
    public Image panel_Image1;
    public Image panel_Image2;
    public Image panel_Image3;

    public Material material;

    public InputField rgbInputField;
    public Text rgb_Text;
    public Slider alphaSlider;
    public Text alpha_Text;

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

    public void OnRGBInputEndEdit()
    {
        string hexRGB = rgbInputField.text;

        // �Է°��� '#'�� ������ �ٿ��ݴϴ�.
        if (!hexRGB.StartsWith("#"))
            hexRGB = "#" + hexRGB;

        Color newColor;

        if (ColorUtility.TryParseHtmlString(hexRGB, out newColor))
        {
            // ������ Alpha ���� �����մϴ�.
            newColor.a = material.color.a;

            material.color = newColor;

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
        Color current = panel_Image1.color;
        float newAlpha = alphaSlider.value;
        current.a = newAlpha;
        panel_Image1.color = current;
        panel_Image2.color = current;
        panel_Image3.color = current;
        alpha_Text.text = Math.Round(newAlpha, 2).ToString();
        Debug.Log("�г��� Alpha�� " + newAlpha + "�� ����Ǿ����ϴ�.");
    }
}