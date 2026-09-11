using UnityEngine;

namespace NsfwDelivery.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/OrderInfo", fileName = "OrderInfo")]
    public class OrderInfo : ScriptableObject
    {
        public int Input, Output;
    }
}