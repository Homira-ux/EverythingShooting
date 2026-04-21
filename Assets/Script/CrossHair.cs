using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    public Image crosshairImage; // クロスヘアのImageコンポーネント

    void Start()
    {
        // クロスヘアの位置を画面中央に設定
        RectTransform rt = crosshairImage.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f); // アンカーを中央に設定
        rt.anchorMax = new Vector2(0.5f, 0.5f); // アンカーを中央に設定
        rt.anchoredPosition = Vector2.zero; // 位置を(0,0)に設定
    }
}
