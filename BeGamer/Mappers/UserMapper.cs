using BeGamer.DTOs;
using BeGamer.Models;

namespace BeGamer.Mappers
{
        public class UserMapper
    {

        public User ToModel(UserDTO userDto)
        {
            return new User
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Nickname = userDto.Nickname
            };
        }
        public IEnumerable<User> ToModelList(IEnumerable<UserDTO> userDtos)
        {
            return userDtos.Select(dto => ToModel(dto)).ToList();
        }

        public UserDTO ToDTO(User user)
        {
            return new UserDTO(
                user.Id,
                user.Username,
                user.Nickname
            );
        }
        public IEnumerable<UserDTO> ToDTOList(IEnumerable<User> users)
        {
            return users.Select(user => ToDTO(user)).ToList();
        }
    }
}
