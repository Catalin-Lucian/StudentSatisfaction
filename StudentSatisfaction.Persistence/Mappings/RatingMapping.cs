using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Mappings
{
    internal abstract class RatingMapping
    {
        internal static void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever()
                .IsRequired();
        }
    }
}
