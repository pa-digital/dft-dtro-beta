using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfT.DTRO.Migrations
{
    public partial class SwaCodesIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SwaCodes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "SwaCodes",
                columns: new[] { "Id", "IsActive", "IsAdmin", "Name", "Prefix", "TraId" },
                values: SeedData.Tras.ToArray());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0016e5ce-c46d-4ad4-9236-6449330dc635"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("012a8c07-e04f-4b4a-b487-590116f549f8"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("01582089-bd13-469e-b8e8-6672d1d63fc5"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("019d0fc8-cd2f-4c58-922f-b6e6ce6a9a7a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0210a160-25c5-4d45-935e-14f7def9592b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("02881862-a75d-4e43-b8ba-5e5a2656b0be"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("02cff556-78a6-42f4-979f-d3476782e526"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("030f7399-4039-4fac-982f-8083dc99233c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0322f859-cecf-4895-be20-b2839431505a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("03439b42-0638-4cf9-bffd-e225b3a4a88b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("03b8aec2-3032-48d7-8b41-fe0e77e01ca5"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("04564dd4-da2b-4ed2-a189-f1b3137afa95"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("04f4cb30-cb8b-4b07-908b-3c2ca5774473"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0554e48e-58a4-4905-a137-624c206f1b80"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("05576e80-526a-4818-a660-a8d6d9f2bd32"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("057e4fad-7baa-42e5-ac3a-d09267bbb441"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("05855d2e-5559-4456-83b6-ab7a863d292c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("05eedf45-7c56-4ea8-9cb1-260ae4bb3610"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("06f2340b-f3aa-4758-aa7e-6f3de8a43267"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("07ceb1ee-5ffc-4eb3-8980-fa64a5f56d51"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("07e37d39-e217-4797-954b-8ddc8df8d411"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0804f3b0-363f-457c-9a97-5625646db551"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("09408df1-7e09-479f-bbe9-c83f0be91750"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("09e33824-7afb-466f-bf5e-7c3dc41316e0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("09f22419-b014-473b-9120-a76f8c8ac07d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0a675406-880d-4a8f-bb63-afb428800fa4"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0a81e478-a569-4dee-9c32-c1d928de5f91"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0a9dd22d-e307-4878-a820-ed1421a8c7eb"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0ccfd949-ef0d-4716-9812-babb69de3a61"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0d890247-3fd6-4e92-b0a2-0025bbbb3c98"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0d99e3c6-b43e-41b3-bf44-43c71d097b30"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0ded0259-fbef-4e5a-84e7-2258203ac7fa"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0ef01648-175b-4dba-8ba7-a354594b2aa2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0f32f588-35f8-4f60-a3e3-ee9759e43fc4"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0f74fa03-2d91-4c7d-a2cd-1cab3cd84d76"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("0fd1354c-9172-4853-84b9-2db844bb7ffa"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("112dd1d4-3ea8-41b9-8017-4719fec4f21f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1155f335-e9d3-4e91-8a04-4a1e88b0e3e3"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("117a0f10-84b2-4c26-84ed-7109cf588eec"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("121f4633-95bb-4764-9860-ecd89b487838"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("12c56d95-4f4e-417e-a470-0973a408d5e2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("12ceb5a5-ac00-4347-a754-907124992903"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("14333987-36cd-4dd4-ba40-885b6c06649b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("14bb602d-c69e-4c85-a12c-c0aa12c1926b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1527efce-93aa-4592-9387-cd6990a7588c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("15bd8653-7bdd-4dc3-934c-9e6b7c6658fa"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("15f85e76-9dc0-47a7-b917-449def337d21"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("163ef86f-2ed0-468d-ab0e-a1bde256b899"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("17009f37-ccef-4c0b-8896-719efac9906a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1776a92a-dd9f-4243-98ec-80e897a02550"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("17afc3ab-da95-4bde-8f25-2ef12b3b452d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("17e3b93c-9ba0-47c1-8542-d556f1e5788c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("18d6a3df-3031-4e2d-b123-b8ea56b0a8a0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("18fecef3-c5de-46f7-951e-3e43f78e4150"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1a260d6f-0b59-44c2-9b36-fbe46aad283f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1a696785-76c5-405c-9758-b4dcd692d351"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1a69ddc5-6036-4c16-aba9-d551c85fcb92"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1aa8fe77-a1cd-45bb-bf80-d9e4950067ee"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1af98442-719d-4831-89ce-3534bf12c78f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1b4ed0b5-25c9-4e16-9a6d-e9a3b3ec0afc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1b594aeb-557a-408d-a1eb-1cb98d50064f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1b597bb5-cc97-4a8d-97bd-79b36b371786"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1bc3695a-4e1b-478d-b0f9-9c247514d1ab"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1c57fa54-6f4b-4dc2-8bf9-8ef9ed3042ef"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1c5a05fd-ba51-456d-889c-d6b1f01d4e94"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1cd883cf-5c6e-4871-a27f-221e458cfb44"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1d1891f5-6f0d-4e56-bfec-b0cb9e2085fd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1d93be2e-1fb3-4131-b2d7-cb5a53ea3170"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1ebb2261-b6e1-4cc1-bf9e-dc93ccef225e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1ee7ac7c-2bae-4819-91e4-cfde56ffea6d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1f0fdbe7-6930-4c48-b7d7-9288c24a2eff"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1f10a0f9-7e10-4959-afd2-0ad58de98109"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1f26d717-9641-4677-abcf-17eddd75d3dd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1f283b3d-3311-4ca1-bdf2-9dd687e1aeba"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1f88c9f4-c721-4209-8860-64ae38fef92d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1f9cc569-87a6-46da-aec8-fd38e6afc96f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1fba15eb-af90-4722-b44f-cfc6581cf177"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("1fc26488-7aab-4266-a294-b0792a383474"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("20e7280f-587e-4186-a3ba-c858e6641ce1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("21c38439-45a0-4bc6-a713-9e56bc36e654"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("223c36cf-76f3-4614-ac68-ca6be0cfeeed"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("242f5b1d-151c-4d3a-9133-c2e90790fed7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("255e578f-79f1-4a46-943d-dcbaf71408fd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("25ffb9fe-46f8-4c23-92e3-91a29a076e2e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("26c6dfe7-fdf8-4fb7-8ef8-a4fa014f261d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("26d48011-4b49-4170-bef7-c79e3593a09b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("270311ad-5d78-4e71-9b77-1110df0428a9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("2712ccb9-633f-4384-b8c4-472faa4b6f35"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("275b6447-2e50-4ca4-be4a-9dfb62e07ea3"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("296b632d-c611-42d6-8d69-643983f65c82"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("29c4b505-794f-43f8-98da-cf82a15c0168"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("2a16640b-b2a4-45ec-a0a1-ff6712d15019"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("2af22c7f-036d-48bf-adc3-ab4d4b38cf4f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("2b26c355-d808-4702-8525-2748056b6434"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("2b3dde38-7fe8-46df-967a-b48cc909cd23"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("2bde9c56-e3b1-4cce-b0d7-7f4d1b0d6e83"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("2c0fc8fa-37eb-45b6-8158-6021ef9c6265"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("2cdfade2-85c2-4b54-9b73-4bab8ac20592"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("2f0bf8f7-1c77-498c-b9f3-72808c6818ff"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("2f4a783e-04b1-4c80-af15-acd76c98cbc4"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("30bc74f7-36e5-4f65-984f-5c8b421fc0cf"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3135423a-02e6-4e7b-ac69-4d1c6b80d7e5"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("31f0b8d9-100a-49a2-b80d-aa468a5a3421"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3251b3b6-7034-48cb-9a32-ccc33bc7f252"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("32a6d064-4d6e-4d5f-93b1-9b88140703f0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("33747d53-87f5-4cfd-91e6-ae46727ac2cf"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("348ef449-470a-4f76-9881-abf4832d46b9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3494e5ec-a1aa-4a43-a7bb-5cb19d2c1b45"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("34d827a9-3b45-4f62-a41c-9583c5d4aac1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("34e24766-25cf-497b-b37d-5114036df81e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("353b397f-6bee-49a3-ae57-304e74374687"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("369736e9-cfc9-458b-8f11-4e48195c7b42"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("373caefd-e9c7-48c6-beb3-74aee6cf3fd5"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("385cb082-9434-4b40-80f0-b5017fb787d6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("387deafa-18ea-41a3-abd2-bb42ccbaaceb"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("38c32bca-a278-4436-b900-77fd7a76be85"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("394da162-9742-49a4-b647-6887d47cd9a9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3987668f-3d46-44f8-99da-350964e1a0f9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("39bdb612-b08b-4508-a54f-93ea5929a3ff"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3a6231f1-6295-4307-8d5f-15d8778a10d1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3ab3ded1-fbe8-4fe5-b5b9-097b6232a3d1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3b259c70-2530-40d4-8785-1fbdfd6a594e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3b490f89-46cd-4b2e-ac46-d75da4eecf66"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3bb24dc5-1efa-4248-8183-7e5d66fd0e2b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3be47b8b-255f-4ee6-8d00-420acd3b5e28"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3c27dbac-5c07-432d-9609-bba2fb4b0680"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3c288847-76c3-4293-9123-cc3f11b77ba1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3c3ae4e2-2894-484f-9286-be08f2efca09"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3d24c89e-1962-41a3-b57b-b4445aeddcc1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3da91b89-4200-4ea3-bc5e-cd81afad56a2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3db3d62f-6cf8-4716-86d5-510e1eb17b4f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3e1f1a57-ab2a-4b16-a72e-859d15e8375f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3e863792-73eb-4a66-8022-7cd3e96013c2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3ed4fa9e-5167-4b69-ac83-4dd95640953d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3edbd84c-be1b-402b-80d4-88d3dcab15dc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3fd7487d-8ddf-4da4-a695-9c0adf2bd1ea"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("3fee691f-7f26-4af8-a3e2-ea07bf69af95"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("4019d8c6-d000-4bdd-90c3-bce6609f6ab0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("40aad9cd-9e13-4b7b-98e0-3f1eedf615f2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("4187672c-6033-473d-8d95-856a0278c92d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("418d795a-59aa-4ed2-a958-f16083237646"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("41d70572-3180-474a-94f6-d7abc8af5c98"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("41dca8bd-9ae9-40fc-b300-ad207b48fe44"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("41e93ded-cb2b-4e0e-bedb-b1ff8e96ab5c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("42a08b2e-0d46-49ca-ae3a-f181ca383b26"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("42ac50a3-4b92-4e67-b993-1d28b8408f14"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("4364c9fe-5f75-4d62-ad70-de03808eae63"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("43e7d76e-a859-44e3-bad2-753304b6c727"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("441187da-88ef-48cc-a61f-3a40d6c74a85"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("444f77ef-2e97-415b-a1e4-ee4a53555802"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("451516ef-202f-4b5a-bb04-8c839e99a760"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("45531ed2-10ce-470f-befb-f2433d8802ba"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("48606e61-deb5-4bc8-b7ad-20932a24c4c0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("490dabac-6889-4015-8742-c00ac79636af"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("4932b499-ad65-4080-8bee-0b8da68b4ef5"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("49a79847-0afc-414d-b25d-f53e3306ca66"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("4b0e2cd5-4632-41ab-901a-34d9caee726a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("4b51ef26-75a4-4d8a-ae62-d25703355514"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("4c069a87-d9bd-4b1c-9460-543f66bb6c87"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("4d9023cf-3997-48a6-b4c9-e7e92b078d6b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("4dc4db4d-4a5f-4319-9772-bf1fa696576a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("4fd25d11-42e7-4bbc-9cf2-c3e8ed702451"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5178516f-6259-4ea6-a77d-3870a5cb0d1f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("520cc3d0-2593-4c2b-af44-20904a1fcbe2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("52273063-39ab-4ff1-a5e6-1da9f106b723"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5249fa7e-a16b-4b97-affe-b6dc580c5480"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("524b2ff4-152f-4b9c-b8dd-42de9903da00"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5351dac8-6c5d-49de-bd60-0fffa13c18fc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("535f9041-eb50-4141-ba2d-099b2ca7c3e2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("53c5a827-b6b0-4cb7-a088-86b6afe6351d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("549d2d58-afcd-4ea4-bdbb-fd485f734029"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("54bd1657-c516-4d4e-968e-cbeb5e89ef43"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("555f1bf1-afb6-4580-8bed-383799d78c74"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("55b8e177-6ba7-4378-9cdf-990765b46690"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("55da8c7c-f965-424b-977d-c2ba9661e030"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("56063d25-2f97-4484-8606-f7720bc01617"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("560b5217-1b6a-4a61-9284-3500e94d58c6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5741e059-ab8e-4c71-b576-87c6369326e7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5779c71e-7f96-4f36-9211-252913a5c323"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("57e7d69f-6a7f-442b-b1bd-4a6ef305c587"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("57feec53-bd1a-4e0d-b845-904d39d88bd8"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("586335e4-fd0c-4dc4-a9b9-b1de074c37de"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("58ea9e26-4eb2-4b3b-96a5-8e918a596b49"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("596495ed-85ac-4fad-b527-6859b453d5b3"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("597ba01b-9e5e-43be-bc59-b791f8e73849"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("59aa89b3-34bf-4f0c-b0ef-d8da2b416a0a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5a1a6c68-53bc-459e-b48c-8d318bd08ebc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5a914122-3610-4f79-a61d-88d0b7624889"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5b9534ae-ffa2-46b6-9879-f314b6b431e2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5b9c6767-8756-4591-8997-79b62bc3606f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5bbf75dc-42cf-48a8-8c60-5e989c304322"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5ce3b60e-14ac-4d2b-bbdd-e858524b1aee"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5d702510-ac7f-4e27-9fde-d32c77b1f216"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5df08ed4-3829-4c82-ae40-0b59e3e6b73a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5df25ecf-1160-4c87-9fb1-39c7c3f78dd5"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5f329459-a0c5-4780-8344-97f5f4b9cd01"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5f34526b-5fa6-4256-9e7f-96d2b3b864b7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5f698feb-c35e-49d1-be2b-be6229951688"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("5fbaba87-22b6-4d0a-9361-b6d2fcbf773b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("602dc94a-b527-4bc8-89c3-f32527a510dd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("606653ce-71b3-41e5-b6fc-94a3d7949244"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6079aeef-b07d-405f-aa9b-84cf9b61e1d0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("617e3822-9a4c-4dd8-812e-1f60c116ec5e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("63859eb8-7040-446f-934b-eae9c6ba5b5a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("64e907f9-3724-43b1-a210-566109442015"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("64fbdad4-1e37-448c-879e-72294716c073"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("64fc673d-8fee-47cf-9371-b84f5d0599ab"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("65378c9d-82fa-400e-b4af-a5d35c465eca"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6571e9f8-7024-4753-b910-f98a15dd7797"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("65754b58-d751-4007-ac31-89cd4dfb0f13"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("658e359d-e9a1-43ac-9e4d-c3d2e396947e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6629fa25-575a-437e-930e-33f532690517"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6640b3f5-53a5-4553-943e-6af3be20ecb7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("66570486-89c6-4971-ae7f-2492d48d2e4d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6740c244-77ab-47f5-9ad5-1a107b70419b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("67d2adeb-31ac-4962-8025-c14ef2aa7236"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("67d39fe7-c4d6-4e4e-9fa7-59afe3cc6fb7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("68469280-cd75-4c7e-b8b4-08e9122bcff9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("68df25d2-6413-4411-be8c-48a7f8663b96"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("68ee583c-b68d-42c4-aba1-3148d0386822"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("698917b6-bf66-4a0c-8cce-eea7cd0d500d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6a23aa03-1a04-4205-a623-62676aa4da16"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6b700f66-62a6-466f-9d0f-f19e819d987f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6caf86e6-cf11-43b9-a9d1-6a7f30bc2dad"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6cbe2ddd-822e-4650-897f-abe2cdd1f217"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6ce1d9b9-d9c5-4992-ae18-fd95fa595b01"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6ce986c8-af9d-460c-b9c8-40944a29b0bb"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6d95820f-9301-4559-8d0f-1642556aad4e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6dbabcf3-21da-46a3-b4c3-3c041ff41104"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6de0afa4-d6de-4e38-93f8-4c829160badf"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6e2426fb-5f68-4382-8989-22dfdd494c64"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6e84cd69-e3c7-4298-908c-87563f9c7039"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6ef4a29a-054b-4256-9f4f-c9d954b208b8"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6f107538-5c52-45b2-ba91-c331ae3f7170"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("6f72f5b1-88bb-4df3-90ab-a6fb1e05ad6c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("704d40c0-c61e-4209-a41d-52f5603f13ed"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("720d594c-915a-446d-83e7-2a6e59173365"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("725c3b29-381d-4b23-947f-05940dbc909f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7286dc55-5077-481c-95ae-628a77b48ed2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("72a387e5-f47b-4d90-be5c-662c6d133bb6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("72e1aa79-1851-4253-92f5-7968c95d8f0c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("73965cec-e61c-4e62-b315-cc31952e8635"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("73cab5c1-250f-43e1-83ac-b3f6d2877da2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("73fef303-3c3d-4f67-9d8c-5a943d3b7462"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("75053554-4385-4889-8807-c3b6efa8555f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7567f056-fe93-4d23-b251-c54c552328de"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("758702ad-95d0-4f66-8390-12e9cefa7288"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("759c0bfa-acdb-43ed-8e15-7bace20b0860"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("765eb9f9-b64f-4418-b6f2-d8ad0930b3ca"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("76730e89-b1f0-42a6-b71b-c7eb5c7ef0db"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("772ed1a7-d579-40e7-bb17-d08319ca982b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7835480b-d68d-4ccf-a365-e72a6da8cde0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("783c7300-a8f0-4264-a64e-d6301688b21a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("787e2554-2018-4a49-9f96-764b07dd8ada"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("78a707c0-13d3-483a-a03e-cbee9928840c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7944bc4b-b9df-41bd-a84e-f93228a610df"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("79b3b8e8-cb36-40e2-b25c-abd59566613e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7a7f7f5d-6c49-4676-beb8-83b59c4e3ef0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7afbf1c9-a7ee-457a-bf2d-f3e4ace0a6dc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7c93194b-d8c7-4be8-bdf8-6471c6a04561"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7ce9d0f8-ab55-4890-8fa0-0c334b3fc397"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7d26d92b-4e07-48c3-8574-4a47abdb7b75"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7d37d581-76e0-4a4a-ab7d-135eeb9fc89c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7d9c46fe-56bd-4ec3-ab5c-bbb2da65ff54"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7dc52cba-58ce-4e45-9d94-5882794c95c0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7e789686-2bff-4a69-9140-74d4d4769b1e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7f129983-0556-41bb-82f2-31a178e1afbd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7f2f48cf-0afc-4e2d-82ec-a71bb29ac16a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("7f66ff31-5017-45fa-8647-a8ba61b59b7b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("808fcd22-1118-4769-8503-1af9febf9ce1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("81fb5472-2bf3-4efd-95e3-03834c35f276"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("82046ab7-239c-42ef-b7e6-0b7c69dbf02d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8347c0e2-cf6e-40fd-baf8-8f01bc3247f8"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("841dc90f-419e-45b8-b7ef-b8748569a469"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("84ba86e5-c7ec-4a79-87ea-10c3329f15b3"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("84f20e03-7898-4449-bfb6-b3c8a9887428"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("85091556-633a-41ab-aaa5-86af7678def8"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("85b60975-859b-497b-8796-ddb31b47e3cc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("866db238-12d8-48ef-8fbf-98adf2e9dd38"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("86777047-8cae-406b-bfc9-8d35c040f4b5"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("872891e1-638c-45ef-bb0d-87919b4e0e91"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8733d3b1-cf1f-4516-997f-e112d962f9b7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("879c169f-06e3-4db1-bf87-bd0fa127425f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("890da2b5-4acc-4dfb-9ce8-c0f22af6cf1f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("892807a7-b0df-4f84-b652-5e6c0dee778d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8958fb33-661b-44f6-a8f6-19eb33a362a9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("896e9b33-a164-44e5-933f-09083f76c96d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("89bee6cc-641e-4cb5-9540-a02cbe3dc42b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8abc96a0-129f-4ff9-85a5-c5663dd82834"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8ae7838a-084a-4af7-bbd7-7068ed40b32a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8b14055a-f208-4ad2-ab8c-586646b1b88b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8b97ee2f-d7d8-4557-8bfa-79fef36af15b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8bd30891-3129-4b0b-b3c8-b152969d2dad"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8c228dd8-1648-455f-b122-d489c80b701b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8cbd3ce1-d3e0-4a9e-b66c-6758d16b8884"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8cc545a2-1723-4869-95c8-16e747737176"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8ce590e0-2bb1-4f1a-b4c4-5b40cb63b3bb"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8d93e4f5-2f82-4a2e-abbf-f9e5c28321c7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8dc3be64-a04c-4a19-9c07-62f7663134f3"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8f7c86ef-30c5-4742-b01d-5542d758c52c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("8fc23067-2830-4043-85fc-45288444d0fd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9117e726-42eb-4d9a-852c-14d3f2bff8bc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("91461951-5708-46d2-8f37-0fd4d79f77ea"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("92b46b09-260f-4efc-b256-9fcaf3b3835a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("93241d50-87ab-4a46-b429-55fc5d08b1e1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("93734f37-c056-47a0-886f-3cc800b59947"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("939b3dd6-b6b9-4f4d-a64d-bebdee1fb7ec"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("93e86e41-18d6-45d0-b4d9-0970d907a315"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("959e0e3a-8bfe-4212-acc1-f574a4b4bba1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("95d31732-4aac-4d63-98dd-fc193936992f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("96030ce4-fad3-470b-9a0f-e498981d26b2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("968fb600-0fca-4514-ac2a-227deeedd06e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("97686e29-0dbc-4633-ae57-f1925d86d4c1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("97f5bcf2-bcc8-4cdc-8742-3a4887badf14"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("988c9cae-0995-472c-ac18-1f6f3a098f16"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9911d09d-4966-49ee-98a9-1ebbc584b796"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("995e8449-804b-4e34-8915-fa9efc8461c2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("998a12b1-39b8-48b0-bba9-93ec9d8b6c0f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("99c7bcca-c617-4405-a046-172723d5ebda"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9a4a3c6a-1bbc-4218-a6c0-2cc8fbd726b3"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9ad35810-1bc1-459f-b92e-5e0fd947bd04"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9b1e6ae6-6d2a-4c75-a5d4-de352f7cbe63"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9b39683a-5ccb-45d4-9a23-f3c0588a85eb"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9b73419f-9454-433f-840d-5acc4f80cfbd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9bae21ab-0c87-4109-8a7b-a856d1650775"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9c9694bb-2a39-4578-87ab-984d11b20b7f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9ca8a7af-22ce-4a9e-a390-52adf6136ba3"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9ccff1e3-69c2-4e83-94e8-1b15795106f5"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9dcf1447-2172-424b-bd6e-4f4231f6f15f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9e6034d2-e880-46c3-9fa2-a62753a6bd8d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9ee4516f-c9dd-444f-a0ec-1eebb2c95927"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9f4b0122-edbf-4f46-a9b6-1b67681d1963"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9f8b8050-a523-45fd-ab4b-6159ef83373e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("9fe08799-af3a-4e47-988a-680048367255"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a12957a1-15e7-47d6-bf66-75d5ac82582e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a1fbeb91-ad9c-40eb-a985-fa2a0c2e4a97"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a1fdbd27-dc6a-4a1b-996c-49f7a4bffee9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a207e2f9-337c-4032-938e-82028ba3834a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a2956aeb-cc23-4978-80c5-4eefd8ef7837"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a3212a45-5f24-42d9-b134-abe43f9e7b0c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a3d95632-7597-4a21-9309-00826376be30"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a4090c5f-f6c3-436f-b162-b84bed4d0035"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a56ca8d1-c6cb-4b7b-8615-1ccd9614ff18"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a7ab6da8-4d24-4f7f-a58b-fb7443ae8abe"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a7b7fb59-c3ab-45f3-b9c3-767a4675fc0f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a7d099ef-39e9-4f94-aac8-ae3a4c813f74"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a7f3d282-7f78-4e05-b800-7f966eaf5f4c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a8c1df9c-53f8-4a36-970a-74c77f129df0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a97bc020-7c6c-4fe9-824e-bcdbe0d79d1f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a9aa086f-22d7-49a5-b8fc-940d80a75cea"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("a9e26aec-6c2d-4c46-9721-b700486a7316"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("aaa21fd0-cf8f-484f-bd74-583e5aedd95f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("aae897b9-dc33-48dc-8b7a-42c4bccce93f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("abb2f790-4f53-4a2e-8350-8b514aa56386"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ac517f3a-4b76-4291-8c3e-893b928d94af"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ac8a9aee-166f-49d9-bd9f-1749857cfcba"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ac95a119-4330-468e-8349-1bcbc789ebc6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("acf0e4e2-f9ea-45bf-bf63-f2a2f7d2642a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ad333d6a-1df6-4eea-a10e-9e709b0605b7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ad7a0fec-b872-4901-9570-f7138c88b470"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("addcb0a8-dd63-417f-8b0f-44f10cd34ce7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("aeaa9680-3a11-49f4-9136-9cc294830798"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("aee07860-030c-4fd8-a7db-1c3dadbee4fd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("af33fc67-6a70-4c35-a7ea-1aad6d4c9694"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("af36e71d-6c64-49d3-8078-d7fc352c4566"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("af390f73-73f9-4ef8-8bb7-ee2721af3041"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("afb4adcd-7567-4318-afa0-2c03a9122524"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b0c02f04-4c37-4283-b618-348d5022e4a7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b0cc5b1c-661f-4d95-b8c0-5b8fe97496dd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b0f92fb2-8cae-4c86-9d77-c4412381d8fa"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b12fc8ba-56e2-4bba-8ed6-87caf8a1074b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b17b9104-c2ab-4bae-a3da-ecd4dd05c31f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b259c5cd-c061-46ef-84ba-7b94350ccd8f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b282cc3b-9d87-46e7-85d5-38e5b16f6cd6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b2f222c3-e2af-4da7-992c-155d910015d6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b312fbe3-fd1e-4c42-baad-91b0757ce0ad"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b5002ceb-e043-4610-b441-6a5e1adf2d0b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b56ff32d-4a9c-40cd-bbf0-d82d74a6d4b0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b60abf19-45c2-4d9f-aa58-c90d8bf7c501"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b755d712-a34a-4a09-b102-860df6259487"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b777d5ad-0b43-439b-a631-16fcb3706189"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b7b80bfa-cd53-47db-9af4-f799e71d0125"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b871e3a2-e81b-4baf-8f9f-29740e4a99fd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b894392a-cc95-4cfa-befc-b5662e3a0df9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b97e8b74-4682-47bf-994c-af81a9df143a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("b984597e-8aae-4b82-a5c3-3c3c7c36f6d4"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bb3dbb2e-73ca-4b4d-a2c3-6a007a580ff0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bb5950d2-2f8d-4d25-8757-cc845ccd409b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bb5edd72-154e-4d96-8bd9-4fe0107a439a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bbfd03a6-becc-4c60-8915-0536becb28a5"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bc0a7df2-dabb-43df-9d23-8e74ec317110"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bc6de817-3d37-4f03-8f19-edc2c26a1c6b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bcb84bde-65dc-428f-8322-5cd934f9ffaa"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bce3fbf7-723a-43ea-882e-a6f45b97525e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bcfafd4c-85f7-44b3-b406-4499c33ba3b0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bd853a31-2c0f-43f3-82eb-455f8ecf30c3"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bd9e6c0e-383e-46d1-ac3f-c76b8d6b39e6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("be8babab-5ef3-4eb8-8792-fca54c386966"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bee70d79-4511-4f6b-bb46-2b83e1e2a416"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bf81ee5c-4064-4b3c-9586-2936fae52262"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("bfac1cc4-4e08-469b-b3e4-e53d473660e4"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c07564bb-69c7-4994-8347-c5d8ac6b0264"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c0a068ab-bb34-4489-86c5-de27414aa398"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c0d9828c-211f-4283-9b69-888b400da8d1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c112d0ca-cc2e-4c66-8637-5deb221f31c6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c184c9f0-6303-4f06-8c72-c5285b15b0e8"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c1c600f0-ab2f-4f7f-b0ca-0fe42fb503a4"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c1ec3f54-80f5-486b-a9ee-957192a75b62"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c2938e70-f0c3-46b9-bb72-47b6e99eb5b8"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c4495581-f082-41e7-a8eb-b8fa956e4cf9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c4500aa8-02eb-4f9e-931a-2ca5af82720c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c48a334c-fdf3-4ff4-9d5c-23a842ca1f18"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c4b59ee0-fb22-40f0-bfb3-a4cbfe56a31c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c4b8c25c-a4db-4e1e-ad33-25f942408cc5"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c4d374e7-284c-4ebc-aa57-56a6c10e1d3e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c570d143-9182-44e3-83b8-5dcceb8b0f87"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c6460a0a-73ca-4274-96f7-531d5da6a60a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c7339c2e-94a8-405e-8ea6-21a2ad71e27e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c7a3044f-79b3-4095-b857-39f4ae6e1c5d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c7eb8d85-92ee-4714-b06b-7921fa48652e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c810119f-fc5c-4e59-80f4-15f6f1cb2e3f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c8dded75-df45-4c27-abec-743a29f85c8a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c91639b3-1428-409b-b1a6-81de3f6e6bbb"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c9294bab-ca0d-4ac4-8f36-145645d1b937"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("c95c29d8-9009-4b77-9803-6b5f8db64a43"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ca4cf94a-3af1-4edf-a91b-0fd89e421a8d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cab5341f-a780-4c49-8a3d-5d445892ba32"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cb251dd5-9f0d-432f-a2d5-824568ce754a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cc51afc6-55c8-4be2-95c4-bf2cd9cd62fe"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cc9d967b-c4f9-4108-9623-eb872e9751eb"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ccbb3a08-96cf-46fc-8a14-0a72ef251fac"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ccc2bbc6-3f51-49be-a609-cf3b7ea36a42"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cd5eed80-9bc9-4d35-8f69-5e39dace2cfc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cdc1290c-93ea-4039-9a12-d44c1af1b548"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cdf61c6f-01cd-4763-8506-d3c749aa05c1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ce2d68f5-4f39-4741-a571-9aa5518e2037"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cee30038-9801-4a94-a328-ee4753219914"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cf32f103-a9d9-4a3f-adab-7eef7284d470"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cf520c6e-40d7-4c76-a3a0-a410a76b93f3"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cf99c4f2-c7e3-495e-aeef-803ba6117f4e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("cfbc23c7-b6d0-4d54-9f06-e88b949791b0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d005e481-0128-4137-a1ec-b6bc9f224b74"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d070ee1d-d722-4777-b2e9-fd5fc30339c9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d0cd968b-8868-4b56-90bb-3cd2417f2012"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d1c8c72e-a47a-45e8-96ea-44f6fda8d14c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d1d194fe-9fc5-4fbb-9091-1fe44bffaf33"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d2171030-eeb4-4974-8e83-28eda739860d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d37335ec-9337-4b3c-bb57-c795643a99dc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d389b6d5-0027-4e56-bbf6-6a32f95780b8"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d3d50430-549f-4807-a192-b95d6966f754"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d4fd0dee-163e-4c8c-b2b9-3cb1dbe216d7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d526da63-869a-41ec-826b-8c4661ab1e26"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d6892e31-2109-4bd6-a006-c297b576b1d7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d78b9eac-8998-4994-bfa2-bfb5b450cc35"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d79088f4-e182-4cc6-acc9-b14b50e78427"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("d8701aa4-14ab-4464-b952-171c0dab411c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("da820def-20be-4132-8d4b-304d5a22ec4b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("daaf0314-d3c1-4579-b93c-2bee5979ed41"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("dae37c16-5363-4823-ab50-dca9cf13e773"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("db31db75-acf0-4d1e-8748-48864b0888dd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("dc6e4ee1-7a01-4cad-9243-e59700e99445"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("dc9f741c-abf8-4650-ba35-7392324e3895"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("df0d5098-db69-49bb-89b1-cbbd29fe3285"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("df659faa-ba70-487c-9522-ac4d0ea3acbc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("df90d956-be26-4ac6-a749-9cc288d73631"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("df9af89c-73c5-452b-8380-b52f8916059f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e025d78b-db17-4870-85e0-bea114a2d469"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e035e6f4-cf7e-4da0-bb09-6b225144e87b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e229cb19-7be8-4f60-aa62-89bc85d89442"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e2692021-4974-4144-af16-a6de791a6a03"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e2b5e502-063d-486f-9bcc-7f4e8bee164c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e2d70a49-d9a9-48c4-bec8-a1072734b5df"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e3beb3f8-97a8-410f-b55c-9dfa7d47b5ec"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e3cc8019-61d5-45ed-a479-ed2351c562ad"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e3f3abc7-feb7-4111-8cb4-23c3aa3f127b"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e423fc22-e40e-44e7-a23a-702389fcaeb1"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e4f21d0e-d1f1-4c40-b356-7166125f1751"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e5858459-17b0-40dc-a151-689e57e356a6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e5bf87b1-11b0-4b3a-b3e5-0495c670ff54"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e72e172d-9fe9-4401-aad3-85113bc9b05c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e80006a4-811d-46cc-8835-6f6ff39a8465"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e906c535-c5cb-469b-8102-fd2949eaedbc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e9416c0e-ab77-4c44-936a-8aa193cf2deb"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e95b527d-f663-4d8c-9111-3bac2aa3f6d4"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e96541b0-7644-49ba-8f19-b299ed84e179"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("e987b111-fcbb-4530-86a0-003b970d96e8"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ea67b321-edb4-41bf-b523-f8cb5c6181bf"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("eab9cb4c-77ce-4041-90a4-9311e6292ca3"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("eb3b49c9-51e2-4ebb-b54e-8d0931234a4e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ed11490a-01b7-40d1-a7dd-61db1897de08"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ed559acf-bc9e-4953-aa12-e482259ec4bf"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ed6e7c8e-4dfa-41fb-ac16-203da4271739"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ed971f28-ad4b-41ce-bb29-68cf5ee6c701"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ed9caf58-1442-4f52-ac9b-498996eac1b6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("edb3b739-d6d2-435c-84db-531093c331ed"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("edc3bf73-2d36-4831-aab9-21f40745f358"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ee61c402-641f-49e3-a625-bcd3aee7bdc6"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("efb4a363-33d6-4674-a30e-a59bed94af4a"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f0b73d06-0715-4e89-9c22-f8523ba92fbf"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f131b2aa-d22b-4e7e-ad9a-fea40821a427"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f157c393-d753-4cf7-b7cf-5da7ff26d479"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f218b0d2-a8e2-47b7-919a-7a2534cf0dee"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f23b0ce8-9e86-4fcc-9944-74be176440e9"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f245381a-2a07-439e-942f-0524e2219ec2"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f25cb4c2-aeec-4be1-8f3d-c1f52b295b69"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f2a2d66d-eaf4-4adc-b489-6b02899ebbad"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f2f6b820-9dbc-47b5-954e-b3966f87626e"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f355d1eb-61c3-4e72-bf54-1b1b32da6bab"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f37f9018-2e60-4f1d-a46d-fe0f729c1f31"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f5060206-f062-4329-a0f7-4cffbaa02d1d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f6e9616c-f2c3-4761-b38a-a43829e621b8"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("f90a23fc-1c93-47c0-982e-5d96e4c16f32"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fa1ab884-79fd-4d7e-aa8e-d33bd26d2efc"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fa3370cc-ec5c-464f-9887-aec6366e110c"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fa780956-2352-4383-8069-df6675dab1a7"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fb0f4341-4fcf-4f16-b9ba-8e02d0828ea0"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fb574e28-e2f0-4b32-8a37-34c2e21dd5cd"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fc77df45-fc02-4850-9772-b9056e68845d"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fcf214ae-def8-4762-b200-025bf89e8d7f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fcf6dc11-26f5-4e75-b53b-b6997ff4e547"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fd6afe52-c688-44f7-8e95-637bd9897d38"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fd981842-1a6a-4a52-8a23-1c71c19f68fa"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("fe9ebd39-1cb9-47b5-b295-094db21ab57f"));

            migrationBuilder.DeleteData(
                table: "SwaCodes",
                keyColumn: "Id",
                keyValue: new Guid("ff56bea2-1b9d-41f1-9e50-f870bcfd0fb7"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SwaCodes");
        }
    }
}
