using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.Dtos
{
    public class RecipeUpdateDto
    {
        public int id { get; set; }
        public string name { get; set; }
        public string preparation { get; set; }
        public List<IngredientUpdateDto> ingredients { get; set; }
    }
}
