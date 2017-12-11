namespace PortfolioManagerProxy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class asd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Shares",
                c => new
                    {
                        ISIN = c.String(nullable: false, maxLength: 128),
                        ShareSymbol = c.String(),
                        Market = c.String(),
                        Volume = c.Int(nullable: false),
                        Currency = c.String(),
                    })
                .PrimaryKey(t => t.ISIN);
            
            CreateTable(
                "dbo.SharesHistoricalPrices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        OpenPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaxPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ClosePrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SaledNumber = c.Int(nullable: false),
                        Share_ISIN = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shares", t => t.Share_ISIN)
                .Index(t => t.Share_ISIN);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        SecondName = c.String(),
                        Patronymic = c.String(),
                        BirthDate = c.DateTime(nullable: false),
                        Profession = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UsersPortfolios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SharesNumber = c.Int(nullable: false),
                        Share_ISIN = c.String(maxLength: 128),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shares", t => t.Share_ISIN)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Share_ISIN)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.UsersSharesTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TransactionDateTime = c.DateTime(nullable: false),
                        TransactionType = c.Boolean(nullable: false),
                        TransactionQuantity = c.Int(nullable: false),
                        Share_ISIN = c.String(maxLength: 128),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Shares", t => t.Share_ISIN)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Share_ISIN)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersSharesTransactions", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UsersSharesTransactions", "Share_ISIN", "dbo.Shares");
            DropForeignKey("dbo.UsersPortfolios", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UsersPortfolios", "Share_ISIN", "dbo.Shares");
            DropForeignKey("dbo.SharesHistoricalPrices", "Share_ISIN", "dbo.Shares");
            DropIndex("dbo.UsersSharesTransactions", new[] { "User_Id" });
            DropIndex("dbo.UsersSharesTransactions", new[] { "Share_ISIN" });
            DropIndex("dbo.UsersPortfolios", new[] { "User_Id" });
            DropIndex("dbo.UsersPortfolios", new[] { "Share_ISIN" });
            DropIndex("dbo.SharesHistoricalPrices", new[] { "Share_ISIN" });
            DropTable("dbo.UsersSharesTransactions");
            DropTable("dbo.UsersPortfolios");
            DropTable("dbo.Users");
            DropTable("dbo.SharesHistoricalPrices");
            DropTable("dbo.Shares");
        }
    }
}
