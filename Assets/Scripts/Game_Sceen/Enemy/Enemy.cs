using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

public abstract class Enemy : MonoBehaviour
{
    public GameObject target { get; protected set; }

    internal EnemyManager enemyManager;

    public PlayableDirector TimeLine;

    [SerializeField]
    protected GameObject enemyForTimeLine;

    [SerializeField]
    FlagData GameOver;


    public abstract void Init();

    public abstract void ManagedUpdate();

    public abstract void Destroy();

    protected virtual void Attack()
    {
        var track = ((TimelineAsset)TimeLine.playableAsset).GetOutputTracks().First(c => c.name.Equals("Control Track"));
        var clip = (ControlPlayableAsset)track.GetClips().First(c => c.displayName == "GameObject").asset;

        clip.prefabGameObject = enemyForTimeLine;

        TimeLine.Play();

        GameOver.GetSetIsBool = true;
    }
}
