﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using server.Infrastructure.Context;

#nullable disable

namespace server.Migrations
{
    [DbContext(typeof(ECommerceDBContext))]
    [Migration("20241113042336_AppliedVouchers")]
    partial class AppliedVouchers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("server.Application.Models.AppliedVouchers", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int?>("CartProductID")
                        .HasColumnType("int");

                    b.Property<int?>("VoucherID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CartProductID");

                    b.HasIndex("VoucherID");

                    b.ToTable("AppliedVouchers");
                });

            modelBuilder.Entity("server.Application.Models.CartProducts", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int>("DiscountedPrice")
                        .HasColumnType("int");

                    b.Property<bool>("IsPaid")
                        .HasColumnType("bit");

                    b.Property<int>("OriginalPrice")
                        .HasColumnType("int");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("ProductID");

                    b.ToTable("CartProducts");
                });

            modelBuilder.Entity("server.Application.Models.FavoriteProducts", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("ProductID");

                    b.ToTable("FavoriteProducts");
                });

            modelBuilder.Entity("server.Application.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("AvailableQuantity")
                        .HasColumnType("int");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<int>("DiscountedPrice")
                        .HasColumnType("int");

                    b.Property<string>("ImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<int>("OriginalPrice")
                        .HasColumnType("int");

                    b.Property<string>("ProductDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UnitSold")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("server.Application.Models.ProductSubImages", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.Property<string>("subImageOne")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("subImageThree")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("subImageTwo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductSubImages");
                });

            modelBuilder.Entity("server.Application.Models.Reviews", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("ProductID");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("server.Application.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Authentication")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("User");

                    b.HasDiscriminator().HasValue("User");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("server.Application.Models.Voucher", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CustomerID")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<int?>("ProductID")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.HasIndex("ProductID");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("server.Application.Models.Customer", b =>
                {
                    b.HasBaseType("server.Application.Models.User");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<bool>("IsMember")
                        .HasColumnType("bit");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("server.Application.Models.AppliedVouchers", b =>
                {
                    b.HasOne("server.Application.Models.CartProducts", "CartProduct")
                        .WithMany()
                        .HasForeignKey("CartProductID");

                    b.HasOne("server.Application.Models.Voucher", "Voucher")
                        .WithMany()
                        .HasForeignKey("VoucherID");

                    b.Navigation("CartProduct");

                    b.Navigation("Voucher");
                });

            modelBuilder.Entity("server.Application.Models.CartProducts", b =>
                {
                    b.HasOne("server.Application.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID");

                    b.HasOne("server.Application.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("server.Application.Models.FavoriteProducts", b =>
                {
                    b.HasOne("server.Application.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID");

                    b.HasOne("server.Application.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("server.Application.Models.ProductSubImages", b =>
                {
                    b.HasOne("server.Application.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("server.Application.Models.Reviews", b =>
                {
                    b.HasOne("server.Application.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID");

                    b.HasOne("server.Application.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("server.Application.Models.Voucher", b =>
                {
                    b.HasOne("server.Application.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID");

                    b.HasOne("server.Application.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}
