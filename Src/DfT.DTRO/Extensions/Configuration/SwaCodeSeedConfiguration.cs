namespace DfT.DTRO.Extensions.Configuration;

[ExcludeFromCodeCoverage]
public class SwaCodeSeedConfiguration : IEntityTypeConfiguration<SwaCode>
{
    public void Configure(EntityTypeBuilder<SwaCode> builder)
    {
        builder.HasData(SwaCodes);
    }


    #region swaCodes
    private readonly IEnumerable<SwaCode> SwaCodes = new List<SwaCode>
    {
            new() { Id = Guid.NewGuid(), Name = "Department of Transport", TraId = 0, Prefix = "DfT", IsAdmin = true },
            new() { Id = Guid.NewGuid(), Name = "(AQ) LIMITED", TraId = 7334, Prefix = "SK", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "1310 Ltd", TraId = 7550, Prefix = "M3", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "ADVANCED ELECTRICITY NETWORKS LIMITED", TraId = 7583, Prefix = "S6"
            },
            new() { Id = Guid.NewGuid(), Name = "Affinity Systems Limited", TraId = 7544, Prefix = "L4", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "AFFINITY WATER EAST LIMITED", TraId = 9132, Prefix = "MT", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "AFFINITY WATER LIMITED", TraId = 9133, Prefix = "MX", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "AFFINITY WATER SOUTHEAST LIMITED", TraId = 9121, Prefix = "EV"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "AIRBAND COMMUNITY INTERNET LIMITED", TraId = 7372, Prefix = "A8"
            },
            new() { Id = Guid.NewGuid(), Name = "AIRWAVE SOLUTIONS LIMITED", TraId = 7297, Prefix = "VB", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "AJ TECHNOLOGIES LIMITED", TraId = 7545, Prefix = "L5", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ALBION WATER LIMITED", TraId = 9139, Prefix = "UZ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ALLPOINTS FIBRE LIMITED", TraId = 7552, Prefix = "M5", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "ALLPOINTS FIBRE NETWORKS LIMITED", TraId = 7528, Prefix = "J3"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ANGLIA CABLE COMMUNICATIONS LIMITED", TraId = 7026, Prefix = "AC"
            },
            new() { Id = Guid.NewGuid(), Name = "ARELION UK LIMITED", TraId = 7230, Prefix = "ZM", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ARQIVA LIMITED", TraId = 7354, Prefix = "TN", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ARQIVA LIMITED (Formerly NATIONAL TRANSCOMMUNICATIONS LTD)",
                TraId = 7217,
                Prefix = "SP"
            },
            new() { Id = Guid.NewGuid(), Name = "AWG GROUP LIMITED", TraId = 9100, Prefix = "AD", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "AXIONE UK LIMITED", TraId = 7541, Prefix = "K9", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "BAA INTERNATIONAL LIMITED", TraId = 17, Prefix = "AF", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "BAI COMMUNICATIONS INFRASTRUCTURE LIMITED",
                TraId = 7563,
                Prefix = "N9"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "BARNSLEY METROPOLITAN BOROUGH COUNCIL", TraId = 4405, Prefix = "AJ"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "BATH AND NORTH EAST SOMERSET COUNCIL", TraId = 114, Prefix = "QD"
            },
            new() { Id = Guid.NewGuid(), Name = "BAZALGETTE TUNNEL LIMITED", TraId = 7345, Prefix = "ZE", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "BEDFORD BOROUGH COUNCIL", TraId = 235, Prefix = "UB", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "BIRMINGHAM CABLE LIMITED", TraId = 7028, Prefix = "AR", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "BIRMINGHAM CABLE LIMITED (Formerly BIRMINGHAM CABLE WYTHALL)",
                TraId = 7198,
                Prefix = "QE"
            },
            new() { Id = Guid.NewGuid(), Name = "BIRMINGHAM CITY COUNCIL", TraId = 4605, Prefix = "AQ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "BLACKBURN WITH DARWEN BOROUGH COUNCIL", TraId = 2372, Prefix = "AE"
            },
            new() { Id = Guid.NewGuid(), Name = "BLACKPOOL BOROUGH COUNCIL", TraId = 2373, Prefix = "CU", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "BLAENAU GWENT COUNTY BOROUGH COUNCIL", TraId = 6910, Prefix = "AS"
            },
            new() { Id = Guid.NewGuid(), Name = "Boldyn Networks Limited", TraId = 7547, Prefix = "L7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "BOLT PRO TEM LIMITED", TraId = 7346, Prefix = "XD", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "BOLTON METROPOLITAN BOROUGH COUNCIL", TraId = 4205, Prefix = "AT"
            },
            new() { Id = Guid.NewGuid(), Name = "BOURNEMOUTH WATER LIMITED", TraId = 9110, Prefix = "AU", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "BOURNEMOUTH, CHRISTCHURCH AND POOLE COUNCIL",
                TraId = 1260,
                Prefix = "B2"
            },
            new() { Id = Guid.NewGuid(), Name = "BOX BROADBAND LIMITED", TraId = 7373, Prefix = "A9", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "BRACKNELL FOREST COUNCIL", TraId = 335, Prefix = "DW", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "BRIDGEND COUNTY BOROUGH COUNCIL", TraId = 6915, Prefix = "AX"
            },
            new() { Id = Guid.NewGuid(), Name = "BRIGHTON & HOVE CITY COUNCIL", TraId = 1445, Prefix = "DU", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "BRISTOL CITY COUNCIL", TraId = 116, Prefix = "QF", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "BRITISH PIPELINE AGENCY LIMITED", TraId = 7089, Prefix = "BA"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "BRITISH TELECOMMUNICATIONS PUBLIC LIMITED COMPANY",
                TraId = 30,
                Prefix = "BC"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "BROADBAND FOR THE RURAL NORTH LIMITED", TraId = 7350, Prefix = "TG"
            },
            new() { Id = Guid.NewGuid(), Name = "BROADWAY PARTNERS LIMITED", TraId = 7392, Prefix = "D7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "BRSK LIMITED", TraId = 7527, Prefix = "J2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "BRYN BLAEN WIND FARM LIMITED", TraId = 7360, Prefix = "TU", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "BUCKINGHAMSHIRE COUNCIL", TraId = 440, Prefix = "D4", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "BURY METROPOLITAN BOROUGH COUNCIL", TraId = 4210, Prefix = "BJ"
            },
            new() { Id = Guid.NewGuid(), Name = "CABLE LONDON LIMITED", TraId = 7030, Prefix = "BL", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLE LONDON LIMITED (Formerly CABLE LONDON PLC ENFIELD)",
                TraId = 7099,
                Prefix = "BM"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLE LONDON LIMITED (Formerly CABLE LONDON PLC HACKNEY)",
                TraId = 7100,
                Prefix = "BN"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLE LONDON LIMITED (Formerly CABLE LONDON PLC HARINGEY)",
                TraId = 7101,
                Prefix = "BP"
            },
            new() { Id = Guid.NewGuid(), Name = "CABLE ON DEMAND LIMITED", TraId = 7113, Prefix = "CX", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLE ON DEMAND LIMITED (Formerly COMCAST TEESSIDE DARLINGTON)",
                TraId = 7112,
                Prefix = "CW"
            },
            new() { Id = Guid.NewGuid(), Name = "CABLECOM INVESTMENTS LIMITED", TraId = 7173, Prefix = "BV", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CABLETEL BEDFORDSHIRE", TraId = 7032, Prefix = "BW", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "CABLETEL HERTS AND BEDS LIMITED", TraId = 7108, Prefix = "CA"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLETEL HERTS AND BEDS LIMITED (Formerly CABLETEL CENTRAL HERTFORDSHIRE)",
                TraId = 7106,
                Prefix = "BX"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLETEL HERTS AND BEDS LIMITED (Formerly CABLETEL HERTFORDSHIRE)",
                TraId = 7107,
                Prefix = "BY"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "CABLETEL SURREY AND HAMPSHIRE LIMITED", TraId = 7111, Prefix = "CE"
            },
            new() { Id = Guid.NewGuid(), Name = "CADENT GAS LIMITED", TraId = 10, Prefix = "AZ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "CAERPHILLY COUNTY BOROUGH COUNCIL", TraId = 6920, Prefix = "CG"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CALDERDALE METROPOLITAN BOROUGH COUNCIL",
                TraId = 4710,
                Prefix = "CH"
            },
            new() { Id = Guid.NewGuid(), Name = "CALL FLOW SOLUTIONS LTD", TraId = 7339, Prefix = "UY", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CAMBRIDGE FIBRE NETWORKS LTD", TraId = 7371, Prefix = "A7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CAMBRIDGE WATER PLC", TraId = 9113, Prefix = "CK", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CAMBRIDGESHIRE COUNTY COUNCIL", TraId = 535, Prefix = "CL", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "CARMARTHENSHIRE COUNTY COUNCIL", TraId = 6825, Prefix = "QQ"
            },
            new() { Id = Guid.NewGuid(), Name = "CELLNEX (ON TOWER UK LTD)", TraId = 7513, Prefix = "G5", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CENTRAL BEDFORDSHIRE COUNCIL", TraId = 240, Prefix = "UC", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CENTRO (WEST MIDLANDS PASSENGER TRANSPORT EXECUTIVE)",
                TraId = 9234,
                Prefix = "WB"
            },
            new() { Id = Guid.NewGuid(), Name = "CEREDIGION COUNTY COUNCIL", TraId = 6820, Prefix = "QP", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CHESHIRE EAST COUNCIL", TraId = 660, Prefix = "UD", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "CHESHIRE WEST AND CHESTER COUNCIL", TraId = 665, Prefix = "UE"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CHOLDERTON AND DISTRICT WATER COMPANY LIMITED",
                TraId = 9115,
                Prefix = "CQ"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "CITY AND COUNTY OF SWANSEA COUNCIL", TraId = 6855, Prefix = "MD"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CITY OF BRADFORD METROPOLITAN DISTRICT COUNCIL",
                TraId = 4705,
                Prefix = "AV"
            },
            new() { Id = Guid.NewGuid(), Name = "CITY OF CARDIFF COUNCIL", TraId = 6815, Prefix = "QN", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CITY OF DONCASTER COUNCIL", TraId = 4410, Prefix = "DQ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CITY OF LONDON CORPORATION", TraId = 5030, Prefix = "CR", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CITY OF WAKEFIELD METROPOLITAN DISTRICT COUNCIL",
                TraId = 4725,
                Prefix = "NY"
            },
            new() { Id = Guid.NewGuid(), Name = "CITY OF WESTMINSTER", TraId = 5990, Prefix = "CT", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CITY OF WOLVERHAMPTON COUNCIL", TraId = 4635, Prefix = "PP", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CITY OF YORK COUNCIL", TraId = 2741, Prefix = "SH", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "CITYFIBRE METRO NETWORKS LIMITED", TraId = 7330, Prefix = "KG"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "CITYLINK TELECOMMUNICATIONS LIMITED", TraId = 7261, Prefix = "XB"
            },
            new() { Id = Guid.NewGuid(), Name = "CLOUD HQ DIDCOT FIBRE GP LTD", TraId = 7508, Prefix = "F7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "COLT TECHNOLOGY SERVICES", TraId = 7075, Prefix = "CS", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "COMMUNICATIONS INFRASTRUCTURE NETWORKS LIMITED",
                TraId = 7358,
                Prefix = "TS"
            },
            new() { Id = Guid.NewGuid(), Name = "COMMUNITY FIBRE LIMITED", TraId = 7364, Prefix = "TY", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CONCEPT SOLUTIONS PEOPLE LTD", TraId = 7335, Prefix = "SR", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CONNEXIN LIMITED", TraId = 7531, Prefix = "J7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CONWY COUNTY BOROUGH COUNCIL", TraId = 6905, Prefix = "AA", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CORANGE PERSONAL COMMUNICATIONS SERVICES LIMITED (Formerly HUTCHINSON MICROTEL)",
                TraId = 7077,
                Prefix = "FS"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CORNERSTONE TELECOMMUNICATIONS INFRASTRUCTURE LIMITED",
                TraId = 7567,
                Prefix = "P6"
            },
            new() { Id = Guid.NewGuid(), Name = "CORNWALL COUNCIL", TraId = 840, Prefix = "UF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "COUNCIL OF THE ISLES OF SCILLY", TraId = 835, Prefix = "HJ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "COUNTY BROADBAND LTD", TraId = 7369, Prefix = "A5", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "COVENTRY CITY COUNCIL", TraId = 4610, Prefix = "DB", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CROSS RAIL LTD", TraId = 7318, Prefix = "UM", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CROSSKIT LIMITED", TraId = 7573, Prefix = "R4", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "CUMBERLAND COUNCIL", TraId = 940, Prefix = "P5", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "DARLINGTON BOROUGH COUNCIL", TraId = 1350, Prefix = "HF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "DENBIGHSHIRE COUNTY COUNCIL", TraId = 6830, Prefix = "QR", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "DERBY CITY COUNCIL", TraId = 1055, Prefix = "SZ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "DERBYSHIRE COUNTY COUNCIL", TraId = 1050, Prefix = "DF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "DEVON COUNTY COUNCIL", TraId = 1155, Prefix = "DG", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "DFT ROAD STATISTICS DIVISION", TraId = 7188, Prefix = "QU", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "DIAMOND CABLE (MANSFIELD)", TraId = 7116, Prefix = "DL", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "DIAMOND CABLE COMMUNICATIONS LIMITED", TraId = 7189, Prefix = "QS"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE DARLINGTON)",
                TraId = 7114,
                Prefix = "DJ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE GRANTHAM)",
                TraId = 7040,
                Prefix = "DH"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE LINCOLN)",
                TraId = 7115,
                Prefix = "DK"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE MELTON MOWBRAY)",
                TraId = 7117,
                Prefix = "DM"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE NEWARK)",
                TraId = 7118,
                Prefix = "DN"
            },
            new() { Id = Guid.NewGuid(), Name = "DIGITAL INFRASTRUCTURE LTD", TraId = 7518, Prefix = "H2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "DORSET COUNCIL", TraId = 1265, Prefix = "B3", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "DUDGEON OFFSHORE WIND LIMITED", TraId = 7333, Prefix = "RB", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "DUDLEY METROPOLITAN BOROUGH COUNCIL", TraId = 4615, Prefix = "DS"
            },
            new() { Id = Guid.NewGuid(), Name = "DURHAM COUNTY COUNCIL", TraId = 1355, Prefix = "UG", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "DWR CYMRU CYFYNGEDIG (WELSH WATER)", TraId = 9107, Prefix = "PE"
            },
            new() { Id = Guid.NewGuid(), Name = "E S PIPELINES LTD", TraId = 7260, Prefix = "ZY", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "EAST ANGLIA THREE LIMITED", TraId = 7581, Prefix = "S4", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "EAST RIDING OF YORKSHIRE COUNCIL", TraId = 2001, Prefix = "QV"
            },
            new() { Id = Guid.NewGuid(), Name = "EAST SUSSEX COUNTY COUNCIL", TraId = 1440, Prefix = "EA", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "EASTERN POWER NETWORKS PLC", TraId = 7010, Prefix = "EC", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "ECLIPSE POWER NETWORKS LIMITED", TraId = 7357, Prefix = "TR"
            },
            new() { Id = Guid.NewGuid(), Name = "EDF ENERGY CUSTOMERS LIMITED", TraId = 7009, Prefix = "GU", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "EDF ENERGY RENEWABLES LTD", TraId = 7557, Prefix = "N2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "EE LIMITED", TraId = 7250, Prefix = "YN", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "EIRCOM (UK) LIMITED", TraId = 7243, Prefix = "YD", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "EIRGRID UK HOLDINGS LIMITED", TraId = 7325, Prefix = "UU", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ELECLINK LIMITED", TraId = 7338, Prefix = "ZB", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "ELECTRICITY NORTH WEST LIMITED", TraId = 7005, Prefix = "JG"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ENERGIS COMMUNICATIONS LIMITED", TraId = 7080, Prefix = "EL"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ENERGY ASSETS NETWORKS LIMITED", TraId = 7359, Prefix = "TT"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ENERGY ASSETS PIPELINES LIMITED", TraId = 7348, Prefix = "TD"
            },
            new() { Id = Guid.NewGuid(), Name = "ENVIRONMENT AGENCY", TraId = 7220, Prefix = "SV", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ESP CONNECTIONS LIMITED", TraId = 7242, Prefix = "YC", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ESP ELECTRICITY LIMITED", TraId = 7309, Prefix = "VQ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ESP ELECTRICITY LIMITED (Formerly LAING ENERGY LTD)",
                TraId = 7268,
                Prefix = "XU"
            },
            new() { Id = Guid.NewGuid(), Name = "ESP NETWORKS LIMITED", TraId = 7255, Prefix = "YU", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ESP PIPELINES LIMITED", TraId = 7256, Prefix = "YV", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ESP WATER LIMITED", TraId = 7564, Prefix = "N8", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "ESSEX AND SUFFOLK WATER LIMITED", TraId = 9120, Prefix = "EN"
            },
            new() { Id = Guid.NewGuid(), Name = "ESSEX COUNTY COUNCIL", TraId = 1585, Prefix = "EP", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ESSO EXPLORATION AND PRODUCTION UK LIMITED",
                TraId = 7091,
                Prefix = "EQ"
            },
            new() { Id = Guid.NewGuid(), Name = "EUNETWORKS GROUP LIMITED", TraId = 7307, Prefix = "VN", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "E-VOLVE SOLUTIONS LTD", TraId = 7579, Prefix = "S2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "EXASCALE LIMITED", TraId = 7503, Prefix = "F2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "EXPONENTIAL-E LIMITED", TraId = 7540, Prefix = "K8", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "F & W NETWORKS LTD", TraId = 7386, Prefix = "C8", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FACTCO", TraId = 7511, Prefix = "G4", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FIBERNET UK LIMITED", TraId = 7223, Prefix = "SY", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FIBRE ASSETS LTD", TraId = 7510, Prefix = "G3", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FIBRENATION LIMITED", TraId = 7554, Prefix = "M7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FIBRESPEED LIMITED", TraId = 7305, Prefix = "VL", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FIBREWAVE NETWORKS LIMITED", TraId = 7332, Prefix = "LH", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FIBRUS NETWORKS GB LTD", TraId = 7580, Prefix = "S3", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FIBRUS NETWORKS LIMITED", TraId = 7537, Prefix = "K6", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FLINTSHIRE COUNTY COUNCIL", TraId = 6835, Prefix = "QX", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FREEDOM FIBRE LIMITED", TraId = 7539, Prefix = "K5", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FUJITSU SERVICES LIMITED", TraId = 7328, Prefix = "KB", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "FULCRUM ELECTRICITY ASSETS LIMITED", TraId = 7368, Prefix = "A4"
            },
            new() { Id = Guid.NewGuid(), Name = "FULCRUM PIPELINES LIMITED", TraId = 7294, Prefix = "WY", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "FULL FIBRE LIMITED", TraId = 7376, Prefix = "B6", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "G.NETWORK COMMUNICATIONS LIMITED", TraId = 7362, Prefix = "TW"
            },
            new() { Id = Guid.NewGuid(), Name = "GAMMA TELECOM LTD", TraId = 7241, Prefix = "YB", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "GATESHEAD METROPOLITAN BOROUGH COUNCIL", TraId = 4505, Prefix = "EX"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "GC PAN EUROPEAN CROSSING UK LIMITED", TraId = 7229, Prefix = "ZL"
            },
            new() { Id = Guid.NewGuid(), Name = "GDF SUEZ TEESSIDE LIMITED", TraId = 7016, Prefix = "SE", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "GIGACLEAR LIMITED", TraId = 7329, Prefix = "KA", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "GLIDE BUSINESS LIMITED", TraId = 7343, Prefix = "ZC", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "GLOBAL ONE COMMUNICATIONS LIMITED", TraId = 7084, Prefix = "LS"
            },
            new() { Id = Guid.NewGuid(), Name = "GLOBAL REACH NETWORKS LIMITED", TraId = 7553, Prefix = "M6", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "GLOBAL TELECOMMUNICATIONS LIMITED", TraId = 7228, Prefix = "ZK"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "GLOUCESTERSHIRE COUNTY COUNCIL", TraId = 1600, Prefix = "EY"
            },
            new() { Id = Guid.NewGuid(), Name = "GOFIBRE HOLDINGS LIMITED", TraId = 7551, Prefix = "M4", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "GRAIN COMMUNICATIONS LIMITED", TraId = 7351, Prefix = "TJ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "GRAYSHOTT GIGABIT LIMITED", TraId = 7538, Prefix = "K7", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "GREENLINK INTERCONNECTOR LIMITED", TraId = 7548, Prefix = "L8"
            },
            new() { Id = Guid.NewGuid(), Name = "GTC PIPELINES LIMITED", TraId = 7231, Prefix = "ZP", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "GWYNEDD COUNCIL", TraId = 6810, Prefix = "QM", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HAFREN DYFRDWY CYFYNGEDIG", TraId = 9138, Prefix = "ZU", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "HAFREN DYFRDWY CYFYNGEDIG (Formerly CHESTER WATERWORKS)",
                TraId = 9114,
                Prefix = "CP"
            },
            new() { Id = Guid.NewGuid(), Name = "HALTON BOROUGH COUNCIL", TraId = 650, Prefix = "AN", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HAMPSHIRE COUNTY COUNCIL", TraId = 1770, Prefix = "FF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HAMPSTEAD FIBRE LIMITED", TraId = 7543, Prefix = "L3", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "HARLAXTON ENERGY NETWORKS LIMITED", TraId = 7342, Prefix = "YZ"
            },
            new() { Id = Guid.NewGuid(), Name = "HARLAXTON GAS NETWORKS LTD", TraId = 7374, Prefix = "B4", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HARTLEPOOL BOROUGH COUNCIL", TraId = 724, Prefix = "RA", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HARTLEPOOL WATER", TraId = 9122, Prefix = "FJ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HEN BEUDY SERVICES LIMITED", TraId = 7396, Prefix = "E3", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HENDY WIND FARM LIMITED", TraId = 7577, Prefix = "R8", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HEREFORDSHIRE COUNCIL", TraId = 1850, Prefix = "FL", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HERTFORDSHIRE COUNTY COUNCIL", TraId = 1900, Prefix = "FM", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "HIBERNIA ATLANTIC (UK) LIMITED", TraId = 7331, Prefix = "KK"
            },
            new() { Id = Guid.NewGuid(), Name = "HS2 LTD", TraId = 7347, Prefix = "TA", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HULL CITY COUNCIL", TraId = 2004, Prefix = "RG", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "Hutchison 3G UK Limited", TraId = 7264, Prefix = "XJ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "HYPEROPTIC LTD", TraId = 7349, Prefix = "TF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ICOSA WATER LTD", TraId = 7380, Prefix = "C2", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "IN FOCUS PUBLIC NETWORKS LIMITED", TraId = 60, Prefix = "RE"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "INDEPENDENT DISTRIBUTION CONNECTION SPECIALISTS LIMITED",
                TraId = 7582,
                Prefix = "S5"
            },
            new() { Id = Guid.NewGuid(), Name = "INDEPENDENT PIPELINES LIMITED", TraId = 7218, Prefix = "ST", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "INDEPENDENT POWER NETWORKS LIMITED", TraId = 7281, Prefix = "WK"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "INDEPENDENT WATER NETWORKS LIMITED", TraId = 7326, Prefix = "UV"
            },
            new() { Id = Guid.NewGuid(), Name = "INDIGO PIPELINES LIMITED", TraId = 7313, Prefix = "VX", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "INDIGO POWER LTD", TraId = 7555, Prefix = "M8", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "INTERNET CONNECTIONS LTD", TraId = 7352, Prefix = "TK", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "INTERNETTY LTD", TraId = 7391, Prefix = "D6", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "INTEROUTE NETWORKS LIMITED", TraId = 7245, Prefix = "YF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "IONICA LIMITED", TraId = 7074, Prefix = "FT", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "ISLE OF ANGLESEY COUNTY COUNCIL", TraId = 6805, Prefix = "QC"
            },
            new() { Id = Guid.NewGuid(), Name = "ISLE OF WIGHT COUNCIL", TraId = 2114, Prefix = "RF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ITS TECHNOLOGY GROUP LIMITED", TraId = 7370, Prefix = "A6", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "IX WIRELESS LIMITED", TraId = 7377, Prefix = "B7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "JOHN JONES LTD", TraId = 7174, Prefix = "FW", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "JURASSIC FIBRE LIMITED", TraId = 7387, Prefix = "C9", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "KCOM GROUP LIMITED", TraId = 7073, Prefix = "GG", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "KCOM GROUP LIMITED (NATIONAL)", TraId = 7082, Prefix = "MY", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "KENT COUNTY COUNCIL", TraId = 2275, Prefix = "GE", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "KIRKLEES COUNCIL", TraId = 4715, Prefix = "GJ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "KNOWSLEY METROPOLITAN BOROUGH COUNCIL", TraId = 4305, Prefix = "GK"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "KPN EURORINGS B.V. (Formerly KPN TELECOM UK LTD)",
                TraId = 7227,
                Prefix = "ZJ"
            },
            new() { Id = Guid.NewGuid(), Name = "KPN EURORINGS BV", TraId = 7267, Prefix = "XT", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LANCASHIRE COUNTY COUNCIL", TraId = 2371, Prefix = "GM", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "LANCASTER UNIVERSITY NETWORK SERVICES LIMITED",
                TraId = 7277,
                Prefix = "WE"
            },
            new() { Id = Guid.NewGuid(), Name = "LAST MILE ELECTRICITY LIMITED", TraId = 7385, Prefix = "C7", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "LAST MILE ELECTRICITY LIMITED (Formerly GLOBAL UTILITY CONNECTIONS)",
                TraId = 7269,
                Prefix = "XV"
            },
            new() { Id = Guid.NewGuid(), Name = "LAST MILE GAS LIMITED", TraId = 7311, Prefix = "VS", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LAST MILE TELECOM LIMITED", TraId = 7568, Prefix = "P7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LATOS DATA CENTRE LTD", TraId = 7559, Prefix = "N4", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LEEDS CITY COUNCIL", TraId = 4720, Prefix = "GP", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "LEEP ELECTRICITY NETWORKS LIMITED", TraId = 7506, Prefix = "F5"
            },
            new() { Id = Guid.NewGuid(), Name = "LEEP NETWORKS (WATER) LIMITED", TraId = 7356, Prefix = "TQ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LEICESTER CITY COUNCIL", TraId = 2465, Prefix = "EW", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LEICESTERSHIRE COUNTY COUNCIL", TraId = 2460, Prefix = "GQ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "LEVEL 3 COMMUNICATIONS LIMITED", TraId = 7232, Prefix = "ZQ"
            },
            new() { Id = Guid.NewGuid(), Name = "LIGHTNING FIBRE LIMITED", TraId = 7378, Prefix = "B8", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LIGHTSPEED NETWORKS LTD", TraId = 7524, Prefix = "H8", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LINCOLNSHIRE COUNTY COUNCIL", TraId = 2500, Prefix = "GS", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LIT FIBRE GROUP LTD", TraId = 7509, Prefix = "G2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LIVERPOOL CITY COUNCIL", TraId = 4310, Prefix = "GT", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF BARKING AND DAGENHAM", TraId = 5060, Prefix = "AG"
            },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF BARNET", TraId = 5090, Prefix = "AH", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF BEXLEY", TraId = 5120, Prefix = "AP", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF BRENT", TraId = 5150, Prefix = "AW", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF BROMLEY", TraId = 5180, Prefix = "BF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF CAMDEN", TraId = 5210, Prefix = "CM", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF CROYDON", TraId = 5240, Prefix = "DD", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF EALING", TraId = 5270, Prefix = "DV", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF ENFIELD", TraId = 5300, Prefix = "EM", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HACKNEY", TraId = 5360, Prefix = "FD", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HAMMERSMITH & FULHAM", TraId = 5390, Prefix = "FE"
            },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HARINGEY", TraId = 5420, Prefix = "FG", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HARROW", TraId = 5450, Prefix = "FH", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HAVERING", TraId = 5480, Prefix = "FK", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HILLINGDON", TraId = 5510, Prefix = "FP", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HOUNSLOW", TraId = 5540, Prefix = "FQ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF ISLINGTON", TraId = 5570, Prefix = "FV", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF LAMBETH", TraId = 5660, Prefix = "GL", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF LEWISHAM", TraId = 5690, Prefix = "GR", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF MERTON", TraId = 5720, Prefix = "HC", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF NEWHAM", TraId = 5750, Prefix = "HS", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF REDBRIDGE", TraId = 5780, Prefix = "KM", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF RICHMOND UPON THAMES", TraId = 5810, Prefix = "KP"
            },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF SOUTHWARK", TraId = 5840, Prefix = "LR", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF SUTTON", TraId = 5870, Prefix = "MB", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF TOWER HAMLETS", TraId = 5900, Prefix = "NA"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF WALTHAM FOREST", TraId = 5930, Prefix = "PA"
            },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF WANDSWORTH", TraId = 5960, Prefix = "PB", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON TRANSPORT LIMITED", TraId = 7210, Prefix = "RK", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LONDON UNDERGROUND LIMITED", TraId = 7072, Prefix = "GV", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LUMEN TECHNOLOGIES INC", TraId = 7183, Prefix = "RT", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "LUMEN TECHNOLOGIES INC (Formerly CENTURYLINK COMMUNICATIONS UK LIMITED)",
                TraId = 7094,
                Prefix = "BB"
            },
            new() { Id = Guid.NewGuid(), Name = "LUNS LTD", TraId = 7320, Prefix = "UP", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "LUTON BOROUGH COUNCIL", TraId = 230, Prefix = "JA", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "MAINLINE PIPELINES LIMITED", TraId = 7090, Prefix = "GW", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "MANCHESTER CITY COUNCIL", TraId = 4215, Prefix = "GX", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "MEDWAY COUNCIL", TraId = 2280, Prefix = "JL", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "MERTHYR TYDFIL COUNTY BOROUGH COUNCIL", TraId = 6925, Prefix = "HB"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "METRO DIGITAL TELEVISION LIMITED", TraId = 7176, Prefix = "HD"
            },
            new() { Id = Guid.NewGuid(), Name = "MIDDLESBROUGH BOROUGH COUNCIL", TraId = 734, Prefix = "RL", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "MILTON KEYNES CITY COUNCIL", TraId = 435, Prefix = "JM", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "MLL TELECOM LTD.", TraId = 7278, Prefix = "WF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "MONMOUTHSHIRE COUNTY COUNCIL", TraId = 6840, Prefix = "RM", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "MS3 NETWORKS LIMITED", TraId = 7523, Prefix = "H7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "MUA ELECTRICITY LIMITED", TraId = 7366, Prefix = "A2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "MUA GAS LIMITED", TraId = 7367, Prefix = "A3", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "MY FIBRE LIMITED", TraId = 7504, Prefix = "F3", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NATIONAL GAS TRANSMISSION", TraId = 7397, Prefix = "E4", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (EAST MIDLANDS)",
                TraId = 7011,
                Prefix = "DY"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (SOUTH WALES)",
                TraId = 7012,
                Prefix = "LL"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (SOUTH WEST)",
                TraId = 7003,
                Prefix = "LN"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (WEST MIDLANDS)",
                TraId = 7007,
                Prefix = "HM"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NATIONAL GRID ELECTRICITY TRANSMISSION PLC",
                TraId = 7015,
                Prefix = "HP"
            },
            new() { Id = Guid.NewGuid(), Name = "NATIONAL GRID TELECOMS", TraId = 7145, Prefix = "SJ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NATIONAL HIGHWAYS", TraId = 11, Prefix = "FN", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NEATH PORT TALBOT COUNTY BOROUGH COUNCIL",
                TraId = 6930,
                Prefix = "HQ"
            },
            new() { Id = Guid.NewGuid(), Name = "NEOS NETWORKS LIMITED", TraId = 7244, Prefix = "YE", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NETOMNIA LIMITED", TraId = 7388, Prefix = "D2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NETWORK RAIL", TraId = 7093, Prefix = "KL", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NEW ASH GREEN VILLAGE ASSOCIATION LIMITED",
                TraId = 7398,
                Prefix = "E5"
            },
            new() { Id = Guid.NewGuid(), Name = "NEWCASTLE CITY COUNCIL", TraId = 4510, Prefix = "HR", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NEWPORT CITY COUNCIL", TraId = 6935, Prefix = "HT", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NEXFIBRE NETWORKS LIMITED", TraId = 7562, Prefix = "N7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NEXTGENACCESS LTD", TraId = 7353, Prefix = "TL", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NMCN PLC", TraId = 7327, Prefix = "UW", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NORFOLK COUNTY COUNCIL", TraId = 2600, Prefix = "HU", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "NORTH EAST LINCOLNSHIRE COUNCIL", TraId = 2002, Prefix = "RN"
            },
            new() { Id = Guid.NewGuid(), Name = "NORTH LINCOLNSHIRE COUNCIL", TraId = 2003, Prefix = "RP", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "NORTH NORTHAMPTONSHIRE COUNCIL", TraId = 2840, Prefix = "F8"
            },
            new() { Id = Guid.NewGuid(), Name = "NORTH SOMERSET COUNCIL", TraId = 121, Prefix = "RQ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NORTH TYNESIDE COUNCIL", TraId = 4515, Prefix = "HY", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NORTH YORKSHIRE COUNCIL", TraId = 2745, Prefix = "JB", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NORTHERN GAS NETWORKS LIMITED", TraId = 7271, Prefix = "XX", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "NORTHERN POWERGRID (NORTHEAST) LIMITED", TraId = 7006, Prefix = "JD"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "NORTHERN POWERGRID (YORKSHIRE) PLC", TraId = 7001, Prefix = "QA"
            },
            new() { Id = Guid.NewGuid(), Name = "NORTHUMBERLAND COUNTY COUNCIL", TraId = 2935, Prefix = "UH", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NORTHUMBRIAN WATER LIMITED", TraId = 9101, Prefix = "JF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NOTTINGHAM CITY COUNCIL", TraId = 3060, Prefix = "SQ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "NOTTINGHAMSHIRE COUNTY COUNCIL", TraId = 3055, Prefix = "JK"
            },
            new() { Id = Guid.NewGuid(), Name = "NTL (B) LIMITED", TraId = 7096, Prefix = "AM", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NTL (BROADLAND) LIMITED", TraId = 7029, Prefix = "BE", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NTL (SOUTH EAST) LIMITED", TraId = 7120, Prefix = "EE", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS EPPING FOREST)",
                TraId = 7121,
                Prefix = "EF"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS GREATER LONDON)",
                TraId = 7122,
                Prefix = "EG"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS HAVERING)",
                TraId = 7123,
                Prefix = "EH"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS NEWHAM)",
                TraId = 7124,
                Prefix = "EJ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS WALTHAM FOREST)",
                TraId = 7125,
                Prefix = "EK"
            },
            new() { Id = Guid.NewGuid(), Name = "NTL CAMBRIDGE LIMITED", TraId = 7034, Prefix = "CJ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NTL KIRKLEES", TraId = 7047, Prefix = "BZ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NTL MIDLANDS LIMITED", TraId = 7119, Prefix = "DP", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NTL NATIONAL NETWORKS LIMITED", TraId = 7274, Prefix = "WA", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NTL SOUTH CENTRAL LIMITED", TraId = 7058, Prefix = "LU", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NTL TELECOM SERVICES LIMITED", TraId = 7219, Prefix = "SU", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NWP STREET LIMITED", TraId = 7226, Prefix = "ZH", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "NYNET LTD", TraId = 7522, Prefix = "H6", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "OCIUSNET UK LTD", TraId = 7542, Prefix = "L2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "OGI NETWORKS LIMITED", TraId = 7505, Prefix = "F4", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "OLDHAM METROPOLITAN BOROUGH COUNCIL", TraId = 4220, Prefix = "KC"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "OPEN FIBRE NETWORKS (WHOLESALE) LIMITED",
                TraId = 7336,
                Prefix = "UX"
            },
            new() { Id = Guid.NewGuid(), Name = "Open Infra Ltd", TraId = 7556, Prefix = "M9", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "OPEN NETWORK SYSTEMS LIMITED", TraId = 7381, Prefix = "C3", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "OPTICAL FIBRE INFRASTRUCTURE LIMITED", TraId = 7502, Prefix = "E9"
            },
            new() { Id = Guid.NewGuid(), Name = "OPTIMAL POWER NETWORKS", TraId = 7500, Prefix = "E7", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ORANGE PERSONAL COMMUNICATIONS SERVICES LIMITED",
                TraId = 7233,
                Prefix = "ZR"
            },
            new() { Id = Guid.NewGuid(), Name = "ORBITAL NET LTD", TraId = 7517, Prefix = "G9", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ORSTED HORNSEA PROJECT THREE (UK) LIMITED",
                TraId = 7566,
                Prefix = "P3"
            },
            new() { Id = Guid.NewGuid(), Name = "OXFORDSHIRE COUNTY COUNCIL", TraId = 3100, Prefix = "KE", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "PEMBROKESHIRE COUNTY COUNCIL", TraId = 6845, Prefix = "RR", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "PEOPLE'S FIBRE LTD", TraId = 7399, Prefix = "E6", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "PERSIMMON HOMES LIMITED", TraId = 7534, Prefix = "K2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "PETERBOROUGH CITY COUNCIL", TraId = 540, Prefix = "FB", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "PINE MEDIA LIMITED", TraId = 7533, Prefix = "J9", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "PLYMOUTH CITY COUNCIL", TraId = 1160, Prefix = "SL", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "PORTSMOUTH CITY COUNCIL", TraId = 1775, Prefix = "FC", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "PORTSMOUTH WATER LIMITED", TraId = 9128, Prefix = "KH", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "POWERSYSTEMS UK LTD", TraId = 7316, Prefix = "UA", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "POWYS COUNTY COUNCIL", TraId = 6850, Prefix = "KJ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "QUICKLINE COMMUNICATIONS LIMITED", TraId = 7365, Prefix = "TZ"
            },
            new() { Id = Guid.NewGuid(), Name = "RAILSITE TELECOM LIMITED", TraId = 7521, Prefix = "H5", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "READING BOROUGH COUNCIL", TraId = 345, Prefix = "JN", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "REDCAR AND CLEVELAND BOROUGH COUNCIL", TraId = 728, Prefix = "RU"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "REDCENTRIC COMMUNICATIONS LIMITED", TraId = 7247, Prefix = "YJ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "RHONDDA CYNON TAF COUNTY BOROUGH COUNCIL",
                TraId = 6940,
                Prefix = "KN"
            },
            new() { Id = Guid.NewGuid(), Name = "Riverside Energy Park Ltd", TraId = 7569, Prefix = "P8", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "RM PROPERTY AND FACILITIES SOLUTIONS LIMITED",
                TraId = 7221,
                Prefix = "SW"
            },
            new() { Id = Guid.NewGuid(), Name = "ROCHDALE BOROUGH COUNCIL", TraId = 4225, Prefix = "KQ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ROLTA UK LIMITED", TraId = 7322, Prefix = "UR", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "ROTHERHAM METROPOLITAN BOROUGH COUNCIL", TraId = 4415, Prefix = "KR"
            },
            new() { Id = Guid.NewGuid(), Name = "ROYAL BOROUGH OF GREENWICH", TraId = 5330, Prefix = "FA", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ROYAL BOROUGH OF KENSINGTON AND CHELSEA",
                TraId = 5600,
                Prefix = "GD"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ROYAL BOROUGH OF KINGSTON UPON THAMES", TraId = 5630, Prefix = "GH"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ROYAL BOROUGH OF WINDSOR AND MAIDENHEAD", TraId = 355, Prefix = "JW"
            },
            new() { Id = Guid.NewGuid(), Name = "RUNFIBRE LTD", TraId = 7565, Prefix = "P2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "RUTLAND COUNTY COUNCIL", TraId = 2470, Prefix = "JP", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SALFORD CITY COUNCIL", TraId = 4230, Prefix = "KS", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "SANDWELL METROPOLITAN BOROUGH COUNCIL", TraId = 4620, Prefix = "KU"
            },
            new() { Id = Guid.NewGuid(), Name = "SCOTLAND GAS NETWORKS PLC", TraId = 7273, Prefix = "XZ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SCOTTISH WATER LIMITED", TraId = 9137, Prefix = "ZN", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "SCOTTISHPOWER ENERGY RETAIL LIMITED", TraId = 7019, Prefix = "KY"
            },
            new() { Id = Guid.NewGuid(), Name = "SECURE WEB SERVICES LIMITED", TraId = 7514, Prefix = "G6", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "SEFTON METROPOLITAN BOROUGH COUNCIL", TraId = 4320, Prefix = "LA"
            },
            new() { Id = Guid.NewGuid(), Name = "SERVERHOUSE LTD", TraId = 7575, Prefix = "R6", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SES WATER LIMITED", TraId = 9118, Prefix = "DZ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "SEVERN TRENT SERVICES OPERATIONS UK LIMITED",
                TraId = 7319,
                Prefix = "UN"
            },
            new() { Id = Guid.NewGuid(), Name = "SEVERN TRENT WATER LIMITED", TraId = 9103, Prefix = "LB", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SHEFFIELD CITY COUNCIL", TraId = 4420, Prefix = "LC", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SHROPSHIRE COUNCIL", TraId = 3245, Prefix = "UJ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "SKY TELECOMMUNICATIONS SERVICES LIMITED",
                TraId = 7225,
                Prefix = "ZG"
            },
            new() { Id = Guid.NewGuid(), Name = "SLOUGH BOROUGH COUNCIL", TraId = 350, Prefix = "JQ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SMARTFIBRE BROADBAND LIMITED", TraId = 7578, Prefix = "R9", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "SOLIHULL METROPOLITAN BOROUGH COUNCIL", TraId = 4625, Prefix = "LF"
            },
            new() { Id = Guid.NewGuid(), Name = "SOMERSET COUNCIL", TraId = 3300, Prefix = "LG", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SOUTH EAST WATER LIMITED", TraId = 9117, Prefix = "EB", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "SOUTH EASTERN POWER NETWORKS PLC", TraId = 7004, Prefix = "KZ"
            },
            new() { Id = Guid.NewGuid(), Name = "SOUTH GLOUCESTERSHIRE COUNCIL", TraId = 119, Prefix = "RZ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SOUTH STAFFORDSHIRE WATER PLC", TraId = 9129, Prefix = "LJ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SOUTH TYNESIDE COUNCIL", TraId = 4520, Prefix = "LK", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SOUTH WEST WATER LIMITED", TraId = 9105, Prefix = "LM", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "SOUTH WEST WATER LIMITED (Formerly BRISTOL WATER PLC)",
                TraId = 9111,
                Prefix = "AY"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "SOUTH YORKSHIRE PASSENGER TRANSPORT EXECUTIVE",
                TraId = 9236,
                Prefix = "XN"
            },
            new() { Id = Guid.NewGuid(), Name = "SOUTHAMPTON CITY COUNCIL", TraId = 1780, Prefix = "FU", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SOUTHEND-ON-SEA CITY COUNCIL", TraId = 1590, Prefix = "JR", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "SOUTHERN ELECTRIC POWER DISTRIBUTION PLC",
                TraId = 7002,
                Prefix = "LP"
            },
            new() { Id = Guid.NewGuid(), Name = "SOUTHERN GAS NETWORKS PLC", TraId = 7270, Prefix = "XW", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SOUTHERN WATER LIMITED", TraId = 9104, Prefix = "LQ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SP MANWEB PLC", TraId = 7008, Prefix = "GY", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SPRING FIBRE LIMITED", TraId = 7529, Prefix = "J5", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SQUIRE ENERGY LIMITED", TraId = 7383, Prefix = "C5", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "SQUIRE ENERGY METERING LIMITED", TraId = 7571, Prefix = "R2"
            },
            new() { Id = Guid.NewGuid(), Name = "ST. HELENS COUNCIL", TraId = 4315, Prefix = "LT", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "STAFFORDSHIRE COUNTY COUNCIL", TraId = 3450, Prefix = "LV", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "STIX INTERNET LIMITED", TraId = 7530, Prefix = "J6", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "STOCKPORT METROPOLITAN BOROUGH COUNCIL", TraId = 4235, Prefix = "LX"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "STOCKTON-ON-TEES BOROUGH COUNCIL", TraId = 738, Prefix = "SB"
            },
            new() { Id = Guid.NewGuid(), Name = "STOKE-ON-TRENT CITY COUNCIL", TraId = 3455, Prefix = "GF", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SUBTOPIA LIMITED", TraId = 7344, Prefix = "ZD", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "SUEZ ADVANCED SOLUTIONS UL LTD", TraId = 7558, Prefix = "N3"
            },
            new() { Id = Guid.NewGuid(), Name = "SUFFOLK COUNTY COUNCIL", TraId = 3500, Prefix = "LY", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SUNDERLAND CITY COUNCIL", TraId = 4525, Prefix = "LZ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SURREY COUNTY COUNCIL", TraId = 3600, Prefix = "MA", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SWINDON BOROUGH COUNCIL", TraId = 3935, Prefix = "SN", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "SWISH FIBRE LIMITED", TraId = 7384, Prefix = "C6", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "TALKTALK COMMUNICATIONS LIMITED", TraId = 7299, Prefix = "VD"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "TAMESIDE METROPOLITAN BOROUGH COUNCIL", TraId = 4240, Prefix = "MF"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "TATA COMMUNICATIONS (UK) LIMITED", TraId = 7248, Prefix = "YL"
            },
            new() { Id = Guid.NewGuid(), Name = "TELCOM INFRASTRUCTURE LIMITED", TraId = 7390, Prefix = "D5", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS (HERTS) LIMITED PARTNERSHIP",
                TraId = 7152,
                Prefix = "MQ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS (NORTHANTS) LIMITED PARTNERSHIP",
                TraId = 7146,
                Prefix = "MJ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name =
                    "TELECENTIAL COMMUNICATIONS (NORTHANTS) LIMITED PARTNERSHIP (Formerly TELECENTIAL COMMS NORTHAMPTON)",
                TraId = 7147,
                Prefix = "MK"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS (WARWICKSHIRE) LIMITED PARTNERSHIP",
                TraId = 7149,
                Prefix = "MM"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS LIMITED PARTNERSHIP",
                TraId = 7148,
                Prefix = "ML"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS LIMITED PARTNERSHIP (Formerly TELECENTIAL COMMS RUGBY)",
                TraId = 7150,
                Prefix = "MN"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS LIMITED PARTNERSHIP (Formerly TELECENTIAL COMMS STRATFORD)",
                TraId = 7151,
                Prefix = "MP"
            },
            new() { Id = Guid.NewGuid(), Name = "TELEFONICA UK LIMITED", TraId = 7182, Prefix = "MG", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELEWEST COMMUNICATIONS (NORTH EAST) LIMITED",
                TraId = 7158,
                Prefix = "NH"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELEWEST COMMUNICATIONS (SOUTH EAST) LIMITED",
                TraId = 7159,
                Prefix = "NJ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELEWEST COMMUNICATIONS (SOUTH WEST) LIMITED",
                TraId = 7061,
                Prefix = "NC"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELEWEST COMMUNICATIONS (TYNESIDE) LIMITED",
                TraId = 7035,
                Prefix = "CY"
            },
            new() { Id = Guid.NewGuid(), Name = "TELEWEST LIMITED", TraId = 7237, Prefix = "ZX", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "TELFORD & WREKIN COUNCIL", TraId = 3240, Prefix = "JS", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "TELSTRA GLOBAL LIMITED", TraId = 7085, Prefix = "MS", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "THAMES WATER UTILITIES LIMITED", TraId = 9106, Prefix = "MU"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "THE ELECTRICITY NETWORK COMPANY LIMITED",
                TraId = 7560,
                Prefix = "N5"
            },
            new() { Id = Guid.NewGuid(), Name = "THE FIBRE GUYS LTD", TraId = 7520, Prefix = "H4", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "THE OIL & PIPELINES AGENCY", TraId = 7092, Prefix = "EZ", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "THE ROCHESTER BRIDGE TRUST", TraId = 7337, Prefix = "ZA", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "THURROCK COUNCIL", TraId = 1595, Prefix = "JT", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "THUS LIMITED", TraId = 7224, Prefix = "ZF", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "TIMICO PARTNER SERVICES LIMITED", TraId = 7275, Prefix = "WC"
            },
            new() { Id = Guid.NewGuid(), Name = "TOOB LIMITED", TraId = 7375, Prefix = "B5", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "TORBAY COUNCIL", TraId = 1165, Prefix = "SM", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "TORFAEN COUNTY BOROUGH COUNCIL", TraId = 6945, Prefix = "MZ"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "TRAFFORD METROPOLITAN BOROUGH COUNCIL", TraId = 4245, Prefix = "NB"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "TRANSPORT FOR GREATER MANCHESTER", TraId = 7215, Prefix = "QZ"
            },
            new() { Id = Guid.NewGuid(), Name = "TRANSPORT FOR LONDON (TFL)", TraId = 20, Prefix = "YG", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "TRANSPORT FOR WALES (TFW)", TraId = 7549, Prefix = "M2", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "TRUESPEED COMMUNICATIONS LTD", TraId = 7355, Prefix = "TP", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "UK BROADBAND LIMITED", TraId = 7341, Prefix = "PH", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "UK POWER DISTRIBUTION LIMITED", TraId = 7361, Prefix = "TV", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "UNITED UTILITIES WATER LIMITED", TraId = 9102, Prefix = "HZ"
            },
            new() { Id = Guid.NewGuid(), Name = "UPP (Corporation Limited)", TraId = 7526, Prefix = "H9", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "UTILITY GRID INSTALLATIONS LIMITED", TraId = 7265, Prefix = "XK"
            },
            new() { Id = Guid.NewGuid(), Name = "VALE OF GLAMORGAN COUNCIL", TraId = 6950, Prefix = "NL", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "VATTENFALL NETWORKS LIMITED", TraId = 7394, Prefix = "D9", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "VATTENFALL WIND POWER LTD", TraId = 7561, Prefix = "N6", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "VEOLIA WATER OUTSOURCING LIMITED", TraId = 7314, Prefix = "VY"
            },
            new() { Id = Guid.NewGuid(), Name = "VERIZON UK LIMITED", TraId = 7086, Prefix = "PQ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "VERIZON UK LIMITED (Formerly WORLDCOM INTERNATIONAL LTD)",
                TraId = 7081,
                Prefix = "HE"
            },
            new() { Id = Guid.NewGuid(), Name = "VIRGIN MEDIA LIMITED", TraId = 7160, Prefix = "NK", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "VIRGIN MEDIA PCHC II LIMITED", TraId = 7110, Prefix = "CD", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "VIRGIN MEDIA PCHC II LIMITED (Formerly CABLETEL SOUTH WALES CARDIFF)",
                TraId = 7109,
                Prefix = "CB"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "VIRGIN MEDIA WHOLESALE LIMITED", TraId = 7240, Prefix = "YA"
            },
            new() { Id = Guid.NewGuid(), Name = "VISPA LIMITED", TraId = 7516, Prefix = "G7", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "VITAL ENERGI UTILITIES LIMITED", TraId = 7209, Prefix = "RX"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "VODAFONE ENTERPRISE EUROPE (UK) LIMITED", TraId = 70, Prefix = "QL"
            },
            new() { Id = Guid.NewGuid(), Name = "VODAFONE LIMITED", TraId = 7076, Prefix = "NX", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "VONEUS LIMITED", TraId = 7507, Prefix = "F6", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "VORBOSS LIMITED", TraId = 7389, Prefix = "D3", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "VX FIBER LIMITED", TraId = 7395, Prefix = "E2", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "WALES & WEST UTILITIES LIMITED", TraId = 7272, Prefix = "XY"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "WALSALL METROPOLITAN BOROUGH COUNCIL", TraId = 4630, Prefix = "NZ"
            },
            new() { Id = Guid.NewGuid(), Name = "WARRINGTON BOROUGH COUNCIL", TraId = 655, Prefix = "JU", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WARWICKSHIRE COUNTY COUNCIL", TraId = 3700, Prefix = "PC", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WELINK COMMUNICATIONS UK LTD", TraId = 7532, Prefix = "J8", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WELSH GOVERNMENT", TraId = 16, Prefix = "PD", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "WELSH HIGHLAND RAILWAY CONSTRUCTION LIMITED",
                TraId = 7310,
                Prefix = "VR"
            },
            new() { Id = Guid.NewGuid(), Name = "WESSEX INTERNET", TraId = 7525, Prefix = "J4", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WESSEX WATER LIMITED", TraId = 9108, Prefix = "PG", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WEST BERKSHIRE COUNCIL", TraId = 340, Prefix = "JV", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WEST NORTHAMPTONSHIRE COUNCIL", TraId = 2845, Prefix = "F9", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WEST SUSSEX COUNTY COUNCIL", TraId = 3800, Prefix = "PJ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "WESTMORLAND AND FURNESS COUNCIL", TraId = 935, Prefix = "P4"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "WESTNETWORKS INNOVATIONS LIMITED", TraId = 7519, Prefix = "H3"
            },
            new() { Id = Guid.NewGuid(), Name = "WHYFIBRE LIMITED", TraId = 7572, Prefix = "R3", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WIFINITY LIMITED", TraId = 7570, Prefix = "P9", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "WIGAN METROPOLITAN BOROUGH COUNCIL", TraId = 4250, Prefix = "PL"
            },
            new() { Id = Guid.NewGuid(), Name = "WIGHTFIBRE LIMITED", TraId = 7251, Prefix = "YP", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WILDANET LIMITED", TraId = 7515, Prefix = "G8", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WILDCARD UK LIMITED", TraId = 7363, Prefix = "TX", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WILTSHIRE COUNCIL", TraId = 3940, Prefix = "UK", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WIRRAL BOROUGH COUNCIL", TraId = 4325, Prefix = "PN", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WOKINGHAM BOROUGH COUNCIL", TraId = 360, Prefix = "JX", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "WORCESTERSHIRE COUNTY COUNCIL", TraId = 1855, Prefix = "JZ", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "WREXHAM COUNTY BOROUGH COUNCIL", TraId = 6955, Prefix = "PR"
            },
            new() { Id = Guid.NewGuid(), Name = "WREXHAM WATER LIMITED", TraId = 9135, Prefix = "PS", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "X-FIBRE LTD", TraId = 7576, Prefix = "R7", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "YESFIBRE LTD", TraId = 7546, Prefix = "L6", IsAdmin = false },
            new()
            {
                Id = Guid.NewGuid(), Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED", TraId = 7027, Prefix = "PU"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS BRADFORD)",
                TraId = 7068,
                Prefix = "PV"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS DONCASTER)",
                TraId = 7041,
                Prefix = "PW"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS HALIFAX)",
                TraId = 7170,
                Prefix = "PX"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS SHEFFIELD)",
                TraId = 7057,
                Prefix = "PY"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS WAKEFIELD)",
                TraId = 7063,
                Prefix = "PZ"
            },
            new() { Id = Guid.NewGuid(), Name = "YORKSHIRE WATER LIMITED", TraId = 9109, Prefix = "QB", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "YOUR COMMUNICATIONS LIMITED", TraId = 7083, Prefix = "JH", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ZAYO GROUP UK LIMITED", TraId = 7235, Prefix = "ZV", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ZOOM INTERNET LIMITED", TraId = 7574, Prefix = "R5", IsAdmin = false },
            new() { Id = Guid.NewGuid(), Name = "ZZOOMM PLC", TraId = 7379, Prefix = "B9" }
        };

    #endregion
}