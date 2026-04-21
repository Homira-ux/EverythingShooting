//  PerlinNoiseTextureExample.cs
//  http://kan-kikuchi.hatenablog.com/entry/What_is_PerlinNoise
//
//  Created by kan.kikuchi on 2018.12.15.

using UnityEngine;

/// <summary>
/// パーリンノイズでテクスチャを生成するクラス
/// </summary>
public class PerlinNoiseTextureExample : MonoBehaviour {
  
    public int width = 256;  // テクスチャの幅
    public int height = 256; // テクスチャの高さ
    public float scale = 20f; // ノイズの細かさを調整するスケール

    public Renderer targetRenderer; // テクスチャを適用するオブジェクトのRenderer

    void Start()
    {
        // Perlinノイズテクスチャを生成
        Texture2D noiseTexture = GeneratePerlinNoiseTexture(width, height, scale);

        // 生成したテクスチャを対象のオブジェクトに適用
        if (targetRenderer != null)
        {
            targetRenderer.material.mainTexture = noiseTexture;
        }
    }

    // Perlinノイズでテクスチャを生成する関数
    Texture2D GeneratePerlinNoiseTexture(int width, int height, float scale)
    {
        Texture2D texture = new Texture2D(width, height);

        // 各ピクセルに対してノイズ値を計算し色を設定
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;

                // Mathf.PerlinNoiseで0～1の範囲の値を取得
                float sample = Mathf.PerlinNoise(xCoord, yCoord);

                // ノイズ値をグレースケールの色に変換
                Color color = new Color(sample, sample, sample);
                texture.SetPixel(x, y, color);
            }
        }

        // テクスチャに変更を適用
        texture.Apply();
        return texture;
    }

}