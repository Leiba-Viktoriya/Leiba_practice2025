namespace task04.Spaceships;

public class Cruiser : ISpaceship
{
    public int Speed => 50;
    public int FirePower => 100;

    public int X { get; private set; } = 0;
    public int Y { get; private set; } = 0;
    public int Angle { get; private set; } = 0;
    public int ShotsFired { get; private set; } = 0;

    public void MoveForward()
    {
        X += (int)(Speed * Math.Cos(Angle * Math.PI / 180));
        Y += (int)(Speed * Math.Sin(Angle * Math.PI / 180));
    }

    public void Rotate(int angle)
    {
        Angle = (Angle + angle) % 360;
    }

    public void Fire()
    {
        ShotsFired++;
    }
}
