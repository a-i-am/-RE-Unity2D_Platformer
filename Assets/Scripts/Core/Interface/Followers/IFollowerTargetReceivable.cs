using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFollowerTargetReceivable
{
    Enemy CurrentTarget { get; }
    Vector2 TargetPosition { get; }
    void SetTarget(Enemy enemy);
}
