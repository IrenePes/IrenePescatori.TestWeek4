﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Week4.EsempioEF2.EF;

#nullable disable

namespace Week4.EsempioEF2.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20211222115636_Migration1")]
    partial class Migration1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Week4.EsempioEF2.Entities.Azienda", b =>
                {
                    b.Property<int>("AziendaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AziendaID"), 1L, 1);

                    b.Property<int>("AnnoFondazione")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AziendaID");

                    b.ToTable("Aziende", (string)null);
                });

            modelBuilder.Entity("Week4.EsempioEF2.Entities.Impiegato", b =>
                {
                    b.Property<int>("ImpiegatoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImpiegatoID"), 1L, 1);

                    b.Property<int>("AziendaID")
                        .HasColumnType("int");

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("DataNascita")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ImpiegatoID");

                    b.HasIndex("AziendaID");

                    b.ToTable("Impiegati", (string)null);
                });

            modelBuilder.Entity("Week4.EsempioEF2.Entities.Impiegato", b =>
                {
                    b.HasOne("Week4.EsempioEF2.Entities.Azienda", "Azienda")
                        .WithMany("Impiegati")
                        .HasForeignKey("AziendaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Azienda");
                });

            modelBuilder.Entity("Week4.EsempioEF2.Entities.Azienda", b =>
                {
                    b.Navigation("Impiegati");
                });
#pragma warning restore 612, 618
        }
    }
}