using System;
using System.ComponentModel.DataAnnotations;
using Aperea.Services;

namespace Aperea.EntityModels
{
    public class Login
    {
        protected Login()
        {
        }

        public Login(string username, string email)
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
            Username = username;
            EMail = email;
            PasswordHash = string.Empty;
            Confirmed = false;
            Active = false;
        }

        public int Id { get; private set; }

        [Required]
        public DateTime Created { get; private set; }

        [Required]
        public DateTime Updated { get; private set; }

        [Required]
        [StringLength(128)]
        public string Username { get; private set; }

        [Required]
        [StringLength(256)]
        public string EMail { get; set; }

        [Required]
        [StringLength(64)]
        public string PasswordHash { get; private set; }

        [Required]
        public bool Confirmed { get; private set; }

        [Required]
        public bool Active { get; private set; }

        public DateTime? LastLogin { get; private set; }

        public void SetPassword(string password, IHashing hashing)
        {
            PasswordHash = hashing.GetHash(password, Created.Millisecond.ToString());
        }

        public bool IsPasswordValid(string password, IHashing hashing)
        {
            return PasswordHash == hashing.GetHash(password, Created.Millisecond.ToString());
        }

        public void Confirm()
        {
            Confirmed = true;
            Active = true;
            Changed();
        }

        void Changed()
        {
            Updated = DateTime.UtcNow;
        }
    }
}