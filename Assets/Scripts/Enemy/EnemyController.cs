using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IDamageable
{
    [Header("Attributes: ")]
    [SerializeField] private int _currentHP;
    [SerializeField] private float _cooldown;

    [Header("References: ")]
    [SerializeField] private EnemySO _dataEnemy;
    [SerializeField] private Transform _targetPlayer;
    [SerializeField] private GameObject _originPrefab;

    [Header("Components: ")]
    private NavMeshAgent _agent;

    [Header("Logic: ")]
    private bool _isAttacking = false;
    private bool _playerInRange = false;

    public void SetPrefabReference(GameObject prefab)
    {
        _originPrefab = prefab;
    }
    public GameObject GetPrefabReference()
    {
        return _originPrefab;
    }
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
        _currentHP = _dataEnemy.MaxHP;
        _cooldown = 0f;
        _isAttacking = false;
        _playerInRange = false;

        if (_agent != null)
            _agent.isStopped = false;
    }
    private void Start()
    {
        StartCoroutine(WaitForPlayer());

        if (_dataEnemy != null)
            _agent.speed = _dataEnemy.MoveSpeed;
    }
    private void Update()
    {
        CheckAttack();
    }
    private void FixedUpdate()
    {
        if (_targetPlayer != null && !_playerInRange)
        {
            _agent.SetDestination(_targetPlayer.position);

            Vector3 moveDir = _agent.velocity.normalized;
            if (moveDir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(moveDir);
        }
    }
    private IEnumerator WaitForPlayer()
    {
        while(_targetPlayer == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
            {
                _targetPlayer = foundPlayer.transform;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void CheckAttack()
    {
        if (_targetPlayer == null) return;

        float distance = Vector3.Distance(transform.position, _targetPlayer.position);
        _playerInRange = distance <= _dataEnemy.AttackRange;

        if (_playerInRange && _targetPlayer != null)
        {
            Vector3 lookDir = (_targetPlayer.position - transform.position).normalized;
            lookDir.y = 0f;
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        if (_playerInRange)
        {
            if(!_isAttacking && _cooldown <= 0)
            {
                StartCoroutine(AttackRoutine());
            }
            else
            {
                StopMoving();
            }
        }
        else
        {
            ResumeMoving();
        }
        _cooldown -= Time.deltaTime;
    }
    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        StopMoving();

        _dataEnemy.AttackStrategy.EnemyAttack(transform, _targetPlayer);
        _cooldown = _dataEnemy.AttackCooldown;

        yield return new WaitForSeconds(1f);

        _isAttacking = false;
    }
    private void StopMoving()
    {
        if (_agent.enabled && _agent.isActiveAndEnabled)
            _agent.isStopped = true;
    }

    private void ResumeMoving()
    {
        if (_agent.enabled && _agent.isActiveAndEnabled)
            _agent.isStopped = false;
    }
    public void TakeDamage(int damage)
    {
        _currentHP -= damage;

        if (_currentHP <= 0)
            Die();
    }
    private void Die()
    {
        EnemyPoolManager.Instance.ReturnToPool(_originPrefab, gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _dataEnemy?.AttackRange ?? 1f);
    }
}
