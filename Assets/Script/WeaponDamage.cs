using UnityEngine;
using TMPro;

public class WeaponDamage : MonoBehaviour
{
    public TMP_Text DamageText;
    public float moveUpSpeed = 1f; // 上に移動する速度
    public float fadeDuration = 1f; // フェードアウトの時間
    public Transform cameraTransform; // カメラのTransform（手動で設定可能）

    private float timer;

    public void SetDamage(int damage){
        DamageText.text = damage.ToString();
    }

    void Start(){
        // カメラのTransformを取得（メインカメラを使用）
        if(cameraTransform == null){
            cameraTransform = Camera.main.transform;
        }
    }

    void Update(){
        timer += Time.deltaTime;

        // 上に移動
        transform.Translate(Vector3.up * moveUpSpeed * Time.deltaTime);

        // フェードアウト処理
        if(DamageText != null){
            Color color = DamageText.color;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            DamageText.color = color;
        }

        // カメラの方向を向く
        if(cameraTransform != null){
            transform.LookAt(cameraTransform);
            transform.Rotate(0, 180f, 0); // テキストが逆向きの場合の補正
        }

        // フェードアウト後に削除
        if(timer >= fadeDuration){
            Destroy(gameObject);
        }
    }
}
