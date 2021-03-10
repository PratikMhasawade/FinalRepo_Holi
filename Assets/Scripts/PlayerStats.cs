using UnityEngine;
using UnityEngine.Serialization;

namespace Shubham_Holi.Scripts
{
    public class PlayerStats : MonoBehaviour
    {

        [FormerlySerializedAs("HP")] public int hp = 100;

        public void TakeDamage(int dmg)
        {
            hp -= dmg;
        }
        public void GiveDamage(int dmg)
        {
            hp -= dmg;
        }
    }
}
