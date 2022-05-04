using Dapper;
using DataManager;
using DataManager.Adapters;
using DataManagerTests.Entities.Postgres;
using DataManagerTests.Fixtures;
using Xunit;

namespace DataManagerTests.Tests.PostgresTests;


[Collection("PostgresDatabaseCollection")]
public class CreateTests : PostgresInstanceFixture
{
    public CreateTests(PostgresSetupFixture fixture) : base(fixture) { }
    
    [Fact]
    public void create_child_test()
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
        var child = new ChildEntity
        {
            ChildId = 1,
            ChildUuid = default,
            ParentId = 1,
            ChildName = "Child1",
            ChildAge = 10,
            ChildWealth = 123.45,
            Inserted = default,
            Changed = default
        };
    
        /* Act */
        dataManager.InsertOne(parent);
        dataManager.InsertOne(child);
        var inserted = DbFactory.CreateConnection()
            .ExecuteScalar<int>($"select count(1) from {ChildEntity.GetSchema()}.{ChildEntity.GetTargetTable()}");
    
        /* Assert */
        Assert.Equal(1, inserted);
    }

    
    [Fact]
    public void read_child_test()
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
        var child = new ChildEntity
        {
            ChildId = 1,
            ChildUuid = default,
            ParentId = 1,
            ChildName = "Child1",
            ChildAge = 10,
            ChildWealth = 123.45,
            Inserted = default,
            Changed = default
        };
        dataManager.InsertOne(parent);
        dataManager.InsertOne(child);
        
        /* Act */
        var dbChild = dataManager.QueryOne<ChildEntity>($"where child_id = {child.ChildId}");

        /* Assert */
        Assert.NotNull(dbChild);
        Assert.Equal(dbChild.ChildId, dbChild.ChildId);
        Assert.Equal(dbChild.ParentId, dbChild.ParentId);
        Assert.Equal(dbChild.ChildName, dbChild.ChildName);
        Assert.Equal(dbChild.ChildAge, dbChild.ChildAge);
        Assert.Equal(dbChild.ChildWealth, dbChild.ChildWealth);
    }
    
    [Fact]
    public void merge_creat_child_test()
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
        var child = new ChildEntity
        {
            ChildId = 1,
            ChildUuid = default,
            ParentId = 1,
            ChildName = "Child1",
            ChildAge = 10,
            ChildWealth = 123.45,
            Inserted = default,
            Changed = default
        };
        dataManager.InsertOne(parent);

        /* Act */
        dataManager.MergeOne(child);
        var dbChild = dataManager.QueryOne<ChildEntity>($"where child_id = {child.ChildId}");

        /* Assert */
        Assert.NotNull(dbChild);
        Assert.Equal(dbChild.ChildId, dbChild.ChildId);
        Assert.Equal(dbChild.ParentId, dbChild.ParentId);
        Assert.Equal(dbChild.ChildName, dbChild.ChildName);
        Assert.Equal(dbChild.ChildAge, dbChild.ChildAge);
        Assert.Equal(dbChild.ChildWealth, dbChild.ChildWealth);
    }
    
    
    [Fact]
    public void merge_update_child_test()
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
        var child = new ChildEntity
        {
            ChildId = 1,
            ChildUuid = default,
            ParentId = 1,
            ChildName = "Child1",
            ChildAge = 10,
            ChildWealth = 123.45,
            Inserted = default,
            Changed = default
        };
        dataManager.InsertOne(parent);
        dataManager.InsertOne(child);
        
        var updateChild = new ChildEntity
        {
            ChildId = 1,
            ChildUuid = default,
            ParentId = 1,
            ChildName = "Child1",
            ChildAge = 11,
            ChildWealth = 234.56,
            Inserted = default,
            Changed = default
        };

        /* Act */
        dataManager.MergeOne(updateChild);
        var childCount = DbFactory.CreateConnection()
            .ExecuteScalar<int>($"select count(1) from {ChildEntity.GetSchema()}.{ChildEntity.GetTargetTable()}");
        var dbChild = dataManager.QueryOne<ChildEntity>($"where child_id = {updateChild.ChildId}");

        /* Assert */
        Assert.Equal(1, childCount);
        Assert.Equal(child.ChildId, dbChild.ChildId);
        Assert.Equal(child.ParentId, dbChild.ParentId);
        Assert.Equal(child.ChildName, dbChild.ChildName);
        Assert.Equal(updateChild.ChildAge, dbChild.ChildAge);
        Assert.Equal(updateChild.ChildWealth, dbChild.ChildWealth);
    }
}
