namespace KinematicsVisualizer.Models
{
    public enum ComponentAxis
    {
        X,
        Y
    }

    public static class ComponentAxisProvider
    {
        public static ComponentAxis[] All => (ComponentAxis[])Enum.GetValues(typeof(ComponentAxis));
    }
}