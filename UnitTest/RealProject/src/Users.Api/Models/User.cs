namespace Users.Api.Models
{
    public sealed class User
    {
        public User()
        {
            Id = Guid.NewGuid();
            
        }

        public Guid Id { get; set; }
        public string FullName { get; set; }
    }
}
