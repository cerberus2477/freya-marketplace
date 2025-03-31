namespace FreyaMarketplace.Model;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string City { get; set; }
    public string Birthdate { get; set; }
    public int RoleId { get; set; }
    public string Picture { get; set; }
    public string Description { get; set; }

    //TODO: json desirialise. the json contains role_id
}

