namespace ArticoleCalarie.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Orders : DbMigration
    {
        public override void Up()
        {         
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalePercentage = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Size = c.String(),
                        Color = c.String(),
                        ProductCode = c.String(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderNumber = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        Email = c.String(nullable: false),
                        DeliveryAddressId = c.Int(nullable: false),
                        BillingAddressId = c.Int(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.BillingAddressId, cascadeDelete: false)
                .ForeignKey("dbo.Addresses", t => t.DeliveryAddressId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.OrderNumber)
                .Index(t => t.UserId)
                .Index(t => t.DeliveryAddressId)
                .Index(t => t.BillingAddressId);            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "DeliveryAddressId", "dbo.Addresses");
            DropForeignKey("dbo.Orders", "BillingAddressId", "dbo.Addresses");
            DropIndex("dbo.Orders", new[] { "BillingAddressId" });
            DropIndex("dbo.Orders", new[] { "DeliveryAddressId" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.Orders", new[] { "OrderNumber" });
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
        }
    }
}
