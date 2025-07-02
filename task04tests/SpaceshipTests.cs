using task04.Spaceships;
using Xunit;

namespace task04tests;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        var cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        var fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void MoveForward_ShouldChangeCoordinates()
    {
        var cruiser = new Cruiser();
        cruiser.MoveForward();
        Assert.NotEqual(0, cruiser.X);
        Assert.Equal(0, cruiser.Y);
    }

    [Fact]
    public void Rotate_ShouldUpdateAngle()
    {
        var fighter = new Fighter();
        fighter.Rotate(90);
        Assert.Equal(90, fighter.Angle);
    }

    [Fact]
    public void Fire_ShouldIncrementShotsFired()
    {
        var cruiser = new Cruiser();
        cruiser.Fire();
        cruiser.Fire();
        Assert.Equal(2, cruiser.ShotsFired);
    }
}
