using UnityEngine;

namespace Enemies
{
    public class EnemyReceiveDamage: MonoBehaviour
    {
        [SerializeField] private GameObject parent;
        public void ReceiveDamage()
        {
            //TODO animate
            Destroy(parent);
        }
    }
}