﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.Dtos
{
    public class IngredientUpdateDto
    {

        public int id { get; set; }
        public string name { get; set; }
        public string quantity { get; set; }
        public int recipe_id { get; set; }
    }
}
