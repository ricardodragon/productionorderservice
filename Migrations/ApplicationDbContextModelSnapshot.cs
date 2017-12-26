﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using productionorderservice.Data;
using productionorderservice.Model;
using System;

namespace productionorderservice.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("productionorderservice.Model.AdditionalInformation", b =>
                {
                    b.Property<int>("additionalInformationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Information")
                        .HasMaxLength(50);

                    b.Property<string>("Value")
                        .HasMaxLength(50);

                    b.Property<int?>("productId");

                    b.HasKey("additionalInformationId");

                    b.HasIndex("productId");

                    b.ToTable("AdditionalInformations");
                });

            modelBuilder.Entity("productionorderservice.Model.Phase", b =>
                {
                    b.Property<int>("phaseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("phaseCode")
                        .HasMaxLength(100);

                    b.Property<string>("phaseName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("recipeId");

                    b.HasKey("phaseId");

                    b.HasIndex("recipeId");

                    b.ToTable("Phases");
                });

            modelBuilder.Entity("productionorderservice.Model.PhaseParameter", b =>
                {
                    b.Property<int>("phaseParameterId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("maxValue")
                        .HasMaxLength(50);

                    b.Property<string>("measurementUnit")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("minValue")
                        .HasMaxLength(50);

                    b.Property<int?>("phaseId");

                    b.Property<string>("setupValue")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("tagId");

                    b.HasKey("phaseParameterId");

                    b.HasIndex("phaseId");

                    b.ToTable("PhaseParameters");
                });

            modelBuilder.Entity("productionorderservice.Model.PhaseProduct", b =>
                {
                    b.Property<int>("phaseProductId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("measurementUnit")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("phaseId");

                    b.Property<int>("phaseProductType");

                    b.Property<int>("productId");

                    b.Property<string>("value")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("phaseProductId");

                    b.HasIndex("phaseId");

                    b.ToTable("PhaseProducts");
                });

            modelBuilder.Entity("productionorderservice.Model.Product", b =>
                {
                    b.Property<int>("productId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("enabled");

                    b.Property<int[]>("parentProductsIds");

                    b.Property<string>("productCode")
                        .HasMaxLength(50);

                    b.Property<string>("productDescription")
                        .HasMaxLength(100);

                    b.Property<string>("productGTIN")
                        .HasMaxLength(50);

                    b.Property<string>("productName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("productId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("productionorderservice.Model.ProductionOrder", b =>
                {
                    b.Property<int>("productionOrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("productionOrderNumber")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("productionOrderTypeId");

                    b.Property<int>("recipeId");

                    b.HasKey("productionOrderId");

                    b.HasIndex("productionOrderTypeId");

                    b.HasIndex("recipeId");

                    b.ToTable("ProductionOrders");
                });

            modelBuilder.Entity("productionorderservice.Model.ProductionOrderType", b =>
                {
                    b.Property<int>("productionOrderTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("typeDescription")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("typeScope")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("productionOrderTypeId");

                    b.ToTable("ProductionOrderTypes");
                });

            modelBuilder.Entity("productionorderservice.Model.Recipe", b =>
                {
                    b.Property<int>("recipeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("recipeCode")
                        .HasMaxLength(50);

                    b.Property<string>("recipeName")
                        .HasMaxLength(50);

                    b.Property<int?>("recipeProductphaseProductId");

                    b.HasKey("recipeId");

                    b.HasIndex("recipeProductphaseProductId");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("productionorderservice.Model.Tag", b =>
                {
                    b.Property<int>("tagId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("tagDescription");

                    b.Property<string>("tagName");

                    b.Property<int>("thingGroupId");

                    b.HasKey("tagId");

                    b.HasIndex("thingGroupId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("productionorderservice.Model.ThingGroup", b =>
                {
                    b.Property<int>("thingGroupId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("groupCode");

                    b.Property<string>("groupName");

                    b.HasKey("thingGroupId");

                    b.ToTable("ThingGroups");
                });

            modelBuilder.Entity("productionorderservice.Model.AdditionalInformation", b =>
                {
                    b.HasOne("productionorderservice.Model.Product")
                        .WithMany("additionalInformation")
                        .HasForeignKey("productId");
                });

            modelBuilder.Entity("productionorderservice.Model.Phase", b =>
                {
                    b.HasOne("productionorderservice.Model.Recipe")
                        .WithMany("phases")
                        .HasForeignKey("recipeId");
                });

            modelBuilder.Entity("productionorderservice.Model.PhaseParameter", b =>
                {
                    b.HasOne("productionorderservice.Model.Phase")
                        .WithMany("phaseParameters")
                        .HasForeignKey("phaseId");
                });

            modelBuilder.Entity("productionorderservice.Model.PhaseProduct", b =>
                {
                    b.HasOne("productionorderservice.Model.Phase")
                        .WithMany("phaseProducts")
                        .HasForeignKey("phaseId");
                });

            modelBuilder.Entity("productionorderservice.Model.ProductionOrder", b =>
                {
                    b.HasOne("productionorderservice.Model.ProductionOrderType", "productionOrderType")
                        .WithMany()
                        .HasForeignKey("productionOrderTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("productionorderservice.Model.Recipe", "recipe")
                        .WithMany()
                        .HasForeignKey("recipeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("productionorderservice.Model.Recipe", b =>
                {
                    b.HasOne("productionorderservice.Model.PhaseProduct", "recipeProduct")
                        .WithMany()
                        .HasForeignKey("recipeProductphaseProductId");
                });

            modelBuilder.Entity("productionorderservice.Model.Tag", b =>
                {
                    b.HasOne("productionorderservice.Model.ThingGroup", "thingGroup")
                        .WithMany()
                        .HasForeignKey("thingGroupId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
