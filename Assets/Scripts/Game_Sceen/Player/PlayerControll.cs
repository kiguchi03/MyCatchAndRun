using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using static PlayerState;

/// <summary>
/// プレイヤーの動きを制御
/// </summary>
public class PlayerControll : Player
{
    internal StateMachine<PlayerControll> stateMachine { get; private set; }

    [SerializeField]
    internal Slider stamina;

    [SerializeField]
    internal CameraControll cameraControll;

    [SerializeField]
    internal float currentSpeed;

    [Header("移動スピード")]
    [SerializeField]
    internal float moveSpeed = 1.3f;

    internal float dashSpeed = 2f;

    public float staminaRate;

    [SerializeField]
    [Range(1.0f, 10.0f)]
    internal float normalStaRate;

    [SerializeField]
    [Range(1.0f, 10.0f)]
    internal float tiredStaRate;

    [SerializeField]
    [Range(-10.0f, -1.0f)]
    internal float dashStaRate;

    [SerializeField]
    internal float recoverySpeed;

    [Tooltip("足音のSE")]
    [SerializeField]
    internal AudioClip stepClip;

    [SerializeField]
    FlagData GameOver;

    internal GameObject _camera;

    internal Rigidbody rid;

    internal Animator animator;

    internal Vector3 playerMove;

    internal float InputVertical;

    internal float InputHorizontal;

    [SerializeField]
    FlagData gameClear;

    [SerializeField]
    FlagData gameOver;

    public override void Init()
    {
        _camera = GameObject.FindGameObjectWithTag("MainCamera");
        rid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        stateMachine = new StateMachine<PlayerControll>(this);

        stateMachine.AddTransition<Move, Running>((int)PlayerStates.Running);
        stateMachine.AddTransition<Running, Move>((int)PlayerStates.Normal);
        stateMachine.AddTransition<Running, Tired>((int)PlayerStates.tired);
        stateMachine.AddTransition<Tired, Move>((int)PlayerStates.Normal);

        stateMachine.AddForceTransition<Stop>((int)PlayerStates.Stop);
        stateMachine.AddForceTransition<Dead>((int)PlayerStates.Dead);

        stateMachine.Start<Move>();
    }

    public override void ManagedUpdate()
    {
        if (gameClear.GetSetIsBool)
        {
            stateMachine.OfferTransition((int)PlayerStates.Stop);
        }

        if (gameOver.GetSetIsBool)
        {
            stateMachine.OfferTransition((int)PlayerStates.Dead);
        }
        stateMachine.Update();
    }

    public void AnimEvent_Step()
    {
        SoundManager.instance.PlaySE(stepClip);
    }
}
