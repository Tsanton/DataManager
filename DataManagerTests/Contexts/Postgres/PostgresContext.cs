using System;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace DataManagerTests.Contexts.Postgres
{
    public partial class PostgresContext : DbContext
    {
        private readonly string _connStr;
        public PostgresContext(string connStr)
        {
            _connStr = connStr;
        }

        public PostgresContext(DbContextOptions<PostgresContext> options) : base(options) { }


        public virtual DbSet<Parent> Parents { get; set; }
        public virtual DbSet<Child> Children { get; set; }
        public virtual DbSet<NestedObject> ComplexObjects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(_connStr, pgOptions =>
                {
                    pgOptions.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                    pgOptions.ExecutionStrategy(c => new NpgsqlRetryingExecutionStrategy(c, 10));
                }).UseSnakeCaseNamingConvention();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parent>(entity =>
            {
                entity.HasKey(e => e.ParentId)
                    .HasName("parent_pkey");

                entity.ToTable("parents", "public");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.Property(e => e.Changed)
                    .HasColumnName("changed")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.ParentName)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("parent_name");

                entity.Property(e => e.ParentAge)
                    .IsRequired()
                    .HasColumnName("parent_age");

                entity.Property(e => e.ParentWealth)
                    .IsRequired()
                    .HasColumnName("parent_wealth");

                entity.Property(e => e.Inserted)
                    .HasColumnName("inserted")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.ParentUuid)
                    .HasColumnName("parent_uuid")
                    .HasDefaultValueSql("gen_random_uuid()");
            });


            modelBuilder.Entity<Child>(entity =>
            {
                entity.HasKey(e => e.ChildId)
                    .HasName("child_pkey");

                entity.ToTable("children", "public");

                entity.Property(e => e.ChildId).HasColumnName("child_id");

                entity.Property(e => e.ChildUuid)
                    .HasColumnName("child_uuid")
                    .HasDefaultValueSql("gen_random_uuid()");

                entity.Property(e => e.ParentId)
                    .HasColumnName("parent_id");

                entity.Property(e => e.ChildName)
                    .IsRequired()
                    .HasColumnType("character varying")
                    .HasColumnName("child_name");

                entity.Property(e => e.ChildAge)
                    .IsRequired()
                    .HasColumnName("child_age");

                entity.Property(e => e.ChildWealth)
                    .IsRequired()
                    .HasColumnName("child_wealth");

                entity.Property(e => e.Changed)
                    .HasColumnName("changed")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Inserted)
                    .HasColumnName("inserted")
                    .HasDefaultValueSql("now()");

                entity.HasOne(d => d.Parent)
                      .WithMany(p => p.Children)
                      .HasForeignKey(x => x.ParentId)
                      .HasConstraintName("FK_children_parent_relation");
            });

            modelBuilder.Entity<NestedObject>(entity =>
            {

                entity.HasKey(e => e.NestedId)
                    .HasName("nested_object_pkey");

                entity.ToTable("nested_objects", "public");

                entity.Property(e => e.NestedId).HasColumnName("nested_id");

                entity.Property(e => e.Tag)
                    .IsRequired()
                    .HasColumnType("jsonb")
                    .HasColumnName("tag");

                entity.Property(e => e.Tags)
                    .IsRequired()
                    .HasColumnType("jsonb")
                    .HasColumnName("tags");

                entity.Property(e => e.Changed)
                    .HasColumnName("changed")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Inserted)
                    .HasColumnName("inserted")
                    .HasDefaultValueSql("now()");
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
