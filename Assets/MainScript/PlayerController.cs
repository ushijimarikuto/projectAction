using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Quaternion targetRotation;
    Animator animator;
    CharacterController controller;
    public GameObject HpGauge;

    public bool IsGround, DieArea, ClearArea = false;
    private float gravity = 9.8f;
    private Vector3 moveDirection;

    //音声ファイル格納用変数
    public AudioClip sound1;
    AudioSource audioSource;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Awake()
    {
        //初期化
        TryGetComponent(out animator);
        targetRotation = transform.rotation;
        Cursor.lockState = CursorLockMode.Locked; //マウスカーソルをロック
        Cursor.visible = false;
    }
    
    void Update()
    {
        //カメラの向きで補正した入力ベクトルの取得
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y,Vector3.up);
        var velocity =  horizontalRotation * new Vector3(horizontal, 0 ,vertical).normalized;
        

        //速度の取得
        var speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        var rotationSpeed = 600 * Time.deltaTime;

        //移動方向を向く
        if(velocity.magnitude > 0.5f)
        {
            targetRotation = Quaternion.LookRotation(velocity, Vector3.up);
        }
        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed);

        //移動速度をAnimatorに反映
        animator.SetFloat("Speed", velocity.magnitude * speed, 0.1f, Time.deltaTime);


        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                moveDirection.y = 5.0f;
                animator.SetBool("Jumping", true);
            }
            else
            {
                animator.SetBool("Jumping", false);
            }
            animator.SetBool("Grounded", true);
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (HpGauge.GetComponent<Image>().fillAmount <= 0)
        {
            StartCoroutine("PlayerDie");
        }
    }

    //キャラクターコントローラーの衝突処理
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Damage")
        {
            //当たるとダメージを受ける処理
            GameObject director = GameObject.Find("GaugeDirector");
            director.GetComponent<GaugeDirector>().DecreaseHp();
        }

        if (hit.gameObject.tag == "DieArea")
        {
            DieArea = true;
            StartCoroutine("PlayerDie");
        }

        if (hit.gameObject.tag == "ClearArea")
        {
            ClearArea = true;
            PlayerEscape();
        }
    }

    IEnumerator PlayerDie()
    {     
        yield return new WaitForSeconds(0.2f);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("OverScene");
    }

    void PlayerEscape()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        SceneManager.LoadScene("ClearScene");
    }
}
