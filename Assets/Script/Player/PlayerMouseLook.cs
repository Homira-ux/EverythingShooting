using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouseLook : MonoBehaviour
{
    public float mouseSensitivity = GameDataManager.Instance.gameData.MouseSensitivity; //マウス感度
    private Transform playerBody;
    private Transform EnemyBody;

    public bool isSpecialCharge = false;

    public GameManager gamemanager;
    public MonoBehaviour playermovement;

    private float cameraAngleX = 0f;
    private float cameraAngleY = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        //Playerの検索
        playerBody = GameObject.FindWithTag("Player").transform;
        //マウスカーソルをロック(画面中央に固定)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EnemyBody = GameObject.FindWithTag("Enemy").transform;
        playermovement = playerBody.GetComponent<MonoBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleCharacter();
    }

    private void HandleCharacter(){
        if(gamemanager.GamePaused){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else{ 
            if(Input.GetKey(KeyCode.LeftAlt)||Input.GetKey(KeyCode.RightAlt)||!gamemanager.isPlayerlives){
                playermovement.enabled = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if(Input.GetKeyUp(KeyCode.LeftAlt)||Input.GetKeyUp(KeyCode.RightAlt)){
                    playermovement.enabled = true;
                }
            }    
            else{
                playermovement.enabled = true;
                if(isSpecialCharge){
                    Vector3 LookEnemy = (EnemyBody.position - playerBody.position).normalized;
                    Quaternion targetRotation = Quaternion.LookRotation(LookEnemy);

                    Vector3 eulerAngles = targetRotation.eulerAngles;
                    eulerAngles.z = 0;
                    targetRotation = Quaternion.Euler(eulerAngles);

                    playerBody.rotation = Quaternion.Lerp(playerBody.rotation, targetRotation, Time.deltaTime*mouseSensitivity);
                    cameraAngleX = playerBody.rotation.eulerAngles.x;
                    cameraAngleX = (cameraAngleX > 180) ? cameraAngleX - 360 : cameraAngleX; // -180～180に補正
                    cameraAngleY = playerBody.rotation.eulerAngles.y;
                }
                else{    
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    //マウスの入力を取得*感度*フレーム間の時間
                    float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                    float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
                    //カメラの上下回転
                    cameraAngleX -= mouseY;
                    cameraAngleY += mouseX;

                    cameraAngleX = Mathf.Clamp(cameraAngleX, -79f, 79f);
                    playerBody.rotation = Quaternion.Euler(cameraAngleX, cameraAngleY, 0f);
                }
            }
        }
    }
}
