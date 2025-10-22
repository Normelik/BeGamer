namespace BeGamer.Utils
{
    public class GuidGenerator
    {
        public GuidGenerator() { }
        public Guid GenerateUniqueGuid(Func<Guid, bool> existsById)
        {
            Guid newGuid = Guid.NewGuid();
            while (existsById(newGuid))
            {
                newGuid = Guid.NewGuid();
            }
            return newGuid;
        }


        public string GenerateUniqueStringGuid(Func<string, bool> existsById)
        {
            string newGuid = Guid.NewGuid().ToString();
            while (existsById(newGuid))
            {
                newGuid = Guid.NewGuid().ToString();
            }
            return newGuid;
        }
    }
}
