using AutoMapper;
using BeGamer.DTOs.Address;
using BeGamer.DTOs.Game;
using BeGamer.DTOs.GameEvent;
using BeGamer.DTOs.User;
using BeGamer.Models;

namespace BeGamer.Mappers
{
    public class GenericMapper : Profile
    {
        public GenericMapper()
        {
            //Game Mappings
            CreateMap<Game, GameDTO>().ReverseMap();
            CreateMap<Game, CreateGameDTO>().ReverseMap();
            CreateMap<Game, UpdateGameDTO>().ReverseMap();

            //Address Mappings
            CreateMap<Address, AddressDTO>().ReverseMap();

            //GameEvent Mappings
            CreateMap<GameEvent, GameEventDTO>().ReverseMap();
            CreateMap<GameEvent, CreateGameEventDTO>().ReverseMap();

            //User Mappings
            CreateMap<CustomUser, UserDTO>().ReverseMap();

        }
    }
}
