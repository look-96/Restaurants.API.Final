namespace Restaurants.Application.User;

public record CurrentUser(string Id, 
    string Email, 
    IEnumerable<string> Roles, 
    string? Nationality)
{
    public bool IsInRole(string role) => Roles.Contains(role);
}
