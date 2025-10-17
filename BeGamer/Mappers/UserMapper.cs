using BeGamer.DTOs.User;
using BeGamer.Models;

namespace BeGamer.Mappers
{
        public class UserMapper
    {

        public User ToModel(CreateUserDTO createUserDTO)
        {
            return new User
            {
                Username = createUserDTO.Username,
                Password = createUserDTO.Password,
                Nickname = createUserDTO.Nickname
            };
        }
        public IEnumerable<User> ToModelList(IEnumerable<CreateUserDTO> userDtos)
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
