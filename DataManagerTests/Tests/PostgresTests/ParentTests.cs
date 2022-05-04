using Dapper;
using DataManager;
using DataManager.Adapters;
using DataManagerTests.Entities.Postgres;
using DataManagerTests.Fixtures;
using Xunit;

namespace DataManagerTests.Tests.PostgresTests;



[Collection("PostgresDatabaseCollection")]
public class ParentTests : PostgresInstanceFixture
{
    public ParentTests(PostgresSetupFixture fixture) : base(fixture) { }
    
    [Fact]
    public void create_parent_test()
    {
        /* Arrange */
        var dataManager = new Manager(DbFactory, new PostgresAdapter());
        var parent = new ParentEntity
        {
            ParentId = 1,
            ParentUuid = default,
            ParentName = "Test",
            ParentAge = 40,
            ParentWealth = 567.89,
            Inserted = default,
            Changed = default
        };

        /* Act */
        dataManager.InsertOne(parent);
        var inserted = DbFactory.CreateConnection()
            .ExecuteScalar<int>($"select count(1) from {ParentEntity.GetSchema()}.{ParentEntity.GetTargetTable()}");

        /* Assert */
        Assert.Equal(1, inserted);
    }
    
    
    [Fact]
    public void read_parent_test()
    {
        /* Arrange */
        var dataManager = new Manager(DbFactory, new PostgresAdapter());
        var parent = new ParentEntity
        {
            ParentId = 1,
            ParentUuid = default,
            ParentName = "Test",
            ParentAge = 40,
            ParentWealth = 567.89,
            Inserted = default,
            Changed = default
        };
        dataManager.InsertOne(parent);
        
        /* Act */
        var dbParent = dataManager.QueryOne<ParentEntity>($"where parent_id = {parent.ParentId}");

        /* Assert */
        Assert.NotNull(dbParent);
        Assert.Equal(parent.ParentName, dbParent.ParentName);
        Assert.Equal(parent.ParentAge, dbParent.ParentAge);
        Assert.Equal(parent.ParentWealth, dbParent.ParentWealth);
    }
    
    
    [Fact]
    public void merge_create_parent_test()
    {
        /* Arrange */
        var dataManager = new Manager(DbFactory, new PostgresAdapter());
        var parent = new ParentEntity
        {
            ParentId = 1,
            ParentUuid = default,
            ParentName = "Parent1",
            ParentAge = 40,
            ParentWealth = 567.89,
            Inserted = default,
            Changed = default
        };

        /* Act */
        dataManager.MergeOne(parent);
        var dbParent = dataManager.QueryOne<ParentEntity>($"where parent_id = {parent.ParentId}");
    
        /* Assert */
        Assert.Equal(parent.ParentId, dbParent.ParentId);
        Assert.Equal(parent.ParentName, dbParent.ParentName);
        Assert.Equal(parent.ParentAge, dbParent.ParentAge);
        Assert.Equal(parent.ParentWealth, dbParent.ParentWealth);
    }
    
    
    [Fact]
    public void merge_update_parent_test()
    {
        /* Arrange */
        var dataManager = new Manager(DbFactory, new PostgresAdapter());
        var parent = new ParentEntity
        {
            ParentId = 1,
            ParentUuid = default,
            ParentName = "Parent1",
            ParentAge = 40,
            ParentWealth = 567.89,
            Inserted = default,
            Changed = default
        };
    
        dataManager.InsertOne(parent);
    
        var updatedParent = new ParentEntity
        {
            ParentId = 1,
            ParentUuid = default,
            ParentName = "Parent1",
            ParentAge = 45,
            ParentWealth = 678.90,
            Inserted = default,
            Changed = default
        };
    
        /* Act */
        dataManager.MergeOne(updatedParent);
        var parentCount = dataManager.ExecuteScalar<int>($"select count(1) from {ParentEntity.GetSchema()}.{ParentEntity.GetTargetTable()}");
        var dbParent = dataManager.QueryOne<ParentEntity>($"where parent_id = {parent.ParentId}");
    
        /* Assert */
        Assert.Equal(1, parentCount);
        Assert.Equal(parent.ParentId, dbParent.ParentId);
        Assert.Equal(parent.ParentName, dbParent.ParentName);
        Assert.Equal(updatedParent.ParentAge, dbParent.ParentAge);
        Assert.Equal(updatedParent.ParentWealth, dbParent.ParentWealth);
    }
    
    
    [Fact]
    public void bulk_insert_parent_test()
    {
        /* Arrange */
        var dataManager = new Manager(DbFactory, new PostgresAdapter());
        var parents = new List<ParentEntity>();
        for (var i = 1; i <= 100000; i++)
        {
            parents.Add(new ParentEntity
            {
                ParentId = i,
                ParentUuid = default,
                ParentName = $"Test{i}",
                ParentAge = i,
                ParentWealth = 0.89 + i,
                Inserted = default,
                Changed = default
            });
        }
    
        /* Act */
        dataManager.InsertMany(parents);
        var inserted = DbFactory.CreateConnection()
            .ExecuteScalar<int>($"select count(1) from {ParentEntity.GetSchema()}.{ParentEntity.GetTargetTable()}");
    
        /* Assert */
        Assert.Equal(100000, inserted);
    }
    
    [Fact]
    public void bulk_merge_parent_test()
    {
        /* Arrange */
        var dataManager = new Manager(DbFactory, new PostgresAdapter());
        var parents = new List<ParentEntity>();
        for (var i = 1; i <= 100000; i++)
        {
            parents.Add(new ParentEntity
            {
                ParentId = i,
                ParentUuid = default,
                ParentName = $"Test{i}",
                ParentAge = i,
                ParentWealth = 0.89 + i,
                Inserted = default,
                Changed = default
            });
        }

        /* Act */
        dataManager.MergeMany(parents);
        var inserted = DbFactory.CreateConnection()
            .ExecuteScalar<int>($"select count(1) from {ParentEntity.GetSchema()}.{ParentEntity.GetTargetTable()}");

        /* Assert */
        Assert.Equal(100000, inserted);
    }
}