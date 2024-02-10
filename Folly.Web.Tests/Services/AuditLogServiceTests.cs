using Folly.Services;
using Folly.Web.Tests.Fixtures;
using DTO = Folly.Models;

namespace Folly.Web.Tests.Services;

[Collection(nameof(DatabaseCollection))]
public class AuditLogServiceTests(DatabaseFixture fixture) {
    private readonly DatabaseFixture _Fixture = fixture;
    private readonly AuditLogService _AuditLogService = new(fixture.CreateContext());

    [Fact]
    public async Task GetLogByIdAsync_ReturnsFakeAuditLogDTO() {
        // arrange
        var fakeLog = _Fixture.CreateContext().AuditLog.Find(1L)!;

        // act
        var log = await _AuditLogService.GetLogByIdAsync(1);

        // assert
        Assert.NotNull(log);
        Assert.IsType<DTO.AuditLog>(log);
        Assert.Equal(fakeLog.BatchId, log.BatchId);
        Assert.Equal(fakeLog.Date, log.Date);
        Assert.Equal(fakeLog.Entity, log.Entity);
        Assert.Equal(fakeLog.State, log.State);
        Assert.Equal(fakeLog.PrimaryKey, log.PrimaryKey);
        Assert.Equal(fakeLog.OldValues, log.OldValues);
        Assert.Equal(fakeLog.NewValues, log.NewValues);
    }

    [Fact]
    public async Task GetLogByIdAsync_WithInvalidId_ReturnsNull() {
        // arrange
        var logIdToGet = -200;

        // act
        var log = await _AuditLogService.GetLogByIdAsync(logIdToGet);

        // assert
        Assert.Null(log);
    }

    [Fact]
    public async Task SearchLogsAsync_WithNoCriteria_ReturnsTwoLogDTOs() {
        // arrange
        using var context = _Fixture.CreateContext();
        var fakeLog1 = context.AuditLog.Find(1L)!;
        var fakeLog2 = context.AuditLog.Find(2L)!;
        var search = new DTO.AuditLogSearch { StartDate = DateOnly.FromDateTime(DateTime.MinValue), EndDate = DateOnly.FromDateTime(DateTime.MaxValue.Date) };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.NotEmpty(logs);
        Assert.IsAssignableFrom<IEnumerable<DTO.AuditLogSearchResult>>(logs);
        Assert.Equal(2, logs.Count());
        Assert.Collection(logs,
            x => Assert.Equal(fakeLog1.Id, x.Id),
            x => Assert.Equal(fakeLog2.Id, x.Id)
        );
    }

    [Fact]
    public async Task SearchLogsAsync_WithFutureStartDate_ReturnsNoDTOs() {
        // arrange
        // fixture creates two auditlogs with date=DateTime.Now, so searching for records in the future should find none
        var search = new DTO.AuditLogSearch { StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)), EndDate = DateOnly.FromDateTime(DateTime.MaxValue) };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.Empty(logs);
    }

    [Fact]
    public async Task SearchLogsAsync_WithPastStartDate_ReturnsNoDTOs() {
        // arrange
        // fixture creates two auditlogs with date=DateTime.Now, so searching for records in the past should find none
        var search = new DTO.AuditLogSearch { StartDate = DateOnly.FromDateTime(DateTime.MinValue), EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.Empty(logs);
    }

    [Fact]
    public async Task SearchLogsAsync_WithValidStartDateNoEndDate_ReturnsTwoLogDTOs() {
        // arrange
        using var context = _Fixture.CreateContext();
        var fakeLog1 = context.AuditLog.Find(1L)!;
        var fakeLog2 = context.AuditLog.Find(2L)!;
        // fixture creates two auditlogs with date=DateTime.Now
        var search = new DTO.AuditLogSearch { StartDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)), EndDate = null };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.NotEmpty(logs);
        Assert.IsAssignableFrom<IEnumerable<DTO.AuditLogSearchResult>>(logs);
        Assert.Equal(2, logs.Count());
        Assert.Collection(logs,
            x => Assert.Equal(fakeLog1.Id, x.Id),
            x => Assert.Equal(fakeLog2.Id, x.Id)
        );
    }

    [Fact]
    public async Task SearchLogsAsync_WithValidEndDateNoStartDate_ReturnsTwoLogDTOs() {
        // arrange
        using var context = _Fixture.CreateContext();
        var fakeLog1 = context.AuditLog.Find(1L)!;
        var fakeLog2 = context.AuditLog.Find(2L)!;
        // fixture creates two auditlogs with date=DateTime.Now
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)) };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.NotEmpty(logs);
        Assert.IsAssignableFrom<IEnumerable<DTO.AuditLogSearchResult>>(logs);
        Assert.Equal(2, logs.Count());
        Assert.Collection(logs,
            x => Assert.Equal(fakeLog1.Id, x.Id),
            x => Assert.Equal(fakeLog2.Id, x.Id)
        );
    }

    [Fact]
    public async Task SearchLogsAsync_WithValidBatch_ReturnsOneLogDTO() {
        // arrange
        var fakeLog1 = _Fixture.CreateContext().AuditLog.Find(1L)!;
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, BatchId = fakeLog1.BatchId };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.NotEmpty(logs);
        Assert.IsAssignableFrom<IEnumerable<DTO.AuditLogSearchResult>>(logs);
        var singleResult = Assert.Single(logs);
        Assert.Equal(fakeLog1.Id, singleResult.Id);
    }

    [Fact]
    public async Task SearchLogsAsync_WithNoValidBatch_ReturnsNoDTO() {
        // arrange
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, BatchId = Guid.NewGuid() };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.Empty(logs);
    }

    [Fact]
    public async Task SearchLogsAsync_WithValidEntity_ReturnsOneLogDTO() {
        // arrange
        var fakeLog1 = _Fixture.CreateContext().AuditLog.Find(1L)!;
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, Entity = fakeLog1.Entity };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.NotEmpty(logs);
        Assert.IsAssignableFrom<IEnumerable<DTO.AuditLogSearchResult>>(logs);
        var singleResult = Assert.Single(logs);
        Assert.Equal(fakeLog1.Id, singleResult.Id);
    }

    [Fact]
    public async Task SearchLogsAsync_WithNoValidEntity_ReturnsNoDTO() {
        // arrange
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, Entity = "gibberish" };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.Empty(logs);
    }

    [Fact]
    public async Task SearchLogsAsync_WithValidPrimaryKey_ReturnsOneLogDTO() {
        // arrange
        var fakeLog1 = _Fixture.CreateContext().AuditLog.Find(1L)!;
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, PrimaryKey = fakeLog1.PrimaryKey };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.NotEmpty(logs);
        Assert.IsAssignableFrom<IEnumerable<DTO.AuditLogSearchResult>>(logs);
        var singleResult = Assert.Single(logs);
        Assert.Equal(fakeLog1.Id, singleResult.Id);
    }

    [Fact]
    public async Task SearchLogsAsync_WithNoValidPrimaryKey_ReturnsNoDTO() {
        // arrange
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, PrimaryKey = 999 };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.Empty(logs);
    }

    [Fact]
    public async Task SearchLogsAsync_WithModifiedState_ReturnsOneLogDTO() {
        // arrange
        var fakeLog1 = _Fixture.CreateContext().AuditLog.Find(1L)!;
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, State = fakeLog1.State };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.NotEmpty(logs);
        Assert.IsAssignableFrom<IEnumerable<DTO.AuditLogSearchResult>>(logs);
        var singleResult = Assert.Single(logs);
        Assert.Equal(fakeLog1.Id, singleResult.Id);
    }

    [Fact]
    public async Task SearchLogsAsync_WithDeletedState_ReturnsOneLogDTO() {
        // arrange
        var fakeLog2 = _Fixture.CreateContext().AuditLog.Find(2L)!;
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, State = fakeLog2.State };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.NotEmpty(logs);
        Assert.IsAssignableFrom<IEnumerable<DTO.AuditLogSearchResult>>(logs);
        var singleResult = Assert.Single(logs);
        Assert.Equal(fakeLog2.Id, singleResult.Id);
    }

    [Fact]
    public async Task SearchLogsAsync_WithNoValidState_ReturnsNoDTO() {
        // arrange
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, State = Microsoft.EntityFrameworkCore.EntityState.Unchanged };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.Empty(logs);
    }

    [Fact]
    public async Task SearchLogsAsync_WithValidUser_ReturnsOneLogDTO() {
        // arrange
        var user1 = _Fixture.User;
        var fakeLog1 = _Fixture.CreateContext().AuditLog.Find(1L)!;
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, UserId = user1.Id };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.NotEmpty(logs);
        Assert.IsAssignableFrom<IEnumerable<DTO.AuditLogSearchResult>>(logs);
        var singleResult = Assert.Single(logs);
        Assert.Equal(fakeLog1.Id, singleResult.Id);
    }

    [Fact]
    public async Task SearchLogsAsync_WithNoValidUser_ReturnsNoDTO() {
        // arrange
        var search = new DTO.AuditLogSearch { StartDate = null, EndDate = null, UserId = 999 };

        // act
        var logs = await _AuditLogService.SearchLogsAsync(search);

        // assert
        Assert.Empty(logs);
    }
}
