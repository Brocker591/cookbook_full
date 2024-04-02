using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.Models
{
    public class Ingredient : BaseModel
    {
        private int id;
        [PrimaryKey]
        public int Id
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

        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;
                name = value;
                this.OnPropertyChanged();
            }
        }
        private string quantity;
        public string Quantity
        {
            get => quantity;
            set
            {
                if (quantity == value)
                    return;
                quantity = value;
                this.OnPropertyChanged();
            }
        }
        private int recipeId;
        public int RecipeId
        {
            get => recipeId;
            set
            {
                if (recipeId == value)
                    return;
                recipeId = value;
                this.OnPropertyChanged();
            }
        }
    }
}
