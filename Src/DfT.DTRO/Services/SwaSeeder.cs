using System;
using System.Collections.Generic;
using System.Linq;
using DfT.DTRO.DAL;
using DfT.DTRO.Models.DataBase;

namespace DfT.DTRO.Services;

/// <summary>
/// SWA class seeder.
/// </summary>
public class SwaSeeder : ISwaSeeder
{
    private readonly DtroContext _dtroContext;

    public SwaSeeder(DtroContext dtroContext)
    {
        _dtroContext = dtroContext;
    }

    /// <summary>
    /// Seed SWA codes implementation
    /// </summary>
    public void Seed()
    {
        if (!_dtroContext.Database.CanConnect())
        {
            return;
        }

        if (_dtroContext.SwaCodes.Any())
        {
            return;
        }

        IEnumerable<SwaCode> swaCodes = InitiateSwaCodes();
        _dtroContext.SwaCodes.AddRange(swaCodes);
        _dtroContext.SaveChanges();
    }

    #region SwaCodes

    private IEnumerable<SwaCode> InitiateSwaCodes() =>
        new List<SwaCode>
        {
            new() { Id = Guid.NewGuid(), Name = "(AQ) LIMITED", Code = 7334, Prefix = "SK" },
            new() { Id = Guid.NewGuid(), Name = "1310 Ltd", Code = 7550, Prefix = "M3" },
            new()
            {
                Id = Guid.NewGuid(), Name = "ADVANCED ELECTRICITY NETWORKS LIMITED", Code = 7583, Prefix = "S6"
            },
            new() { Id = Guid.NewGuid(), Name = "Affinity Systems Limited", Code = 7544, Prefix = "L4" },
            new() { Id = Guid.NewGuid(), Name = "AFFINITY WATER EAST LIMITED", Code = 9132, Prefix = "MT" },
            new() { Id = Guid.NewGuid(), Name = "AFFINITY WATER LIMITED", Code = 9133, Prefix = "MX" },
            new()
            {
                Id = Guid.NewGuid(), Name = "AFFINITY WATER SOUTHEAST LIMITED", Code = 9121, Prefix = "EV"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "AIRBAND COMMUNITY INTERNET LIMITED", Code = 7372, Prefix = "A8"
            },
            new() { Id = Guid.NewGuid(), Name = "AIRWAVE SOLUTIONS LIMITED", Code = 7297, Prefix = "VB" },
            new() { Id = Guid.NewGuid(), Name = "AJ TECHNOLOGIES LIMITED", Code = 7545, Prefix = "L5" },
            new() { Id = Guid.NewGuid(), Name = "ALBION WATER LIMITED", Code = 9139, Prefix = "UZ" },
            new() { Id = Guid.NewGuid(), Name = "ALLPOINTS FIBRE LIMITED", Code = 7552, Prefix = "M5" },
            new()
            {
                Id = Guid.NewGuid(), Name = "ALLPOINTS FIBRE NETWORKS LIMITED", Code = 7528, Prefix = "J3"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ANGLIA CABLE COMMUNICATIONS LIMITED", Code = 7026, Prefix = "AC"
            },
            new() { Id = Guid.NewGuid(), Name = "ARELION UK LIMITED", Code = 7230, Prefix = "ZM" },
            new() { Id = Guid.NewGuid(), Name = "ARQIVA LIMITED", Code = 7354, Prefix = "TN" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ARQIVA LIMITED (Formerly NATIONAL TRANSCOMMUNICATIONS LTD)",
                Code = 7217,
                Prefix = "SP"
            },
            new() { Id = Guid.NewGuid(), Name = "AWG GROUP LIMITED", Code = 9100, Prefix = "AD" },
            new() { Id = Guid.NewGuid(), Name = "AXIONE UK LIMITED", Code = 7541, Prefix = "K9" },
            new() { Id = Guid.NewGuid(), Name = "BAA INTERNATIONAL LIMITED", Code = 17, Prefix = "AF" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "BAI COMMUNICATIONS INFRASTRUCTURE LIMITED",
                Code = 7563,
                Prefix = "N9"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "BARNSLEY METROPOLITAN BOROUGH COUNCIL", Code = 4405, Prefix = "AJ"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "BATH AND NORTH EAST SOMERSET COUNCIL", Code = 114, Prefix = "QD"
            },
            new() { Id = Guid.NewGuid(), Name = "BAZALGETTE TUNNEL LIMITED", Code = 7345, Prefix = "ZE" },
            new() { Id = Guid.NewGuid(), Name = "BEDFORD BOROUGH COUNCIL", Code = 235, Prefix = "UB" },
            new() { Id = Guid.NewGuid(), Name = "BIRMINGHAM CABLE LIMITED", Code = 7028, Prefix = "AR" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "BIRMINGHAM CABLE LIMITED (Formerly BIRMINGHAM CABLE WYTHALL)",
                Code = 7198,
                Prefix = "QE"
            },
            new() { Id = Guid.NewGuid(), Name = "BIRMINGHAM CITY COUNCIL", Code = 4605, Prefix = "AQ" },
            new()
            {
                Id = Guid.NewGuid(), Name = "BLACKBURN WITH DARWEN BOROUGH COUNCIL", Code = 2372, Prefix = "AE"
            },
            new() { Id = Guid.NewGuid(), Name = "BLACKPOOL BOROUGH COUNCIL", Code = 2373, Prefix = "CU" },
            new()
            {
                Id = Guid.NewGuid(), Name = "BLAENAU GWENT COUNTY BOROUGH COUNCIL", Code = 6910, Prefix = "AS"
            },
            new() { Id = Guid.NewGuid(), Name = "Boldyn Networks Limited", Code = 7547, Prefix = "L7" },
            new() { Id = Guid.NewGuid(), Name = "BOLT PRO TEM LIMITED", Code = 7346, Prefix = "XD" },
            new()
            {
                Id = Guid.NewGuid(), Name = "BOLTON METROPOLITAN BOROUGH COUNCIL", Code = 4205, Prefix = "AT"
            },
            new() { Id = Guid.NewGuid(), Name = "BOURNEMOUTH WATER LIMITED", Code = 9110, Prefix = "AU" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "BOURNEMOUTH, CHRISTCHURCH AND POOLE COUNCIL",
                Code = 1260,
                Prefix = "B2"
            },
            new() { Id = Guid.NewGuid(), Name = "BOX BROADBAND LIMITED", Code = 7373, Prefix = "A9" },
            new() { Id = Guid.NewGuid(), Name = "BRACKNELL FOREST COUNCIL", Code = 335, Prefix = "DW" },
            new()
            {
                Id = Guid.NewGuid(), Name = "BRIDGEND COUNTY BOROUGH COUNCIL", Code = 6915, Prefix = "AX"
            },
            new() { Id = Guid.NewGuid(), Name = "BRIGHTON & HOVE CITY COUNCIL", Code = 1445, Prefix = "DU" },
            new() { Id = Guid.NewGuid(), Name = "BRISTOL CITY COUNCIL", Code = 116, Prefix = "QF" },
            new()
            {
                Id = Guid.NewGuid(), Name = "BRITISH PIPELINE AGENCY LIMITED", Code = 7089, Prefix = "BA"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "BRITISH TELECOMMUNICATIONS PUBLIC LIMITED COMPANY",
                Code = 30,
                Prefix = "BC"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "BROADBAND FOR THE RURAL NORTH LIMITED", Code = 7350, Prefix = "TG"
            },
            new() { Id = Guid.NewGuid(), Name = "BROADWAY PARTNERS LIMITED", Code = 7392, Prefix = "D7" },
            new() { Id = Guid.NewGuid(), Name = "BRSK LIMITED", Code = 7527, Prefix = "J2" },
            new() { Id = Guid.NewGuid(), Name = "BRYN BLAEN WIND FARM LIMITED", Code = 7360, Prefix = "TU" },
            new() { Id = Guid.NewGuid(), Name = "BUCKINGHAMSHIRE COUNCIL", Code = 440, Prefix = "D4" },
            new()
            {
                Id = Guid.NewGuid(), Name = "BURY METROPOLITAN BOROUGH COUNCIL", Code = 4210, Prefix = "BJ"
            },
            new() { Id = Guid.NewGuid(), Name = "CABLE LONDON LIMITED", Code = 7030, Prefix = "BL" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLE LONDON LIMITED (Formerly CABLE LONDON PLC ENFIELD)",
                Code = 7099,
                Prefix = "BM"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLE LONDON LIMITED (Formerly CABLE LONDON PLC HACKNEY)",
                Code = 7100,
                Prefix = "BN"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLE LONDON LIMITED (Formerly CABLE LONDON PLC HARINGEY)",
                Code = 7101,
                Prefix = "BP"
            },
            new() { Id = Guid.NewGuid(), Name = "CABLE ON DEMAND LIMITED", Code = 7113, Prefix = "CX" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLE ON DEMAND LIMITED (Formerly COMCAST TEESSIDE DARLINGTON)",
                Code = 7112,
                Prefix = "CW"
            },
            new() { Id = Guid.NewGuid(), Name = "CABLECOM INVESTMENTS LIMITED", Code = 7173, Prefix = "BV" },
            new() { Id = Guid.NewGuid(), Name = "CABLETEL BEDFORDSHIRE", Code = 7032, Prefix = "BW" },
            new()
            {
                Id = Guid.NewGuid(), Name = "CABLETEL HERTS AND BEDS LIMITED", Code = 7108, Prefix = "CA"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLETEL HERTS AND BEDS LIMITED (Formerly CABLETEL CENTRAL HERTFORDSHIRE)",
                Code = 7106,
                Prefix = "BX"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CABLETEL HERTS AND BEDS LIMITED (Formerly CABLETEL HERTFORDSHIRE)",
                Code = 7107,
                Prefix = "BY"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "CABLETEL SURREY AND HAMPSHIRE LIMITED", Code = 7111, Prefix = "CE"
            },
            new() { Id = Guid.NewGuid(), Name = "CADENT GAS LIMITED", Code = 10, Prefix = "AZ" },
            new()
            {
                Id = Guid.NewGuid(), Name = "CAERPHILLY COUNTY BOROUGH COUNCIL", Code = 6920, Prefix = "CG"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CALDERDALE METROPOLITAN BOROUGH COUNCIL",
                Code = 4710,
                Prefix = "CH"
            },
            new() { Id = Guid.NewGuid(), Name = "CALL FLOW SOLUTIONS LTD", Code = 7339, Prefix = "UY" },
            new() { Id = Guid.NewGuid(), Name = "CAMBRIDGE FIBRE NETWORKS LTD", Code = 7371, Prefix = "A7" },
            new() { Id = Guid.NewGuid(), Name = "CAMBRIDGE WATER PLC", Code = 9113, Prefix = "CK" },
            new() { Id = Guid.NewGuid(), Name = "CAMBRIDGESHIRE COUNTY COUNCIL", Code = 535, Prefix = "CL" },
            new()
            {
                Id = Guid.NewGuid(), Name = "CARMARTHENSHIRE COUNTY COUNCIL", Code = 6825, Prefix = "QQ"
            },
            new() { Id = Guid.NewGuid(), Name = "CELLNEX (ON TOWER UK LTD)", Code = 7513, Prefix = "G5" },
            new() { Id = Guid.NewGuid(), Name = "CENTRAL BEDFORDSHIRE COUNCIL", Code = 240, Prefix = "UC" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CENTRO (WEST MIDLANDS PASSENGER TRANSPORT EXECUTIVE)",
                Code = 9234,
                Prefix = "WB"
            },
            new() { Id = Guid.NewGuid(), Name = "CEREDIGION COUNTY COUNCIL", Code = 6820, Prefix = "QP" },
            new() { Id = Guid.NewGuid(), Name = "CHESHIRE EAST COUNCIL", Code = 660, Prefix = "UD" },
            new()
            {
                Id = Guid.NewGuid(), Name = "CHESHIRE WEST AND CHESTER COUNCIL", Code = 665, Prefix = "UE"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CHOLDERTON AND DISTRICT WATER COMPANY LIMITED",
                Code = 9115,
                Prefix = "CQ"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "CITY AND COUNTY OF SWANSEA COUNCIL", Code = 6855, Prefix = "MD"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CITY OF BRADFORD METROPOLITAN DISTRICT COUNCIL",
                Code = 4705,
                Prefix = "AV"
            },
            new() { Id = Guid.NewGuid(), Name = "CITY OF CARDIFF COUNCIL", Code = 6815, Prefix = "QN" },
            new() { Id = Guid.NewGuid(), Name = "CITY OF DONCASTER COUNCIL", Code = 4410, Prefix = "DQ" },
            new() { Id = Guid.NewGuid(), Name = "CITY OF LONDON CORPORATION", Code = 5030, Prefix = "CR" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CITY OF WAKEFIELD METROPOLITAN DISTRICT COUNCIL",
                Code = 4725,
                Prefix = "NY"
            },
            new() { Id = Guid.NewGuid(), Name = "CITY OF WESTMINSTER", Code = 5990, Prefix = "CT" },
            new() { Id = Guid.NewGuid(), Name = "CITY OF WOLVERHAMPTON COUNCIL", Code = 4635, Prefix = "PP" },
            new() { Id = Guid.NewGuid(), Name = "CITY OF YORK COUNCIL", Code = 2741, Prefix = "SH" },
            new()
            {
                Id = Guid.NewGuid(), Name = "CITYFIBRE METRO NETWORKS LIMITED", Code = 7330, Prefix = "KG"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "CITYLINK TELECOMMUNICATIONS LIMITED", Code = 7261, Prefix = "XB"
            },
            new() { Id = Guid.NewGuid(), Name = "CLOUD HQ DIDCOT FIBRE GP LTD", Code = 7508, Prefix = "F7" },
            new() { Id = Guid.NewGuid(), Name = "COLT TECHNOLOGY SERVICES", Code = 7075, Prefix = "CS" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "COMMUNICATIONS INFRASTRUCTURE NETWORKS LIMITED",
                Code = 7358,
                Prefix = "TS"
            },
            new() { Id = Guid.NewGuid(), Name = "COMMUNITY FIBRE LIMITED", Code = 7364, Prefix = "TY" },
            new() { Id = Guid.NewGuid(), Name = "CONCEPT SOLUTIONS PEOPLE LTD", Code = 7335, Prefix = "SR" },
            new() { Id = Guid.NewGuid(), Name = "CONNEXIN LIMITED", Code = 7531, Prefix = "J7" },
            new() { Id = Guid.NewGuid(), Name = "CONWY COUNTY BOROUGH COUNCIL", Code = 6905, Prefix = "AA" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CORANGE PERSONAL COMMUNICATIONS SERVICES LIMITED (Formerly HUTCHINSON MICROTEL)",
                Code = 7077,
                Prefix = "FS"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "CORNERSTONE TELECOMMUNICATIONS INFRASTRUCTURE LIMITED",
                Code = 7567,
                Prefix = "P6"
            },
            new() { Id = Guid.NewGuid(), Name = "CORNWALL COUNCIL", Code = 840, Prefix = "UF" },
            new() { Id = Guid.NewGuid(), Name = "COUNCIL OF THE ISLES OF SCILLY", Code = 835, Prefix = "HJ" },
            new() { Id = Guid.NewGuid(), Name = "COUNTY BROADBAND LTD", Code = 7369, Prefix = "A5" },
            new() { Id = Guid.NewGuid(), Name = "COVENTRY CITY COUNCIL", Code = 4610, Prefix = "DB" },
            new() { Id = Guid.NewGuid(), Name = "CROSS RAIL LTD", Code = 7318, Prefix = "UM" },
            new() { Id = Guid.NewGuid(), Name = "CROSSKIT LIMITED", Code = 7573, Prefix = "R4" },
            new() { Id = Guid.NewGuid(), Name = "CUMBERLAND COUNCIL", Code = 940, Prefix = "P5" },
            new() { Id = Guid.NewGuid(), Name = "DARLINGTON BOROUGH COUNCIL", Code = 1350, Prefix = "HF" },
            new() { Id = Guid.NewGuid(), Name = "DENBIGHSHIRE COUNTY COUNCIL", Code = 6830, Prefix = "QR" },
            new() { Id = Guid.NewGuid(), Name = "DERBY CITY COUNCIL", Code = 1055, Prefix = "SZ" },
            new() { Id = Guid.NewGuid(), Name = "DERBYSHIRE COUNTY COUNCIL", Code = 1050, Prefix = "DF" },
            new() { Id = Guid.NewGuid(), Name = "DEVON COUNTY COUNCIL", Code = 1155, Prefix = "DG" },
            new() { Id = Guid.NewGuid(), Name = "DFT ROAD STATISTICS DIVISION", Code = 7188, Prefix = "QU" },
            new() { Id = Guid.NewGuid(), Name = "DIAMOND CABLE (MANSFIELD)", Code = 7116, Prefix = "DL" },
            new()
            {
                Id = Guid.NewGuid(), Name = "DIAMOND CABLE COMMUNICATIONS LIMITED", Code = 7189, Prefix = "QS"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE DARLINGTON)",
                Code = 7114,
                Prefix = "DJ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE GRANTHAM)",
                Code = 7040,
                Prefix = "DH"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE LINCOLN)",
                Code = 7115,
                Prefix = "DK"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE MELTON MOWBRAY)",
                Code = 7117,
                Prefix = "DM"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "DIAMOND CABLE COMMUNICATIONS LIMITED (Formerly DIAMOND CABLE NEWARK)",
                Code = 7118,
                Prefix = "DN"
            },
            new() { Id = Guid.NewGuid(), Name = "DIGITAL INFRASTRUCTURE LTD", Code = 7518, Prefix = "H2" },
            new() { Id = Guid.NewGuid(), Name = "DORSET COUNCIL", Code = 1265, Prefix = "B3" },
            new() { Id = Guid.NewGuid(), Name = "DUDGEON OFFSHORE WIND LIMITED", Code = 7333, Prefix = "RB" },
            new()
            {
                Id = Guid.NewGuid(), Name = "DUDLEY METROPOLITAN BOROUGH COUNCIL", Code = 4615, Prefix = "DS"
            },
            new() { Id = Guid.NewGuid(), Name = "DURHAM COUNTY COUNCIL", Code = 1355, Prefix = "UG" },
            new()
            {
                Id = Guid.NewGuid(), Name = "DWR CYMRU CYFYNGEDIG (WELSH WATER)", Code = 9107, Prefix = "PE"
            },
            new() { Id = Guid.NewGuid(), Name = "E S PIPELINES LTD", Code = 7260, Prefix = "ZY" },
            new() { Id = Guid.NewGuid(), Name = "EAST ANGLIA THREE LIMITED", Code = 7581, Prefix = "S4" },
            new()
            {
                Id = Guid.NewGuid(), Name = "EAST RIDING OF YORKSHIRE COUNCIL", Code = 2001, Prefix = "QV"
            },
            new() { Id = Guid.NewGuid(), Name = "EAST SUSSEX COUNTY COUNCIL", Code = 1440, Prefix = "EA" },
            new() { Id = Guid.NewGuid(), Name = "EASTERN POWER NETWORKS PLC", Code = 7010, Prefix = "EC" },
            new()
            {
                Id = Guid.NewGuid(), Name = "ECLIPSE POWER NETWORKS LIMITED", Code = 7357, Prefix = "TR"
            },
            new() { Id = Guid.NewGuid(), Name = "EDF ENERGY CUSTOMERS LIMITED", Code = 7009, Prefix = "GU" },
            new() { Id = Guid.NewGuid(), Name = "EDF ENERGY RENEWABLES LTD", Code = 7557, Prefix = "N2" },
            new() { Id = Guid.NewGuid(), Name = "EE LIMITED", Code = 7250, Prefix = "YN" },
            new() { Id = Guid.NewGuid(), Name = "EIRCOM (UK) LIMITED", Code = 7243, Prefix = "YD" },
            new() { Id = Guid.NewGuid(), Name = "EIRGRID UK HOLDINGS LIMITED", Code = 7325, Prefix = "UU" },
            new() { Id = Guid.NewGuid(), Name = "ELECLINK LIMITED", Code = 7338, Prefix = "ZB" },
            new()
            {
                Id = Guid.NewGuid(), Name = "ELECTRICITY NORTH WEST LIMITED", Code = 7005, Prefix = "JG"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ENERGIS COMMUNICATIONS LIMITED", Code = 7080, Prefix = "EL"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ENERGY ASSETS NETWORKS LIMITED", Code = 7359, Prefix = "TT"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ENERGY ASSETS PIPELINES LIMITED", Code = 7348, Prefix = "TD"
            },
            new() { Id = Guid.NewGuid(), Name = "ENVIRONMENT AGENCY", Code = 7220, Prefix = "SV" },
            new() { Id = Guid.NewGuid(), Name = "ESP CONNECTIONS LIMITED", Code = 7242, Prefix = "YC" },
            new() { Id = Guid.NewGuid(), Name = "ESP ELECTRICITY LIMITED", Code = 7309, Prefix = "VQ" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ESP ELECTRICITY LIMITED (Formerly LAING ENERGY LTD)",
                Code = 7268,
                Prefix = "XU"
            },
            new() { Id = Guid.NewGuid(), Name = "ESP NETWORKS LIMITED", Code = 7255, Prefix = "YU" },
            new() { Id = Guid.NewGuid(), Name = "ESP PIPELINES LIMITED", Code = 7256, Prefix = "YV" },
            new() { Id = Guid.NewGuid(), Name = "ESP WATER LIMITED", Code = 7564, Prefix = "N8" },
            new()
            {
                Id = Guid.NewGuid(), Name = "ESSEX AND SUFFOLK WATER LIMITED", Code = 9120, Prefix = "EN"
            },
            new() { Id = Guid.NewGuid(), Name = "ESSEX COUNTY COUNCIL", Code = 1585, Prefix = "EP" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ESSO EXPLORATION AND PRODUCTION UK LIMITED",
                Code = 7091,
                Prefix = "EQ"
            },
            new() { Id = Guid.NewGuid(), Name = "EUNETWORKS GROUP LIMITED", Code = 7307, Prefix = "VN" },
            new() { Id = Guid.NewGuid(), Name = "E-VOLVE SOLUTIONS LTD", Code = 7579, Prefix = "S2" },
            new() { Id = Guid.NewGuid(), Name = "EXASCALE LIMITED", Code = 7503, Prefix = "F2" },
            new() { Id = Guid.NewGuid(), Name = "EXPONENTIAL-E LIMITED", Code = 7540, Prefix = "K8" },
            new() { Id = Guid.NewGuid(), Name = "F & W NETWORKS LTD", Code = 7386, Prefix = "C8" },
            new() { Id = Guid.NewGuid(), Name = "FACTCO", Code = 7511, Prefix = "G4" },
            new() { Id = Guid.NewGuid(), Name = "FIBERNET UK LIMITED", Code = 7223, Prefix = "SY" },
            new() { Id = Guid.NewGuid(), Name = "FIBRE ASSETS LTD", Code = 7510, Prefix = "G3" },
            new() { Id = Guid.NewGuid(), Name = "FIBRENATION LIMITED", Code = 7554, Prefix = "M7" },
            new() { Id = Guid.NewGuid(), Name = "FIBRESPEED LIMITED", Code = 7305, Prefix = "VL" },
            new() { Id = Guid.NewGuid(), Name = "FIBREWAVE NETWORKS LIMITED", Code = 7332, Prefix = "LH" },
            new() { Id = Guid.NewGuid(), Name = "FIBRUS NETWORKS GB LTD", Code = 7580, Prefix = "S3" },
            new() { Id = Guid.NewGuid(), Name = "FIBRUS NETWORKS LIMITED", Code = 7537, Prefix = "K6" },
            new() { Id = Guid.NewGuid(), Name = "FLINTSHIRE COUNTY COUNCIL", Code = 6835, Prefix = "QX" },
            new() { Id = Guid.NewGuid(), Name = "FREEDOM FIBRE LIMITED", Code = 7539, Prefix = "K5" },
            new() { Id = Guid.NewGuid(), Name = "FUJITSU SERVICES LIMITED", Code = 7328, Prefix = "KB" },
            new()
            {
                Id = Guid.NewGuid(), Name = "FULCRUM ELECTRICITY ASSETS LIMITED", Code = 7368, Prefix = "A4"
            },
            new() { Id = Guid.NewGuid(), Name = "FULCRUM PIPELINES LIMITED", Code = 7294, Prefix = "WY" },
            new() { Id = Guid.NewGuid(), Name = "FULL FIBRE LIMITED", Code = 7376, Prefix = "B6" },
            new()
            {
                Id = Guid.NewGuid(), Name = "G.NETWORK COMMUNICATIONS LIMITED", Code = 7362, Prefix = "TW"
            },
            new() { Id = Guid.NewGuid(), Name = "GAMMA TELECOM LTD", Code = 7241, Prefix = "YB" },
            new()
            {
                Id = Guid.NewGuid(), Name = "GATESHEAD METROPOLITAN BOROUGH COUNCIL", Code = 4505, Prefix = "EX"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "GC PAN EUROPEAN CROSSING UK LIMITED", Code = 7229, Prefix = "ZL"
            },
            new() { Id = Guid.NewGuid(), Name = "GDF SUEZ TEESSIDE LIMITED", Code = 7016, Prefix = "SE" },
            new() { Id = Guid.NewGuid(), Name = "GIGACLEAR LIMITED", Code = 7329, Prefix = "KA" },
            new() { Id = Guid.NewGuid(), Name = "GLIDE BUSINESS LIMITED", Code = 7343, Prefix = "ZC" },
            new()
            {
                Id = Guid.NewGuid(), Name = "GLOBAL ONE COMMUNICATIONS LIMITED", Code = 7084, Prefix = "LS"
            },
            new() { Id = Guid.NewGuid(), Name = "GLOBAL REACH NETWORKS LIMITED", Code = 7553, Prefix = "M6" },
            new()
            {
                Id = Guid.NewGuid(), Name = "GLOBAL TELECOMMUNICATIONS LIMITED", Code = 7228, Prefix = "ZK"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "GLOUCESTERSHIRE COUNTY COUNCIL", Code = 1600, Prefix = "EY"
            },
            new() { Id = Guid.NewGuid(), Name = "GOFIBRE HOLDINGS LIMITED", Code = 7551, Prefix = "M4" },
            new() { Id = Guid.NewGuid(), Name = "GRAIN COMMUNICATIONS LIMITED", Code = 7351, Prefix = "TJ" },
            new() { Id = Guid.NewGuid(), Name = "GRAYSHOTT GIGABIT LIMITED", Code = 7538, Prefix = "K7" },
            new()
            {
                Id = Guid.NewGuid(), Name = "GREENLINK INTERCONNECTOR LIMITED", Code = 7548, Prefix = "L8"
            },
            new() { Id = Guid.NewGuid(), Name = "GTC PIPELINES LIMITED", Code = 7231, Prefix = "ZP" },
            new() { Id = Guid.NewGuid(), Name = "GWYNEDD COUNCIL", Code = 6810, Prefix = "QM" },
            new() { Id = Guid.NewGuid(), Name = "HAFREN DYFRDWY CYFYNGEDIG", Code = 9138, Prefix = "ZU" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "HAFREN DYFRDWY CYFYNGEDIG (Formerly CHESTER WATERWORKS)",
                Code = 9114,
                Prefix = "CP"
            },
            new() { Id = Guid.NewGuid(), Name = "HALTON BOROUGH COUNCIL", Code = 650, Prefix = "AN" },
            new() { Id = Guid.NewGuid(), Name = "HAMPSHIRE COUNTY COUNCIL", Code = 1770, Prefix = "FF" },
            new() { Id = Guid.NewGuid(), Name = "HAMPSTEAD FIBRE LIMITED", Code = 7543, Prefix = "L3" },
            new()
            {
                Id = Guid.NewGuid(), Name = "HARLAXTON ENERGY NETWORKS LIMITED", Code = 7342, Prefix = "YZ"
            },
            new() { Id = Guid.NewGuid(), Name = "HARLAXTON GAS NETWORKS LTD", Code = 7374, Prefix = "B4" },
            new() { Id = Guid.NewGuid(), Name = "HARTLEPOOL BOROUGH COUNCIL", Code = 724, Prefix = "RA" },
            new() { Id = Guid.NewGuid(), Name = "HARTLEPOOL WATER", Code = 9122, Prefix = "FJ" },
            new() { Id = Guid.NewGuid(), Name = "HEN BEUDY SERVICES LIMITED", Code = 7396, Prefix = "E3" },
            new() { Id = Guid.NewGuid(), Name = "HENDY WIND FARM LIMITED", Code = 7577, Prefix = "R8" },
            new() { Id = Guid.NewGuid(), Name = "HEREFORDSHIRE COUNCIL", Code = 1850, Prefix = "FL" },
            new() { Id = Guid.NewGuid(), Name = "HERTFORDSHIRE COUNTY COUNCIL", Code = 1900, Prefix = "FM" },
            new()
            {
                Id = Guid.NewGuid(), Name = "HIBERNIA ATLANTIC (UK) LIMITED", Code = 7331, Prefix = "KK"
            },
            new() { Id = Guid.NewGuid(), Name = "HS2 LTD", Code = 7347, Prefix = "TA" },
            new() { Id = Guid.NewGuid(), Name = "HULL CITY COUNCIL", Code = 2004, Prefix = "RG" },
            new() { Id = Guid.NewGuid(), Name = "Hutchison 3G UK Limited", Code = 7264, Prefix = "XJ" },
            new() { Id = Guid.NewGuid(), Name = "HYPEROPTIC LTD", Code = 7349, Prefix = "TF" },
            new() { Id = Guid.NewGuid(), Name = "ICOSA WATER LTD", Code = 7380, Prefix = "C2" },
            new()
            {
                Id = Guid.NewGuid(), Name = "IN FOCUS PUBLIC NETWORKS LIMITED", Code = 60, Prefix = "RE"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "INDEPENDENT DISTRIBUTION CONNECTION SPECIALISTS LIMITED",
                Code = 7582,
                Prefix = "S5"
            },
            new() { Id = Guid.NewGuid(), Name = "INDEPENDENT PIPELINES LIMITED", Code = 7218, Prefix = "ST" },
            new()
            {
                Id = Guid.NewGuid(), Name = "INDEPENDENT POWER NETWORKS LIMITED", Code = 7281, Prefix = "WK"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "INDEPENDENT WATER NETWORKS LIMITED", Code = 7326, Prefix = "UV"
            },
            new() { Id = Guid.NewGuid(), Name = "INDIGO PIPELINES LIMITED", Code = 7313, Prefix = "VX" },
            new() { Id = Guid.NewGuid(), Name = "INDIGO POWER LTD", Code = 7555, Prefix = "M8" },
            new() { Id = Guid.NewGuid(), Name = "INTERNET CONNECTIONS LTD", Code = 7352, Prefix = "TK" },
            new() { Id = Guid.NewGuid(), Name = "INTERNETTY LTD", Code = 7391, Prefix = "D6" },
            new() { Id = Guid.NewGuid(), Name = "INTEROUTE NETWORKS LIMITED", Code = 7245, Prefix = "YF" },
            new() { Id = Guid.NewGuid(), Name = "IONICA LIMITED", Code = 7074, Prefix = "FT" },
            new()
            {
                Id = Guid.NewGuid(), Name = "ISLE OF ANGLESEY COUNTY COUNCIL", Code = 6805, Prefix = "QC"
            },
            new() { Id = Guid.NewGuid(), Name = "ISLE OF WIGHT COUNCIL", Code = 2114, Prefix = "RF" },
            new() { Id = Guid.NewGuid(), Name = "ITS TECHNOLOGY GROUP LIMITED", Code = 7370, Prefix = "A6" },
            new() { Id = Guid.NewGuid(), Name = "IX WIRELESS LIMITED", Code = 7377, Prefix = "B7" },
            new() { Id = Guid.NewGuid(), Name = "JOHN JONES LTD", Code = 7174, Prefix = "FW" },
            new() { Id = Guid.NewGuid(), Name = "JURASSIC FIBRE LIMITED", Code = 7387, Prefix = "C9" },
            new() { Id = Guid.NewGuid(), Name = "KCOM GROUP LIMITED", Code = 7073, Prefix = "GG" },
            new() { Id = Guid.NewGuid(), Name = "KCOM GROUP LIMITED (NATIONAL)", Code = 7082, Prefix = "MY" },
            new() { Id = Guid.NewGuid(), Name = "KENT COUNTY COUNCIL", Code = 2275, Prefix = "GE" },
            new() { Id = Guid.NewGuid(), Name = "KIRKLEES COUNCIL", Code = 4715, Prefix = "GJ" },
            new()
            {
                Id = Guid.NewGuid(), Name = "KNOWSLEY METROPOLITAN BOROUGH COUNCIL", Code = 4305, Prefix = "GK"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "KPN EURORINGS B.V. (Formerly KPN TELECOM UK LTD)",
                Code = 7227,
                Prefix = "ZJ"
            },
            new() { Id = Guid.NewGuid(), Name = "KPN EURORINGS BV", Code = 7267, Prefix = "XT" },
            new() { Id = Guid.NewGuid(), Name = "LANCASHIRE COUNTY COUNCIL", Code = 2371, Prefix = "GM" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "LANCASTER UNIVERSITY NETWORK SERVICES LIMITED",
                Code = 7277,
                Prefix = "WE"
            },
            new() { Id = Guid.NewGuid(), Name = "LAST MILE ELECTRICITY LIMITED", Code = 7385, Prefix = "C7" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "LAST MILE ELECTRICITY LIMITED (Formerly GLOBAL UTILITY CONNECTIONS)",
                Code = 7269,
                Prefix = "XV"
            },
            new() { Id = Guid.NewGuid(), Name = "LAST MILE GAS LIMITED", Code = 7311, Prefix = "VS" },
            new() { Id = Guid.NewGuid(), Name = "LAST MILE TELECOM LIMITED", Code = 7568, Prefix = "P7" },
            new() { Id = Guid.NewGuid(), Name = "LATOS DATA CENTRE LTD", Code = 7559, Prefix = "N4" },
            new() { Id = Guid.NewGuid(), Name = "LEEDS CITY COUNCIL", Code = 4720, Prefix = "GP" },
            new()
            {
                Id = Guid.NewGuid(), Name = "LEEP ELECTRICITY NETWORKS LIMITED", Code = 7506, Prefix = "F5"
            },
            new() { Id = Guid.NewGuid(), Name = "LEEP NETWORKS (WATER) LIMITED", Code = 7356, Prefix = "TQ" },
            new() { Id = Guid.NewGuid(), Name = "LEICESTER CITY COUNCIL", Code = 2465, Prefix = "EW" },
            new() { Id = Guid.NewGuid(), Name = "LEICESTERSHIRE COUNTY COUNCIL", Code = 2460, Prefix = "GQ" },
            new()
            {
                Id = Guid.NewGuid(), Name = "LEVEL 3 COMMUNICATIONS LIMITED", Code = 7232, Prefix = "ZQ"
            },
            new() { Id = Guid.NewGuid(), Name = "LIGHTNING FIBRE LIMITED", Code = 7378, Prefix = "B8" },
            new() { Id = Guid.NewGuid(), Name = "LIGHTSPEED NETWORKS LTD", Code = 7524, Prefix = "H8" },
            new() { Id = Guid.NewGuid(), Name = "LINCOLNSHIRE COUNTY COUNCIL", Code = 2500, Prefix = "GS" },
            new() { Id = Guid.NewGuid(), Name = "LIT FIBRE GROUP LTD", Code = 7509, Prefix = "G2" },
            new() { Id = Guid.NewGuid(), Name = "LIVERPOOL CITY COUNCIL", Code = 4310, Prefix = "GT" },
            new()
            {
                Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF BARKING AND DAGENHAM", Code = 5060, Prefix = "AG"
            },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF BARNET", Code = 5090, Prefix = "AH" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF BEXLEY", Code = 5120, Prefix = "AP" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF BRENT", Code = 5150, Prefix = "AW" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF BROMLEY", Code = 5180, Prefix = "BF" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF CAMDEN", Code = 5210, Prefix = "CM" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF CROYDON", Code = 5240, Prefix = "DD" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF EALING", Code = 5270, Prefix = "DV" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF ENFIELD", Code = 5300, Prefix = "EM" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HACKNEY", Code = 5360, Prefix = "FD" },
            new()
            {
                Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HAMMERSMITH & FULHAM", Code = 5390, Prefix = "FE"
            },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HARINGEY", Code = 5420, Prefix = "FG" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HARROW", Code = 5450, Prefix = "FH" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HAVERING", Code = 5480, Prefix = "FK" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HILLINGDON", Code = 5510, Prefix = "FP" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF HOUNSLOW", Code = 5540, Prefix = "FQ" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF ISLINGTON", Code = 5570, Prefix = "FV" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF LAMBETH", Code = 5660, Prefix = "GL" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF LEWISHAM", Code = 5690, Prefix = "GR" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF MERTON", Code = 5720, Prefix = "HC" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF NEWHAM", Code = 5750, Prefix = "HS" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF REDBRIDGE", Code = 5780, Prefix = "KM" },
            new()
            {
                Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF RICHMOND UPON THAMES", Code = 5810, Prefix = "KP"
            },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF SOUTHWARK", Code = 5840, Prefix = "LR" },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF SUTTON", Code = 5870, Prefix = "MB" },
            new()
            {
                Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF TOWER HAMLETS", Code = 5900, Prefix = "NA"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF WALTHAM FOREST", Code = 5930, Prefix = "PA"
            },
            new() { Id = Guid.NewGuid(), Name = "LONDON BOROUGH OF WANDSWORTH", Code = 5960, Prefix = "PB" },
            new() { Id = Guid.NewGuid(), Name = "LONDON TRANSPORT LIMITED", Code = 7210, Prefix = "RK" },
            new() { Id = Guid.NewGuid(), Name = "LONDON UNDERGROUND LIMITED", Code = 7072, Prefix = "GV" },
            new() { Id = Guid.NewGuid(), Name = "LUMEN TECHNOLOGIES INC", Code = 7183, Prefix = "RT" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "LUMEN TECHNOLOGIES INC (Formerly CENTURYLINK COMMUNICATIONS UK LIMITED)",
                Code = 7094,
                Prefix = "BB"
            },
            new() { Id = Guid.NewGuid(), Name = "LUNS LTD", Code = 7320, Prefix = "UP" },
            new() { Id = Guid.NewGuid(), Name = "LUTON BOROUGH COUNCIL", Code = 230, Prefix = "JA" },
            new() { Id = Guid.NewGuid(), Name = "MAINLINE PIPELINES LIMITED", Code = 7090, Prefix = "GW" },
            new() { Id = Guid.NewGuid(), Name = "MANCHESTER CITY COUNCIL", Code = 4215, Prefix = "GX" },
            new() { Id = Guid.NewGuid(), Name = "MEDWAY COUNCIL", Code = 2280, Prefix = "JL" },
            new()
            {
                Id = Guid.NewGuid(), Name = "MERTHYR TYDFIL COUNTY BOROUGH COUNCIL", Code = 6925, Prefix = "HB"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "METRO DIGITAL TELEVISION LIMITED", Code = 7176, Prefix = "HD"
            },
            new() { Id = Guid.NewGuid(), Name = "MIDDLESBROUGH BOROUGH COUNCIL", Code = 734, Prefix = "RL" },
            new() { Id = Guid.NewGuid(), Name = "MILTON KEYNES CITY COUNCIL", Code = 435, Prefix = "JM" },
            new() { Id = Guid.NewGuid(), Name = "MLL TELECOM LTD.", Code = 7278, Prefix = "WF" },
            new() { Id = Guid.NewGuid(), Name = "MONMOUTHSHIRE COUNTY COUNCIL", Code = 6840, Prefix = "RM" },
            new() { Id = Guid.NewGuid(), Name = "MS3 NETWORKS LIMITED", Code = 7523, Prefix = "H7" },
            new() { Id = Guid.NewGuid(), Name = "MUA ELECTRICITY LIMITED", Code = 7366, Prefix = "A2" },
            new() { Id = Guid.NewGuid(), Name = "MUA GAS LIMITED", Code = 7367, Prefix = "A3" },
            new() { Id = Guid.NewGuid(), Name = "MY FIBRE LIMITED", Code = 7504, Prefix = "F3" },
            new() { Id = Guid.NewGuid(), Name = "NATIONAL GAS TRANSMISSION", Code = 7397, Prefix = "E4" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (EAST MIDLANDS)",
                Code = 7011,
                Prefix = "DY"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (SOUTH WALES)",
                Code = 7012,
                Prefix = "LL"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (SOUTH WEST)",
                Code = 7003,
                Prefix = "LN"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NATIONAL GRID ELECTRICITY DISTRIBUTION (WEST MIDLANDS)",
                Code = 7007,
                Prefix = "HM"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NATIONAL GRID ELECTRICITY TRANSMISSION PLC",
                Code = 7015,
                Prefix = "HP"
            },
            new() { Id = Guid.NewGuid(), Name = "NATIONAL GRID TELECOMS", Code = 7145, Prefix = "SJ" },
            new() { Id = Guid.NewGuid(), Name = "NATIONAL HIGHWAYS", Code = 11, Prefix = "FN" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NEATH PORT TALBOT COUNTY BOROUGH COUNCIL",
                Code = 6930,
                Prefix = "HQ"
            },
            new() { Id = Guid.NewGuid(), Name = "NEOS NETWORKS LIMITED", Code = 7244, Prefix = "YE" },
            new() { Id = Guid.NewGuid(), Name = "NETOMNIA LIMITED", Code = 7388, Prefix = "D2" },
            new() { Id = Guid.NewGuid(), Name = "NETWORK RAIL", Code = 7093, Prefix = "KL" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NEW ASH GREEN VILLAGE ASSOCIATION LIMITED",
                Code = 7398,
                Prefix = "E5"
            },
            new() { Id = Guid.NewGuid(), Name = "NEWCASTLE CITY COUNCIL", Code = 4510, Prefix = "HR" },
            new() { Id = Guid.NewGuid(), Name = "NEWPORT CITY COUNCIL", Code = 6935, Prefix = "HT" },
            new() { Id = Guid.NewGuid(), Name = "NEXFIBRE NETWORKS LIMITED", Code = 7562, Prefix = "N7" },
            new() { Id = Guid.NewGuid(), Name = "NEXTGENACCESS LTD", Code = 7353, Prefix = "TL" },
            new() { Id = Guid.NewGuid(), Name = "NMCN PLC", Code = 7327, Prefix = "UW" },
            new() { Id = Guid.NewGuid(), Name = "NORFOLK COUNTY COUNCIL", Code = 2600, Prefix = "HU" },
            new()
            {
                Id = Guid.NewGuid(), Name = "NORTH EAST LINCOLNSHIRE COUNCIL", Code = 2002, Prefix = "RN"
            },
            new() { Id = Guid.NewGuid(), Name = "NORTH LINCOLNSHIRE COUNCIL", Code = 2003, Prefix = "RP" },
            new()
            {
                Id = Guid.NewGuid(), Name = "NORTH NORTHAMPTONSHIRE COUNCIL", Code = 2840, Prefix = "F8"
            },
            new() { Id = Guid.NewGuid(), Name = "NORTH SOMERSET COUNCIL", Code = 121, Prefix = "RQ" },
            new() { Id = Guid.NewGuid(), Name = "NORTH TYNESIDE COUNCIL", Code = 4515, Prefix = "HY" },
            new() { Id = Guid.NewGuid(), Name = "NORTH YORKSHIRE COUNCIL", Code = 2745, Prefix = "JB" },
            new() { Id = Guid.NewGuid(), Name = "NORTHERN GAS NETWORKS LIMITED", Code = 7271, Prefix = "XX" },
            new()
            {
                Id = Guid.NewGuid(), Name = "NORTHERN POWERGRID (NORTHEAST) LIMITED", Code = 7006, Prefix = "JD"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "NORTHERN POWERGRID (YORKSHIRE) PLC", Code = 7001, Prefix = "QA"
            },
            new() { Id = Guid.NewGuid(), Name = "NORTHUMBERLAND COUNTY COUNCIL", Code = 2935, Prefix = "UH" },
            new() { Id = Guid.NewGuid(), Name = "NORTHUMBRIAN WATER LIMITED", Code = 9101, Prefix = "JF" },
            new() { Id = Guid.NewGuid(), Name = "NOTTINGHAM CITY COUNCIL", Code = 3060, Prefix = "SQ" },
            new()
            {
                Id = Guid.NewGuid(), Name = "NOTTINGHAMSHIRE COUNTY COUNCIL", Code = 3055, Prefix = "JK"
            },
            new() { Id = Guid.NewGuid(), Name = "NTL (B) LIMITED", Code = 7096, Prefix = "AM" },
            new() { Id = Guid.NewGuid(), Name = "NTL (BROADLAND) LIMITED", Code = 7029, Prefix = "BE" },
            new() { Id = Guid.NewGuid(), Name = "NTL (SOUTH EAST) LIMITED", Code = 7120, Prefix = "EE" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS EPPING FOREST)",
                Code = 7121,
                Prefix = "EF"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS GREATER LONDON)",
                Code = 7122,
                Prefix = "EG"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS HAVERING)",
                Code = 7123,
                Prefix = "EH"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS NEWHAM)",
                Code = 7124,
                Prefix = "EJ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "NTL (SOUTH EAST) LIMITED (Formerly ENCOM CABLE TV AND COMMS WALTHAM FOREST)",
                Code = 7125,
                Prefix = "EK"
            },
            new() { Id = Guid.NewGuid(), Name = "NTL CAMBRIDGE LIMITED", Code = 7034, Prefix = "CJ" },
            new() { Id = Guid.NewGuid(), Name = "NTL KIRKLEES", Code = 7047, Prefix = "BZ" },
            new() { Id = Guid.NewGuid(), Name = "NTL MIDLANDS LIMITED", Code = 7119, Prefix = "DP" },
            new() { Id = Guid.NewGuid(), Name = "NTL NATIONAL NETWORKS LIMITED", Code = 7274, Prefix = "WA" },
            new() { Id = Guid.NewGuid(), Name = "NTL SOUTH CENTRAL LIMITED", Code = 7058, Prefix = "LU" },
            new() { Id = Guid.NewGuid(), Name = "NTL TELECOM SERVICES LIMITED", Code = 7219, Prefix = "SU" },
            new() { Id = Guid.NewGuid(), Name = "NWP STREET LIMITED", Code = 7226, Prefix = "ZH" },
            new() { Id = Guid.NewGuid(), Name = "NYNET LTD", Code = 7522, Prefix = "H6" },
            new() { Id = Guid.NewGuid(), Name = "OCIUSNET UK LTD", Code = 7542, Prefix = "L2" },
            new() { Id = Guid.NewGuid(), Name = "OGI NETWORKS LIMITED", Code = 7505, Prefix = "F4" },
            new()
            {
                Id = Guid.NewGuid(), Name = "OLDHAM METROPOLITAN BOROUGH COUNCIL", Code = 4220, Prefix = "KC"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "OPEN FIBRE NETWORKS (WHOLESALE) LIMITED",
                Code = 7336,
                Prefix = "UX"
            },
            new() { Id = Guid.NewGuid(), Name = "Open Infra Ltd", Code = 7556, Prefix = "M9" },
            new() { Id = Guid.NewGuid(), Name = "OPEN NETWORK SYSTEMS LIMITED", Code = 7381, Prefix = "C3" },
            new()
            {
                Id = Guid.NewGuid(), Name = "OPTICAL FIBRE INFRASTRUCTURE LIMITED", Code = 7502, Prefix = "E9"
            },
            new() { Id = Guid.NewGuid(), Name = "OPTIMAL POWER NETWORKS", Code = 7500, Prefix = "E7" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ORANGE PERSONAL COMMUNICATIONS SERVICES LIMITED",
                Code = 7233,
                Prefix = "ZR"
            },
            new() { Id = Guid.NewGuid(), Name = "ORBITAL NET LTD", Code = 7517, Prefix = "G9" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ORSTED HORNSEA PROJECT THREE (UK) LIMITED",
                Code = 7566,
                Prefix = "P3"
            },
            new() { Id = Guid.NewGuid(), Name = "OXFORDSHIRE COUNTY COUNCIL", Code = 3100, Prefix = "KE" },
            new() { Id = Guid.NewGuid(), Name = "PEMBROKESHIRE COUNTY COUNCIL", Code = 6845, Prefix = "RR" },
            new() { Id = Guid.NewGuid(), Name = "PEOPLE'S FIBRE LTD", Code = 7399, Prefix = "E6" },
            new() { Id = Guid.NewGuid(), Name = "PERSIMMON HOMES LIMITED", Code = 7534, Prefix = "K2" },
            new() { Id = Guid.NewGuid(), Name = "PETERBOROUGH CITY COUNCIL", Code = 540, Prefix = "FB" },
            new() { Id = Guid.NewGuid(), Name = "PINE MEDIA LIMITED", Code = 7533, Prefix = "J9" },
            new() { Id = Guid.NewGuid(), Name = "PLYMOUTH CITY COUNCIL", Code = 1160, Prefix = "SL" },
            new() { Id = Guid.NewGuid(), Name = "PORTSMOUTH CITY COUNCIL", Code = 1775, Prefix = "FC" },
            new() { Id = Guid.NewGuid(), Name = "PORTSMOUTH WATER LIMITED", Code = 9128, Prefix = "KH" },
            new() { Id = Guid.NewGuid(), Name = "POWERSYSTEMS UK LTD", Code = 7316, Prefix = "UA" },
            new() { Id = Guid.NewGuid(), Name = "POWYS COUNTY COUNCIL", Code = 6850, Prefix = "KJ" },
            new()
            {
                Id = Guid.NewGuid(), Name = "QUICKLINE COMMUNICATIONS LIMITED", Code = 7365, Prefix = "TZ"
            },
            new() { Id = Guid.NewGuid(), Name = "RAILSITE TELECOM LIMITED", Code = 7521, Prefix = "H5" },
            new() { Id = Guid.NewGuid(), Name = "READING BOROUGH COUNCIL", Code = 345, Prefix = "JN" },
            new()
            {
                Id = Guid.NewGuid(), Name = "REDCAR AND CLEVELAND BOROUGH COUNCIL", Code = 728, Prefix = "RU"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "REDCENTRIC COMMUNICATIONS LIMITED", Code = 7247, Prefix = "YJ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "RHONDDA CYNON TAF COUNTY BOROUGH COUNCIL",
                Code = 6940,
                Prefix = "KN"
            },
            new() { Id = Guid.NewGuid(), Name = "Riverside Energy Park Ltd", Code = 7569, Prefix = "P8" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "RM PROPERTY AND FACILITIES SOLUTIONS LIMITED",
                Code = 7221,
                Prefix = "SW"
            },
            new() { Id = Guid.NewGuid(), Name = "ROCHDALE BOROUGH COUNCIL", Code = 4225, Prefix = "KQ" },
            new() { Id = Guid.NewGuid(), Name = "ROLTA UK LIMITED", Code = 7322, Prefix = "UR" },
            new()
            {
                Id = Guid.NewGuid(), Name = "ROTHERHAM METROPOLITAN BOROUGH COUNCIL", Code = 4415, Prefix = "KR"
            },
            new() { Id = Guid.NewGuid(), Name = "ROYAL BOROUGH OF GREENWICH", Code = 5330, Prefix = "FA" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "ROYAL BOROUGH OF KENSINGTON AND CHELSEA",
                Code = 5600,
                Prefix = "GD"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ROYAL BOROUGH OF KINGSTON UPON THAMES", Code = 5630, Prefix = "GH"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "ROYAL BOROUGH OF WINDSOR AND MAIDENHEAD", Code = 355, Prefix = "JW"
            },
            new() { Id = Guid.NewGuid(), Name = "RUNFIBRE LTD", Code = 7565, Prefix = "P2" },
            new() { Id = Guid.NewGuid(), Name = "RUTLAND COUNTY COUNCIL", Code = 2470, Prefix = "JP" },
            new() { Id = Guid.NewGuid(), Name = "SALFORD CITY COUNCIL", Code = 4230, Prefix = "KS" },
            new()
            {
                Id = Guid.NewGuid(), Name = "SANDWELL METROPOLITAN BOROUGH COUNCIL", Code = 4620, Prefix = "KU"
            },
            new() { Id = Guid.NewGuid(), Name = "SCOTLAND GAS NETWORKS PLC", Code = 7273, Prefix = "XZ" },
            new() { Id = Guid.NewGuid(), Name = "SCOTTISH WATER LIMITED", Code = 9137, Prefix = "ZN" },
            new()
            {
                Id = Guid.NewGuid(), Name = "SCOTTISHPOWER ENERGY RETAIL LIMITED", Code = 7019, Prefix = "KY"
            },
            new() { Id = Guid.NewGuid(), Name = "SECURE WEB SERVICES LIMITED", Code = 7514, Prefix = "G6" },
            new()
            {
                Id = Guid.NewGuid(), Name = "SEFTON METROPOLITAN BOROUGH COUNCIL", Code = 4320, Prefix = "LA"
            },
            new() { Id = Guid.NewGuid(), Name = "SERVERHOUSE LTD", Code = 7575, Prefix = "R6" },
            new() { Id = Guid.NewGuid(), Name = "SES WATER LIMITED", Code = 9118, Prefix = "DZ" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "SEVERN TRENT SERVICES OPERATIONS UK LIMITED",
                Code = 7319,
                Prefix = "UN"
            },
            new() { Id = Guid.NewGuid(), Name = "SEVERN TRENT WATER LIMITED", Code = 9103, Prefix = "LB" },
            new() { Id = Guid.NewGuid(), Name = "SHEFFIELD CITY COUNCIL", Code = 4420, Prefix = "LC" },
            new() { Id = Guid.NewGuid(), Name = "SHROPSHIRE COUNCIL", Code = 3245, Prefix = "UJ" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "SKY TELECOMMUNICATIONS SERVICES LIMITED",
                Code = 7225,
                Prefix = "ZG"
            },
            new() { Id = Guid.NewGuid(), Name = "SLOUGH BOROUGH COUNCIL", Code = 350, Prefix = "JQ" },
            new() { Id = Guid.NewGuid(), Name = "SMARTFIBRE BROADBAND LIMITED", Code = 7578, Prefix = "R9" },
            new()
            {
                Id = Guid.NewGuid(), Name = "SOLIHULL METROPOLITAN BOROUGH COUNCIL", Code = 4625, Prefix = "LF"
            },
            new() { Id = Guid.NewGuid(), Name = "SOMERSET COUNCIL", Code = 3300, Prefix = "LG" },
            new() { Id = Guid.NewGuid(), Name = "SOUTH EAST WATER LIMITED", Code = 9117, Prefix = "EB" },
            new()
            {
                Id = Guid.NewGuid(), Name = "SOUTH EASTERN POWER NETWORKS PLC", Code = 7004, Prefix = "KZ"
            },
            new() { Id = Guid.NewGuid(), Name = "SOUTH GLOUCESTERSHIRE COUNCIL", Code = 119, Prefix = "RZ" },
            new() { Id = Guid.NewGuid(), Name = "SOUTH STAFFORDSHIRE WATER PLC", Code = 9129, Prefix = "LJ" },
            new() { Id = Guid.NewGuid(), Name = "SOUTH TYNESIDE COUNCIL", Code = 4520, Prefix = "LK" },
            new() { Id = Guid.NewGuid(), Name = "SOUTH WEST WATER LIMITED", Code = 9105, Prefix = "LM" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "SOUTH WEST WATER LIMITED (Formerly BRISTOL WATER PLC)",
                Code = 9111,
                Prefix = "AY"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "SOUTH YORKSHIRE PASSENGER TRANSPORT EXECUTIVE",
                Code = 9236,
                Prefix = "XN"
            },
            new() { Id = Guid.NewGuid(), Name = "SOUTHAMPTON CITY COUNCIL", Code = 1780, Prefix = "FU" },
            new() { Id = Guid.NewGuid(), Name = "SOUTHEND-ON-SEA CITY COUNCIL", Code = 1590, Prefix = "JR" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "SOUTHERN ELECTRIC POWER DISTRIBUTION PLC",
                Code = 7002,
                Prefix = "LP"
            },
            new() { Id = Guid.NewGuid(), Name = "SOUTHERN GAS NETWORKS PLC", Code = 7270, Prefix = "XW" },
            new() { Id = Guid.NewGuid(), Name = "SOUTHERN WATER LIMITED", Code = 9104, Prefix = "LQ" },
            new() { Id = Guid.NewGuid(), Name = "SP MANWEB PLC", Code = 7008, Prefix = "GY" },
            new() { Id = Guid.NewGuid(), Name = "SPRING FIBRE LIMITED", Code = 7529, Prefix = "J5" },
            new() { Id = Guid.NewGuid(), Name = "SQUIRE ENERGY LIMITED", Code = 7383, Prefix = "C5" },
            new()
            {
                Id = Guid.NewGuid(), Name = "SQUIRE ENERGY METERING LIMITED", Code = 7571, Prefix = "R2"
            },
            new() { Id = Guid.NewGuid(), Name = "ST. HELENS COUNCIL", Code = 4315, Prefix = "LT" },
            new() { Id = Guid.NewGuid(), Name = "STAFFORDSHIRE COUNTY COUNCIL", Code = 3450, Prefix = "LV" },
            new() { Id = Guid.NewGuid(), Name = "STIX INTERNET LIMITED", Code = 7530, Prefix = "J6" },
            new()
            {
                Id = Guid.NewGuid(), Name = "STOCKPORT METROPOLITAN BOROUGH COUNCIL", Code = 4235, Prefix = "LX"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "STOCKTON-ON-TEES BOROUGH COUNCIL", Code = 738, Prefix = "SB"
            },
            new() { Id = Guid.NewGuid(), Name = "STOKE-ON-TRENT CITY COUNCIL", Code = 3455, Prefix = "GF" },
            new() { Id = Guid.NewGuid(), Name = "SUBTOPIA LIMITED", Code = 7344, Prefix = "ZD" },
            new()
            {
                Id = Guid.NewGuid(), Name = "SUEZ ADVANCED SOLUTIONS UL LTD", Code = 7558, Prefix = "N3"
            },
            new() { Id = Guid.NewGuid(), Name = "SUFFOLK COUNTY COUNCIL", Code = 3500, Prefix = "LY" },
            new() { Id = Guid.NewGuid(), Name = "SUNDERLAND CITY COUNCIL", Code = 4525, Prefix = "LZ" },
            new() { Id = Guid.NewGuid(), Name = "SURREY COUNTY COUNCIL", Code = 3600, Prefix = "MA" },
            new() { Id = Guid.NewGuid(), Name = "SWINDON BOROUGH COUNCIL", Code = 3935, Prefix = "SN" },
            new() { Id = Guid.NewGuid(), Name = "SWISH FIBRE LIMITED", Code = 7384, Prefix = "C6" },
            new()
            {
                Id = Guid.NewGuid(), Name = "TALKTALK COMMUNICATIONS LIMITED", Code = 7299, Prefix = "VD"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "TAMESIDE METROPOLITAN BOROUGH COUNCIL", Code = 4240, Prefix = "MF"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "TATA COMMUNICATIONS (UK) LIMITED", Code = 7248, Prefix = "YL"
            },
            new() { Id = Guid.NewGuid(), Name = "TELCOM INFRASTRUCTURE LIMITED", Code = 7390, Prefix = "D5" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS (HERTS) LIMITED PARTNERSHIP",
                Code = 7152,
                Prefix = "MQ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS (NORTHANTS) LIMITED PARTNERSHIP",
                Code = 7146,
                Prefix = "MJ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name =
                    "TELECENTIAL COMMUNICATIONS (NORTHANTS) LIMITED PARTNERSHIP (Formerly TELECENTIAL COMMS NORTHAMPTON)",
                Code = 7147,
                Prefix = "MK"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS (WARWICKSHIRE) LIMITED PARTNERSHIP",
                Code = 7149,
                Prefix = "MM"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS LIMITED PARTNERSHIP",
                Code = 7148,
                Prefix = "ML"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS LIMITED PARTNERSHIP (Formerly TELECENTIAL COMMS RUGBY)",
                Code = 7150,
                Prefix = "MN"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELECENTIAL COMMUNICATIONS LIMITED PARTNERSHIP (Formerly TELECENTIAL COMMS STRATFORD)",
                Code = 7151,
                Prefix = "MP"
            },
            new() { Id = Guid.NewGuid(), Name = "TELEFONICA UK LIMITED", Code = 7182, Prefix = "MG" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELEWEST COMMUNICATIONS (NORTH EAST) LIMITED",
                Code = 7158,
                Prefix = "NH"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELEWEST COMMUNICATIONS (SOUTH EAST) LIMITED",
                Code = 7159,
                Prefix = "NJ"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELEWEST COMMUNICATIONS (SOUTH WEST) LIMITED",
                Code = 7061,
                Prefix = "NC"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "TELEWEST COMMUNICATIONS (TYNESIDE) LIMITED",
                Code = 7035,
                Prefix = "CY"
            },
            new() { Id = Guid.NewGuid(), Name = "TELEWEST LIMITED", Code = 7237, Prefix = "ZX" },
            new() { Id = Guid.NewGuid(), Name = "TELFORD & WREKIN COUNCIL", Code = 3240, Prefix = "JS" },
            new() { Id = Guid.NewGuid(), Name = "TELSTRA GLOBAL LIMITED", Code = 7085, Prefix = "MS" },
            new()
            {
                Id = Guid.NewGuid(), Name = "THAMES WATER UTILITIES LIMITED", Code = 9106, Prefix = "MU"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "THE ELECTRICITY NETWORK COMPANY LIMITED",
                Code = 7560,
                Prefix = "N5"
            },
            new() { Id = Guid.NewGuid(), Name = "THE FIBRE GUYS LTD", Code = 7520, Prefix = "H4" },
            new() { Id = Guid.NewGuid(), Name = "THE OIL & PIPELINES AGENCY", Code = 7092, Prefix = "EZ" },
            new() { Id = Guid.NewGuid(), Name = "THE ROCHESTER BRIDGE TRUST", Code = 7337, Prefix = "ZA" },
            new() { Id = Guid.NewGuid(), Name = "THURROCK COUNCIL", Code = 1595, Prefix = "JT" },
            new() { Id = Guid.NewGuid(), Name = "THUS LIMITED", Code = 7224, Prefix = "ZF" },
            new()
            {
                Id = Guid.NewGuid(), Name = "TIMICO PARTNER SERVICES LIMITED", Code = 7275, Prefix = "WC"
            },
            new() { Id = Guid.NewGuid(), Name = "TOOB LIMITED", Code = 7375, Prefix = "B5" },
            new() { Id = Guid.NewGuid(), Name = "TORBAY COUNCIL", Code = 1165, Prefix = "SM" },
            new()
            {
                Id = Guid.NewGuid(), Name = "TORFAEN COUNTY BOROUGH COUNCIL", Code = 6945, Prefix = "MZ"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "TRAFFORD METROPOLITAN BOROUGH COUNCIL", Code = 4245, Prefix = "NB"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "TRANSPORT FOR GREATER MANCHESTER", Code = 7215, Prefix = "QZ"
            },
            new() { Id = Guid.NewGuid(), Name = "TRANSPORT FOR LONDON (TFL)", Code = 20, Prefix = "YG" },
            new() { Id = Guid.NewGuid(), Name = "TRANSPORT FOR WALES (TFW)", Code = 7549, Prefix = "M2" },
            new() { Id = Guid.NewGuid(), Name = "TRUESPEED COMMUNICATIONS LTD", Code = 7355, Prefix = "TP" },
            new() { Id = Guid.NewGuid(), Name = "UK BROADBAND LIMITED", Code = 7341, Prefix = "PH" },
            new() { Id = Guid.NewGuid(), Name = "UK POWER DISTRIBUTION LIMITED", Code = 7361, Prefix = "TV" },
            new()
            {
                Id = Guid.NewGuid(), Name = "UNITED UTILITIES WATER LIMITED", Code = 9102, Prefix = "HZ"
            },
            new() { Id = Guid.NewGuid(), Name = "UPP (Corporation Limited)", Code = 7526, Prefix = "H9" },
            new()
            {
                Id = Guid.NewGuid(), Name = "UTILITY GRID INSTALLATIONS LIMITED", Code = 7265, Prefix = "XK"
            },
            new() { Id = Guid.NewGuid(), Name = "VALE OF GLAMORGAN COUNCIL", Code = 6950, Prefix = "NL" },
            new() { Id = Guid.NewGuid(), Name = "VATTENFALL NETWORKS LIMITED", Code = 7394, Prefix = "D9" },
            new() { Id = Guid.NewGuid(), Name = "VATTENFALL WIND POWER LTD", Code = 7561, Prefix = "N6" },
            new()
            {
                Id = Guid.NewGuid(), Name = "VEOLIA WATER OUTSOURCING LIMITED", Code = 7314, Prefix = "VY"
            },
            new() { Id = Guid.NewGuid(), Name = "VERIZON UK LIMITED", Code = 7086, Prefix = "PQ" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "VERIZON UK LIMITED (Formerly WORLDCOM INTERNATIONAL LTD)",
                Code = 7081,
                Prefix = "HE"
            },
            new() { Id = Guid.NewGuid(), Name = "VIRGIN MEDIA LIMITED", Code = 7160, Prefix = "NK" },
            new() { Id = Guid.NewGuid(), Name = "VIRGIN MEDIA PCHC II LIMITED", Code = 7110, Prefix = "CD" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "VIRGIN MEDIA PCHC II LIMITED (Formerly CABLETEL SOUTH WALES CARDIFF)",
                Code = 7109,
                Prefix = "CB"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "VIRGIN MEDIA WHOLESALE LIMITED", Code = 7240, Prefix = "YA"
            },
            new() { Id = Guid.NewGuid(), Name = "VISPA LIMITED", Code = 7516, Prefix = "G7" },
            new()
            {
                Id = Guid.NewGuid(), Name = "VITAL ENERGI UTILITIES LIMITED", Code = 7209, Prefix = "RX"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "VODAFONE ENTERPRISE EUROPE (UK) LIMITED", Code = 70, Prefix = "QL"
            },
            new() { Id = Guid.NewGuid(), Name = "VODAFONE LIMITED", Code = 7076, Prefix = "NX" },
            new() { Id = Guid.NewGuid(), Name = "VONEUS LIMITED", Code = 7507, Prefix = "F6" },
            new() { Id = Guid.NewGuid(), Name = "VORBOSS LIMITED", Code = 7389, Prefix = "D3" },
            new() { Id = Guid.NewGuid(), Name = "VX FIBER LIMITED", Code = 7395, Prefix = "E2" },
            new()
            {
                Id = Guid.NewGuid(), Name = "WALES & WEST UTILITIES LIMITED", Code = 7272, Prefix = "XY"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "WALSALL METROPOLITAN BOROUGH COUNCIL", Code = 4630, Prefix = "NZ"
            },
            new() { Id = Guid.NewGuid(), Name = "WARRINGTON BOROUGH COUNCIL", Code = 655, Prefix = "JU" },
            new() { Id = Guid.NewGuid(), Name = "WARWICKSHIRE COUNTY COUNCIL", Code = 3700, Prefix = "PC" },
            new() { Id = Guid.NewGuid(), Name = "WELINK COMMUNICATIONS UK LTD", Code = 7532, Prefix = "J8" },
            new() { Id = Guid.NewGuid(), Name = "WELSH GOVERNMENT", Code = 16, Prefix = "PD" },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "WELSH HIGHLAND RAILWAY CONSTRUCTION LIMITED",
                Code = 7310,
                Prefix = "VR"
            },
            new() { Id = Guid.NewGuid(), Name = "WESSEX INTERNET", Code = 7525, Prefix = "J4" },
            new() { Id = Guid.NewGuid(), Name = "WESSEX WATER LIMITED", Code = 9108, Prefix = "PG" },
            new() { Id = Guid.NewGuid(), Name = "WEST BERKSHIRE COUNCIL", Code = 340, Prefix = "JV" },
            new() { Id = Guid.NewGuid(), Name = "WEST NORTHAMPTONSHIRE COUNCIL", Code = 2845, Prefix = "F9" },
            new() { Id = Guid.NewGuid(), Name = "WEST SUSSEX COUNTY COUNCIL", Code = 3800, Prefix = "PJ" },
            new()
            {
                Id = Guid.NewGuid(), Name = "WESTMORLAND AND FURNESS COUNCIL", Code = 935, Prefix = "P4"
            },
            new()
            {
                Id = Guid.NewGuid(), Name = "WESTNETWORKS INNOVATIONS LIMITED", Code = 7519, Prefix = "H3"
            },
            new() { Id = Guid.NewGuid(), Name = "WHYFIBRE LIMITED", Code = 7572, Prefix = "R3" },
            new() { Id = Guid.NewGuid(), Name = "WIFINITY LIMITED", Code = 7570, Prefix = "P9" },
            new()
            {
                Id = Guid.NewGuid(), Name = "WIGAN METROPOLITAN BOROUGH COUNCIL", Code = 4250, Prefix = "PL"
            },
            new() { Id = Guid.NewGuid(), Name = "WIGHTFIBRE LIMITED", Code = 7251, Prefix = "YP" },
            new() { Id = Guid.NewGuid(), Name = "WILDANET LIMITED", Code = 7515, Prefix = "G8" },
            new() { Id = Guid.NewGuid(), Name = "WILDCARD UK LIMITED", Code = 7363, Prefix = "TX" },
            new() { Id = Guid.NewGuid(), Name = "WILTSHIRE COUNCIL", Code = 3940, Prefix = "UK" },
            new() { Id = Guid.NewGuid(), Name = "WIRRAL BOROUGH COUNCIL", Code = 4325, Prefix = "PN" },
            new() { Id = Guid.NewGuid(), Name = "WOKINGHAM BOROUGH COUNCIL", Code = 360, Prefix = "JX" },
            new() { Id = Guid.NewGuid(), Name = "WORCESTERSHIRE COUNTY COUNCIL", Code = 1855, Prefix = "JZ" },
            new()
            {
                Id = Guid.NewGuid(), Name = "WREXHAM COUNTY BOROUGH COUNCIL", Code = 6955, Prefix = "PR"
            },
            new() { Id = Guid.NewGuid(), Name = "WREXHAM WATER LIMITED", Code = 9135, Prefix = "PS" },
            new() { Id = Guid.NewGuid(), Name = "X-FIBRE LTD", Code = 7576, Prefix = "R7" },
            new() { Id = Guid.NewGuid(), Name = "YESFIBRE LTD", Code = 7546, Prefix = "L6" },
            new()
            {
                Id = Guid.NewGuid(), Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED", Code = 7027, Prefix = "PU"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS BRADFORD)",
                Code = 7068,
                Prefix = "PV"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS DONCASTER)",
                Code = 7041,
                Prefix = "PW"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS HALIFAX)",
                Code = 7170,
                Prefix = "PX"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS SHEFFIELD)",
                Code = 7057,
                Prefix = "PY"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "YORKSHIRE CABLE COMMUNICATIONS LIMITED (Formerly YORKSHIRE CABLE COMMS WAKEFIELD)",
                Code = 7063,
                Prefix = "PZ"
            },
            new() { Id = Guid.NewGuid(), Name = "YORKSHIRE WATER LIMITED", Code = 9109, Prefix = "QB" },
            new() { Id = Guid.NewGuid(), Name = "YOUR COMMUNICATIONS LIMITED", Code = 7083, Prefix = "JH" },
            new() { Id = Guid.NewGuid(), Name = "ZAYO GROUP UK LIMITED", Code = 7235, Prefix = "ZV" },
            new() { Id = Guid.NewGuid(), Name = "ZOOM INTERNET LIMITED", Code = 7574, Prefix = "R5" },
            new() { Id = Guid.NewGuid(), Name = "ZZOOMM PLC", Code = 7379, Prefix = "B9" }
        };

    #endregion
}