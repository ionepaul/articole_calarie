namespace ArticoleCalarie.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageOnOrderItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "ImageName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItems", "ImageName");
        }
    }
}
