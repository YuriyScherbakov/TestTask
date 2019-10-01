using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TestTaskPerson.Api.DAL
{
    public sealed class PersonContext : DbContext
    {
        public PersonContext(DbContextOptions<PersonContext> options)
            : base(options)
        {
            // create new DB if not exists
            if (Database.EnsureCreated())
            {
                // gets test data
                var loader = new PersonLoader(this);
                loader.LoadPersonsFromOpenApi();
            }
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
    }

    public class Person
    {
        public int PersonId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public DateTime? Birthday { get; set; }

        [Required]
        [StringLength(100)]
        public string Surname { get; set; }
        public ICollection<CreditCard> CreditCards { get; set; }
    }

    public class CreditCard
    {
        public int CreditCardId { get; set; }

        [Required]
        [StringLength(100)]
        public string Number { get; set; }

        [Required]
        [StringLength(100)]
        public string Pin { get; set; }

        [Required]
        [StringLength(100)]
        public string Security { get; set; }
        public int PersonId { get; set; }

        [JsonIgnore]
        public Person Person { get; set; }
    }
}
