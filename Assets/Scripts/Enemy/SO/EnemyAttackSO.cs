using UnityEngine;

public abstract class EnemyAttackSO : ScriptableObject
{
    public abstract void EnemyAttack(Transform enemy, Transform target);
}
