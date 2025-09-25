using UnityEngine;

public class PlantZone : MonoBehaviour
{
    public bool IsPlanted {  get; private set; }

    public void Plant()
    {
        IsPlanted = true;
        //hiệu ứng + animation làm sau
    }
}
