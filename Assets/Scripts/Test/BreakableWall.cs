using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private int _pieces;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Explode();
        }
    }

    void Explode()
    {
        float cubeSize = 1f / _pieces;
        for (int x = 0; x < _pieces; x++)
        {
            for (int y = 0; y < _pieces; y++)
            {
                for (int z = 0; z < _pieces; z++)
                {
                    GameObject piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    piece.transform.localScale = transform.localScale / _pieces;
                    piece.transform.position = transform.position + new Vector3(
                        (x - _pieces / 2f) * piece.transform.localScale.x,
                        (y - _pieces / 2f) * piece.transform.localScale.y,
                        (z - _pieces / 2f) * piece.transform.localScale.z
                    );

                    Rigidbody rb = piece.AddComponent<Rigidbody>();
                    rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);

                    Destroy(piece, Random.Range(2f, 5f));
                }
            }
        }
        
        Destroy(gameObject);
    }
}
