using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace StudentSatisfaction.Persistence.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot:ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            modelBuilder.Entity("TripLooking.Entities.Users.Utilizator", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Type")
                    .HasColumnType("nvarchar(max)");
                b.Property<Guid>("Id_Date_Personale")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("Id");
                b.HasIndex("Id_Date_Personale");
                b.ToTable("Utilizator");
            });

#pragma warning restore 612, 618

        }
    }
}
