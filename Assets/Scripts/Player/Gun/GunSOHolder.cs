using UnityEngine;

public class GunSOHolder : MonoBehaviour
{
    [SerializeField] private GunAttributes _gunAttributes;
    public GunAttributes GunAttributes => _gunAttributes;
}
