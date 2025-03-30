using DATA.Context;
using DATA.Interface;
using DATA.Models;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository class for handling user-related database operations.
/// Implements IUserRepository.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly EFDbContext _dbContext;

    /// <summary>
    /// Constructor to initialize the database context.
    /// </summary>
    /// <param name="dbContext">EF Core database context instance.</param>
    public UserRepository(EFDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves a user by their username.
    /// </summary>
    /// <param name="userName">The username of the user.</param>
    /// <returns>The user object if found; otherwise, null.</returns>
    public async Task<ApplicationUser> GetUser(string userName)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName);
    }

    /// <summary>
    /// Checks if a user exists by their phone number.
    /// </summary>
    /// <param name="phoneNumber">The phone number of the user.</param>
    /// <returns>True if the user exists; otherwise, false.</returns>
    public async Task<bool> GetUserbyPhone(string phoneNumber)
    {
        return await _dbContext.Users.AnyAsync(x => x.PhoneNumber == phoneNumber);
    }

    /// <summary>
    /// Creates a new user with an email and phone number.
    /// </summary>
    /// <param name="user">The user object to be created.</param>
    /// <returns>True if user is created successfully; otherwise, false.</returns>
    public async Task<bool> CreateUser(ApplicationUser user)
    {
        if(await _dbContext.Users.AnyAsync(x => x.Email == user.Email || x.PhoneNumber == user.PhoneNumber))
        {
            return false; // User already exists
        }

        _dbContext.Users.Add(user);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0 ? true : false;
    }

    /// <summary>
    /// Edits a user's details based on their ID.
    /// </summary>
    /// <param name="user">The updated user object.</param>
    /// <returns>True if update is successful; otherwise, false.</returns>
    public async Task<bool> EditUser(ApplicationUser user)
    {
        var existingUser = await _dbContext.Users.FindAsync(user.Id);
        if(existingUser == null)
            return false;

        // Update fields
        existingUser.UserName = user.UserName;
        existingUser.Email = user.Email;
        existingUser.PhoneNumber = user.PhoneNumber;

        _dbContext.Users.Update(existingUser);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0 ? true : false;
    }

    /// <summary>
    /// Updates a specific field of the user.
    /// <returns>True if update is successful; otherwise, false.</returns>
    public async Task<bool> UpdateUser(ApplicationUser userupdate)
    {
        var user = await _dbContext.Users.FindAsync(userupdate.Id);


        _dbContext.Users.Update(userupdate);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0 ? true : false;
    }

    /// <summary>
    /// Deletes a user by ID.
    /// </summary>
    /// <param name="userId">The ID of the user to be deleted.</param>
    /// <returns>True if deletion is successful; otherwise, false.</returns>
    public async Task<bool> DeleteUser(int userId)
    {
        var user = await _dbContext.Users.FindAsync(userId);
        if(user == null)
            return false;

        _dbContext.Users.Remove(user);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0 ? true : false;
    }
}
