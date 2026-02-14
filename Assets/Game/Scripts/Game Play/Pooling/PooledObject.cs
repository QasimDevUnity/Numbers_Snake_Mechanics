using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public string PoolKey { get; set; }

    public void ReturnToPool()
    {
        GameManager.Instance.poolManager.ReturnToPool(this);
    }
}