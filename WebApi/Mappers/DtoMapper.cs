using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Entities;

namespace WebApi.Mappers
{

    public class DtoMapper
    {
        private IMapper _mapper;

        public DtoMapper()
        {
            _mapper = new MapperConfiguration(config => 
            {
                config.CreateMap<Item, ItemDto>()
                .ReverseMap();
            }).CreateMapper();

        }

        public Item Map(ItemDto itemDto) => _mapper.Map<Item>(itemDto);

        public IEnumerable<Item> Map(IEnumerable<ItemDto> itemDto) => _mapper.Map<IEnumerable<Item>>(itemDto);

        public ItemDto Map(Item item) => _mapper.Map<ItemDto>(item);

        public IEnumerable<ItemDto> Map(IEnumerable<Item> item) => _mapper.Map<IEnumerable<ItemDto>>(item);
    }
}
