namespace KinematicsVisualizer.Models
{
    public enum MotionType
    {
        MRU,
        MRUA,
        FreeFall,
        VerticalThrow,
        ParabolicThrow
    }

    public enum GraphType
    {
        PosicionVsTiempo,
        VelocidadVsTiempo,
        AceleracionVsTiempo,
        XY // solo para ParabolicThrow
    }
}
