using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Mappings
{
    internal abstract class NotificationMapping
    {
        internal static void Map(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>()
                .Property(q => q.Id)
                .HasColumnName("Id")
                .IsRequired()
                .ValueGeneratedNever();

            modelBuilder.Entity<Notification>()
                .Property(q => q.UserId)
                .HasColumnName("UserId")
                .IsRequired()
                .ValueGeneratedNever();
        }
    }
}
