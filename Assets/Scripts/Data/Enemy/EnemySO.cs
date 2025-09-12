using UnityEngine;

public enum TypeEnemy
{
    normal,
    ranger,
    boss,
}

[CreateAssetMenu(fileName = "EnemySO", menuName = "EnemySO/DataEnemy")]
public class EnemySO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _maxHP;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private float _attackRange;
    [SerializeField] private TypeEnemy _typeEnemy;
    [SerializeField] private EnemyAttackSO _attackStrategy;


    public string Name => _name;
    public int MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;
    public float AttackCooldown => _attackCooldown;
    public float AttackRange => _attackRange;
    public TypeEnemy TypeEnemy => _typeEnemy;
    public EnemyAttackSO AttackStrategy => _attackStrategy;
}
