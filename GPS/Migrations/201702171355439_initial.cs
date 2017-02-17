namespace GPS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Graph",
                c => new
                    {
                        GPSGraphId = c.Int(nullable: false, identity: true),
                        graphName = c.String(),
                    })
                .PrimaryKey(t => t.GPSGraphId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Graph");
        }
    }
}
