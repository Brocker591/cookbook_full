using SQLite;

namespace Cookbook.App.Models;

public class User : BaseModel
{
    private Guid id;
    
    [PrimaryKey]
    public Guid Id
    {
        get => id;
        set
        {
            if (id == value)
                return;
            id = value;
            this.OnPropertyChanged();
        }
    }

    private int userId;
    public int UserId
    {
        get => userId;
        set
        {
            if (userId == value)
                return;
            userId = value;
            this.OnPropertyChanged();
        }
    }

    private string userName;
    public string UserName
    {
        get => userName;
        set
        {
            if (userName == value)
                return;
            userName = value;
            this.OnPropertyChanged();
        }
    }
    private string password;
    public string Password
    {
        get => password;
        set
        {
            if (password == value)
                return;
            password = value;
            this.OnPropertyChanged();
        }
    }

    private string token;
    public string Token
    {
        get => token;
        set
        {
            if (token == value)
                return;
            token = value;
            this.OnPropertyChanged();
        }
    }

    private DateTime expireDate;
    public DateTime ExpireDate
    {
        get => expireDate;
        set
        {
            if (expireDate == value)
                return;
            expireDate = value;
            this.OnPropertyChanged();
        }
    }
}
