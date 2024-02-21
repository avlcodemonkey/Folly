using Folly.Domain.Models;
using Folly.Extensions.Services;
using Folly.Utils;

namespace Folly.Web.Tests.Extensions.Services;

public class AuditLogServiceExtensionsTests {
    [Fact]
    public void SelectSingleAsDTO_ReturnsProjectedDTO() {
        // arrange
        var user = new User { FirstName = "first", LastName = "last" };
        var auditLog = new AuditLog {
            Id = 1, BatchId = Guid.NewGuid(), Date = DateTime.MinValue, Entity = "test", State = Microsoft.EntityFrameworkCore.EntityState.Modified,
            PrimaryKey = 100, UserId = 200, OldValues = "old", NewValues = "new", User = user
        };
        var auditLogs = new List<AuditLog> { auditLog }.AsQueryable();

        // act
        var dtos = auditLogs.SelectAsDTO().ToList();

        // assert
        Assert.NotNull(dtos);
        var singleDTO = Assert.Single(dtos);

        Assert.Equal(auditLog.Id, singleDTO.Id);
        Assert.Equal(auditLog.BatchId, singleDTO.BatchId);
        Assert.Equal(auditLog.Date, singleDTO.Date);
        Assert.Equal(auditLog.Entity, singleDTO.Entity);
        Assert.Equal(auditLog.State, singleDTO.State);
        Assert.Equal(auditLog.PrimaryKey, singleDTO.PrimaryKey);
        Assert.Equal(auditLog.OldValues, singleDTO.OldValues);
        Assert.Equal(auditLog.NewValues, singleDTO.NewValues);
        Assert.Equal(auditLog.User.FirstName, singleDTO.UserFirstName);
        Assert.Equal(auditLog.User.LastName, singleDTO.UserLastName);
    }

    [Fact]
    public void SelectMultipleAsDTO_ReturnsProjectedDTOs() {
        // arrange
        var auditLog1 = new AuditLog {
            Id = 2, BatchId = Guid.NewGuid(), Date = DateTime.MinValue, Entity = "test2", State = Microsoft.EntityFrameworkCore.EntityState.Added,
            PrimaryKey = 200, UserId = 200, OldValues = "old2", NewValues = "new2"
        };
        var auditLog2 = new AuditLog {
            Id = 3, BatchId = Guid.NewGuid(), Date = DateTime.MinValue, Entity = "test3", State = Microsoft.EntityFrameworkCore.EntityState.Deleted,
            PrimaryKey = 300, UserId = 300, OldValues = "old3", NewValues = "new3"
        };
        var auditLogs = new List<AuditLog> { auditLog1, auditLog2 }.AsQueryable();

        // act
        var dtos = auditLogs.SelectAsDTO().ToList();

        // assert
        Assert.NotNull(dtos);
        Assert.Equal(2, dtos.Count);

        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.Id, x.Id),
            x => Assert.Equal(auditLog2.Id, x.Id)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.BatchId, x.BatchId),
            x => Assert.Equal(auditLog2.BatchId, x.BatchId)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.Date, x.Date),
            x => Assert.Equal(auditLog2.Date, x.Date)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.Entity, x.Entity),
            x => Assert.Equal(auditLog2.Entity, x.Entity)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.State, x.State),
            x => Assert.Equal(auditLog2.State, x.State)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.PrimaryKey, x.PrimaryKey),
            x => Assert.Equal(auditLog2.PrimaryKey, x.PrimaryKey)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.OldValues, x.OldValues),
            x => Assert.Equal(auditLog2.OldValues, x.OldValues)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.NewValues, x.NewValues),
            x => Assert.Equal(auditLog2.NewValues, x.NewValues)
        );
        Assert.Collection(dtos,
            x => Assert.Equal("", x.UserFirstName),
            x => Assert.Equal("", x.UserFirstName)
        );
        Assert.Collection(dtos,
            x => Assert.Null(x.UserLastName),
            x => Assert.Null(x.UserLastName)
        );
    }

    [Fact]
    public void SelectSingleAsSearchResultDTO_ReturnsProjectedDTO() {
        // arrange
        var user = new User { FirstName = "first", LastName = "last" };
        var auditLog = new AuditLog {
            Id = 1, BatchId = Guid.NewGuid(), Date = DateTime.MinValue, Entity = "test", State = Microsoft.EntityFrameworkCore.EntityState.Modified,
            PrimaryKey = 100, UserId = 200, OldValues = "old", NewValues = "new", User = user
        };
        var auditLogs = new List<AuditLog> { auditLog }.AsQueryable();

        // act
        var dtos = auditLogs.SelectAsSearchResultDTO().ToList();

        // assert
        Assert.NotNull(dtos);
        var singleDTO = Assert.Single(dtos);

        Assert.Equal(auditLog.Id, singleDTO.Id);
        Assert.Equal(auditLog.BatchId, singleDTO.BatchId);
        Assert.Equal(auditLog.Date.ToString("u"), singleDTO.UniversalDate);
        Assert.Equal(auditLog.Entity, singleDTO.Entity);
        Assert.Equal(auditLog.State.ToString(), singleDTO.State);
        Assert.Equal(NameHelper.DisplayName(user.FirstName, user.LastName), singleDTO.UserFullName);
    }

    [Fact]
    public void SelectMultipleAsSearchResultsDTO_ReturnsProjectedDTOs() {
        // arrange
        var user = new User { FirstName = "first", LastName = "last" };
        var auditLog1 = new AuditLog {
            Id = 2, BatchId = Guid.NewGuid(), Date = DateTime.MinValue, Entity = "test2", State = Microsoft.EntityFrameworkCore.EntityState.Added,
            PrimaryKey = 200, UserId = 200, OldValues = "old2", NewValues = "new2", User = user
        };
        var auditLog2 = new AuditLog {
            Id = 3, BatchId = Guid.NewGuid(), Date = DateTime.MinValue, Entity = "test3", State = Microsoft.EntityFrameworkCore.EntityState.Deleted,
            PrimaryKey = 300, UserId = 300, OldValues = "old3", NewValues = "new3", User = user
        };
        var auditLogs = new List<AuditLog> { auditLog1, auditLog2 }.AsQueryable();

        // act
        var dtos = auditLogs.SelectAsSearchResultDTO().ToList();

        // assert
        Assert.NotNull(dtos);
        Assert.Equal(2, dtos.Count);

        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.Id, x.Id),
            x => Assert.Equal(auditLog2.Id, x.Id)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.BatchId, x.BatchId),
            x => Assert.Equal(auditLog2.BatchId, x.BatchId)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.Date.ToString("u"), x.UniversalDate),
            x => Assert.Equal(auditLog2.Date.ToString("u"), x.UniversalDate)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.Entity, x.Entity),
            x => Assert.Equal(auditLog2.Entity, x.Entity)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(auditLog1.State.ToString(), x.State),
            x => Assert.Equal(auditLog2.State.ToString(), x.State)
        );
        Assert.Collection(dtos,
            x => Assert.Equal(NameHelper.DisplayName(user.FirstName, user.LastName), x.UserFullName),
            x => Assert.Equal(NameHelper.DisplayName(user.FirstName, user.LastName), x.UserFullName)
        );
    }
}
