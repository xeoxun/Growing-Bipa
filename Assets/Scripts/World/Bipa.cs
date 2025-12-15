using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bipa : MonoBehaviour
{
    public float speed; // 캐릭터 이동 속도
    public Vector2 inputVec;

    Rigidbody2D rigid;
    SpriteRenderer spriter;
    Animator anim;

    void Awake()
    {
        transform.DOKill();

        Application.targetFrameRate = 75;

        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        rigid.gravityScale = 0;

        // X축 위치 및 회전 고정
        //rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

    }
    void FixedUpdate()
    {
        Vector2 nextPos = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextPos);
    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }
    }

    void OnDestroy()
    {
        DOTween.Kill(this);
    }
}
