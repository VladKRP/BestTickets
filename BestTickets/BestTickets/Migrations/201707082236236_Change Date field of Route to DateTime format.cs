namespace BestTickets.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDatefieldofRoutetoDateTimeformat : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RouteRequest", "Route_Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RouteRequest", "Route_Date", c => c.String(nullable: false));
        }
    }
}
