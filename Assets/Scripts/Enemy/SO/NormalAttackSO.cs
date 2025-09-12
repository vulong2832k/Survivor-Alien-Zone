using UnityEngine;

[CreateAssetMenu(fileName = "NormalAttackSO", menuName = "EnemySO/NormalAttackSO")]
public class NormalAttackSO : EnemyAttackSO
{
    [SerializeField] private int _damage;
    public override void EnemyAttack(Transform enemy, Transform target)
    {
        Debug.Log($"Enemy gây được {_damage} sát thương");
        target.GetComponent<IDamageable>()?.TakeDamage(_damage);
    }
}
