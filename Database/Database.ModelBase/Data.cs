namespace Database.ModelBase
{
    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // 通过创建对象属性，在生成数据表时，会自动生成外键
        public int CreatorId { get; set; }
        public User Creator { get; set; }
    }
}
