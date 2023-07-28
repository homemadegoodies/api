﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Data.Contexts;
using Data.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(GoodiesDataContext))]
    [Migration("20230727100558_InitialDatabase")]
    partial class InitialDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseSerialColumns(modelBuilder);

            modelBuilder.Entity("Data.Models.Domain.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<List<CartProduct>>("CartProducts")
                        .HasColumnType("jsonb")
                        .HasColumnName("cart_products");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<Guid>("KitchenId")
                        .HasColumnType("uuid")
                        .HasColumnName("kitchen_id");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("total_price");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("KitchenId");

                    b.ToTable("carts");
                });

            modelBuilder.Entity("Data.Models.Domain.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("address");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("country");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsCustomer")
                        .HasColumnType("boolean")
                        .HasColumnName("is_customer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("last_name");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_hash");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("text")
                        .HasColumnName("reset_token");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_salt");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("postal_code");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("text")
                        .HasColumnName("profile_picture");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("province");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("reset_token_expires");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("username");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("text")
                        .HasColumnName("verification_token");

                    b.Property<DateTime?>("VerifiedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("verified_at");

                    b.HasKey("Id");

                    b.ToTable("customers");
                });

            modelBuilder.Entity("Data.Models.Domain.Fave", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<List<FaveProduct>>("FaveProducts")
                        .HasColumnType("jsonb")
                        .HasColumnName("fave_products");

                    b.Property<Guid>("KitchenId")
                        .HasColumnType("uuid")
                        .HasColumnName("kitchen_id");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("KitchenId");

                    b.ToTable("faves");
                });

            modelBuilder.Entity("Data.Models.Domain.Kitchen", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("city");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Prices")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("prices");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("VendorId")
                        .HasColumnType("uuid")
                        .HasColumnName("vendor_id");

                    b.HasKey("Id");

                    b.HasIndex("VendorId");

                    b.ToTable("kitchens");
                });

            modelBuilder.Entity("Data.Models.Domain.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<DateTime?>("DeliveryDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("delivery_date");

                    b.Property<Guid>("KitchenId")
                        .HasColumnType("uuid")
                        .HasColumnName("kitchen_id");

                    b.Property<List<OrderProduct>>("OrderProducts")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("order_products");

                    b.Property<PaymentDetail>("PaymentDetails")
                        .HasColumnType("jsonb")
                        .HasColumnName("payment_details");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("double precision")
                        .HasColumnName("total_price");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<Guid>("VendorId")
                        .HasColumnType("uuid")
                        .HasColumnName("vendor_id");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("VendorId");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("Data.Models.Domain.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Calories")
                        .HasColumnType("integer")
                        .HasColumnName("calories");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("description");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("image_url");

                    b.Property<List<Ingredient>>("Ingredients")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("ingredients");

                    b.Property<Guid>("KitchenId")
                        .HasColumnType("uuid")
                        .HasColumnName("kitchen_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<double>("Price")
                        .HasColumnType("double precision")
                        .HasColumnName("price");

                    b.Property<List<Step>>("Recipe")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("recipe");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id");

                    b.HasIndex("KitchenId");

                    b.ToTable("products");
                });

            modelBuilder.Entity("Data.Models.Domain.Vendor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("address");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("city");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("country");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("first_name");

                    b.Property<bool>("IsVendor")
                        .HasColumnType("boolean")
                        .HasColumnName("is_vendor");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("last_name");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_hash");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("text")
                        .HasColumnName("reset_token");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("password_salt");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("postal_code");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("text")
                        .HasColumnName("profile_picture");

                    b.Property<string>("Province")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("province");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("reset_token_expires");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("username");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("text")
                        .HasColumnName("verification_token");

                    b.Property<DateTime?>("VerifiedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("verified_at");

                    b.HasKey("Id");

                    b.ToTable("vendors");
                });

            modelBuilder.Entity("Data.Models.Domain.Cart", b =>
                {
                    b.HasOne("Data.Models.Domain.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.Domain.Kitchen", "Kitchen")
                        .WithMany()
                        .HasForeignKey("KitchenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Kitchen");
                });

            modelBuilder.Entity("Data.Models.Domain.Fave", b =>
                {
                    b.HasOne("Data.Models.Domain.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.Domain.Kitchen", "Kitchen")
                        .WithMany()
                        .HasForeignKey("KitchenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Kitchen");
                });

            modelBuilder.Entity("Data.Models.Domain.Kitchen", b =>
                {
                    b.HasOne("Data.Models.Domain.Vendor", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("Data.Models.Domain.Order", b =>
                {
                    b.HasOne("Data.Models.Domain.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.Domain.Vendor", "Vendor")
                        .WithMany()
                        .HasForeignKey("VendorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Vendor");
                });

            modelBuilder.Entity("Data.Models.Domain.Product", b =>
                {
                    b.HasOne("Data.Models.Domain.Kitchen", "Kitchen")
                        .WithMany()
                        .HasForeignKey("KitchenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kitchen");
                });
#pragma warning restore 612, 618
        }
    }
}