using FluentMigrator;

namespace Data.Migrations;

/// <summary>
/// Initial migartion.
/// </summary>
[Migration(202204120001)]
public class InitialTables202204120001 : Migration
{
    /// <summary>
    /// Up migration.
    /// </summary>
    public override void Up()
    {
        Create.Table("ConnectionInfo")
            .WithColumn("Id").AsString(256).NotNullable().PrimaryKey()
            .WithColumn("UserName").AsString(512).Nullable()
            .WithColumn("Os").AsString(128).Nullable()
            .WithColumn("AppVersion").AsString(128).Nullable()
            .WithColumn("LastConnection").AsDateTimeOffset().NotNullable();

        Create.Table("ConnectionEvent")
            .WithColumn("Id").AsString(256).NotNullable().PrimaryKey()
            .WithColumn("Name").AsString(50).Nullable()
            .WithColumn("ConnectionId").AsString(256).NotNullable().ForeignKey("ConnectionInfo", "Id")
            .WithColumn("EventTime").AsDateTimeOffset().NotNullable();

        Create.Index("ConnectionEvent_ConnectionId")
            .OnTable("ConnectionEvent")
            .OnColumn("ConnectionId");
    }

    /// <summary>
    /// Down migration.
    /// </summary>
    public override void Down()
    {
        Delete.Index("ConnectionEvent_ConnectionId");
        Delete.Table("ConnectionInfo");
        Delete.Table("ConnectionEvent");
    }
}
