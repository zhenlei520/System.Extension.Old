using EDapper.Extension;

namespace System.ETest
{
    class Program
    {
        static void Main(string[] args)
        {
            SqlMapperExtension.Delete(new { Id = 1, Name = 2 }, "User");
        }
    }
}
