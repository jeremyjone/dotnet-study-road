namespace ObjectMapper.Projection
{
    public class MemberSource
    {
        public string Name { get; set; }
        public MemberInnerSource MemberInnerSource { get; set; }
        public MemberOtherInnerSource MemberOtherInnerSource { get; set; }
    }

    public class MemberInnerSource
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class MemberOtherInnerSource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }

    public class MemberDestination
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
    }
}
