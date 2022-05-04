using Dapper;
using DataManager;
using DataManager.Adapters;
using DataManagerTests.Entities.Postgres;
using DataManagerTests.Fixtures;
using DataManagerTests.Handlers.Postgres;
using Newtonsoft.Json;
using Xunit;
using NestedObjectEntity = DataManagerTests.Entities.Postgres.NestedObjectEntity;

namespace DataManagerTests.Tests.PostgresTests;

[Collection("PostgresDatabaseCollection")]
public class NestedObjectTests : PostgresInstanceFixture
{
   
    public NestedObjectTests(PostgresSetupFixture fixture) : base(fixture) { }

    [Fact]
    public void create_nested_object_test()
    {
        /* Arrange */
        SqlMapper.AddTypeHandler(new TagHandle());
        SqlMapper.AddTypeHandler(new TagsHandle());
        var dataManager = new Manager(DbFactory, new PostgresAdapter());
        var obj = new NestedObjectEntity
        {
            NestedId = 1,
            Tag = new Tag
            {
                Name = "T1",
                Age = 11,
                TotalWealth = 11.10,
                MeasurementDate = DateTime.UtcNow.AddDays(-1)
            },
            Tags = new List<Tag>()
            {
                new Tag
                {
                    Name = "T2",
                    Age = 12,
                    TotalWealth = 12.10,
                    MeasurementDate = DateTime.UtcNow.AddDays(-2)
                },
                new Tag
                {
                    Name = "T3",
                    Age = 13,
                    TotalWealth = 13.10,
                    MeasurementDate = DateTime.UtcNow.AddDays(-3)
                }
            }
        };
        dataManager.InsertOne(obj);
    
        /* Act */
        var inserted =
            dataManager.ExecuteScalar<int>(
                $"select count(1) from {NestedObjectEntity.GetSchema()}.{NestedObjectEntity.GetTargetTable()}");
    
        /* Assert */
        Assert.Equal(1, inserted);
    }
    
    
    [Fact]
    public void read_nested_object_test()
    {
        /* Arrange */
        SqlMapper.AddTypeHandler(new TagHandle());
        SqlMapper.AddTypeHandler(new TagsHandle());
        var dataManager = new Manager(DbFactory, new PostgresAdapter());
        var obj = new NestedObjectEntity
        {
            NestedId = 1,
            Tag = new Tag
            {
                Name = "T1",
                Age = 11,
                TotalWealth = 11.10,
                MeasurementDate = DateTime.UtcNow.AddDays(-1)
            },
            Tags = new List<Tag>()
            {
                new Tag
                {
                    Name = "T2",
                    Age = 12,
                    TotalWealth = 12.10,
                    MeasurementDate = DateTime.UtcNow.AddDays(-2)
                },
                new Tag
                {
                    Name = "T3",
                    Age = 13,
                    TotalWealth = 13.10,
                    MeasurementDate = DateTime.UtcNow.AddDays(-3)
                }
            }
        };
        dataManager.InsertOne(obj);
    
        /* Act */
        var dbObj = dataManager.QueryOne<NestedObjectEntity>($"where nested_id = 1");

        /* Assert */
        Assert.Equal(obj.NestedId, dbObj.NestedId);
        Assert.Equal(JsonConvert.SerializeObject(obj.Tag), JsonConvert.SerializeObject(dbObj.Tag));
        Assert.Equal(obj.Tags.Count, dbObj.Tags.Count);
        Assert.True(obj.Tags.SequenceEqual(dbObj.Tags));
        Assert.True(obj.Tags.All(dbObj.Tags.Contains));
    }
    
    
    [Fact]
    public void merge_create_nested_object_test()
    {
        /* Arrange */
        SqlMapper.AddTypeHandler(new TagHandle());
        SqlMapper.AddTypeHandler(new TagsHandle());
        var dataManager = new Manager(DbFactory, new PostgresAdapter());
        var obj = new NestedObjectEntity
        {
            NestedId = 1,
            Tag = new Tag
            {
                Name = "T1",
                Age = 11,
                TotalWealth = 11.10,
                MeasurementDate = DateTime.UtcNow.AddDays(-1)
            },
            Tags = new List<Tag>()
            {
                new Tag
                {
                    Name = "T2",
                    Age = 12,
                    TotalWealth = 12.10,
                    MeasurementDate = DateTime.UtcNow.AddDays(-2)
                },
                new Tag
                {
                    Name = "T3",
                    Age = 13,
                    TotalWealth = 13.10,
                    MeasurementDate = DateTime.UtcNow.AddDays(-3)
                }
            }
        };
        /* Act */
        dataManager.MergeOne(obj);
        var dbObject = dataManager.QueryOne<NestedObjectEntity>($"where nested_id = {obj.NestedId}");
        
        /* Assert */
        Assert.Equal(obj.NestedId, dbObject.NestedId);
        Assert.Equal(JsonConvert.SerializeObject(obj.Tag), JsonConvert.SerializeObject(dbObject.Tag));
        Assert.Equal(obj.Tags.Count, dbObject.Tags.Count);
        Assert.True(obj.Tags.SequenceEqual(dbObject.Tags));
        Assert.True(obj.Tags.All(dbObject.Tags.Contains));
    }
    
    
    [Fact]
    public void merge_update_nested_object_test()
    {
        /* Arrange */
        SqlMapper.AddTypeHandler(new TagHandle());
        SqlMapper.AddTypeHandler(new TagsHandle());
        var dataManager = new Manager(DbFactory, new PostgresAdapter());
        var obj = new NestedObjectEntity
        {
            NestedId = 1,
            Tag = new Tag
            {
                Name = "T1",
                Age = 11,
                TotalWealth = 11.10,
                MeasurementDate = DateTime.UtcNow.AddDays(-1)
            },
            Tags = new List<Tag>()
            {
                new Tag
                {
                    Name = "T2",
                    Age = 12,
                    TotalWealth = 12.10,
                    MeasurementDate = DateTime.UtcNow.AddDays(-2)
                },
                new Tag
                {
                    Name = "T3",
                    Age = 13,
                    TotalWealth = 13.10,
                    MeasurementDate = DateTime.UtcNow.AddDays(-3)
                }
            }
        };
        dataManager.InsertOne(obj);
        
        var updatedObject = new NestedObjectEntity
        {
            NestedId = 1,
            Tag = new Tag
            {
                 Name = "T1",
                 Age = 11,
                 TotalWealth = 20.20,
                 MeasurementDate = DateTime.UtcNow.AddDays(+1)
            },
            Tags = new List<Tag>()
            {
                new Tag
                {
                     Name = "T2",
                     Age = 12,
                     TotalWealth = 12.10,
                     MeasurementDate = DateTime.UtcNow.AddDays(-2)
                },
                new Tag
                {
                    Name = "T4",
                    Age = 14,
                    TotalWealth = 14.10,
                    MeasurementDate = DateTime.UtcNow.AddDays(-4)
                }
            }
        };
        
        /* Act */
        dataManager.MergeOne(updatedObject);
        var objectCount = dataManager.ExecuteScalar<int>($"select count(1) from {NestedObjectEntity.GetSchema()}.{NestedObjectEntity.GetTargetTable()}");
        var dbObject = dataManager.QueryOne<NestedObjectEntity>($"where nested_id = {obj.NestedId}");
        
        /* Assert */
        Assert.Equal(1, objectCount);
        Assert.Equal(obj.NestedId, dbObject.NestedId);
        Assert.Equal(JsonConvert.SerializeObject(updatedObject.Tag), JsonConvert.SerializeObject(dbObject.Tag));
        Assert.Equal(updatedObject.Tags.Count, dbObject.Tags.Count);
        Assert.True(updatedObject.Tags.SequenceEqual(dbObject.Tags));
        Assert.True(updatedObject.Tags.All(dbObject.Tags.Contains));
    }
}
