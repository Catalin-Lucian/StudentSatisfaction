using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.EntityFrameworkCore.Migrations;


namespace StudentSatisfaction.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name:"Date Personale",
                columns: table=> new
                    {
                        Id=table.Column<Guid>(nullable: false),
                        Nume=table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                        Prenume=table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                        Varsta=table.Column<int>(nullable:true),
                        An_nastere=table.Column<int>(nullable:true),
                        Email=table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                        Telefon=table.Column<int>(nullable:true),
                        Id_date_facultate=table.Column<Guid>(nullable:false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Date", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id_date_facultate",
                        column: x=>x.Id_date_facultate,
                        principalTable: "Date Facultate",
                        principalColumn:"Id",
                        onDelete:ReferentialAction.Cascade
                        );
                }
            );

            migrationBuilder.CreateTable(
                name: "Date Facultate",
                columns: table=>new
                {
                    Id=table.Column<Guid>(nullable:false),
                    Start=table.Column<DateTime>(nullable:false),
                    End=table.Column<DateTime>(nullable:false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Date_Facultate", x => x.Id);

                }
            );

            migrationBuilder.CreateTable(
                name: "Utilizatori",
                columns:table=>new
                {
                    Id=table.Column<Guid>(nullable:false),
                    Type=table.Column<string>(nullable:false),
                    Id_Date_Personale=table.Column<Guid>(nullable:false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizatori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Id_Date_Personale",
                        column: x => x.Id_Date_Personale,
                        principalTable: "Date Personale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                        );
                }
            );

            migrationBuilder.CreateTable(
                name: "Login",
                columns:table=>new
                {
                    Id=table.Column<Guid>(nullable:false),
                    Username=table.Column<string>(nullable:false),
                    Password=table.Column<string>(nullable:false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Login", x => x.Id);
                }

            );
        }
    }
}
