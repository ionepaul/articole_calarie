namespace ArticoleCalarie.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NameAndPhoneOnAddress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "FullName", c => c.String(nullable: false));
            AddColumn("dbo.Addresses", "PhoneNumber", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "PhoneNumber");
            DropColumn("dbo.Addresses", "FullName");
        }
    }
}
