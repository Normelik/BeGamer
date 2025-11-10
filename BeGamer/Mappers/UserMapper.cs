//using BeGamer.DTOs.Auth;
//using BeGamer.DTOs.User;
//using BeGamer.Models;

//namespace BeGamer.Mappers
//{
//        public class UserMapper
//    {

//        public CustomUser ToModel(RegisterUserDTO registerUserDTO)
//        {
//            return new CustomUser
//            {
//                UserName = registerUserDTO.Username,
//                PasswordHash = registerUserDTO.Password
//            };
//        }
//        public IEnumerable<CustomUser> ToModelList(IEnumerable<RegisterUserDTO> userDtos)
//        {
//            return userDtos.Select(dto => ToModel(dto)).ToList();
//        }

//        public UserDTO ToDTO(CustomUser user)
//        {
//            return new UserDTO(
//                user.Id,
//                user.UserName,
//                user.Nickname
//            );
//        }
//        public IEnumerable<UserDTO> ToDTOList(IEnumerable<CustomUser> users)
//        {
//            return users.Select(user => ToDTO(user)).ToList();
//        }
//    }
//}
