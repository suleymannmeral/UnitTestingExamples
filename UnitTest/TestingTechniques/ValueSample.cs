

namespace TestingTechniques;

public class ValueSample
{
    public string FullName = "Süleyman Meral";
    public int Age = 23;
    public DateOnly Birth= new DateOnly(2002,06,13);

    public Users AppUser = new()
    {
        FullName = "Süleyman Meral",
        Age = 23,
        DateOfBirth = new DateOnly(2002, 06, 13)
    };

    public IEnumerable<Users> UsersList = new[]
    {
        new Users() { FullName = "Süleyman Meral", Age = 23, DateOfBirth = new DateOnly(2002, 06, 13) },
        new Users() { FullName = "John Doe", Age = 30, DateOfBirth = new DateOnly(1993, 01, 01) },
        new Users() { FullName = "Jane Smith", Age = 28, DateOfBirth = new DateOnly(1995, 05, 15) }
    };

    public IEnumerable<int> Numbers= new[] { 53,68,06,61 };

}
