namespace DfT.DTRO.Migrations;

public static class SeedData
{
    public static SystemConfig SystemConfig = new SystemConfig()
    {
        Id = new Guid("a7ab6da8-4d24-4f7f-a58b-fb7443ae8abe"),
        SystemName = "TRA Test System"
        
    };

    public static List<SwaCode> TrafficAuthorities = new List<SwaCode>(){
                        new SwaCode
                        {
                            Id = new Guid("67d2adeb-31ac-4962-8025-c14ef2aa7236"),
                            IsActive = true,
                            IsAdmin = true,
                            Name = "Department of Transport X",
                            Prefix = "DfT",
                            TraId = -1
                        },
                        new SwaCode
                        {
                            Id = new Guid("a7ab6da8-4d24-4f7f-a58b-fb7443ae8abe"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "(AQ) LIMITED",
                            Prefix = "SK",
                            TraId = 7334
                        },
                        new SwaCode
                        {
                            Id = new Guid("f5060206-f062-4329-a0f7-4cffbaa02d1d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "1310 Ltd",
                            Prefix = "M3",
                            TraId = 7550
                        },
                        new SwaCode
                        {
                            Id = new Guid("bfac1cc4-4e08-469b-b3e4-e53d473660e4"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ADVANCED ELECTRICITY NETWORKS LIMITED",
                            Prefix = "S6",
                            TraId = 7583
                        },
                        new SwaCode
                        {
                            Id = new Guid("aaa21fd0-cf8f-484f-bd74-583e5aedd95f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "Affinity Systems Limited",
                            Prefix = "L4",
                            TraId = 7544
                        },
                        new SwaCode
                        {
                            Id = new Guid("d79088f4-e182-4cc6-acc9-b14b50e78427"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "AFFINITY WATER EAST LIMITED",
                            Prefix = "MT",
                            TraId = 9132
                        },
                        new SwaCode
                        {
                            Id = new Guid("be8babab-5ef3-4eb8-8792-fca54c386966"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "AFFINITY WATER LIMITED",
                            Prefix = "MX",
                            TraId = 9133
                        },
                        new SwaCode
                        {
                            Id = new Guid("0d99e3c6-b43e-41b3-bf44-43c71d097b30"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "AFFINITY WATER SOUTHEAST LIMITED",
                            Prefix = "EV",
                            TraId = 9121
                        },
                        new SwaCode
                        {
                            Id = new Guid("112dd1d4-3ea8-41b9-8017-4719fec4f21f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "AIRBAND COMMUNITY INTERNET LIMITED",
                            Prefix = "A8",
                            TraId = 7372
                        },
                        new SwaCode
                        {
                            Id = new Guid("05576e80-526a-4818-a660-a8d6d9f2bd32"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "AIRWAVE SOLUTIONS LIMITED",
                            Prefix = "VB",
                            TraId = 7297
                        },
                        new SwaCode
                        {
                            Id = new Guid("2b3dde38-7fe8-46df-967a-b48cc909cd23"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "AJ TECHNOLOGIES LIMITED",
                            Prefix = "L5",
                            TraId = 7545
                        },
                        new SwaCode
                        {
                            Id = new Guid("2712ccb9-633f-4384-b8c4-472faa4b6f35"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ALBION WATER LIMITED",
                            Prefix = "UZ",
                            TraId = 9139
                        },
                        new SwaCode
                        {
                            Id = new Guid("2f0bf8f7-1c77-498c-b9f3-72808c6818ff"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ALLPOINTS FIBRE LIMITED",
                            Prefix = "M5",
                            TraId = 7552
                        },
                        new SwaCode
                        {
                            Id = new Guid("aee07860-030c-4fd8-a7db-1c3dadbee4fd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ALLPOINTS FIBRE NETWORKS LIMITED",
                            Prefix = "J3",
                            TraId = 7528
                        },
                        new SwaCode
                        {
                            Id = new Guid("9dcf1447-2172-424b-bd6e-4f4231f6f15f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ANGLIA CABLE COMMUNICATIONS LIMITED",
                            Prefix = "AC",
                            TraId = 7026
                        },
                        new SwaCode
                        {
                            Id = new Guid("14333987-36cd-4dd4-ba40-885b6c06649b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ARELION UK LIMITED",
                            Prefix = "ZM",
                            TraId = 7230
                        },
                        new SwaCode
                        {
                            Id = new Guid("bb3dbb2e-73ca-4b4d-a2c3-6a007a580ff0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ARQIVA LIMITED",
                            Prefix = "TN",
                            TraId = 7354
                        },
                        new SwaCode
                        {
                            Id = new Guid("441187da-88ef-48cc-a61f-3a40d6c74a85"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ARQIVA LIMITED (Formerly NATIONAL TRANSCOMMUNICATIONS LTD)",
                            Prefix = "SP",
                            TraId = 7217
                        },
                        new SwaCode
                        {
                            Id = new Guid("012a8c07-e04f-4b4a-b487-590116f549f8"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "AWG GROUP LIMITED",
                            Prefix = "AD",
                            TraId = 9100
                        },
                        new SwaCode
                        {
                            Id = new Guid("5d702510-ac7f-4e27-9fde-d32c77b1f216"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "AXIONE UK LIMITED",
                            Prefix = "K9",
                            TraId = 7541
                        },
                        new SwaCode
                        {
                            Id = new Guid("8958fb33-661b-44f6-a8f6-19eb33a362a9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BAA INTERNATIONAL LIMITED",
                            Prefix = "AF",
                            TraId = 17
                        },
                        new SwaCode
                        {
                            Id = new Guid("e9416c0e-ab77-4c44-936a-8aa193cf2deb"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BAI COMMUNICATIONS INFRASTRUCTURE LIMITED",
                            Prefix = "N9",
                            TraId = 7563
                        },
                        new SwaCode
                        {
                            Id = new Guid("d8701aa4-14ab-4464-b952-171c0dab411c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BARNSLEY METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "AJ",
                            TraId = 4405
                        },
                        new SwaCode
                        {
                            Id = new Guid("a97bc020-7c6c-4fe9-824e-bcdbe0d79d1f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BATH AND NORTH EAST SOMERSET COUNCIL",
                            Prefix = "QD",
                            TraId = 114
                        },
                        new SwaCode
                        {
                            Id = new Guid("20e7280f-587e-4186-a3ba-c858e6641ce1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BAZALGETTE TUNNEL LIMITED",
                            Prefix = "ZE",
                            TraId = 7345
                        },
                        new SwaCode
                        {
                            Id = new Guid("866db238-12d8-48ef-8fbf-98adf2e9dd38"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BEDFORD BOROUGH COUNCIL",
                            Prefix = "UB",
                            TraId = 235
                        },
                        new SwaCode
                        {
                            Id = new Guid("93241d50-87ab-4a46-b429-55fc5d08b1e1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BIRMINGHAM CABLE LIMITED",
                            Prefix = "AR",
                            TraId = 7028
                        },
                        new SwaCode
                        {
                            Id = new Guid("560b5217-1b6a-4a61-9284-3500e94d58c6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BIRMINGHAM CABLE LIMITED (Formerly BIRMINGHAM CABLE WYTHALL)",
                            Prefix = "QE",
                            TraId = 7198
                        },
                        new SwaCode
                        {
                            Id = new Guid("e906c535-c5cb-469b-8102-fd2949eaedbc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BIRMINGHAM CITY COUNCIL",
                            Prefix = "AQ",
                            TraId = 4605
                        },
                        new SwaCode
                        {
                            Id = new Guid("6079aeef-b07d-405f-aa9b-84cf9b61e1d0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BLACKBURN WITH DARWEN BOROUGH COUNCIL",
                            Prefix = "AE",
                            TraId = 2372
                        },
                        new SwaCode
                        {
                            Id = new Guid("b259c5cd-c061-46ef-84ba-7b94350ccd8f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BLACKPOOL BOROUGH COUNCIL",
                            Prefix = "CU",
                            TraId = 2373
                        },
                        new SwaCode
                        {
                            Id = new Guid("6f107538-5c52-45b2-ba91-c331ae3f7170"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BLAENAU GWENT COUNTY BOROUGH COUNCIL",
                            Prefix = "AS",
                            TraId = 6910
                        },
                        new SwaCode
                        {
                            Id = new Guid("31f0b8d9-100a-49a2-b80d-aa468a5a3421"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "Boldyn Networks Limited",
                            Prefix = "L7",
                            TraId = 7547
                        },
                        new SwaCode
                        {
                            Id = new Guid("a3212a45-5f24-42d9-b134-abe43f9e7b0c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BOLT PRO TEM LIMITED",
                            Prefix = "XD",
                            TraId = 7346
                        },
                        new SwaCode
                        {
                            Id = new Guid("4b0e2cd5-4632-41ab-901a-34d9caee726a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BOLTON METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "AT",
                            TraId = 4205
                        },
                        new SwaCode
                        {
                            Id = new Guid("9ca8a7af-22ce-4a9e-a390-52adf6136ba3"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BOURNEMOUTH WATER LIMITED",
                            Prefix = "AU",
                            TraId = 9110
                        },
                        new SwaCode
                        {
                            Id = new Guid("8dc3be64-a04c-4a19-9c07-62f7663134f3"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BOURNEMOUTH, CHRISTCHURCH AND POOLE COUNCIL",
                            Prefix = "B2",
                            TraId = 1260
                        },
                        new SwaCode
                        {
                            Id = new Guid("5ce3b60e-14ac-4d2b-bbdd-e858524b1aee"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BOX BROADBAND LIMITED",
                            Prefix = "A9",
                            TraId = 7373
                        },
                        new SwaCode
                        {
                            Id = new Guid("c4d374e7-284c-4ebc-aa57-56a6c10e1d3e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BRACKNELL FOREST COUNCIL",
                            Prefix = "DW",
                            TraId = 335
                        },
                        new SwaCode
                        {
                            Id = new Guid("63859eb8-7040-446f-934b-eae9c6ba5b5a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BRIDGEND COUNTY BOROUGH COUNCIL",
                            Prefix = "AX",
                            TraId = 6915
                        },
                        new SwaCode
                        {
                            Id = new Guid("ccc2bbc6-3f51-49be-a609-cf3b7ea36a42"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BRIGHTON & HOVE CITY COUNCIL",
                            Prefix = "DU",
                            TraId = 1445
                        },
                        new SwaCode
                        {
                            Id = new Guid("72e1aa79-1851-4253-92f5-7968c95d8f0c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BRISTOL CITY COUNCIL",
                            Prefix = "QF",
                            TraId = 116
                        },
                        new SwaCode
                        {
                            Id = new Guid("c570d143-9182-44e3-83b8-5dcceb8b0f87"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BRITISH PIPELINE AGENCY LIMITED",
                            Prefix = "BA",
                            TraId = 7089
                        },
                        new SwaCode
                        {
                            Id = new Guid("01582089-bd13-469e-b8e8-6672d1d63fc5"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BRITISH TELECOMMUNICATIONS PUBLIC LIMITED COMPANY",
                            Prefix = "BC",
                            TraId = 30
                        },
                        new SwaCode
                        {
                            Id = new Guid("d78b9eac-8998-4994-bfa2-bfb5b450cc35"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BROADBAND FOR THE RURAL NORTH LIMITED",
                            Prefix = "TG",
                            TraId = 7350
                        },
                        new SwaCode
                        {
                            Id = new Guid("bcb84bde-65dc-428f-8322-5cd934f9ffaa"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BROADWAY PARTNERS LIMITED",
                            Prefix = "D7",
                            TraId = 7392
                        },
                        new SwaCode
                        {
                            Id = new Guid("720d594c-915a-446d-83e7-2a6e59173365"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BRSK LIMITED",
                            Prefix = "J2",
                            TraId = 7527
                        },
                        new SwaCode
                        {
                            Id = new Guid("e95b527d-f663-4d8c-9111-3bac2aa3f6d4"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BRYN BLAEN WIND FARM LIMITED",
                            Prefix = "TU",
                            TraId = 7360
                        },
                        new SwaCode
                        {
                            Id = new Guid("9f8b8050-a523-45fd-ab4b-6159ef83373e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BUCKINGHAMSHIRE COUNCIL",
                            Prefix = "D4",
                            TraId = 440
                        },
                        new SwaCode
                        {
                            Id = new Guid("d4fd0dee-163e-4c8c-b2b9-3cb1dbe216d7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "BURY METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "BJ",
                            TraId = 4210
                        },
                        new SwaCode
                        {
                            Id = new Guid("8abc96a0-129f-4ff9-85a5-c5663dd82834"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLE LONDON LIMITED",
                            Prefix = "BL",
                            TraId = 7030
                        },
                        new SwaCode
                        {
                            Id = new Guid("6571e9f8-7024-4753-b910-f98a15dd7797"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLE LONDON LIMITED (Formerly CABLE LONDON PLC ENFIELD)",
                            Prefix = "BM",
                            TraId = 7099
                        },
                        new SwaCode
                        {
                            Id = new Guid("afb4adcd-7567-4318-afa0-2c03a9122524"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLE LONDON LIMITED (Formerly CABLE LONDON PLC HACKNEY)",
                            Prefix = "BN",
                            TraId = 7100
                        },
                        new SwaCode
                        {
                            Id = new Guid("acf0e4e2-f9ea-45bf-bf63-f2a2f7d2642a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLE LONDON LIMITED (Formerly CABLE LONDON PLC HARINGEY)",
                            Prefix = "BP",
                            TraId = 7101
                        },
                        new SwaCode
                        {
                            Id = new Guid("7d9c46fe-56bd-4ec3-ab5c-bbb2da65ff54"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLE ON DEMAND LIMITED",
                            Prefix = "CX",
                            TraId = 7113
                        },
                        new SwaCode
                        {
                            Id = new Guid("a1fdbd27-dc6a-4a1b-996c-49f7a4bffee9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLE ON DEMAND LIMITED (Formerly COMCAST TEESSIDE DARLINGTON)",
                            Prefix = "CW",
                            TraId = 7112
                        },
                        new SwaCode
                        {
                            Id = new Guid("daaf0314-d3c1-4579-b93c-2bee5979ed41"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLECOM INVESTMENTS LIMITED",
                            Prefix = "BV",
                            TraId = 7173
                        },
                        new SwaCode
                        {
                            Id = new Guid("6ce986c8-af9d-460c-b9c8-40944a29b0bb"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLETEL BEDFORDSHIRE",
                            Prefix = "BW",
                            TraId = 7032
                        },
                        new SwaCode
                        {
                            Id = new Guid("ac95a119-4330-468e-8349-1bcbc789ebc6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLETEL HERTS AND BEDS LIMITED",
                            Prefix = "CA",
                            TraId = 7108
                        },
                        new SwaCode
                        {
                            Id = new Guid("bee70d79-4511-4f6b-bb46-2b83e1e2a416"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLETEL HERTS AND BEDS LIMITED (Formerly CABLETEL CENTRAL HERTFORDSHIRE)",
                            Prefix = "BX",
                            TraId = 7106
                        },
                        new SwaCode
                        {
                            Id = new Guid("09e33824-7afb-466f-bf5e-7c3dc41316e0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLETEL HERTS AND BEDS LIMITED (Formerly CABLETEL HERTFORDSHIRE)",
                            Prefix = "BY",
                            TraId = 7107
                        },
                        new SwaCode
                        {
                            Id = new Guid("8347c0e2-cf6e-40fd-baf8-8f01bc3247f8"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CABLETEL SURREY AND HAMPSHIRE LIMITED",
                            Prefix = "CE",
                            TraId = 7111
                        },
                        new SwaCode
                        {
                            Id = new Guid("524b2ff4-152f-4b9c-b8dd-42de9903da00"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CADENT GAS LIMITED",
                            Prefix = "AZ",
                            TraId = 10
                        },
                        new SwaCode
                        {
                            Id = new Guid("dae37c16-5363-4823-ab50-dca9cf13e773"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CAERPHILLY COUNTY BOROUGH COUNCIL",
                            Prefix = "CG",
                            TraId = 6920
                        },
                        new SwaCode
                        {
                            Id = new Guid("57feec53-bd1a-4e0d-b845-904d39d88bd8"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CALDERDALE METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "CH",
                            TraId = 4710
                        },
                        new SwaCode
                        {
                            Id = new Guid("e4f21d0e-d1f1-4c40-b356-7166125f1751"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CALL FLOW SOLUTIONS LTD",
                            Prefix = "UY",
                            TraId = 7339
                        },
                        new SwaCode
                        {
                            Id = new Guid("ee61c402-641f-49e3-a625-bcd3aee7bdc6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CAMBRIDGE FIBRE NETWORKS LTD",
                            Prefix = "A7",
                            TraId = 7371
                        },
                        new SwaCode
                        {
                            Id = new Guid("1ee7ac7c-2bae-4819-91e4-cfde56ffea6d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CAMBRIDGE WATER PLC",
                            Prefix = "CK",
                            TraId = 9113
                        },
                        new SwaCode
                        {
                            Id = new Guid("92b46b09-260f-4efc-b256-9fcaf3b3835a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CAMBRIDGESHIRE COUNTY COUNCIL",
                            Prefix = "CL",
                            TraId = 535
                        },
                        new SwaCode
                        {
                            Id = new Guid("df0d5098-db69-49bb-89b1-cbbd29fe3285"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CARMARTHENSHIRE COUNTY COUNCIL",
                            Prefix = "QQ",
                            TraId = 6825
                        },
                        new SwaCode
                        {
                            Id = new Guid("1cd883cf-5c6e-4871-a27f-221e458cfb44"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CELLNEX (ON TOWER UK LTD)",
                            Prefix = "G5",
                            TraId = 7513
                        },
                        new SwaCode
                        {
                            Id = new Guid("019d0fc8-cd2f-4c58-922f-b6e6ce6a9a7a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CENTRAL BEDFORDSHIRE COUNCIL",
                            Prefix = "UC",
                            TraId = 240
                        },
                        new SwaCode
                        {
                            Id = new Guid("ed971f28-ad4b-41ce-bb29-68cf5ee6c701"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CENTRO (WEST MIDLANDS PASSENGER TRANSPORT EXECUTIVE)",
                            Prefix = "WB",
                            TraId = 9234
                        },
                        new SwaCode
                        {
                            Id = new Guid("ad333d6a-1df6-4eea-a10e-9e709b0605b7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CEREDIGION COUNTY COUNCIL",
                            Prefix = "QP",
                            TraId = 6820
                        },
                        new SwaCode
                        {
                            Id = new Guid("bf81ee5c-4064-4b3c-9586-2936fae52262"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CHESHIRE EAST COUNCIL",
                            Prefix = "UD",
                            TraId = 660
                        },
                        new SwaCode
                        {
                            Id = new Guid("6e84cd69-e3c7-4298-908c-87563f9c7039"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CHESHIRE WEST AND CHESTER COUNCIL",
                            Prefix = "UE",
                            TraId = 665
                        },
                        new SwaCode
                        {
                            Id = new Guid("65378c9d-82fa-400e-b4af-a5d35c465eca"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CHOLDERTON AND DISTRICT WATER COMPANY LIMITED",
                            Prefix = "CQ",
                            TraId = 9115
                        },
                        new SwaCode
                        {
                            Id = new Guid("242f5b1d-151c-4d3a-9133-c2e90790fed7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITY AND COUNTY OF SWANSEA COUNCIL",
                            Prefix = "MD",
                            TraId = 6855
                        },
                        new SwaCode
                        {
                            Id = new Guid("d2171030-eeb4-4974-8e83-28eda739860d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITY OF BRADFORD METROPOLITAN DISTRICT COUNCIL",
                            Prefix = "AV",
                            TraId = 4705
                        },
                        new SwaCode
                        {
                            Id = new Guid("49a79847-0afc-414d-b25d-f53e3306ca66"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITY OF CARDIFF COUNCIL",
                            Prefix = "QN",
                            TraId = 6815
                        },
                        new SwaCode
                        {
                            Id = new Guid("772ed1a7-d579-40e7-bb17-d08319ca982b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITY OF DONCASTER COUNCIL",
                            Prefix = "DQ",
                            TraId = 4410
                        },
                        new SwaCode
                        {
                            Id = new Guid("da820def-20be-4132-8d4b-304d5a22ec4b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITY OF LONDON CORPORATION",
                            Prefix = "CR",
                            TraId = 5030
                        },
                        new SwaCode
                        {
                            Id = new Guid("fe9ebd39-1cb9-47b5-b295-094db21ab57f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITY OF WAKEFIELD METROPOLITAN DISTRICT COUNCIL",
                            Prefix = "NY",
                            TraId = 4725
                        },
                        new SwaCode
                        {
                            Id = new Guid("a7f3d282-7f78-4e05-b800-7f966eaf5f4c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITY OF WESTMINSTER",
                            Prefix = "CT",
                            TraId = 5990
                        },
                        new SwaCode
                        {
                            Id = new Guid("82046ab7-239c-42ef-b7e6-0b7c69dbf02d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITY OF WOLVERHAMPTON COUNCIL",
                            Prefix = "PP",
                            TraId = 4635
                        },
                        new SwaCode
                        {
                            Id = new Guid("3fee691f-7f26-4af8-a3e2-ea07bf69af95"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITY OF YORK COUNCIL",
                            Prefix = "SH",
                            TraId = 2741
                        },
                        new SwaCode
                        {
                            Id = new Guid("ad7a0fec-b872-4901-9570-f7138c88b470"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITYFIBRE METRO NETWORKS LIMITED",
                            Prefix = "KG",
                            TraId = 7330
                        },
                        new SwaCode
                        {
                            Id = new Guid("759c0bfa-acdb-43ed-8e15-7bace20b0860"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CITYLINK TELECOMMUNICATIONS LIMITED",
                            Prefix = "XB",
                            TraId = 7261
                        },
                        new SwaCode
                        {
                            Id = new Guid("4c069a87-d9bd-4b1c-9460-543f66bb6c87"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CLOUD HQ DIDCOT FIBRE GP LTD",
                            Prefix = "F7",
                            TraId = 7508
                        },
                        new SwaCode
                        {
                            Id = new Guid("9b39683a-5ccb-45d4-9a23-f3c0588a85eb"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "COLT TECHNOLOGY SERVICES",
                            Prefix = "CS",
                            TraId = 7075
                        },
                        new SwaCode
                        {
                            Id = new Guid("1a260d6f-0b59-44c2-9b36-fbe46aad283f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "COMMUNICATIONS INFRASTRUCTURE NETWORKS LIMITED",
                            Prefix = "TS",
                            TraId = 7358
                        },
                        new SwaCode
                        {
                            Id = new Guid("ac8a9aee-166f-49d9-bd9f-1749857cfcba"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "COMMUNITY FIBRE LIMITED",
                            Prefix = "TY",
                            TraId = 7364
                        },
                        new SwaCode
                        {
                            Id = new Guid("3a6231f1-6295-4307-8d5f-15d8778a10d1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CONCEPT SOLUTIONS PEOPLE LTD",
                            Prefix = "SR",
                            TraId = 7335
                        },
                        new SwaCode
                        {
                            Id = new Guid("85091556-633a-41ab-aaa5-86af7678def8"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CONNEXIN LIMITED",
                            Prefix = "J7",
                            TraId = 7531
                        },
                        new SwaCode
                        {
                            Id = new Guid("879c169f-06e3-4db1-bf87-bd0fa127425f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CONWY COUNTY BOROUGH COUNCIL",
                            Prefix = "AA",
                            TraId = 6905
                        },
                        new SwaCode
                        {
                            Id = new Guid("0fd1354c-9172-4853-84b9-2db844bb7ffa"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CORANGE PERSONAL COMMUNICATIONS SERVICES LIMITED (Formerly HUTCHINSON MICROTEL)",
                            Prefix = "FS",
                            TraId = 7077
                        },
                        new SwaCode
                        {
                            Id = new Guid("fb0f4341-4fcf-4f16-b9ba-8e02d0828ea0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CORNERSTONE TELECOMMUNICATIONS INFRASTRUCTURE LIMITED",
                            Prefix = "P6",
                            TraId = 7567
                        },
                        new SwaCode
                        {
                            Id = new Guid("dc9f741c-abf8-4650-ba35-7392324e3895"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CORNWALL COUNCIL",
                            Prefix = "UF",
                            TraId = 840
                        },
                        new SwaCode
                        {
                            Id = new Guid("892807a7-b0df-4f84-b652-5e6c0dee778d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "COUNCIL OF THE ISLES OF SCILLY",
                            Prefix = "HJ",
                            TraId = 835
                        },
                        new SwaCode
                        {
                            Id = new Guid("a12957a1-15e7-47d6-bf66-75d5ac82582e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "COUNTY BROADBAND LTD",
                            Prefix = "A5",
                            TraId = 7369
                        },
                        new SwaCode
                        {
                            Id = new Guid("3c3ae4e2-2894-484f-9286-be08f2efca09"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "COVENTRY CITY COUNCIL",
                            Prefix = "DB",
                            TraId = 4610
                        },
                        new SwaCode
                        {
                            Id = new Guid("c7339c2e-94a8-405e-8ea6-21a2ad71e27e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CROSS RAIL LTD",
                            Prefix = "UM",
                            TraId = 7318
                        },
                        new SwaCode
                        {
                            Id = new Guid("8cbd3ce1-d3e0-4a9e-b66c-6758d16b8884"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CROSSKIT LIMITED",
                            Prefix = "R4",
                            TraId = 7573
                        },
                        new SwaCode
                        {
                            Id = new Guid("84f20e03-7898-4449-bfb6-b3c8a9887428"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "CUMBERLAND COUNCIL",
                            Prefix = "P5",
                            TraId = 940
                        },
                        new SwaCode
                        {
                            Id = new Guid("8ce590e0-2bb1-4f1a-b4c4-5b40cb63b3bb"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DARLINGTON BOROUGH COUNCIL",
                            Prefix = "HF",
                            TraId = 1350
                        },
                        new SwaCode
                        {
                            Id = new Guid("ed11490a-01b7-40d1-a7dd-61db1897de08"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DENBIGHSHIRE COUNTY COUNCIL",
                            Prefix = "QR",
                            TraId = 6830
                        },
                        new SwaCode
                        {
                            Id = new Guid("af36e71d-6c64-49d3-8078-d7fc352c4566"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DERBY CITY COUNCIL",
                            Prefix = "SZ",
                            TraId = 1055
                        },
                        new SwaCode
                        {
                            Id = new Guid("ed6e7c8e-4dfa-41fb-ac16-203da4271739"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DERBYSHIRE COUNTY COUNCIL",
                            Prefix = "DF",
                            TraId = 1050
                        },
                        new SwaCode
                        {
                            Id = new Guid("c8dded75-df45-4c27-abec-743a29f85c8a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DEVON COUNTY COUNCIL",
                            Prefix = "DG",
                            TraId = 1155
                        },
                        new SwaCode
                        {
                            Id = new Guid("9ccff1e3-69c2-4e83-94e8-1b15795106f5"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DFT ROAD STATISTICS DIVISION",
                            Prefix = "QU",
                            TraId = 7188
                        },
                        new SwaCode
                        {
                            Id = new Guid("02881862-a75d-4e43-b8ba-5e5a2656b0be"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DIAMOND CABLE (MANSFIELD)",
                            Prefix = "DL",
                            TraId = 7116
                        },
                        new SwaCode
                        {
                            Id = new Guid("c2938e70-f0c3-46b9-bb72-47b6e99eb5b8"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DIAMOND CABLE COMMUNICATIONS LIMITED",
                            Prefix = "QS",
                            TraId = 7189
                        },
                        new SwaCode
                        {
                            Id = new Guid("c1ec3f54-80f5-486b-a9ee-957192a75b62"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE DARLINGTON)",
                            Prefix = "DJ",
                            TraId = 7114
                        },
                        new SwaCode
                        {
                            Id = new Guid("bd853a31-2c0f-43f3-82eb-455f8ecf30c3"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE GRANTHAM)",
                            Prefix = "DH",
                            TraId = 7040
                        },
                        new SwaCode
                        {
                            Id = new Guid("2a16640b-b2a4-45ec-a0a1-ff6712d15019"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE LINCOLN)",
                            Prefix = "DK",
                            TraId = 7115
                        },
                        new SwaCode
                        {
                            Id = new Guid("1776a92a-dd9f-4243-98ec-80e897a02550"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE MELTON MOWBRAY)",
                            Prefix = "DM",
                            TraId = 7117
                        },
                        new SwaCode
                        {
                            Id = new Guid("7afbf1c9-a7ee-457a-bf2d-f3e4ace0a6dc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE NEWARK)",
                            Prefix = "DN",
                            TraId = 7118
                        },
                        new SwaCode
                        {
                            Id = new Guid("3ab3ded1-fbe8-4fe5-b5b9-097b6232a3d1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DIGITAL INFRASTRUCTURE LTD",
                            Prefix = "H2",
                            TraId = 7518
                        },
                        new SwaCode
                        {
                            Id = new Guid("97f5bcf2-bcc8-4cdc-8742-3a4887badf14"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DORSET COUNCIL",
                            Prefix = "B3",
                            TraId = 1265
                        },
                        new SwaCode
                        {
                            Id = new Guid("3b259c70-2530-40d4-8785-1fbdfd6a594e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DUDGEON OFFSHORE WIND LIMITED",
                            Prefix = "RB",
                            TraId = 7333
                        },
                        new SwaCode
                        {
                            Id = new Guid("18fecef3-c5de-46f7-951e-3e43f78e4150"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DUDLEY METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "DS",
                            TraId = 4615
                        },
                        new SwaCode
                        {
                            Id = new Guid("17e3b93c-9ba0-47c1-8542-d556f1e5788c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DURHAM COUNTY COUNCIL",
                            Prefix = "UG",
                            TraId = 1355
                        },
                        new SwaCode
                        {
                            Id = new Guid("43e7d76e-a859-44e3-bad2-753304b6c727"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "DWR CYMRU CYFYNGEDIG (WELSH WATER)",
                            Prefix = "PE",
                            TraId = 9107
                        },
                        new SwaCode
                        {
                            Id = new Guid("bb5950d2-2f8d-4d25-8757-cc845ccd409b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "E S PIPELINES LTD",
                            Prefix = "ZY",
                            TraId = 7260
                        },
                        new SwaCode
                        {
                            Id = new Guid("03439b42-0638-4cf9-bffd-e225b3a4a88b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EAST ANGLIA THREE LIMITED",
                            Prefix = "S4",
                            TraId = 7581
                        },
                        new SwaCode
                        {
                            Id = new Guid("617e3822-9a4c-4dd8-812e-1f60c116ec5e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EAST RIDING OF YORKSHIRE COUNCIL",
                            Prefix = "QV",
                            TraId = 2001
                        },
                        new SwaCode
                        {
                            Id = new Guid("c184c9f0-6303-4f06-8c72-c5285b15b0e8"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EAST SUSSEX COUNTY COUNCIL",
                            Prefix = "EA",
                            TraId = 1440
                        },
                        new SwaCode
                        {
                            Id = new Guid("41dca8bd-9ae9-40fc-b300-ad207b48fe44"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EASTERN POWER NETWORKS PLC",
                            Prefix = "EC",
                            TraId = 7010
                        },
                        new SwaCode
                        {
                            Id = new Guid("606653ce-71b3-41e5-b6fc-94a3d7949244"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ECLIPSE POWER NETWORKS LIMITED",
                            Prefix = "TR",
                            TraId = 7357
                        },
                        new SwaCode
                        {
                            Id = new Guid("edc3bf73-2d36-4831-aab9-21f40745f358"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EDF ENERGY CUSTOMERS LIMITED",
                            Prefix = "GU",
                            TraId = 7009
                        },
                        new SwaCode
                        {
                            Id = new Guid("5f698feb-c35e-49d1-be2b-be6229951688"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EDF ENERGY RENEWABLES LTD",
                            Prefix = "N2",
                            TraId = 7557
                        },
                        new SwaCode
                        {
                            Id = new Guid("e229cb19-7be8-4f60-aa62-89bc85d89442"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EE LIMITED",
                            Prefix = "YN",
                            TraId = 7250
                        },
                        new SwaCode
                        {
                            Id = new Guid("db31db75-acf0-4d1e-8748-48864b0888dd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EIRCOM (UK) LIMITED",
                            Prefix = "YD",
                            TraId = 7243
                        },
                        new SwaCode
                        {
                            Id = new Guid("7d26d92b-4e07-48c3-8574-4a47abdb7b75"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EIRGRID UK HOLDINGS LIMITED",
                            Prefix = "UU",
                            TraId = 7325
                        },
                        new SwaCode
                        {
                            Id = new Guid("2b26c355-d808-4702-8525-2748056b6434"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ELECLINK LIMITED",
                            Prefix = "ZB",
                            TraId = 7338
                        },
                        new SwaCode
                        {
                            Id = new Guid("d6892e31-2109-4bd6-a006-c297b576b1d7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ELECTRICITY NORTH WEST LIMITED",
                            Prefix = "JG",
                            TraId = 7005
                        },
                        new SwaCode
                        {
                            Id = new Guid("89bee6cc-641e-4cb5-9540-a02cbe3dc42b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ENERGIS COMMUNICATIONS LIMITED",
                            Prefix = "EL",
                            TraId = 7080
                        },
                        new SwaCode
                        {
                            Id = new Guid("3b490f89-46cd-4b2e-ac46-d75da4eecf66"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ENERGY ASSETS NETWORKS LIMITED",
                            Prefix = "TT",
                            TraId = 7359
                        },
                        new SwaCode
                        {
                            Id = new Guid("451516ef-202f-4b5a-bb04-8c839e99a760"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ENERGY ASSETS PIPELINES LIMITED",
                            Prefix = "TD",
                            TraId = 7348
                        },
                        new SwaCode
                        {
                            Id = new Guid("e035e6f4-cf7e-4da0-bb09-6b225144e87b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ENVIRONMENT AGENCY",
                            Prefix = "SV",
                            TraId = 7220
                        },
                        new SwaCode
                        {
                            Id = new Guid("84ba86e5-c7ec-4a79-87ea-10c3329f15b3"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ESP CONNECTIONS LIMITED",
                            Prefix = "YC",
                            TraId = 7242
                        },
                        new SwaCode
                        {
                            Id = new Guid("3251b3b6-7034-48cb-9a32-ccc33bc7f252"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ESP ELECTRICITY LIMITED",
                            Prefix = "VQ",
                            TraId = 7309
                        },
                        new SwaCode
                        {
                            Id = new Guid("c1c600f0-ab2f-4f7f-b0ca-0fe42fb503a4"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ESP ELECTRICITY LIMITED (Formerly LAING ENERGY LTD)",
                            Prefix = "XU",
                            TraId = 7268
                        },
                        new SwaCode
                        {
                            Id = new Guid("373caefd-e9c7-48c6-beb3-74aee6cf3fd5"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ESP NETWORKS LIMITED",
                            Prefix = "YU",
                            TraId = 7255
                        },
                        new SwaCode
                        {
                            Id = new Guid("7567f056-fe93-4d23-b251-c54c552328de"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ESP PIPELINES LIMITED",
                            Prefix = "YV",
                            TraId = 7256
                        },
                        new SwaCode
                        {
                            Id = new Guid("e2b5e502-063d-486f-9bcc-7f4e8bee164c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ESP WATER LIMITED",
                            Prefix = "N8",
                            TraId = 7564
                        },
                        new SwaCode
                        {
                            Id = new Guid("52273063-39ab-4ff1-a5e6-1da9f106b723"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ESSEX AND SUFFOLK WATER LIMITED",
                            Prefix = "EN",
                            TraId = 9120
                        },
                        new SwaCode
                        {
                            Id = new Guid("c7eb8d85-92ee-4714-b06b-7921fa48652e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ESSEX COUNTY COUNCIL",
                            Prefix = "EP",
                            TraId = 1585
                        },
                        new SwaCode
                        {
                            Id = new Guid("d070ee1d-d722-4777-b2e9-fd5fc30339c9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ESSO EXPLORATION AND PRODUCTION UK LIMITED",
                            Prefix = "EQ",
                            TraId = 7091
                        },
                        new SwaCode
                        {
                            Id = new Guid("39bdb612-b08b-4508-a54f-93ea5929a3ff"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EUNETWORKS GROUP LIMITED",
                            Prefix = "VN",
                            TraId = 7307
                        },
                        new SwaCode
                        {
                            Id = new Guid("6cbe2ddd-822e-4650-897f-abe2cdd1f217"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "E-VOLVE SOLUTIONS LTD",
                            Prefix = "S2",
                            TraId = 7579
                        },
                        new SwaCode
                        {
                            Id = new Guid("cab5341f-a780-4c49-8a3d-5d445892ba32"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EXASCALE LIMITED",
                            Prefix = "F2",
                            TraId = 7503
                        },
                        new SwaCode
                        {
                            Id = new Guid("14bb602d-c69e-4c85-a12c-c0aa12c1926b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "EXPONENTIAL-E LIMITED",
                            Prefix = "K8",
                            TraId = 7540
                        },
                        new SwaCode
                        {
                            Id = new Guid("5a914122-3610-4f79-a61d-88d0b7624889"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "F & W NETWORKS LTD",
                            Prefix = "C8",
                            TraId = 7386
                        },
                        new SwaCode
                        {
                            Id = new Guid("bce3fbf7-723a-43ea-882e-a6f45b97525e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FACTCO",
                            Prefix = "G4",
                            TraId = 7511
                        },
                        new SwaCode
                        {
                            Id = new Guid("12ceb5a5-ac00-4347-a754-907124992903"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FIBERNET UK LIMITED",
                            Prefix = "SY",
                            TraId = 7223
                        },
                        new SwaCode
                        {
                            Id = new Guid("b60abf19-45c2-4d9f-aa58-c90d8bf7c501"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FIBRE ASSETS LTD",
                            Prefix = "G3",
                            TraId = 7510
                        },
                        new SwaCode
                        {
                            Id = new Guid("1a69ddc5-6036-4c16-aba9-d551c85fcb92"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FIBRENATION LIMITED",
                            Prefix = "M7",
                            TraId = 7554
                        },
                        new SwaCode
                        {
                            Id = new Guid("0ccfd949-ef0d-4716-9812-babb69de3a61"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FIBRESPEED LIMITED",
                            Prefix = "VL",
                            TraId = 7305
                        },
                        new SwaCode
                        {
                            Id = new Guid("6a23aa03-1a04-4205-a623-62676aa4da16"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FIBREWAVE NETWORKS LIMITED",
                            Prefix = "LH",
                            TraId = 7332
                        },
                        new SwaCode
                        {
                            Id = new Guid("c112d0ca-cc2e-4c66-8637-5deb221f31c6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FIBRUS NETWORKS GB LTD",
                            Prefix = "S3",
                            TraId = 7580
                        },
                        new SwaCode
                        {
                            Id = new Guid("c9294bab-ca0d-4ac4-8f36-145645d1b937"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FIBRUS NETWORKS LIMITED",
                            Prefix = "K6",
                            TraId = 7537
                        },
                        new SwaCode
                        {
                            Id = new Guid("704d40c0-c61e-4209-a41d-52f5603f13ed"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FLINTSHIRE COUNTY COUNCIL",
                            Prefix = "QX",
                            TraId = 6835
                        },
                        new SwaCode
                        {
                            Id = new Guid("cf99c4f2-c7e3-495e-aeef-803ba6117f4e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FREEDOM FIBRE LIMITED",
                            Prefix = "K5",
                            TraId = 7539
                        },
                        new SwaCode
                        {
                            Id = new Guid("05eedf45-7c56-4ea8-9cb1-260ae4bb3610"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FUJITSU SERVICES LIMITED",
                            Prefix = "KB",
                            TraId = 7328
                        },
                        new SwaCode
                        {
                            Id = new Guid("85b60975-859b-497b-8796-ddb31b47e3cc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FULCRUM ELECTRICITY ASSETS LIMITED",
                            Prefix = "A4",
                            TraId = 7368
                        },
                        new SwaCode
                        {
                            Id = new Guid("6dbabcf3-21da-46a3-b4c3-3c041ff41104"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FULCRUM PIPELINES LIMITED",
                            Prefix = "WY",
                            TraId = 7294
                        },
                        new SwaCode
                        {
                            Id = new Guid("53c5a827-b6b0-4cb7-a088-86b6afe6351d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "FULL FIBRE LIMITED",
                            Prefix = "B6",
                            TraId = 7376
                        },
                        new SwaCode
                        {
                            Id = new Guid("ccbb3a08-96cf-46fc-8a14-0a72ef251fac"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "G.NETWORK COMMUNICATIONS LIMITED",
                            Prefix = "TW",
                            TraId = 7362
                        },
                        new SwaCode
                        {
                            Id = new Guid("8fc23067-2830-4043-85fc-45288444d0fd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GAMMA TELECOM LTD",
                            Prefix = "YB",
                            TraId = 7241
                        },
                        new SwaCode
                        {
                            Id = new Guid("6caf86e6-cf11-43b9-a9d1-6a7f30bc2dad"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GATESHEAD METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "EX",
                            TraId = 4505
                        },
                        new SwaCode
                        {
                            Id = new Guid("a8c1df9c-53f8-4a36-970a-74c77f129df0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GC PAN EUROPEAN CROSSING UK LIMITED",
                            Prefix = "ZL",
                            TraId = 7229
                        },
                        new SwaCode
                        {
                            Id = new Guid("1f10a0f9-7e10-4959-afd2-0ad58de98109"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GDF SUEZ TEESSIDE LIMITED",
                            Prefix = "SE",
                            TraId = 7016
                        },
                        new SwaCode
                        {
                            Id = new Guid("808fcd22-1118-4769-8503-1af9febf9ce1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GIGACLEAR LIMITED",
                            Prefix = "KA",
                            TraId = 7329
                        },
                        new SwaCode
                        {
                            Id = new Guid("b894392a-cc95-4cfa-befc-b5662e3a0df9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GLIDE BUSINESS LIMITED",
                            Prefix = "ZC",
                            TraId = 7343
                        },
                        new SwaCode
                        {
                            Id = new Guid("d005e481-0128-4137-a1ec-b6bc9f224b74"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GLOBAL ONE COMMUNICATIONS LIMITED",
                            Prefix = "LS",
                            TraId = 7084
                        },
                        new SwaCode
                        {
                            Id = new Guid("223c36cf-76f3-4614-ac68-ca6be0cfeeed"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GLOBAL REACH NETWORKS LIMITED",
                            Prefix = "M6",
                            TraId = 7553
                        },
                        new SwaCode
                        {
                            Id = new Guid("163ef86f-2ed0-468d-ab0e-a1bde256b899"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GLOBAL TELECOMMUNICATIONS LIMITED",
                            Prefix = "ZK",
                            TraId = 7228
                        },
                        new SwaCode
                        {
                            Id = new Guid("8f7c86ef-30c5-4742-b01d-5542d758c52c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GLOUCESTERSHIRE COUNTY COUNCIL",
                            Prefix = "EY",
                            TraId = 1600
                        },
                        new SwaCode
                        {
                            Id = new Guid("4dc4db4d-4a5f-4319-9772-bf1fa696576a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GOFIBRE HOLDINGS LIMITED",
                            Prefix = "M4",
                            TraId = 7551
                        },
                        new SwaCode
                        {
                            Id = new Guid("3135423a-02e6-4e7b-ac69-4d1c6b80d7e5"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GRAIN COMMUNICATIONS LIMITED",
                            Prefix = "TJ",
                            TraId = 7351
                        },
                        new SwaCode
                        {
                            Id = new Guid("fd6afe52-c688-44f7-8e95-637bd9897d38"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GRAYSHOTT GIGABIT LIMITED",
                            Prefix = "K7",
                            TraId = 7538
                        },
                        new SwaCode
                        {
                            Id = new Guid("b312fbe3-fd1e-4c42-baad-91b0757ce0ad"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GREENLINK INTERCONNECTOR LIMITED",
                            Prefix = "L8",
                            TraId = 7548
                        },
                        new SwaCode
                        {
                            Id = new Guid("bc6de817-3d37-4f03-8f19-edc2c26a1c6b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GTC PIPELINES LIMITED",
                            Prefix = "ZP",
                            TraId = 7231
                        },
                        new SwaCode
                        {
                            Id = new Guid("b0f92fb2-8cae-4c86-9d77-c4412381d8fa"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "GWYNEDD COUNCIL",
                            Prefix = "QM",
                            TraId = 6810
                        },
                        new SwaCode
                        {
                            Id = new Guid("c07564bb-69c7-4994-8347-c5d8ac6b0264"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HAFREN DYFRDWY CYFYNGEDIG",
                            Prefix = "ZU",
                            TraId = 9138
                        },
                        new SwaCode
                        {
                            Id = new Guid("3ed4fa9e-5167-4b69-ac83-4dd95640953d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HAFREN DYFRDWY CYFYNGEDIG (Formerly CHESTER WATERWORKS)",
                            Prefix = "CP",
                            TraId = 9114
                        },
                        new SwaCode
                        {
                            Id = new Guid("896e9b33-a164-44e5-933f-09083f76c96d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HALTON BOROUGH COUNCIL",
                            Prefix = "AN",
                            TraId = 650
                        },
                        new SwaCode
                        {
                            Id = new Guid("fb574e28-e2f0-4b32-8a37-34c2e21dd5cd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HAMPSHIRE COUNTY COUNCIL",
                            Prefix = "FF",
                            TraId = 1770
                        },
                        new SwaCode
                        {
                            Id = new Guid("a7b7fb59-c3ab-45f3-b9c3-767a4675fc0f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HAMPSTEAD FIBRE LIMITED",
                            Prefix = "L3",
                            TraId = 7543
                        },
                        new SwaCode
                        {
                            Id = new Guid("8c228dd8-1648-455f-b122-d489c80b701b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HARLAXTON ENERGY NETWORKS LIMITED",
                            Prefix = "YZ",
                            TraId = 7342
                        },
                        new SwaCode
                        {
                            Id = new Guid("9e6034d2-e880-46c3-9fa2-a62753a6bd8d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HARLAXTON GAS NETWORKS LTD",
                            Prefix = "B4",
                            TraId = 7374
                        },
                        new SwaCode
                        {
                            Id = new Guid("a2956aeb-cc23-4978-80c5-4eefd8ef7837"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HARTLEPOOL BOROUGH COUNCIL",
                            Prefix = "RA",
                            TraId = 724
                        },
                        new SwaCode
                        {
                            Id = new Guid("8b97ee2f-d7d8-4557-8bfa-79fef36af15b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HARTLEPOOL WATER",
                            Prefix = "FJ",
                            TraId = 9122
                        },
                        new SwaCode
                        {
                            Id = new Guid("998a12b1-39b8-48b0-bba9-93ec9d8b6c0f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HEN BEUDY SERVICES LIMITED",
                            Prefix = "E3",
                            TraId = 7396
                        },
                        new SwaCode
                        {
                            Id = new Guid("e3cc8019-61d5-45ed-a479-ed2351c562ad"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HENDY WIND FARM LIMITED",
                            Prefix = "R8",
                            TraId = 7577
                        },
                        new SwaCode
                        {
                            Id = new Guid("1d1891f5-6f0d-4e56-bfec-b0cb9e2085fd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HEREFORDSHIRE COUNCIL",
                            Prefix = "FL",
                            TraId = 1850
                        },
                        new SwaCode
                        {
                            Id = new Guid("3c288847-76c3-4293-9123-cc3f11b77ba1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HERTFORDSHIRE COUNTY COUNCIL",
                            Prefix = "FM",
                            TraId = 1900
                        },
                        new SwaCode
                        {
                            Id = new Guid("490dabac-6889-4015-8742-c00ac79636af"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HIBERNIA ATLANTIC (UK) LIMITED",
                            Prefix = "KK",
                            TraId = 7331
                        },
                        new SwaCode
                        {
                            Id = new Guid("841dc90f-419e-45b8-b7ef-b8748569a469"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HS2 LTD",
                            Prefix = "TA",
                            TraId = 7347
                        },
                        new SwaCode
                        {
                            Id = new Guid("658e359d-e9a1-43ac-9e4d-c3d2e396947e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HULL CITY COUNCIL",
                            Prefix = "RG",
                            TraId = 2004
                        },
                        new SwaCode
                        {
                            Id = new Guid("353b397f-6bee-49a3-ae57-304e74374687"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "Hutchison 3G UK Limited",
                            Prefix = "XJ",
                            TraId = 7264
                        },
                        new SwaCode
                        {
                            Id = new Guid("2af22c7f-036d-48bf-adc3-ab4d4b38cf4f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "HYPEROPTIC LTD",
                            Prefix = "TF",
                            TraId = 7349
                        },
                        new SwaCode
                        {
                            Id = new Guid("5249fa7e-a16b-4b97-affe-b6dc580c5480"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ICOSA WATER LTD",
                            Prefix = "C2",
                            TraId = 7380
                        },
                        new SwaCode
                        {
                            Id = new Guid("2c0fc8fa-37eb-45b6-8158-6021ef9c6265"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "IN FOCUS PUBLIC NETWORKS LIMITED",
                            Prefix = "RE",
                            TraId = 60
                        },
                        new SwaCode
                        {
                            Id = new Guid("3e1f1a57-ab2a-4b16-a72e-859d15e8375f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "INDEPENDENT DISTRIBUTION CONNECTION SPECIALISTS LIMITED",
                            Prefix = "S5",
                            TraId = 7582
                        },
                        new SwaCode
                        {
                            Id = new Guid("58ea9e26-4eb2-4b3b-96a5-8e918a596b49"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "INDEPENDENT PIPELINES LIMITED",
                            Prefix = "ST",
                            TraId = 7218
                        },
                        new SwaCode
                        {
                            Id = new Guid("3fd7487d-8ddf-4da4-a695-9c0adf2bd1ea"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "INDEPENDENT POWER NETWORKS LIMITED",
                            Prefix = "WK",
                            TraId = 7281
                        },
                        new SwaCode
                        {
                            Id = new Guid("549d2d58-afcd-4ea4-bdbb-fd485f734029"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "INDEPENDENT WATER NETWORKS LIMITED",
                            Prefix = "UV",
                            TraId = 7326
                        },
                        new SwaCode
                        {
                            Id = new Guid("ac517f3a-4b76-4291-8c3e-893b928d94af"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "INDIGO PIPELINES LIMITED",
                            Prefix = "VX",
                            TraId = 7313
                        },
                        new SwaCode
                        {
                            Id = new Guid("1a696785-76c5-405c-9758-b4dcd692d351"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "INDIGO POWER LTD",
                            Prefix = "M8",
                            TraId = 7555
                        },
                        new SwaCode
                        {
                            Id = new Guid("34d827a9-3b45-4f62-a41c-9583c5d4aac1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "INTERNET CONNECTIONS LTD",
                            Prefix = "TK",
                            TraId = 7352
                        },
                        new SwaCode
                        {
                            Id = new Guid("54bd1657-c516-4d4e-968e-cbeb5e89ef43"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "INTERNETTY LTD",
                            Prefix = "D6",
                            TraId = 7391
                        },
                        new SwaCode
                        {
                            Id = new Guid("d526da63-869a-41ec-826b-8c4661ab1e26"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "INTEROUTE NETWORKS LIMITED",
                            Prefix = "YF",
                            TraId = 7245
                        },
                        new SwaCode
                        {
                            Id = new Guid("555f1bf1-afb6-4580-8bed-383799d78c74"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "IONICA LIMITED",
                            Prefix = "FT",
                            TraId = 7074
                        },
                        new SwaCode
                        {
                            Id = new Guid("6640b3f5-53a5-4553-943e-6af3be20ecb7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ISLE OF ANGLESEY COUNTY COUNCIL",
                            Prefix = "QC",
                            TraId = 6805
                        },
                        new SwaCode
                        {
                            Id = new Guid("030f7399-4039-4fac-982f-8083dc99233c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ISLE OF WIGHT COUNCIL",
                            Prefix = "RF",
                            TraId = 2114
                        },
                        new SwaCode
                        {
                            Id = new Guid("f218b0d2-a8e2-47b7-919a-7a2534cf0dee"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ITS TECHNOLOGY GROUP LIMITED",
                            Prefix = "A6",
                            TraId = 7370
                        },
                        new SwaCode
                        {
                            Id = new Guid("cee30038-9801-4a94-a328-ee4753219914"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "IX WIRELESS LIMITED",
                            Prefix = "B7",
                            TraId = 7377
                        },
                        new SwaCode
                        {
                            Id = new Guid("66570486-89c6-4971-ae7f-2492d48d2e4d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "JOHN JONES LTD",
                            Prefix = "FW",
                            TraId = 7174
                        },
                        new SwaCode
                        {
                            Id = new Guid("c7a3044f-79b3-4095-b857-39f4ae6e1c5d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "JURASSIC FIBRE LIMITED",
                            Prefix = "C9",
                            TraId = 7387
                        },
                        new SwaCode
                        {
                            Id = new Guid("0016e5ce-c46d-4ad4-9236-6449330dc635"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "KCOM GROUP LIMITED",
                            Prefix = "GG",
                            TraId = 7073
                        },
                        new SwaCode
                        {
                            Id = new Guid("9a4a3c6a-1bbc-4218-a6c0-2cc8fbd726b3"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "KCOM GROUP LIMITED (NATIONAL)",
                            Prefix = "MY",
                            TraId = 7082
                        },
                        new SwaCode
                        {
                            Id = new Guid("0a9dd22d-e307-4878-a820-ed1421a8c7eb"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "KENT COUNTY COUNCIL",
                            Prefix = "GE",
                            TraId = 2275
                        },
                        new SwaCode
                        {
                            Id = new Guid("7ce9d0f8-ab55-4890-8fa0-0c334b3fc397"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "KIRKLEES COUNCIL",
                            Prefix = "GJ",
                            TraId = 4715
                        },
                        new SwaCode
                        {
                            Id = new Guid("6d95820f-9301-4559-8d0f-1642556aad4e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "KNOWSLEY METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "GK",
                            TraId = 4305
                        },
                        new SwaCode
                        {
                            Id = new Guid("a9e26aec-6c2d-4c46-9721-b700486a7316"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "KPN EURORINGS B.V. (Formerly KPN TELECOM UK LTD)",
                            Prefix = "ZJ",
                            TraId = 7227
                        },
                        new SwaCode
                        {
                            Id = new Guid("3d24c89e-1962-41a3-b57b-b4445aeddcc1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "KPN EURORINGS BV",
                            Prefix = "XT",
                            TraId = 7267
                        },
                        new SwaCode
                        {
                            Id = new Guid("369736e9-cfc9-458b-8f11-4e48195c7b42"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LANCASHIRE COUNTY COUNCIL",
                            Prefix = "GM",
                            TraId = 2371
                        },
                        new SwaCode
                        {
                            Id = new Guid("1fba15eb-af90-4722-b44f-cfc6581cf177"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LANCASTER UNIVERSITY NETWORK SERVICES LIMITED",
                            Prefix = "WE",
                            TraId = 7277
                        },
                        new SwaCode
                        {
                            Id = new Guid("597ba01b-9e5e-43be-bc59-b791f8e73849"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LAST MILE ELECTRICITY LIMITED",
                            Prefix = "C7",
                            TraId = 7385
                        },
                        new SwaCode
                        {
                            Id = new Guid("596495ed-85ac-4fad-b527-6859b453d5b3"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LAST MILE ELECTRICITY LIMITED (Formerly GLOBAL UTILITY CONNECTIONS)",
                            Prefix = "XV",
                            TraId = 7269
                        },
                        new SwaCode
                        {
                            Id = new Guid("1b594aeb-557a-408d-a1eb-1cb98d50064f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LAST MILE GAS LIMITED",
                            Prefix = "VS",
                            TraId = 7311
                        },
                        new SwaCode
                        {
                            Id = new Guid("aae897b9-dc33-48dc-8b7a-42c4bccce93f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LAST MILE TELECOM LIMITED",
                            Prefix = "P7",
                            TraId = 7568
                        },
                        new SwaCode
                        {
                            Id = new Guid("5178516f-6259-4ea6-a77d-3870a5cb0d1f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LATOS DATA CENTRE LTD",
                            Prefix = "N4",
                            TraId = 7559
                        },
                        new SwaCode
                        {
                            Id = new Guid("68469280-cd75-4c7e-b8b4-08e9122bcff9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LEEDS CITY COUNCIL",
                            Prefix = "GP",
                            TraId = 4720
                        },
                        new SwaCode
                        {
                            Id = new Guid("f23b0ce8-9e86-4fcc-9944-74be176440e9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LEEP ELECTRICITY NETWORKS LIMITED",
                            Prefix = "F5",
                            TraId = 7506
                        },
                        new SwaCode
                        {
                            Id = new Guid("65754b58-d751-4007-ac31-89cd4dfb0f13"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LEEP NETWORKS (WATER) LIMITED",
                            Prefix = "TQ",
                            TraId = 7356
                        },
                        new SwaCode
                        {
                            Id = new Guid("48606e61-deb5-4bc8-b7ad-20932a24c4c0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LEICESTER CITY COUNCIL",
                            Prefix = "EW",
                            TraId = 2465
                        },
                        new SwaCode
                        {
                            Id = new Guid("40aad9cd-9e13-4b7b-98e0-3f1eedf615f2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LEICESTERSHIRE COUNTY COUNCIL",
                            Prefix = "GQ",
                            TraId = 2460
                        },
                        new SwaCode
                        {
                            Id = new Guid("b755d712-a34a-4a09-b102-860df6259487"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LEVEL 3 COMMUNICATIONS LIMITED",
                            Prefix = "ZQ",
                            TraId = 7232
                        },
                        new SwaCode
                        {
                            Id = new Guid("4187672c-6033-473d-8d95-856a0278c92d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LIGHTNING FIBRE LIMITED",
                            Prefix = "B8",
                            TraId = 7378
                        },
                        new SwaCode
                        {
                            Id = new Guid("275b6447-2e50-4ca4-be4a-9dfb62e07ea3"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LIGHTSPEED NETWORKS LTD",
                            Prefix = "H8",
                            TraId = 7524
                        },
                        new SwaCode
                        {
                            Id = new Guid("15f85e76-9dc0-47a7-b917-449def337d21"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LINCOLNSHIRE COUNTY COUNCIL",
                            Prefix = "GS",
                            TraId = 2500
                        },
                        new SwaCode
                        {
                            Id = new Guid("a7d099ef-39e9-4f94-aac8-ae3a4c813f74"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LIT FIBRE GROUP LTD",
                            Prefix = "G2",
                            TraId = 7509
                        },
                        new SwaCode
                        {
                            Id = new Guid("7dc52cba-58ce-4e45-9d94-5882794c95c0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LIVERPOOL CITY COUNCIL",
                            Prefix = "GT",
                            TraId = 4310
                        },
                        new SwaCode
                        {
                            Id = new Guid("cdf61c6f-01cd-4763-8506-d3c749aa05c1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF BARKING AND DAGENHAM",
                            Prefix = "AG",
                            TraId = 5060
                        },
                        new SwaCode
                        {
                            Id = new Guid("fa1ab884-79fd-4d7e-aa8e-d33bd26d2efc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF BARNET",
                            Prefix = "AH",
                            TraId = 5090
                        },
                        new SwaCode
                        {
                            Id = new Guid("41d70572-3180-474a-94f6-d7abc8af5c98"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF BEXLEY",
                            Prefix = "AP",
                            TraId = 5120
                        },
                        new SwaCode
                        {
                            Id = new Guid("aeaa9680-3a11-49f4-9136-9cc294830798"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF BRENT",
                            Prefix = "AW",
                            TraId = 5150
                        },
                        new SwaCode
                        {
                            Id = new Guid("42ac50a3-4b92-4e67-b993-1d28b8408f14"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF BROMLEY",
                            Prefix = "BF",
                            TraId = 5180
                        },
                        new SwaCode
                        {
                            Id = new Guid("f90a23fc-1c93-47c0-982e-5d96e4c16f32"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF CAMDEN",
                            Prefix = "CM",
                            TraId = 5210
                        },
                        new SwaCode
                        {
                            Id = new Guid("c0a068ab-bb34-4489-86c5-de27414aa398"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF CROYDON",
                            Prefix = "DD",
                            TraId = 5240
                        },
                        new SwaCode
                        {
                            Id = new Guid("296b632d-c611-42d6-8d69-643983f65c82"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF EALING",
                            Prefix = "DV",
                            TraId = 5270
                        },
                        new SwaCode
                        {
                            Id = new Guid("33747d53-87f5-4cfd-91e6-ae46727ac2cf"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF ENFIELD",
                            Prefix = "EM",
                            TraId = 5300
                        },
                        new SwaCode
                        {
                            Id = new Guid("5779c71e-7f96-4f36-9211-252913a5c323"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF HACKNEY",
                            Prefix = "FD",
                            TraId = 5360
                        },
                        new SwaCode
                        {
                            Id = new Guid("ff56bea2-1b9d-41f1-9e50-f870bcfd0fb7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF HAMMERSMITH & FULHAM",
                            Prefix = "FE",
                            TraId = 5390
                        },
                        new SwaCode
                        {
                            Id = new Guid("e72e172d-9fe9-4401-aad3-85113bc9b05c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF HARINGEY",
                            Prefix = "FG",
                            TraId = 5420
                        },
                        new SwaCode
                        {
                            Id = new Guid("17009f37-ccef-4c0b-8896-719efac9906a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF HARROW",
                            Prefix = "FH",
                            TraId = 5450
                        },
                        new SwaCode
                        {
                            Id = new Guid("df90d956-be26-4ac6-a749-9cc288d73631"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF HAVERING",
                            Prefix = "FK",
                            TraId = 5480
                        },
                        new SwaCode
                        {
                            Id = new Guid("30bc74f7-36e5-4f65-984f-5c8b421fc0cf"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF HILLINGDON",
                            Prefix = "FP",
                            TraId = 5510
                        },
                        new SwaCode
                        {
                            Id = new Guid("e5858459-17b0-40dc-a151-689e57e356a6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF HOUNSLOW",
                            Prefix = "FQ",
                            TraId = 5540
                        },
                        new SwaCode
                        {
                            Id = new Guid("fd981842-1a6a-4a52-8a23-1c71c19f68fa"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF ISLINGTON",
                            Prefix = "FV",
                            TraId = 5570
                        },
                        new SwaCode
                        {
                            Id = new Guid("057e4fad-7baa-42e5-ac3a-d09267bbb441"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF LAMBETH",
                            Prefix = "GL",
                            TraId = 5660
                        },
                        new SwaCode
                        {
                            Id = new Guid("270311ad-5d78-4e71-9b77-1110df0428a9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF LEWISHAM",
                            Prefix = "GR",
                            TraId = 5690
                        },
                        new SwaCode
                        {
                            Id = new Guid("1bc3695a-4e1b-478d-b0f9-9c247514d1ab"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF MERTON",
                            Prefix = "HC",
                            TraId = 5720
                        },
                        new SwaCode
                        {
                            Id = new Guid("121f4633-95bb-4764-9860-ecd89b487838"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF NEWHAM",
                            Prefix = "HS",
                            TraId = 5750
                        },
                        new SwaCode
                        {
                            Id = new Guid("f0b73d06-0715-4e89-9c22-f8523ba92fbf"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF REDBRIDGE",
                            Prefix = "KM",
                            TraId = 5780
                        },
                        new SwaCode
                        {
                            Id = new Guid("3e863792-73eb-4a66-8022-7cd3e96013c2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF RICHMOND UPON THAMES",
                            Prefix = "KP",
                            TraId = 5810
                        },
                        new SwaCode
                        {
                            Id = new Guid("0322f859-cecf-4895-be20-b2839431505a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF SOUTHWARK",
                            Prefix = "LR",
                            TraId = 5840
                        },
                        new SwaCode
                        {
                            Id = new Guid("55b8e177-6ba7-4378-9cdf-990765b46690"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF SUTTON",
                            Prefix = "MB",
                            TraId = 5870
                        },
                        new SwaCode
                        {
                            Id = new Guid("0210a160-25c5-4d45-935e-14f7def9592b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF TOWER HAMLETS",
                            Prefix = "NA",
                            TraId = 5900
                        },
                        new SwaCode
                        {
                            Id = new Guid("e80006a4-811d-46cc-8835-6f6ff39a8465"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF WALTHAM FOREST",
                            Prefix = "PA",
                            TraId = 5930
                        },
                        new SwaCode
                        {
                            Id = new Guid("f157c393-d753-4cf7-b7cf-5da7ff26d479"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON BOROUGH OF WANDSWORTH",
                            Prefix = "PB",
                            TraId = 5960
                        },
                        new SwaCode
                        {
                            Id = new Guid("1f9cc569-87a6-46da-aec8-fd38e6afc96f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON TRANSPORT LIMITED",
                            Prefix = "RK",
                            TraId = 7210
                        },
                        new SwaCode
                        {
                            Id = new Guid("b17b9104-c2ab-4bae-a3da-ecd4dd05c31f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LONDON UNDERGROUND LIMITED",
                            Prefix = "GV",
                            TraId = 7072
                        },
                        new SwaCode
                        {
                            Id = new Guid("76730e89-b1f0-42a6-b71b-c7eb5c7ef0db"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LUMEN TECHNOLOGIES INC",
                            Prefix = "RT",
                            TraId = 7183
                        },
                        new SwaCode
                        {
                            Id = new Guid("68ee583c-b68d-42c4-aba1-3148d0386822"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LUMEN TECHNOLOGIES INC (Formerly CENTURYLINK COMMUNICATIONS UK LIMITED)",
                            Prefix = "BB",
                            TraId = 7094
                        },
                        new SwaCode
                        {
                            Id = new Guid("0ded0259-fbef-4e5a-84e7-2258203ac7fa"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LUNS LTD",
                            Prefix = "UP",
                            TraId = 7320
                        },
                        new SwaCode
                        {
                            Id = new Guid("7f2f48cf-0afc-4e2d-82ec-a71bb29ac16a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "LUTON BOROUGH COUNCIL",
                            Prefix = "JA",
                            TraId = 230
                        },
                        new SwaCode
                        {
                            Id = new Guid("1f0fdbe7-6930-4c48-b7d7-9288c24a2eff"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MAINLINE PIPELINES LIMITED",
                            Prefix = "GW",
                            TraId = 7090
                        },
                        new SwaCode
                        {
                            Id = new Guid("b0c02f04-4c37-4283-b618-348d5022e4a7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MANCHESTER CITY COUNCIL",
                            Prefix = "GX",
                            TraId = 4215
                        },
                        new SwaCode
                        {
                            Id = new Guid("c0d9828c-211f-4283-9b69-888b400da8d1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MEDWAY COUNCIL",
                            Prefix = "JL",
                            TraId = 2280
                        },
                        new SwaCode
                        {
                            Id = new Guid("17afc3ab-da95-4bde-8f25-2ef12b3b452d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MERTHYR TYDFIL COUNTY BOROUGH COUNCIL",
                            Prefix = "HB",
                            TraId = 6925
                        },
                        new SwaCode
                        {
                            Id = new Guid("6ce1d9b9-d9c5-4992-ae18-fd95fa595b01"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "METRO DIGITAL TELEVISION LIMITED",
                            Prefix = "HD",
                            TraId = 7176
                        },
                        new SwaCode
                        {
                            Id = new Guid("7e789686-2bff-4a69-9140-74d4d4769b1e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MIDDLESBROUGH BOROUGH COUNCIL",
                            Prefix = "RL",
                            TraId = 734
                        },
                        new SwaCode
                        {
                            Id = new Guid("ed559acf-bc9e-4953-aa12-e482259ec4bf"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MILTON KEYNES CITY COUNCIL",
                            Prefix = "JM",
                            TraId = 435
                        },
                        new SwaCode
                        {
                            Id = new Guid("09f22419-b014-473b-9120-a76f8c8ac07d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MLL TELECOM LTD.",
                            Prefix = "WF",
                            TraId = 7278
                        },
                        new SwaCode
                        {
                            Id = new Guid("4364c9fe-5f75-4d62-ad70-de03808eae63"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MONMOUTHSHIRE COUNTY COUNCIL",
                            Prefix = "RM",
                            TraId = 6840
                        },
                        new SwaCode
                        {
                            Id = new Guid("939b3dd6-b6b9-4f4d-a64d-bebdee1fb7ec"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MS3 NETWORKS LIMITED",
                            Prefix = "H7",
                            TraId = 7523
                        },
                        new SwaCode
                        {
                            Id = new Guid("8bd30891-3129-4b0b-b3c8-b152969d2dad"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MUA ELECTRICITY LIMITED",
                            Prefix = "A2",
                            TraId = 7366
                        },
                        new SwaCode
                        {
                            Id = new Guid("8733d3b1-cf1f-4516-997f-e112d962f9b7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MUA GAS LIMITED",
                            Prefix = "A3",
                            TraId = 7367
                        },
                        new SwaCode
                        {
                            Id = new Guid("cc51afc6-55c8-4be2-95c4-bf2cd9cd62fe"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "MY FIBRE LIMITED",
                            Prefix = "F3",
                            TraId = 7504
                        },
                        new SwaCode
                        {
                            Id = new Guid("f131b2aa-d22b-4e7e-ad9a-fea40821a427"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NATIONAL GAS TRANSMISSION",
                            Prefix = "E4",
                            TraId = 7397
                        },
                        new SwaCode
                        {
                            Id = new Guid("b12fc8ba-56e2-4bba-8ed6-87caf8a1074b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (EAST MIDLANDS)",
                            Prefix = "DY",
                            TraId = 7011
                        },
                        new SwaCode
                        {
                            Id = new Guid("93734f37-c056-47a0-886f-3cc800b59947"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (SOUTH WALES)",
                            Prefix = "LL",
                            TraId = 7012
                        },
                        new SwaCode
                        {
                            Id = new Guid("b984597e-8aae-4b82-a5c3-3c3c7c36f6d4"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (SOUTH WEST)",
                            Prefix = "LN",
                            TraId = 7003
                        },
                        new SwaCode
                        {
                            Id = new Guid("4fd25d11-42e7-4bbc-9cf2-c3e8ed702451"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (WEST MIDLANDS)",
                            Prefix = "HM",
                            TraId = 7007
                        },
                        new SwaCode
                        {
                            Id = new Guid("55da8c7c-f965-424b-977d-c2ba9661e030"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NATIONAL GRID ELECTRICITY TRANSMISSION PLC",
                            Prefix = "HP",
                            TraId = 7015
                        },
                        new SwaCode
                        {
                            Id = new Guid("cf32f103-a9d9-4a3f-adab-7eef7284d470"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NATIONAL GRID TELECOMS",
                            Prefix = "SJ",
                            TraId = 7145
                        },
                        new SwaCode
                        {
                            Id = new Guid("f37f9018-2e60-4f1d-a46d-fe0f729c1f31"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NATIONAL HIGHWAYS",
                            Prefix = "FN",
                            TraId = 11
                        },
                        new SwaCode
                        {
                            Id = new Guid("f355d1eb-61c3-4e72-bf54-1b1b32da6bab"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NEATH PORT TALBOT COUNTY BOROUGH COUNCIL",
                            Prefix = "HQ",
                            TraId = 6930
                        },
                        new SwaCode
                        {
                            Id = new Guid("bbfd03a6-becc-4c60-8915-0536becb28a5"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NEOS NETWORKS LIMITED",
                            Prefix = "YE",
                            TraId = 7244
                        },
                        new SwaCode
                        {
                            Id = new Guid("b2f222c3-e2af-4da7-992c-155d910015d6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NETOMNIA LIMITED",
                            Prefix = "D2",
                            TraId = 7388
                        },
                        new SwaCode
                        {
                            Id = new Guid("9911d09d-4966-49ee-98a9-1ebbc584b796"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NETWORK RAIL",
                            Prefix = "KL",
                            TraId = 7093
                        },
                        new SwaCode
                        {
                            Id = new Guid("41e93ded-cb2b-4e0e-bedb-b1ff8e96ab5c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NEW ASH GREEN VILLAGE ASSOCIATION LIMITED",
                            Prefix = "E5",
                            TraId = 7398
                        },
                        new SwaCode
                        {
                            Id = new Guid("bcfafd4c-85f7-44b3-b406-4499c33ba3b0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NEWCASTLE CITY COUNCIL",
                            Prefix = "HR",
                            TraId = 4510
                        },
                        new SwaCode
                        {
                            Id = new Guid("394da162-9742-49a4-b647-6887d47cd9a9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NEWPORT CITY COUNCIL",
                            Prefix = "HT",
                            TraId = 6935
                        },
                        new SwaCode
                        {
                            Id = new Guid("91461951-5708-46d2-8f37-0fd4d79f77ea"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NEXFIBRE NETWORKS LIMITED",
                            Prefix = "N7",
                            TraId = 7562
                        },
                        new SwaCode
                        {
                            Id = new Guid("a207e2f9-337c-4032-938e-82028ba3834a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NEXTGENACCESS LTD",
                            Prefix = "TL",
                            TraId = 7353
                        },
                        new SwaCode
                        {
                            Id = new Guid("3bb24dc5-1efa-4248-8183-7e5d66fd0e2b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NMCN PLC",
                            Prefix = "UW",
                            TraId = 7327
                        },
                        new SwaCode
                        {
                            Id = new Guid("b777d5ad-0b43-439b-a631-16fcb3706189"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORFOLK COUNTY COUNCIL",
                            Prefix = "HU",
                            TraId = 2600
                        },
                        new SwaCode
                        {
                            Id = new Guid("ca4cf94a-3af1-4edf-a91b-0fd89e421a8d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTH EAST LINCOLNSHIRE COUNCIL",
                            Prefix = "RN",
                            TraId = 2002
                        },
                        new SwaCode
                        {
                            Id = new Guid("b282cc3b-9d87-46e7-85d5-38e5b16f6cd6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTH LINCOLNSHIRE COUNCIL",
                            Prefix = "RP",
                            TraId = 2003
                        },
                        new SwaCode
                        {
                            Id = new Guid("602dc94a-b527-4bc8-89c3-f32527a510dd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTH NORTHAMPTONSHIRE COUNCIL",
                            Prefix = "F8",
                            TraId = 2840
                        },
                        new SwaCode
                        {
                            Id = new Guid("e3f3abc7-feb7-4111-8cb4-23c3aa3f127b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTH SOMERSET COUNCIL",
                            Prefix = "RQ",
                            TraId = 121
                        },
                        new SwaCode
                        {
                            Id = new Guid("5741e059-ab8e-4c71-b576-87c6369326e7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTH TYNESIDE COUNCIL",
                            Prefix = "HY",
                            TraId = 4515
                        },
                        new SwaCode
                        {
                            Id = new Guid("75053554-4385-4889-8807-c3b6efa8555f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTH YORKSHIRE COUNCIL",
                            Prefix = "JB",
                            TraId = 2745
                        },
                        new SwaCode
                        {
                            Id = new Guid("9b73419f-9454-433f-840d-5acc4f80cfbd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTHERN GAS NETWORKS LIMITED",
                            Prefix = "XX",
                            TraId = 7271
                        },
                        new SwaCode
                        {
                            Id = new Guid("03b8aec2-3032-48d7-8b41-fe0e77e01ca5"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTHERN POWERGRID (NORTHEAST) LIMITED",
                            Prefix = "JD",
                            TraId = 7006
                        },
                        new SwaCode
                        {
                            Id = new Guid("04564dd4-da2b-4ed2-a189-f1b3137afa95"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTHERN POWERGRID (YORKSHIRE) PLC",
                            Prefix = "QA",
                            TraId = 7001
                        },
                        new SwaCode
                        {
                            Id = new Guid("b5002ceb-e043-4610-b441-6a5e1adf2d0b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTHUMBERLAND COUNTY COUNCIL",
                            Prefix = "UH",
                            TraId = 2935
                        },
                        new SwaCode
                        {
                            Id = new Guid("5b9534ae-ffa2-46b6-9879-f314b6b431e2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NORTHUMBRIAN WATER LIMITED",
                            Prefix = "JF",
                            TraId = 9101
                        },
                        new SwaCode
                        {
                            Id = new Guid("18d6a3df-3031-4e2d-b123-b8ea56b0a8a0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NOTTINGHAM CITY COUNCIL",
                            Prefix = "SQ",
                            TraId = 3060
                        },
                        new SwaCode
                        {
                            Id = new Guid("9117e726-42eb-4d9a-852c-14d3f2bff8bc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NOTTINGHAMSHIRE COUNTY COUNCIL",
                            Prefix = "JK",
                            TraId = 3055
                        },
                        new SwaCode
                        {
                            Id = new Guid("418d795a-59aa-4ed2-a958-f16083237646"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL (B) LIMITED",
                            Prefix = "AM",
                            TraId = 7096
                        },
                        new SwaCode
                        {
                            Id = new Guid("255e578f-79f1-4a46-943d-dcbaf71408fd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL (BROADLAND) LIMITED",
                            Prefix = "BE",
                            TraId = 7029
                        },
                        new SwaCode
                        {
                            Id = new Guid("1f283b3d-3311-4ca1-bdf2-9dd687e1aeba"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL (SOUTH EAST) LIMITED",
                            Prefix = "EE",
                            TraId = 7120
                        },
                        new SwaCode
                        {
                            Id = new Guid("64fbdad4-1e37-448c-879e-72294716c073"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS EPPING FOREST)",
                            Prefix = "EF",
                            TraId = 7121
                        },
                        new SwaCode
                        {
                            Id = new Guid("7c93194b-d8c7-4be8-bdf8-6471c6a04561"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS GREATER LONDON)",
                            Prefix = "EG",
                            TraId = 7122
                        },
                        new SwaCode
                        {
                            Id = new Guid("73fef303-3c3d-4f67-9d8c-5a943d3b7462"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS HAVERING)",
                            Prefix = "EH",
                            TraId = 7123
                        },
                        new SwaCode
                        {
                            Id = new Guid("d1c8c72e-a47a-45e8-96ea-44f6fda8d14c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS NEWHAM)",
                            Prefix = "EJ",
                            TraId = 7124
                        },
                        new SwaCode
                        {
                            Id = new Guid("edb3b739-d6d2-435c-84db-531093c331ed"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS WALTHAM FOREST)",
                            Prefix = "EK",
                            TraId = 7125
                        },
                        new SwaCode
                        {
                            Id = new Guid("67d39fe7-c4d6-4e4e-9fa7-59afe3cc6fb7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL CAMBRIDGE LIMITED",
                            Prefix = "CJ",
                            TraId = 7034
                        },
                        new SwaCode
                        {
                            Id = new Guid("cfbc23c7-b6d0-4d54-9f06-e88b949791b0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL KIRKLEES",
                            Prefix = "BZ",
                            TraId = 7047
                        },
                        new SwaCode
                        {
                            Id = new Guid("64fc673d-8fee-47cf-9371-b84f5d0599ab"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL MIDLANDS LIMITED",
                            Prefix = "DP",
                            TraId = 7119
                        },
                        new SwaCode
                        {
                            Id = new Guid("890da2b5-4acc-4dfb-9ce8-c0f22af6cf1f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL NATIONAL NETWORKS LIMITED",
                            Prefix = "WA",
                            TraId = 7274
                        },
                        new SwaCode
                        {
                            Id = new Guid("59aa89b3-34bf-4f0c-b0ef-d8da2b416a0a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL SOUTH CENTRAL LIMITED",
                            Prefix = "LU",
                            TraId = 7058
                        },
                        new SwaCode
                        {
                            Id = new Guid("09408df1-7e09-479f-bbe9-c83f0be91750"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NTL TELECOM SERVICES LIMITED",
                            Prefix = "SU",
                            TraId = 7219
                        },
                        new SwaCode
                        {
                            Id = new Guid("07e37d39-e217-4797-954b-8ddc8df8d411"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NWP STREET LIMITED",
                            Prefix = "ZH",
                            TraId = 7226
                        },
                        new SwaCode
                        {
                            Id = new Guid("6ef4a29a-054b-4256-9f4f-c9d954b208b8"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "NYNET LTD",
                            Prefix = "H6",
                            TraId = 7522
                        },
                        new SwaCode
                        {
                            Id = new Guid("a3d95632-7597-4a21-9309-00826376be30"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "OCIUSNET UK LTD",
                            Prefix = "L2",
                            TraId = 7542
                        },
                        new SwaCode
                        {
                            Id = new Guid("0ef01648-175b-4dba-8ba7-a354594b2aa2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "OGI NETWORKS LIMITED",
                            Prefix = "F4",
                            TraId = 7505
                        },
                        new SwaCode
                        {
                            Id = new Guid("df9af89c-73c5-452b-8380-b52f8916059f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "OLDHAM METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "KC",
                            TraId = 4220
                        },
                        new SwaCode
                        {
                            Id = new Guid("1c5a05fd-ba51-456d-889c-d6b1f01d4e94"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "OPEN FIBRE NETWORKS (WHOLESALE) LIMITED",
                            Prefix = "UX",
                            TraId = 7336
                        },
                        new SwaCode
                        {
                            Id = new Guid("995e8449-804b-4e34-8915-fa9efc8461c2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "Open Infra Ltd",
                            Prefix = "M9",
                            TraId = 7556
                        },
                        new SwaCode
                        {
                            Id = new Guid("7286dc55-5077-481c-95ae-628a77b48ed2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "OPEN NETWORK SYSTEMS LIMITED",
                            Prefix = "C3",
                            TraId = 7381
                        },
                        new SwaCode
                        {
                            Id = new Guid("0d890247-3fd6-4e92-b0a2-0025bbbb3c98"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "OPTICAL FIBRE INFRASTRUCTURE LIMITED",
                            Prefix = "E9",
                            TraId = 7502
                        },
                        new SwaCode
                        {
                            Id = new Guid("fcf214ae-def8-4762-b200-025bf89e8d7f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "OPTIMAL POWER NETWORKS",
                            Prefix = "E7",
                            TraId = 7500
                        },
                        new SwaCode
                        {
                            Id = new Guid("1155f335-e9d3-4e91-8a04-4a1e88b0e3e3"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ORANGE PERSONAL COMMUNICATIONS SERVICES LIMITED",
                            Prefix = "ZR",
                            TraId = 7233
                        },
                        new SwaCode
                        {
                            Id = new Guid("26c6dfe7-fdf8-4fb7-8ef8-a4fa014f261d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ORBITAL NET LTD",
                            Prefix = "G9",
                            TraId = 7517
                        },
                        new SwaCode
                        {
                            Id = new Guid("0a81e478-a569-4dee-9c32-c1d928de5f91"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ORSTED HORNSEA PROJECT THREE (UK) LIMITED",
                            Prefix = "P3",
                            TraId = 7566
                        },
                        new SwaCode
                        {
                            Id = new Guid("1d93be2e-1fb3-4131-b2d7-cb5a53ea3170"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "OXFORDSHIRE COUNTY COUNCIL",
                            Prefix = "KE",
                            TraId = 3100
                        },
                        new SwaCode
                        {
                            Id = new Guid("968fb600-0fca-4514-ac2a-227deeedd06e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "PEMBROKESHIRE COUNTY COUNCIL",
                            Prefix = "RR",
                            TraId = 6845
                        },
                        new SwaCode
                        {
                            Id = new Guid("ed9caf58-1442-4f52-ac9b-498996eac1b6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "PEOPLE'S FIBRE LTD",
                            Prefix = "E6",
                            TraId = 7399
                        },
                        new SwaCode
                        {
                            Id = new Guid("e025d78b-db17-4870-85e0-bea114a2d469"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "PERSIMMON HOMES LIMITED",
                            Prefix = "K2",
                            TraId = 7534
                        },
                        new SwaCode
                        {
                            Id = new Guid("444f77ef-2e97-415b-a1e4-ee4a53555802"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "PETERBOROUGH CITY COUNCIL",
                            Prefix = "FB",
                            TraId = 540
                        },
                        new SwaCode
                        {
                            Id = new Guid("117a0f10-84b2-4c26-84ed-7109cf588eec"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "PINE MEDIA LIMITED",
                            Prefix = "J9",
                            TraId = 7533
                        },
                        new SwaCode
                        {
                            Id = new Guid("6de0afa4-d6de-4e38-93f8-4c829160badf"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "PLYMOUTH CITY COUNCIL",
                            Prefix = "SL",
                            TraId = 1160
                        },
                        new SwaCode
                        {
                            Id = new Guid("05855d2e-5559-4456-83b6-ab7a863d292c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "PORTSMOUTH CITY COUNCIL",
                            Prefix = "FC",
                            TraId = 1775
                        },
                        new SwaCode
                        {
                            Id = new Guid("520cc3d0-2593-4c2b-af44-20904a1fcbe2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "PORTSMOUTH WATER LIMITED",
                            Prefix = "KH",
                            TraId = 9128
                        },
                        new SwaCode
                        {
                            Id = new Guid("f245381a-2a07-439e-942f-0524e2219ec2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "POWERSYSTEMS UK LTD",
                            Prefix = "UA",
                            TraId = 7316
                        },
                        new SwaCode
                        {
                            Id = new Guid("af390f73-73f9-4ef8-8bb7-ee2721af3041"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "POWYS COUNTY COUNCIL",
                            Prefix = "KJ",
                            TraId = 6850
                        },
                        new SwaCode
                        {
                            Id = new Guid("c4495581-f082-41e7-a8eb-b8fa956e4cf9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "QUICKLINE COMMUNICATIONS LIMITED",
                            Prefix = "TZ",
                            TraId = 7365
                        },
                        new SwaCode
                        {
                            Id = new Guid("5a1a6c68-53bc-459e-b48c-8d318bd08ebc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "RAILSITE TELECOM LIMITED",
                            Prefix = "H5",
                            TraId = 7521
                        },
                        new SwaCode
                        {
                            Id = new Guid("9ee4516f-c9dd-444f-a0ec-1eebb2c95927"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "READING BOROUGH COUNCIL",
                            Prefix = "JN",
                            TraId = 345
                        },
                        new SwaCode
                        {
                            Id = new Guid("5b9c6767-8756-4591-8997-79b62bc3606f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "REDCAR AND CLEVELAND BOROUGH COUNCIL",
                            Prefix = "RU",
                            TraId = 728
                        },
                        new SwaCode
                        {
                            Id = new Guid("c4500aa8-02eb-4f9e-931a-2ca5af82720c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "REDCENTRIC COMMUNICATIONS LIMITED",
                            Prefix = "YJ",
                            TraId = 7247
                        },
                        new SwaCode
                        {
                            Id = new Guid("8cc545a2-1723-4869-95c8-16e747737176"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "RHONDDA CYNON TAF COUNTY BOROUGH COUNCIL",
                            Prefix = "KN",
                            TraId = 6940
                        },
                        new SwaCode
                        {
                            Id = new Guid("cf520c6e-40d7-4c76-a3a0-a410a76b93f3"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "Riverside Energy Park Ltd",
                            Prefix = "P8",
                            TraId = 7569
                        },
                        new SwaCode
                        {
                            Id = new Guid("586335e4-fd0c-4dc4-a9b9-b1de074c37de"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "RM PROPERTY AND FACILITIES SOLUTIONS LIMITED",
                            Prefix = "SW",
                            TraId = 7221
                        },
                        new SwaCode
                        {
                            Id = new Guid("68df25d2-6413-4411-be8c-48a7f8663b96"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ROCHDALE BOROUGH COUNCIL",
                            Prefix = "KQ",
                            TraId = 4225
                        },
                        new SwaCode
                        {
                            Id = new Guid("4d9023cf-3997-48a6-b4c9-e7e92b078d6b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ROLTA UK LIMITED",
                            Prefix = "UR",
                            TraId = 7322
                        },
                        new SwaCode
                        {
                            Id = new Guid("57e7d69f-6a7f-442b-b1bd-4a6ef305c587"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ROTHERHAM METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "KR",
                            TraId = 4415
                        },
                        new SwaCode
                        {
                            Id = new Guid("5f34526b-5fa6-4256-9e7f-96d2b3b864b7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ROYAL BOROUGH OF GREENWICH",
                            Prefix = "FA",
                            TraId = 5330
                        },
                        new SwaCode
                        {
                            Id = new Guid("e987b111-fcbb-4530-86a0-003b970d96e8"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ROYAL BOROUGH OF KENSINGTON AND CHELSEA",
                            Prefix = "GD",
                            TraId = 5600
                        },
                        new SwaCode
                        {
                            Id = new Guid("73cab5c1-250f-43e1-83ac-b3f6d2877da2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ROYAL BOROUGH OF KINGSTON UPON THAMES",
                            Prefix = "GH",
                            TraId = 5630
                        },
                        new SwaCode
                        {
                            Id = new Guid("7f129983-0556-41bb-82f2-31a178e1afbd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ROYAL BOROUGH OF WINDSOR AND MAIDENHEAD",
                            Prefix = "JW",
                            TraId = 355
                        },
                        new SwaCode
                        {
                            Id = new Guid("1f26d717-9641-4677-abcf-17eddd75d3dd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "RUNFIBRE LTD",
                            Prefix = "P2",
                            TraId = 7565
                        },
                        new SwaCode
                        {
                            Id = new Guid("9bae21ab-0c87-4109-8a7b-a856d1650775"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "RUTLAND COUNTY COUNCIL",
                            Prefix = "JP",
                            TraId = 2470
                        },
                        new SwaCode
                        {
                            Id = new Guid("fa3370cc-ec5c-464f-9887-aec6366e110c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SALFORD CITY COUNCIL",
                            Prefix = "KS",
                            TraId = 4230
                        },
                        new SwaCode
                        {
                            Id = new Guid("99c7bcca-c617-4405-a046-172723d5ebda"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SANDWELL METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "KU",
                            TraId = 4620
                        },
                        new SwaCode
                        {
                            Id = new Guid("79b3b8e8-cb36-40e2-b25c-abd59566613e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SCOTLAND GAS NETWORKS PLC",
                            Prefix = "XZ",
                            TraId = 7273
                        },
                        new SwaCode
                        {
                            Id = new Guid("959e0e3a-8bfe-4212-acc1-f574a4b4bba1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SCOTTISH WATER LIMITED",
                            Prefix = "ZN",
                            TraId = 9137
                        },
                        new SwaCode
                        {
                            Id = new Guid("c48a334c-fdf3-4ff4-9d5c-23a842ca1f18"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SCOTTISHPOWER ENERGY RETAIL LIMITED",
                            Prefix = "KY",
                            TraId = 7019
                        },
                        new SwaCode
                        {
                            Id = new Guid("b97e8b74-4682-47bf-994c-af81a9df143a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SECURE WEB SERVICES LIMITED",
                            Prefix = "G6",
                            TraId = 7514
                        },
                        new SwaCode
                        {
                            Id = new Guid("1527efce-93aa-4592-9387-cd6990a7588c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SEFTON METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "LA",
                            TraId = 4320
                        },
                        new SwaCode
                        {
                            Id = new Guid("5351dac8-6c5d-49de-bd60-0fffa13c18fc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SERVERHOUSE LTD",
                            Prefix = "R6",
                            TraId = 7575
                        },
                        new SwaCode
                        {
                            Id = new Guid("1ebb2261-b6e1-4cc1-bf9e-dc93ccef225e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SES WATER LIMITED",
                            Prefix = "DZ",
                            TraId = 9118
                        },
                        new SwaCode
                        {
                            Id = new Guid("32a6d064-4d6e-4d5f-93b1-9b88140703f0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SEVERN TRENT SERVICES OPERATIONS UK LIMITED",
                            Prefix = "UN",
                            TraId = 7319
                        },
                        new SwaCode
                        {
                            Id = new Guid("06f2340b-f3aa-4758-aa7e-6f3de8a43267"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SEVERN TRENT WATER LIMITED",
                            Prefix = "LB",
                            TraId = 9103
                        },
                        new SwaCode
                        {
                            Id = new Guid("34e24766-25cf-497b-b37d-5114036df81e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SHEFFIELD CITY COUNCIL",
                            Prefix = "LC",
                            TraId = 4420
                        },
                        new SwaCode
                        {
                            Id = new Guid("6740c244-77ab-47f5-9ad5-1a107b70419b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SHROPSHIRE COUNCIL",
                            Prefix = "UJ",
                            TraId = 3245
                        },
                        new SwaCode
                        {
                            Id = new Guid("8d93e4f5-2f82-4a2e-abbf-f9e5c28321c7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SKY TELECOMMUNICATIONS SERVICES LIMITED",
                            Prefix = "ZG",
                            TraId = 7225
                        },
                        new SwaCode
                        {
                            Id = new Guid("efb4a363-33d6-4674-a30e-a59bed94af4a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SLOUGH BOROUGH COUNCIL",
                            Prefix = "JQ",
                            TraId = 350
                        },
                        new SwaCode
                        {
                            Id = new Guid("9f4b0122-edbf-4f46-a9b6-1b67681d1963"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SMARTFIBRE BROADBAND LIMITED",
                            Prefix = "R9",
                            TraId = 7578
                        },
                        new SwaCode
                        {
                            Id = new Guid("d0cd968b-8868-4b56-90bb-3cd2417f2012"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOLIHULL METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "LF",
                            TraId = 4625
                        },
                        new SwaCode
                        {
                            Id = new Guid("1b597bb5-cc97-4a8d-97bd-79b36b371786"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOMERSET COUNCIL",
                            Prefix = "LG",
                            TraId = 3300
                        },
                        new SwaCode
                        {
                            Id = new Guid("c810119f-fc5c-4e59-80f4-15f6f1cb2e3f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTH EAST WATER LIMITED",
                            Prefix = "EB",
                            TraId = 9117
                        },
                        new SwaCode
                        {
                            Id = new Guid("535f9041-eb50-4141-ba2d-099b2ca7c3e2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTH EASTERN POWER NETWORKS PLC",
                            Prefix = "KZ",
                            TraId = 7004
                        },
                        new SwaCode
                        {
                            Id = new Guid("73965cec-e61c-4e62-b315-cc31952e8635"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTH GLOUCESTERSHIRE COUNCIL",
                            Prefix = "RZ",
                            TraId = 119
                        },
                        new SwaCode
                        {
                            Id = new Guid("348ef449-470a-4f76-9881-abf4832d46b9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTH STAFFORDSHIRE WATER PLC",
                            Prefix = "LJ",
                            TraId = 9129
                        },
                        new SwaCode
                        {
                            Id = new Guid("2cdfade2-85c2-4b54-9b73-4bab8ac20592"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTH TYNESIDE COUNCIL",
                            Prefix = "LK",
                            TraId = 4520
                        },
                        new SwaCode
                        {
                            Id = new Guid("7d37d581-76e0-4a4a-ab7d-135eeb9fc89c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTH WEST WATER LIMITED",
                            Prefix = "LM",
                            TraId = 9105
                        },
                        new SwaCode
                        {
                            Id = new Guid("fcf6dc11-26f5-4e75-b53b-b6997ff4e547"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTH WEST WATER LIMITED (Formerly BRISTOL WATER PLC)",
                            Prefix = "AY",
                            TraId = 9111
                        },
                        new SwaCode
                        {
                            Id = new Guid("7a7f7f5d-6c49-4676-beb8-83b59c4e3ef0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTH YORKSHIRE PASSENGER TRANSPORT EXECUTIVE",
                            Prefix = "XN",
                            TraId = 9236
                        },
                        new SwaCode
                        {
                            Id = new Guid("95d31732-4aac-4d63-98dd-fc193936992f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTHAMPTON CITY COUNCIL",
                            Prefix = "FU",
                            TraId = 1780
                        },
                        new SwaCode
                        {
                            Id = new Guid("bd9e6c0e-383e-46d1-ac3f-c76b8d6b39e6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTHEND-ON-SEA CITY COUNCIL",
                            Prefix = "JR",
                            TraId = 1590
                        },
                        new SwaCode
                        {
                            Id = new Guid("df659faa-ba70-487c-9522-ac4d0ea3acbc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTHERN ELECTRIC POWER DISTRIBUTION PLC",
                            Prefix = "LP",
                            TraId = 7002
                        },
                        new SwaCode
                        {
                            Id = new Guid("c95c29d8-9009-4b77-9803-6b5f8db64a43"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTHERN GAS NETWORKS PLC",
                            Prefix = "XW",
                            TraId = 7270
                        },
                        new SwaCode
                        {
                            Id = new Guid("86777047-8cae-406b-bfc9-8d35c040f4b5"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SOUTHERN WATER LIMITED",
                            Prefix = "LQ",
                            TraId = 9104
                        },
                        new SwaCode
                        {
                            Id = new Guid("e2692021-4974-4144-af16-a6de791a6a03"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SP MANWEB PLC",
                            Prefix = "GY",
                            TraId = 7008
                        },
                        new SwaCode
                        {
                            Id = new Guid("15bd8653-7bdd-4dc3-934c-9e6b7c6658fa"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SPRING FIBRE LIMITED",
                            Prefix = "J5",
                            TraId = 7529
                        },
                        new SwaCode
                        {
                            Id = new Guid("e3beb3f8-97a8-410f-b55c-9dfa7d47b5ec"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SQUIRE ENERGY LIMITED",
                            Prefix = "C5",
                            TraId = 7383
                        },
                        new SwaCode
                        {
                            Id = new Guid("b0cc5b1c-661f-4d95-b8c0-5b8fe97496dd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SQUIRE ENERGY METERING LIMITED",
                            Prefix = "R2",
                            TraId = 7571
                        },
                        new SwaCode
                        {
                            Id = new Guid("0804f3b0-363f-457c-9a97-5625646db551"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ST. HELENS COUNCIL",
                            Prefix = "LT",
                            TraId = 4315
                        },
                        new SwaCode
                        {
                            Id = new Guid("9c9694bb-2a39-4578-87ab-984d11b20b7f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "STAFFORDSHIRE COUNTY COUNCIL",
                            Prefix = "LV",
                            TraId = 3450
                        },
                        new SwaCode
                        {
                            Id = new Guid("56063d25-2f97-4484-8606-f7720bc01617"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "STIX INTERNET LIMITED",
                            Prefix = "J6",
                            TraId = 7530
                        },
                        new SwaCode
                        {
                            Id = new Guid("2bde9c56-e3b1-4cce-b0d7-7f4d1b0d6e83"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "STOCKPORT METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "LX",
                            TraId = 4235
                        },
                        new SwaCode
                        {
                            Id = new Guid("3edbd84c-be1b-402b-80d4-88d3dcab15dc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "STOCKTON-ON-TEES BOROUGH COUNCIL",
                            Prefix = "SB",
                            TraId = 738
                        },
                        new SwaCode
                        {
                            Id = new Guid("e423fc22-e40e-44e7-a23a-702389fcaeb1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "STOKE-ON-TRENT CITY COUNCIL",
                            Prefix = "GF",
                            TraId = 3455
                        },
                        new SwaCode
                        {
                            Id = new Guid("8ae7838a-084a-4af7-bbd7-7068ed40b32a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SUBTOPIA LIMITED",
                            Prefix = "ZD",
                            TraId = 7344
                        },
                        new SwaCode
                        {
                            Id = new Guid("1b4ed0b5-25c9-4e16-9a6d-e9a3b3ec0afc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SUEZ ADVANCED SOLUTIONS UL LTD",
                            Prefix = "N3",
                            TraId = 7558
                        },
                        new SwaCode
                        {
                            Id = new Guid("387deafa-18ea-41a3-abd2-bb42ccbaaceb"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SUFFOLK COUNTY COUNCIL",
                            Prefix = "LY",
                            TraId = 3500
                        },
                        new SwaCode
                        {
                            Id = new Guid("0f74fa03-2d91-4c7d-a2cd-1cab3cd84d76"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SUNDERLAND CITY COUNCIL",
                            Prefix = "LZ",
                            TraId = 4525
                        },
                        new SwaCode
                        {
                            Id = new Guid("fa780956-2352-4383-8069-df6675dab1a7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SURREY COUNTY COUNCIL",
                            Prefix = "MA",
                            TraId = 3600
                        },
                        new SwaCode
                        {
                            Id = new Guid("45531ed2-10ce-470f-befb-f2433d8802ba"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SWINDON BOROUGH COUNCIL",
                            Prefix = "SN",
                            TraId = 3935
                        },
                        new SwaCode
                        {
                            Id = new Guid("988c9cae-0995-472c-ac18-1f6f3a098f16"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "SWISH FIBRE LIMITED",
                            Prefix = "C6",
                            TraId = 7384
                        },
                        new SwaCode
                        {
                            Id = new Guid("4b51ef26-75a4-4d8a-ae62-d25703355514"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TALKTALK COMMUNICATIONS LIMITED",
                            Prefix = "VD",
                            TraId = 7299
                        },
                        new SwaCode
                        {
                            Id = new Guid("7944bc4b-b9df-41bd-a84e-f93228a610df"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TAMESIDE METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "MF",
                            TraId = 4240
                        },
                        new SwaCode
                        {
                            Id = new Guid("c4b8c25c-a4db-4e1e-ad33-25f942408cc5"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TATA COMMUNICATIONS (UK) LIMITED",
                            Prefix = "YL",
                            TraId = 7248
                        },
                        new SwaCode
                        {
                            Id = new Guid("8b14055a-f208-4ad2-ab8c-586646b1b88b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELCOM INFRASTRUCTURE LIMITED",
                            Prefix = "D5",
                            TraId = 7390
                        },
                        new SwaCode
                        {
                            Id = new Guid("a9aa086f-22d7-49a5-b8fc-940d80a75cea"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELECENTIAL COMMUNICATIONS (HERTS) LIMITED PARTNERSHIP",
                            Prefix = "MQ",
                            TraId = 7152
                        },
                        new SwaCode
                        {
                            Id = new Guid("765eb9f9-b64f-4418-b6f2-d8ad0930b3ca"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELECENTIAL COMMUNICATIONS (NORTHANTS) LIMITED PARTNERSHIP",
                            Prefix = "MJ",
                            TraId = 7146
                        },
                        new SwaCode
                        {
                            Id = new Guid("21c38439-45a0-4bc6-a713-9e56bc36e654"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELECENTIAL COMMUNICATIONS (NORTHANTS) LIMITED PARTNERSHIP (Formerly TELECENTIAL COMMS NORTHAMPTON)",
                            Prefix = "MK",
                            TraId = 7147
                        },
                        new SwaCode
                        {
                            Id = new Guid("6e2426fb-5f68-4382-8989-22dfdd494c64"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELECENTIAL COMMUNICATIONS (WARWICKSHIRE) LIMITED PARTNERSHIP",
                            Prefix = "MM",
                            TraId = 7149
                        },
                        new SwaCode
                        {
                            Id = new Guid("bc0a7df2-dabb-43df-9d23-8e74ec317110"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELECENTIAL COMMUNICATIONS LIMITED PARTNERSHIP",
                            Prefix = "ML",
                            TraId = 7148
                        },
                        new SwaCode
                        {
                            Id = new Guid("b56ff32d-4a9c-40cd-bbf0-d82d74a6d4b0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELECENTIAL COMMUNICATIONS LIMITED PARTNERSHIP (Formerly TELECENTIAL COMMS RUGBY)",
                            Prefix = "MN",
                            TraId = 7150
                        },
                        new SwaCode
                        {
                            Id = new Guid("c6460a0a-73ca-4274-96f7-531d5da6a60a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELECENTIAL COMMUNICATIONS LIMITED PARTNERSHIP (Formerly TELECENTIAL COMMS STRATFORD)",
                            Prefix = "MP",
                            TraId = 7151
                        },
                        new SwaCode
                        {
                            Id = new Guid("1af98442-719d-4831-89ce-3534bf12c78f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELEFONICA UK LIMITED",
                            Prefix = "MG",
                            TraId = 7182
                        },
                        new SwaCode
                        {
                            Id = new Guid("a56ca8d1-c6cb-4b7b-8615-1ccd9614ff18"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELEWEST COMMUNICATIONS (NORTH EAST) LIMITED",
                            Prefix = "NH",
                            TraId = 7158
                        },
                        new SwaCode
                        {
                            Id = new Guid("3db3d62f-6cf8-4716-86d5-510e1eb17b4f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELEWEST COMMUNICATIONS (SOUTH EAST) LIMITED",
                            Prefix = "NJ",
                            TraId = 7159
                        },
                        new SwaCode
                        {
                            Id = new Guid("4932b499-ad65-4080-8bee-0b8da68b4ef5"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELEWEST COMMUNICATIONS (SOUTH WEST) LIMITED",
                            Prefix = "NC",
                            TraId = 7061
                        },
                        new SwaCode
                        {
                            Id = new Guid("a4090c5f-f6c3-436f-b162-b84bed4d0035"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELEWEST COMMUNICATIONS (TYNESIDE) LIMITED",
                            Prefix = "CY",
                            TraId = 7035
                        },
                        new SwaCode
                        {
                            Id = new Guid("1aa8fe77-a1cd-45bb-bf80-d9e4950067ee"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELEWEST LIMITED",
                            Prefix = "ZX",
                            TraId = 7237
                        },
                        new SwaCode
                        {
                            Id = new Guid("385cb082-9434-4b40-80f0-b5017fb787d6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELFORD & WREKIN COUNCIL",
                            Prefix = "JS",
                            TraId = 3240
                        },
                        new SwaCode
                        {
                            Id = new Guid("3be47b8b-255f-4ee6-8d00-420acd3b5e28"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TELSTRA GLOBAL LIMITED",
                            Prefix = "MS",
                            TraId = 7085
                        },
                        new SwaCode
                        {
                            Id = new Guid("ce2d68f5-4f39-4741-a571-9aa5518e2037"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "THAMES WATER UTILITIES LIMITED",
                            Prefix = "MU",
                            TraId = 9106
                        },
                        new SwaCode
                        {
                            Id = new Guid("07ceb1ee-5ffc-4eb3-8980-fa64a5f56d51"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "THE ELECTRICITY NETWORK COMPANY LIMITED",
                            Prefix = "N5",
                            TraId = 7560
                        },
                        new SwaCode
                        {
                            Id = new Guid("d3d50430-549f-4807-a192-b95d6966f754"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "THE FIBRE GUYS LTD",
                            Prefix = "H4",
                            TraId = 7520
                        },
                        new SwaCode
                        {
                            Id = new Guid("0554e48e-58a4-4905-a137-624c206f1b80"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "THE OIL & PIPELINES AGENCY",
                            Prefix = "EZ",
                            TraId = 7092
                        },
                        new SwaCode
                        {
                            Id = new Guid("7f66ff31-5017-45fa-8647-a8ba61b59b7b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "THE ROCHESTER BRIDGE TRUST",
                            Prefix = "ZA",
                            TraId = 7337
                        },
                        new SwaCode
                        {
                            Id = new Guid("04f4cb30-cb8b-4b07-908b-3c2ca5774473"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "THURROCK COUNCIL",
                            Prefix = "JT",
                            TraId = 1595
                        },
                        new SwaCode
                        {
                            Id = new Guid("3da91b89-4200-4ea3-bc5e-cd81afad56a2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "THUS LIMITED",
                            Prefix = "ZF",
                            TraId = 7224
                        },
                        new SwaCode
                        {
                            Id = new Guid("b871e3a2-e81b-4baf-8f9f-29740e4a99fd"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TIMICO PARTNER SERVICES LIMITED",
                            Prefix = "WC",
                            TraId = 7275
                        },
                        new SwaCode
                        {
                            Id = new Guid("97686e29-0dbc-4633-ae57-f1925d86d4c1"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TOOB LIMITED",
                            Prefix = "B5",
                            TraId = 7375
                        },
                        new SwaCode
                        {
                            Id = new Guid("3c27dbac-5c07-432d-9609-bba2fb4b0680"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TORBAY COUNCIL",
                            Prefix = "SM",
                            TraId = 1165
                        },
                        new SwaCode
                        {
                            Id = new Guid("5f329459-a0c5-4780-8344-97f5f4b9cd01"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TORFAEN COUNTY BOROUGH COUNCIL",
                            Prefix = "MZ",
                            TraId = 6945
                        },
                        new SwaCode
                        {
                            Id = new Guid("96030ce4-fad3-470b-9a0f-e498981d26b2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TRAFFORD METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "NB",
                            TraId = 4245
                        },
                        new SwaCode
                        {
                            Id = new Guid("5bbf75dc-42cf-48a8-8c60-5e989c304322"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TRANSPORT FOR GREATER MANCHESTER",
                            Prefix = "QZ",
                            TraId = 7215
                        },
                        new SwaCode
                        {
                            Id = new Guid("93e86e41-18d6-45d0-b4d9-0970d907a315"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TRANSPORT FOR LONDON (TFL)",
                            Prefix = "YG",
                            TraId = 20
                        },
                        new SwaCode
                        {
                            Id = new Guid("d389b6d5-0027-4e56-bbf6-6a32f95780b8"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TRANSPORT FOR WALES (TFW)",
                            Prefix = "M2",
                            TraId = 7549
                        },
                        new SwaCode
                        {
                            Id = new Guid("2f4a783e-04b1-4c80-af15-acd76c98cbc4"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "TRUESPEED COMMUNICATIONS LTD",
                            Prefix = "TP",
                            TraId = 7355
                        },
                        new SwaCode
                        {
                            Id = new Guid("f25cb4c2-aeec-4be1-8f3d-c1f52b295b69"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "UK BROADBAND LIMITED",
                            Prefix = "PH",
                            TraId = 7341
                        },
                        new SwaCode
                        {
                            Id = new Guid("f6e9616c-f2c3-4761-b38a-a43829e621b8"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "UK POWER DISTRIBUTION LIMITED",
                            Prefix = "TV",
                            TraId = 7361
                        },
                        new SwaCode
                        {
                            Id = new Guid("f2a2d66d-eaf4-4adc-b489-6b02899ebbad"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "UNITED UTILITIES WATER LIMITED",
                            Prefix = "HZ",
                            TraId = 9102
                        },
                        new SwaCode
                        {
                            Id = new Guid("0a675406-880d-4a8f-bb63-afb428800fa4"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "UPP (Corporation Limited)",
                            Prefix = "H9",
                            TraId = 7526
                        },
                        new SwaCode
                        {
                            Id = new Guid("eb3b49c9-51e2-4ebb-b54e-8d0931234a4e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "UTILITY GRID INSTALLATIONS LIMITED",
                            Prefix = "XK",
                            TraId = 7265
                        },
                        new SwaCode
                        {
                            Id = new Guid("d37335ec-9337-4b3c-bb57-c795643a99dc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VALE OF GLAMORGAN COUNCIL",
                            Prefix = "NL",
                            TraId = 6950
                        },
                        new SwaCode
                        {
                            Id = new Guid("42a08b2e-0d46-49ca-ae3a-f181ca383b26"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VATTENFALL NETWORKS LIMITED",
                            Prefix = "D9",
                            TraId = 7394
                        },
                        new SwaCode
                        {
                            Id = new Guid("7835480b-d68d-4ccf-a365-e72a6da8cde0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VATTENFALL WIND POWER LTD",
                            Prefix = "N6",
                            TraId = 7561
                        },
                        new SwaCode
                        {
                            Id = new Guid("9b1e6ae6-6d2a-4c75-a5d4-de352f7cbe63"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VEOLIA WATER OUTSOURCING LIMITED",
                            Prefix = "VY",
                            TraId = 7314
                        },
                        new SwaCode
                        {
                            Id = new Guid("5df08ed4-3829-4c82-ae40-0b59e3e6b73a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VERIZON UK LIMITED",
                            Prefix = "PQ",
                            TraId = 7086
                        },
                        new SwaCode
                        {
                            Id = new Guid("0f32f588-35f8-4f60-a3e3-ee9759e43fc4"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VERIZON UK LIMITED (Formerly WORLDCOM INTERNATIONAL LTD)",
                            Prefix = "HE",
                            TraId = 7081
                        },
                        new SwaCode
                        {
                            Id = new Guid("29c4b505-794f-43f8-98da-cf82a15c0168"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VIRGIN MEDIA LIMITED",
                            Prefix = "NK",
                            TraId = 7160
                        },
                        new SwaCode
                        {
                            Id = new Guid("dc6e4ee1-7a01-4cad-9243-e59700e99445"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VIRGIN MEDIA PCHC II LIMITED",
                            Prefix = "CD",
                            TraId = 7110
                        },
                        new SwaCode
                        {
                            Id = new Guid("698917b6-bf66-4a0c-8cce-eea7cd0d500d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VIRGIN MEDIA PCHC II LIMITED (Formerly CABLETEL SOUTH WALES CARDIFF)",
                            Prefix = "CB",
                            TraId = 7109
                        },
                        new SwaCode
                        {
                            Id = new Guid("b7b80bfa-cd53-47db-9af4-f799e71d0125"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VIRGIN MEDIA WHOLESALE LIMITED",
                            Prefix = "YA",
                            TraId = 7240
                        },
                        new SwaCode
                        {
                            Id = new Guid("9fe08799-af3a-4e47-988a-680048367255"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VISPA LIMITED",
                            Prefix = "G7",
                            TraId = 7516
                        },
                        new SwaCode
                        {
                            Id = new Guid("5df25ecf-1160-4c87-9fb1-39c7c3f78dd5"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VITAL ENERGI UTILITIES LIMITED",
                            Prefix = "RX",
                            TraId = 7209
                        },
                        new SwaCode
                        {
                            Id = new Guid("6629fa25-575a-437e-930e-33f532690517"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VODAFONE ENTERPRISE EUROPE (UK) LIMITED",
                            Prefix = "QL",
                            TraId = 70
                        },
                        new SwaCode
                        {
                            Id = new Guid("725c3b29-381d-4b23-947f-05940dbc909f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VODAFONE LIMITED",
                            Prefix = "NX",
                            TraId = 7076
                        },
                        new SwaCode
                        {
                            Id = new Guid("72a387e5-f47b-4d90-be5c-662c6d133bb6"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VONEUS LIMITED",
                            Prefix = "F6",
                            TraId = 7507
                        },
                        new SwaCode
                        {
                            Id = new Guid("5fbaba87-22b6-4d0a-9361-b6d2fcbf773b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VORBOSS LIMITED",
                            Prefix = "D3",
                            TraId = 7389
                        },
                        new SwaCode
                        {
                            Id = new Guid("26d48011-4b49-4170-bef7-c79e3593a09b"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "VX FIBER LIMITED",
                            Prefix = "E2",
                            TraId = 7395
                        },
                        new SwaCode
                        {
                            Id = new Guid("3494e5ec-a1aa-4a43-a7bb-5cb19d2c1b45"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WALES & WEST UTILITIES LIMITED",
                            Prefix = "XY",
                            TraId = 7272
                        },
                        new SwaCode
                        {
                            Id = new Guid("eab9cb4c-77ce-4041-90a4-9311e6292ca3"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WALSALL METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "NZ",
                            TraId = 4630
                        },
                        new SwaCode
                        {
                            Id = new Guid("d1d194fe-9fc5-4fbb-9091-1fe44bffaf33"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WARRINGTON BOROUGH COUNCIL",
                            Prefix = "JU",
                            TraId = 655
                        },
                        new SwaCode
                        {
                            Id = new Guid("bb5edd72-154e-4d96-8bd9-4fe0107a439a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WARWICKSHIRE COUNTY COUNCIL",
                            Prefix = "PC",
                            TraId = 3700
                        },
                        new SwaCode
                        {
                            Id = new Guid("f2f6b820-9dbc-47b5-954e-b3966f87626e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WELINK COMMUNICATIONS UK LTD",
                            Prefix = "J8",
                            TraId = 7532
                        },
                        new SwaCode
                        {
                            Id = new Guid("3987668f-3d46-44f8-99da-350964e1a0f9"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WELSH GOVERNMENT",
                            Prefix = "PD",
                            TraId = 16
                        },
                        new SwaCode
                        {
                            Id = new Guid("758702ad-95d0-4f66-8390-12e9cefa7288"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WELSH HIGHLAND RAILWAY CONSTRUCTION LIMITED",
                            Prefix = "VR",
                            TraId = 7310
                        },
                        new SwaCode
                        {
                            Id = new Guid("c91639b3-1428-409b-b1a6-81de3f6e6bbb"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WESSEX INTERNET",
                            Prefix = "J4",
                            TraId = 7525
                        },
                        new SwaCode
                        {
                            Id = new Guid("78a707c0-13d3-483a-a03e-cbee9928840c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WESSEX WATER LIMITED",
                            Prefix = "PG",
                            TraId = 9108
                        },
                        new SwaCode
                        {
                            Id = new Guid("cb251dd5-9f0d-432f-a2d5-824568ce754a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WEST BERKSHIRE COUNCIL",
                            Prefix = "JV",
                            TraId = 340
                        },
                        new SwaCode
                        {
                            Id = new Guid("6f72f5b1-88bb-4df3-90ab-a6fb1e05ad6c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WEST NORTHAMPTONSHIRE COUNCIL",
                            Prefix = "F9",
                            TraId = 2845
                        },
                        new SwaCode
                        {
                            Id = new Guid("e5bf87b1-11b0-4b3a-b3e5-0495c670ff54"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WEST SUSSEX COUNTY COUNCIL",
                            Prefix = "PJ",
                            TraId = 3800
                        },
                        new SwaCode
                        {
                            Id = new Guid("cd5eed80-9bc9-4d35-8f69-5e39dace2cfc"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WESTMORLAND AND FURNESS COUNCIL",
                            Prefix = "P4",
                            TraId = 935
                        },
                        new SwaCode
                        {
                            Id = new Guid("1fc26488-7aab-4266-a294-b0792a383474"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WESTNETWORKS INNOVATIONS LIMITED",
                            Prefix = "H3",
                            TraId = 7519
                        },
                        new SwaCode
                        {
                            Id = new Guid("1c57fa54-6f4b-4dc2-8bf9-8ef9ed3042ef"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WHYFIBRE LIMITED",
                            Prefix = "R3",
                            TraId = 7572
                        },
                        new SwaCode
                        {
                            Id = new Guid("c4b59ee0-fb22-40f0-bfb3-a4cbfe56a31c"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WIFINITY LIMITED",
                            Prefix = "P9",
                            TraId = 7570
                        },
                        new SwaCode
                        {
                            Id = new Guid("12c56d95-4f4e-417e-a470-0973a408d5e2"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WIGAN METROPOLITAN BOROUGH COUNCIL",
                            Prefix = "PL",
                            TraId = 4250
                        },
                        new SwaCode
                        {
                            Id = new Guid("e96541b0-7644-49ba-8f19-b299ed84e179"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WIGHTFIBRE LIMITED",
                            Prefix = "YP",
                            TraId = 7251
                        },
                        new SwaCode
                        {
                            Id = new Guid("783c7300-a8f0-4264-a64e-d6301688b21a"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WILDANET LIMITED",
                            Prefix = "G8",
                            TraId = 7515
                        },
                        new SwaCode
                        {
                            Id = new Guid("872891e1-638c-45ef-bb0d-87919b4e0e91"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WILDCARD UK LIMITED",
                            Prefix = "TX",
                            TraId = 7363
                        },
                        new SwaCode
                        {
                            Id = new Guid("cc9d967b-c4f9-4108-9623-eb872e9751eb"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WILTSHIRE COUNCIL",
                            Prefix = "UK",
                            TraId = 3940
                        },
                        new SwaCode
                        {
                            Id = new Guid("e2d70a49-d9a9-48c4-bec8-a1072734b5df"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WIRRAL BOROUGH COUNCIL",
                            Prefix = "PN",
                            TraId = 4325
                        },
                        new SwaCode
                        {
                            Id = new Guid("9ad35810-1bc1-459f-b92e-5e0fd947bd04"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WOKINGHAM BOROUGH COUNCIL",
                            Prefix = "JX",
                            TraId = 360
                        },
                        new SwaCode
                        {
                            Id = new Guid("4019d8c6-d000-4bdd-90c3-bce6609f6ab0"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WORCESTERSHIRE COUNTY COUNCIL",
                            Prefix = "JZ",
                            TraId = 1855
                        },
                        new SwaCode
                        {
                            Id = new Guid("6b700f66-62a6-466f-9d0f-f19e819d987f"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WREXHAM COUNTY BOROUGH COUNCIL",
                            Prefix = "PR",
                            TraId = 6955
                        },
                        new SwaCode
                        {
                            Id = new Guid("38c32bca-a278-4436-b900-77fd7a76be85"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "WREXHAM WATER LIMITED",
                            Prefix = "PS",
                            TraId = 9135
                        },
                        new SwaCode
                        {
                            Id = new Guid("787e2554-2018-4a49-9f96-764b07dd8ada"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "X-FIBRE LTD",
                            Prefix = "R7",
                            TraId = 7576
                        },
                        new SwaCode
                        {
                            Id = new Guid("abb2f790-4f53-4a2e-8350-8b514aa56386"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "YESFIBRE LTD",
                            Prefix = "L6",
                            TraId = 7546
                        },
                        new SwaCode
                        {
                            Id = new Guid("cdc1290c-93ea-4039-9a12-d44c1af1b548"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED",
                            Prefix = "PU",
                            TraId = 7027
                        },
                        new SwaCode
                        {
                            Id = new Guid("81fb5472-2bf3-4efd-95e3-03834c35f276"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS BRADFORD)",
                            Prefix = "PV",
                            TraId = 7068
                        },
                        new SwaCode
                        {
                            Id = new Guid("addcb0a8-dd63-417f-8b0f-44f10cd34ce7"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS DONCASTER)",
                            Prefix = "PW",
                            TraId = 7041
                        },
                        new SwaCode
                        {
                            Id = new Guid("64e907f9-3724-43b1-a210-566109442015"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS HALIFAX)",
                            Prefix = "PX",
                            TraId = 7170
                        },
                        new SwaCode
                        {
                            Id = new Guid("02cff556-78a6-42f4-979f-d3476782e526"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS SHEFFIELD)",
                            Prefix = "PY",
                            TraId = 7057
                        },
                        new SwaCode
                        {
                            Id = new Guid("a1fbeb91-ad9c-40eb-a985-fa2a0c2e4a97"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS WAKEFIELD)",
                            Prefix = "PZ",
                            TraId = 7063
                        },
                        new SwaCode
                        {
                            Id = new Guid("ea67b321-edb4-41bf-b523-f8cb5c6181bf"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "YORKSHIRE WATER LIMITED",
                            Prefix = "QB",
                            TraId = 9109
                        },
                        new SwaCode
                        {
                            Id = new Guid("af33fc67-6a70-4c35-a7ea-1aad6d4c9694"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "YOUR COMMUNICATIONS LIMITED",
                            Prefix = "JH",
                            TraId = 7083
                        },
                        new SwaCode
                        {
                            Id = new Guid("fc77df45-fc02-4850-9772-b9056e68845d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ZAYO GROUP UK LIMITED",
                            Prefix = "ZV",
                            TraId = 7235
                        },
                        new SwaCode
                        {
                            Id = new Guid("1f88c9f4-c721-4209-8860-64ae38fef92d"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ZOOM INTERNET LIMITED",
                            Prefix = "R5",
                            TraId = 7574
                        },
                        new SwaCode
                        {
                            Id = new Guid("25ffb9fe-46f8-4c23-92e3-91a29a076e2e"),
                            IsActive = true,
                            IsAdmin = false,
                            Name = "ZZOOMM PLC",
                            Prefix = "B9",
                            TraId = 7379
                        }
    };

}

