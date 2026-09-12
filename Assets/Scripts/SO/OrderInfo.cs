namespace NsfwDelivery.SO
{
    [System.Serializable]
    public class OrderInfo
    {
        public int Input, Output;
        public CargoType Cargo;
    }

    public enum CargoType
    {
        Normal,
        Explosive
    }
}