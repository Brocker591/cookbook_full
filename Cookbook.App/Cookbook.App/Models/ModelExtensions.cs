using Cookbook.App.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cookbook.App.Models
{
    public static class ModelExtensions
    {
        public static ItemDto ToDto(this Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                Priority = item.Priority,
                Inventory = item.Inventory
                
            };
        }

        public static Item ToModel(this ItemDto itemDto)
        {
            return new Item
            {
                Id = itemDto.Id,
                Name = itemDto.Name,
                Quantity = itemDto.Quantity,
                Priority = itemDto.Priority,
                Inventory = itemDto.Inventory
            };
        }

        public static ItemCreateDto ToCreateDto(this Item item)
        {
            return new ItemCreateDto
            {
                name = item.Name,
                quantity = item.Quantity
            };
        }
    }
}
