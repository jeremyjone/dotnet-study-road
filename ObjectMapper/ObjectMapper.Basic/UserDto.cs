namespace ObjectMapper.Basic
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
    }

    public class ErrorUserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }

        /// <summary>
        /// 源数据中没有该字段
        /// </summary>
        public string ErrorAttr { get; set; }
    }
}
