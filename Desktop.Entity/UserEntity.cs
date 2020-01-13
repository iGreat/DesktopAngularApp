using System;

namespace Desktop.Entity
{
    public class UserEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }
        
        public string Key { get; set; }
        
        public string Iv { get; set; }
    }
}
