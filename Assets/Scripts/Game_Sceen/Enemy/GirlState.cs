using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GirlState
{
    public enum GirlStates : int
    {
        Chase,
        Stop,
    }

    public class Chase : State<GirlControll>
    {
        protected override void OnUpdate()
        {
            if (owner.isRendered && !owner.noRecieveRender)
            {
                owner.stateMachine.OfferTransition((int)GirlStates.Stop);
            }

            owner.navMeshAgent.isStopped = false;

            owner.navMeshAgent.destination = owner.target.transform.position;
        }
    }

    public class Stop : State<GirlControll>
    {
        protected override void OnUpdate()
        {
            if (!owner.isRendered || owner.noRecieveRender &&(!owner.isAct))
            {
                owner.stateMachine.OfferTransition((int)GirlStates.Chase);
            }

            owner.navMeshAgent.isStopped = true;
        }
    }
}
