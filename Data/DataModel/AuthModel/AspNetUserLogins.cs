using ServerCore.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DataModel.AuthModel
{
    public class AspNetUserLogins
    {
        // Part 1 of composite primary key (set in Context)
        // Max length 128 chars, required
        public string LoginProvider { get; set; }

        // Part 2 of composite primary key (set in Context)
        // Max length 128 chars, required
        public string ProviderKey { get; set; }

        // No max length, nullable
        public string ProviderDisplayName { get; set; }

        // No max length, required
        // Original has a string for the id, 
        // not sure if it can handle the user link or if I need to get a specific foreign key to the id as a string
        public virtual User User { get; set; }
    }

    // Table creation code from the template for reference

    /*            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
     */
}
