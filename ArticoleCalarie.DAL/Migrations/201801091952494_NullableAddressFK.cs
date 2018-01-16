namespace ArticoleCalarie.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableAddressFK : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "BillingAddressId", "dbo.Addresses");
            DropForeignKey("dbo.AspNetUsers", "DeliveryAddressId", "dbo.Addresses");
            DropIndex("dbo.AspNetUsers", new[] { "BillingAddressId" });
            DropIndex("dbo.AspNetUsers", new[] { "DeliveryAddressId" });
            AlterColumn("dbo.AspNetUsers", "BillingAddressId", c => c.Int());
            AlterColumn("dbo.AspNetUsers", "DeliveryAddressId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "BillingAddressId");
            CreateIndex("dbo.AspNetUsers", "DeliveryAddressId");
            AddForeignKey("dbo.AspNetUsers", "BillingAddressId", "dbo.Addresses", "Id");
            AddForeignKey("dbo.AspNetUsers", "DeliveryAddressId", "dbo.Addresses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "DeliveryAddressId", "dbo.Addresses");
            DropForeignKey("dbo.AspNetUsers", "BillingAddressId", "dbo.Addresses");
            DropIndex("dbo.AspNetUsers", new[] { "DeliveryAddressId" });
            DropIndex("dbo.AspNetUsers", new[] { "BillingAddressId" });
            AlterColumn("dbo.AspNetUsers", "DeliveryAddressId", c => c.Int(nullable: false));
            AlterColumn("dbo.AspNetUsers", "BillingAddressId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "DeliveryAddressId");
            CreateIndex("dbo.AspNetUsers", "BillingAddressId");
            AddForeignKey("dbo.AspNetUsers", "DeliveryAddressId", "dbo.Addresses", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUsers", "BillingAddressId", "dbo.Addresses", "Id", cascadeDelete: true);
        }
    }
}
