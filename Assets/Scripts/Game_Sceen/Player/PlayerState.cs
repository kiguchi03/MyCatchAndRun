using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public enum PlayerStates : int
    {
        Normal,
        Running,
        tired,
        Stop,
        Dead,
    }

    internal abstract class BaseMove : State<PlayerControll>
    {
        protected override void OnUpdate()
        {
            float InputHorizontal = Input.GetAxis("Horizontal");
            float InputVertical = Input.GetAxis("Vertical");

            Vector3 playerMove = owner._camera.transform.forward * InputVertical * owner.currentSpeed
                + owner._camera.transform.right * InputHorizontal * owner.currentSpeed;

            owner.rid.velocity = playerMove;

            owner.animator.SetFloat("Speed", owner.rid.velocity.magnitude);

            owner.stamina.value += owner.staminaRate * Time.deltaTime;
        }
    }


    internal class Move : BaseMove
    {
        protected override void OnEnter(State<PlayerControll> preState)
        {
            owner.currentSpeed = owner.moveSpeed;

            owner.staminaRate = owner.normalStaRate;
        }

        protected override void OnUpdate()
        {
            if (Input.GetMouseButton(0) && Mathf.Abs(Input.GetAxisRaw("Horizontal")) +
                Mathf.Abs(Input.GetAxisRaw("Vertical")) != 0)
            {
                owner.stateMachine.OfferTransition((int)PlayerStates.Running);
            }

            base.OnUpdate();
        }
    }

    internal class Running : BaseMove
    {
        float runSpeed = 2.0f;

        protected override void OnEnter(State<PlayerControll> preState)
        {
            owner.currentSpeed = runSpeed;

            owner.staminaRate = owner.dashStaRate;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (!Input.GetMouseButton(0) || Mathf.Abs(Input.GetAxisRaw("Horizontal")) +
                Mathf.Abs(Input.GetAxisRaw("Vertical")) == 0)
            {
                owner.stateMachine.OfferTransition((int)PlayerStates.Normal);
            }

            if (owner.stamina.value <= owner.stamina.minValue)
            {
                StateMachine.OfferTransition((int)PlayerStates.tired);
            }
        }
    }

    internal class Tired : BaseMove
    {
        float tiredSpeed = 0.9f;

        protected override void OnEnter(State<PlayerControll> preState)
        {
            owner.currentSpeed = tiredSpeed;

            owner.staminaRate = owner.tiredStaRate;
        }

        protected override void OnUpdate()
        {
            if (owner.stamina.value >= owner.stamina.maxValue)
            {
                StateMachine.OfferTransition((int)PlayerStates.Normal);
            }

            base.OnUpdate();
        }
    }

    internal class Stop : BaseMove
    {
        protected override void OnEnter(State<PlayerControll> preState)
        {
            owner.rid.velocity = Vector3.zero;
            owner.enabled = false;
            owner.animator.SetFloat("Speed", owner.rid.velocity.magnitude);
        }
    }

    internal class Dead : State<PlayerControll>
    {
        protected override void OnEnter(State<PlayerControll> preState)
        {
            owner.rid.velocity = Vector3.zero;
            owner.enabled = false;
            owner.animator.SetFloat("Speed", owner.rid.velocity.magnitude);
            owner.cameraControll.IsMove = false;
        }
    }
}
