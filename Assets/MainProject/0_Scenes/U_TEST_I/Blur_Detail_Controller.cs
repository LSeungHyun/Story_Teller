using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal; // URP의 경우

public class Blur_Detail_Controller : MonoBehaviour
{
    [Header("Volume & UI References")]
    public Volume volume;           // Volume 컴포넌트를 할당 (Post Process Volume)
    public Slider weightSlider;     // Volume Weight 조절용 슬라이더
    public Text weight_Text;
    public Slider maxRadiusSlider;  // Depth Of Field의 Max Radius 조절용 슬라이더
    public Text maxRadius_Text;
    public InputField colorInputField; // Color Filter를 설정할 때 사용할 Hex 입력 필드 (예: "FFFFFF")
    public Text color_Text;

    [Header("Panel BackGround")]
    public Image panel_Image;
    public InputField rgbInputField;
    public Text rgb_Text;
    public Slider alphaSlider;
    public Text alpha_Text;

    // Volume Weight 슬라이더 값이 바뀔 때 호출 (OnValueChanged 이벤트에 연결)
    public void OnWeightSliderChanged()
    {
        if (volume != null)
        {
            volume.weight = weightSlider.value;
            weight_Text.text = Math.Round(weightSlider.value, 2).ToString();
            Debug.Log("Volume Weight set to: " + weightSlider.value);
        }
    }

    // Max Radius 슬라이더 값이 바뀔 때 호출 (OnValueChanged 이벤트에 연결)
    public void OnMaxRadiusSliderChanged()
    {
        if (volume != null && volume.profile != null && volume.profile.TryGet<DepthOfField>(out var dof))
        {
            dof.gaussianMaxRadius.Override(maxRadiusSlider.value);
            maxRadius_Text.text = Math.Round(maxRadiusSlider.value, 2).ToString();
            Debug.Log("DepthOfField Max Radius set to: " + maxRadiusSlider.value);
        }
    }

    // Color 입력 필드의 값이 바뀔 때 호출 (OnEndEdit 이벤트 등에 연결)
    // 입력 값은 "FFFFFF"와 같이 6자리 Hex 값으로 입력 (선택적으로 '#'를 붙여도 됨)
    public void OnColorInputChanged()
    {
        string hexColor = colorInputField.text;
        // 입력 값에 '#'가 없으면 붙여줍니다.
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
            Debug.LogError("panelImage가 할당되지 않았습니다.");
            return;
        }

        string hexRGB = rgbInputField.text;

        // 입력값에 '#'가 없으면 붙여줍니다.
        if (!hexRGB.StartsWith("#"))
            hexRGB = "#" + hexRGB;

        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexRGB, out newColor))
        {
            // 기존의 Alpha 값을 보존합니다.
            newColor.a = panel_Image.color.a;
            panel_Image.color = newColor;
            Debug.Log("패널의 RGB가 " + hexRGB + "로 변경되었습니다.");
        }
        else
        {
            Debug.LogError("잘못된 RGB 값: " + hexRGB);
        }

        rgb_Text.text = hexRGB;
    }

    public void OnAlphaSliderChanged()
    {
        if (panel_Image == null)
        {
            Debug.LogError("panelImage가 할당되지 않았습니다.");
            return;
        }

        Color current = panel_Image.color;
        float newAlpha = alphaSlider.value;
        current.a = newAlpha;
        panel_Image.color = current;
        alpha_Text.text = Math.Round(newAlpha, 2).ToString();
        Debug.Log("패널의 Alpha가 " + newAlpha + "로 변경되었습니다.");
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
