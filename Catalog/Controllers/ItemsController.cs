using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using Catalog.Entities;
using System.Collections.Generic;
using System;
//using System.Linq;
using Catalog.Dtos;

namespace Catalog.Controllers{
    
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase {
        private readonly IItemsRepository repository;

        public ItemsController(IItemsRepository repository){
            this.repository = repository;
        }

        [HttpGet]
        public IEnumerable<ItemDto> GetItems(){
            var items = repository.GetItems().Select(item => item.AsDto());
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id){
            var item = repository.GetItem(id);

            if(item is null){
                return NotFound(); 
            }
            return item.AsDto();
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto){
            Item item = new(){
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            repository.CreateItem(item);

            return CreatedAtAction(nameof(GetItem), new{ id = item.Id}, item.AsDto());
        }

        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto item){
            var existingItem = repository.GetItem(id);

            if(existingItem is null){
                return NotFound();
            }

            Item updatedItem = existingItem with{
                Name = item.Name,
                Price = item.Price
            };

            repository.UpdateItem(updatedItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id){
            var existingItem = repository.GetItem(id);

            if(existingItem is null){
                return NotFound();
            }

            repository.DeleteItem(id);

            return NoContent();
        }
    }
}