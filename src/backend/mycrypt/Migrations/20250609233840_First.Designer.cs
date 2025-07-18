﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using mycrypt.Data;

#nullable disable

namespace mycrypt.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250609233840_First")]
    partial class First
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("mycrypt.Models.Entidades.Cripto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Codigo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Criptos");
                });

            modelBuilder.Entity("mycrypt.Models.Entidades.Exchange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URL")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Exchanges");
                });

            modelBuilder.Entity("mycrypt.Models.Entidades.Transaccion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Cantidad")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdCripto")
                        .HasColumnType("int");

                    b.Property<int>("IdExchange")
                        .HasColumnType("int");

                    b.Property<int>("IdUsuario")
                        .HasColumnType("int");

                    b.Property<decimal>("MontoARS")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdCripto");

                    b.HasIndex("IdExchange");

                    b.HasIndex("IdUsuario");

                    b.ToTable("Transacciones", t =>
                        {
                            t.HasCheckConstraint("CK_Monto", "MontoARS >= 0");

                            t.HasCheckConstraint("CK_Tipo", "Tipo IN ('Compra', 'Venta')");
                        });
                });

            modelBuilder.Entity("mycrypt.Models.Entidades.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Contrasenia")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPesos")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("mycrypt.Models.Entidades.Transaccion", b =>
                {
                    b.HasOne("mycrypt.Models.Entidades.Cripto", "Cripto")
                        .WithMany("Transacciones")
                        .HasForeignKey("IdCripto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mycrypt.Models.Entidades.Exchange", "Exchange")
                        .WithMany("Transacciones")
                        .HasForeignKey("IdExchange")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("mycrypt.Models.Entidades.Usuario", "Usuario")
                        .WithMany("Transacciones")
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cripto");

                    b.Navigation("Exchange");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("mycrypt.Models.Entidades.Cripto", b =>
                {
                    b.Navigation("Transacciones");
                });

            modelBuilder.Entity("mycrypt.Models.Entidades.Exchange", b =>
                {
                    b.Navigation("Transacciones");
                });

            modelBuilder.Entity("mycrypt.Models.Entidades.Usuario", b =>
                {
                    b.Navigation("Transacciones");
                });
#pragma warning restore 612, 618
        }
    }
}
