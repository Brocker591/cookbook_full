using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.Models
{
    public class UserModel : BaseModel
    {
        private Guid id;
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
}
