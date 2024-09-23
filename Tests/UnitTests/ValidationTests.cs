using Tasks.Validation;

namespace Tests.UnitTests;

public class ValidationTests
{
    [Fact]
    public void ShouldRejectTodaysDate()
    {
        var attribute = new FutureDateAttribute();
        var result = attribute.IsValid(DateTime.Today);
        
        Assert.False(result);
    }

    [Fact]
    public void ShouldAcceptTomorrowsDate()
    {
        var attribute = new FutureDateAttribute();
        var result = attribute.IsValid(DateTime.Today.AddDays(1));
        
        Assert.True(result);
    }
}