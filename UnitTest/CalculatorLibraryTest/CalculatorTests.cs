using CalculatorLibrary;
using FluentAssertions;
using Xunit.Abstractions;

namespace CalculatorLibraryTests;

public class CalculatorTests:IAsyncLifetime
{

    private readonly Calculator _sut = new();
    private readonly ITestOutputHelper _output;

    public CalculatorTests(ITestOutputHelper output )
    {
        _output = output;
        _output.WriteLine("CalculatorTests constructor called");
    }
    [Theory]
    [InlineData(5,5,10)]
    [InlineData(-5,5,0)]
    [InlineData(-15,-5,-20)]
    public void Add_ShouldAddTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b , int expected)
    {
        //Arrange
     

        //Act
        var result = _sut.Add(a,b);
        _output.WriteLine("Hello From Add Method");

        //Assert
        result.Should().Be(expected);


    }
    [Theory]
    [InlineData(5,-5,10)]
    [InlineData(15, -5, 20)]
    [InlineData(-5, -5, 0)]
    [InlineData(-15,-5,-10)]
    [InlineData(5,-10,15)]
    public void Subtract_ShouldSubtractTwoNumbers_WhenTwoNumbersAreIntegers(int a , int b , int excepted)
    {
        //Arrange


        //Act
        var result = _sut.Subtract(a, b);
        _output.WriteLine("Hello From Subtract Method");

        //Assert
        Assert.Equal(excepted, result);


    }
    [Theory]
    [InlineData(5,5,25)]
    [InlineData(50,0,0)]
    [InlineData(-5,5,-25)]
    public void Multiply_ShouldMultiplyTwoNumbers_WhenTwoNumbersAreIntegers(int a , int b, int excepted)
    {
        var result = _sut.Multiply(a, b);
        _output.WriteLine("Hello From Multiply Method");
        Assert.Equal(excepted, result);
    }
    [Theory]
    [InlineData(5, 5, 1)]
    [InlineData(15,5,3)]
    [InlineData(0,0,0,Skip ="Igoner")]
    public void Divide_ShouldDivideTwoNumbers_WhenTwoNumbersAreIntegers(int a, int b, int excepted)
    {
        var result = _sut.Divide(a, b);
        _output.WriteLine("Hello From Divide Method");
        Assert.Equal(excepted, result);
    }
    public void Dispose()
    {
        _output.WriteLine("CalculatorTests Dispose called");
    }

    public async Task InitializeAsync()
    {
        _output.WriteLine("Hello from the initialize");

        Task.Delay(1);
    }

    public async Task DisposeAsync()
    {
        _output.WriteLine("Hello from the dispose");

        Task.Delay(1);
    }
}
