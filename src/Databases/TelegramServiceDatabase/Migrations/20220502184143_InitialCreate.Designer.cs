﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using TelegramServiceDatabase;

namespace TelegramServiceDatabase.Migrations
{
    [DbContext(typeof(TelegramDbContext))]
    [Migration("20220502184143_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("TelegramServiceDatabase.Entities.UserEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint")
                        .HasColumnName("chat_id");

                    b.Property<string>("FirstName")
                        .HasColumnType("text")
                        .HasColumnName("firstname");

                    b.Property<DateTime>("LastAction")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_action");

                    b.Property<string>("LastName")
                        .HasColumnType("text")
                        .HasColumnName("lastname");

                    b.Property<string>("Nickname")
                        .HasColumnType("text")
                        .HasColumnName("nickname");

                    b.Property<long>("TelegramId")
                        .HasColumnType("bigint")
                        .HasColumnName("telegram_id");

                    b.HasKey("Id");

                    b.HasIndex("ChatId")
                        .IsUnique()
                        .HasDatabaseName("IX_users_chat_id");

                    b.HasIndex("FirstName")
                        .HasDatabaseName("IX_users_firstname");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("IX_users_id");

                    b.HasIndex("LastAction")
                        .HasDatabaseName("IX_users_last_action");

                    b.HasIndex("LastName")
                        .HasDatabaseName("IX_users_lastname");

                    b.HasIndex("Nickname")
                        .HasDatabaseName("IX_users_nickname");

                    b.HasIndex("TelegramId")
                        .IsUnique()
                        .HasDatabaseName("IX_users_telegram_id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("TelegramServiceDatabase.Entities.UserStateEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Balance")
                        .HasColumnType("double precision")
                        .HasColumnName("balance");

                    b.Property<string>("BanReason")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("ban_reason");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint")
                        .HasColumnName("user_id");

                    b.Property<string>("UserStatusType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("state_type");

                    b.Property<int>("WarningNumber")
                        .HasColumnType("integer")
                        .HasColumnName("warning_number");

                    b.HasKey("Id");

                    b.HasIndex("Balance")
                        .HasDatabaseName("IX_users_state_balance");

                    b.HasIndex("BanReason")
                        .HasDatabaseName("IX_users_state_ban_reason");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasDatabaseName("IX_users_state_id");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasDatabaseName("IX_users_state_user_id");

                    b.HasIndex("UserStatusType")
                        .HasDatabaseName("IX_users_state_state_type");

                    b.HasIndex("WarningNumber")
                        .HasDatabaseName("IX_users_state_warning_number");

                    b.ToTable("users_state");
                });

            modelBuilder.Entity("TelegramServiceDatabase.Entities.UserStateEntity", b =>
                {
                    b.HasOne("TelegramServiceDatabase.Entities.UserEntity", "User")
                        .WithOne("UserState")
                        .HasForeignKey("TelegramServiceDatabase.Entities.UserStateEntity", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TelegramServiceDatabase.Entities.UserEntity", b =>
                {
                    b.Navigation("UserState");
                });
#pragma warning restore 612, 618
        }
    }
}
