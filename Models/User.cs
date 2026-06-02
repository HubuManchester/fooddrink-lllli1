using SQLite;

namespace FoodLens.Models;

/// <summary>
/// Represents a registered user account.
/// </summary>
[Table("Users")]
public class User
{
    /// <summary>
    /// Unique identifier for the user.
    /// </summary>
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    /// <summary>
    /// Login username (same as email).
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// User's email address, used for login and identification.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// SHA-256 hashed password.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Display name shown in the UI.
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// URL of the user's avatar image.
    /// </summary>
    public string AvatarUrl { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp when the account was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
