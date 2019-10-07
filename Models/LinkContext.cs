using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TestTaskNETCore.Models
{
    public class LinkContext : DbContext
    {

        private static readonly bool[] _migrated = { false };

        public DbSet<Link> Links { get; set; }

        public LinkContext(DbContextOptions<LinkContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}