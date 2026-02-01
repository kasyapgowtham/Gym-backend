using System.Numerics;

namespace backend.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string passwordhash { get; set; }
        public string Phone { get; set; }
    }
}
