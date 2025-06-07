using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyNumberCheck
{
    public void AddActiveEnemy(Enemy enemy);
    public void RemoveActiveEnemy(Enemy enemy);
}
