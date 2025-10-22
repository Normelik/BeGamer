using BeGamer.DTOs.User;
using BeGamer.Models;

namespace BeGamer.Mappers
{
        public class UserMapper
    {

        public CustomUser ToModel(CreateUserDTO createUserDTO)
        {
            return new CustomUser
            {
                UserName = createUserDTO.Username,
                PasswordHash = createUserDTO.Password,
                Nickname = createUserDTO.Nickname
            };
        }
        public IEnumerable<CustomUser> ToModelList(IEnumerable<CreateUserDTO> userDtos)
        {
            return userDtos.Select(dto => ToModel(dto)).ToList();
        }

        public UserDTO ToDTO(CustomUser user)
        {
            return new UserDTO(
                user.Id,
                user.UserName,
                user.Nickname
            );
        }
        public IEnumerable<UserDTO> ToDTOList(IEnumerable<CustomUser> users)
        {
            return users.Select(user => ToDTO(user)).ToList();
        }
    }
}
