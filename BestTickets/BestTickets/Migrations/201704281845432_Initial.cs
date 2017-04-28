namespace BestTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RouteRequest",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Route_DeparturePlace = c.String(nullable: false),
                        Route_ArrivalPlace = c.String(nullable: false),
                        Route_Date = c.String(nullable: false),
                        RequestsCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RouteRequest");
        }
    }
}
