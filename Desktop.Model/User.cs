using System;

namespace Desktop.Model
{
    public class User
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public bool IsValid { get; set; }
    }
}