using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderState<T> : State<T>
{
    private IArtificialMovement _self;
    private INode _root;
    private SteeringType _obsEnum;

    private float _randomAngle;
    private Vector3 _dir;

    public EnemyWanderState(IArtificialMovement self, INode root, SteeringType obsEnum, float randomAngle)
    {
        _self = self;
        _root = root;
        _obsEnum = obsEnum;
        _randomAngle = randomAngle;
    }

    public override void Awake()
    {
        _self.LifeController.OnTakeDamage += TakeHit;
        //_self.OnWallCollision += ChangeDirection;
        _self.Avoidance.SetActualBehaviour(_obsEnum);

        _dir = ChangeAngle();
    }

    public override void Execute()
    {
        if (_self.IsTargetInSight()) //if we took damage or we saw the player....
            _root.Execute();

        _self.LookDir(_self.Avoidance.GetDir(_dir));
        _self.Move(_self.transform.forward, _self.ActorStats.WalkSpeed);
    }

    private Vector3 ChangeAngle()
    {
        //var aux =  Quaternion.Euler(0, Random.Range(-_randomAngle, _randomAngle), 0) * _self.transform.forward;
        var aux = Quaternion.Euler(0, Random.Range(-_randomAngle, _randomAngle), 0) * Vector3.one;
        return aux;
    }

    private void ChangeDirection()
    {
        _root.Execute();
    }

    private void TakeHit()
    {
        _self.TakeHit(true);
        _root.Execute();
    }

    public override void Sleep()
    {
        _self.LifeController.OnTakeDamage -= TakeHit;
    }

}