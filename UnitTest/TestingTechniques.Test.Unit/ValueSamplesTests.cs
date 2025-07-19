using FluentAssertions;

namespace TestingTechniques.Test.Unit
{
    public class ValueSamplesTests
    {
        private readonly ValueSample _sut = new();
        [Fact]
        public void StringAssertionExample()
        {
            var fullName= _sut.FullName;

            fullName.Should().Be("Süleyman Meral", "because it is the full name of the person we are testing.");
            fullName.Should().NotBeEmpty();
            fullName.Should().StartWith("Süleyman");
            fullName.Should().EndWith("Meral");
        }

        [Fact]
        public void IntegerAssertionExample()
        {
            var age = _sut.Age;
            age.Should().Be(23, "because it is the age of the person we are testing.");
            age.Should().BePositive();
            age.Should().BeGreaterThanOrEqualTo(18);
            age.Should().BeInRange(20,50);
        }
        [Fact]
        public void DateOnlyAssertionExample()

        {
            var birthDay= _sut.Birth;
            birthDay.Should().Be(new DateOnly(2002, 06, 13), "because it is the birth date of the person we are testing.");
            birthDay.Should().BeAfter(new DateOnly(2000, 01, 01), "because the person was born after 2000.");
        }
        [Fact]
        public void ObjectAssertionExample()
        {
            var excepted= new Users
            {
                FullName = "Süleyman Meral",
                Age = 23,
                DateOfBirth = new DateOnly(2002, 06, 13)
            };
            var user = _sut.AppUser;
            user.Should().BeEquivalentTo(excepted);
        }
        [Fact]
        public void EnumerableObjectAssertionExample()
        {
            var excepted = new Users
            {
                FullName = "Süleyman Meral",
                Age = 23,
                DateOfBirth = new DateOnly(2002, 06, 13)
            };

            var users = _sut.UsersList.As<Users[]>();

            users.Should().ContainEquivalentOf(excepted);
            users.Should().HaveCount(3);
            users.Should().Contain(u=>u.FullName.StartsWith("Süleyman"), "because we expect to find a user with the name starting with 'Süleyman'.");

        }

        [Fact]
        public void EnumerableNumberAssertionExample()
        {
            var numbers= _sut.Numbers.As<int[]>();
            numbers.Should().HaveCount(4, "because we expect to have 4 numbers in the collection.");
        }
    }
}