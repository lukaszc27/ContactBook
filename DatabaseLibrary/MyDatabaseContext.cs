using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseLibrary
{
    /// <summary>
    /// Kontekst połączenia z bazą danych
    /// -> wskazanie pliku bazy danych z którego kożystamy
    /// -> określenie rekacji w bazie jakie występują między tabelami
    /// </summary>
    public class MyDatabaseContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        // wskazanie pliku bazy danych SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite("Data Source=Database.sqlite");

        /// <summary>
        /// Budowa relacyjnej bazy danych
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .HasOne(p => p.Contact)
                .WithOne(c => c.Person)
                .HasForeignKey<Contact>(c => c.PersonID);
        }
    }
}
