using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PcWeb.Api.Models;

namespace PcWeb.Api.Data
{
    public class GroupDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Member> Members => Set<Member>();
        public DbSet<MemberGroup> MemberGroups => Set<MemberGroup>();
        public DbSet<OwedMoney> OwedMoney => Set<OwedMoney>();
        public DbSet<MemberTransaction> Transactions => Set<MemberTransaction>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberGroup>().HasData(
                new MemberGroup { Id = 1, Name = "Group 1" }
            );
            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, FirstName = "John", LastName = "Johnson", GroupId = 1 }
            );
        }
    }
}