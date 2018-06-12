namespace ArticoleCalarie.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nonrequiredpostalcode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Addresses", "PostalCode", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Addresses", "PostalCode", c => c.String(nullable: false));
        }
    }
}
