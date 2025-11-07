namespace BeGamer.Utils
{
    public class GuidGenerator
    {
        public GuidGenerator() { }
        public async Task<Guid> GenerateUniqueGuidAsync(Func<Guid, Task<bool>> existsById)
        {
            Guid newGuid;

            do
            {
                newGuid = Guid.NewGuid();
            }
            while (await existsById(newGuid));

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
