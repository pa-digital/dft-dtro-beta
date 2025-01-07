namespace DfT.DTRO.Enums;

/// <summary>
/// Represents different types of traffic regulations.
/// </summary>
public enum RegulationType
{
    /// <summary>
    /// Dimension Maximum Height Structural.
    /// </summary>
    [Display(Name = "dimensionMaximumHeightStructural")]
    DimensionMaximumHeightStructural,

    /// <summary>
    /// Dimension Maximum Height With TRO.
    /// </summary>
    [Display(Name = "dimensionMaximumHeightWithTRO")]
    DimensionMaximumHeightWithTRO,

    /// <summary>
    /// Dimension Maximum Length.
    /// </summary>
    [Display(Name = "dimensionMaximumLength")]
    DimensionMaximumLength,

    /// <summary>
    /// Dimension Maximum Weight Environmental.
    /// </summary>
    [Display(Name = "dimensionMaximumWeightEnvironmental")]
    DimensionMaximumWeightEnvironmental,

    /// <summary>
    /// Dimension Maximum Weight Structural.
    /// </summary>
    [Display(Name = "dimensionMaximumWeightStructural")]
    DimensionMaximumWeightStructural,

    /// <summary>
    /// Dimension Maximum Width.
    /// </summary>
    [Display(Name = "dimensionMaximumWidth")]
    DimensionMaximumWidth,

    /// <summary>
    /// Banned Movement No Entry.
    /// </summary>
    [Display(Name = "bannedMovementNoEntry")]
    BannedMovementNoEntry,

    /// <summary>
    /// Banned Movement No Left Turn.
    /// </summary>
    [Display(Name = "bannedMovementNoLeftTurn")]
    BannedMovementNoLeftTurn,

    /// <summary>
    /// Banned Movement No Right Turn.
    /// </summary>
    [Display(Name = "bannedMovementNoRightTurn")]
    BannedMovementNoRightTurn,

    /// <summary>
    /// Banned Movement No U-Turn.
    /// </summary>
    [Display(Name = "bannedMovementNoUTurn")]
    BannedMovementNoUTurn,

    /// <summary>
    /// Mandatory Direction Ahead Only.
    /// </summary>
    [Display(Name = "mandatoryDirectionAheadOnly")]
    MandatoryDirectionAheadOnly,

    /// <summary>
    /// Mandatory Direction Left Turn Only.
    /// </summary>
    [Display(Name = "mandatoryDirectionLeftTurnOnly")]
    MandatoryDirectionLeftTurnOnly,

    /// <summary>
    /// Mandatory Direction One Way.
    /// </summary>
    [Display(Name = "mandatoryDirectionOneWay")]
    MandatoryDirectionOneWay,

    /// <summary>
    /// Mandatory Direction Right Turn Only.
    /// </summary>
    [Display(Name = "mandatoryDirectionRightTurnOnly")]
    MandatoryDirectionRightTurnOnly,

    /// <summary>
    /// Movement Order No Overtaking.
    /// </summary>
    [Display(Name = "movementOrderNoOvertaking")]
    MovementOrderNoOvertaking,

    /// <summary>
    /// Movement Order Priority Over Oncoming Traffic.
    /// </summary>
    [Display(Name = "movementOrderPriorityOverOncomingTraffic")]
    MovementOrderPriorityOverOncomingTraffic,

    /// <summary>
    /// Movement Order Prohibited Access.
    /// </summary>
    [Display(Name = "movementOrderProhibitedAccess")]
    MovementOrderProhibitedAccess,

    /// <summary>
    /// Kerbside Disabled Badge Holders Only.
    /// </summary>
    [Display(Name = "kerbsideDisabledBadgeHoldersOnly")]
    KerbsideDisabledBadgeHoldersOnly,

    /// <summary>
    /// Kerbside Rural Clearway.
    /// </summary>
    [Display(Name = "kerbsideRuralClearway")]
    KerbsideRuralClearway,

    /// <summary>
    /// Kerbside Limited Waiting.
    /// </summary>
    [Display(Name = "kerbsideLimitedWaiting")]
    KerbsideLimitedWaiting,

    /// <summary>
    /// Kerbside Loading Place.
    /// </summary>
    [Display(Name = "kerbsideLoadingPlace")]
    KerbsideLoadingPlace,

    /// <summary>
    /// Kerbside Motorcycle Parking Place.
    /// </summary>
    [Display(Name = "kerbsideMotorcycleParkingPlace")]
    KerbsideMotorcycleParkingPlace,

    /// <summary>
    /// Kerbside No Loading.
    /// </summary>
    [Display(Name = "kerbsideNoLoading")]
    KerbsideNoLoading,

    /// <summary>
    /// Kerbside No Stopping.
    /// </summary>
    [Display(Name = "kerbsideNoStopping")]
    KerbsideNoStopping,

    /// <summary>
    /// Kerbside No Waiting.
    /// </summary>
    [Display(Name = "kerbsideNoWaiting")]
    KerbsideNoWaiting,

    /// <summary>
    /// Kerbside Taxi Rank.
    /// </summary>
    [Display(Name = "kerbsideTaxiRank")]
    KerbsideTaxiRank,

    /// <summary>
    /// Kerbside School Keep Clear Yellow Zig Zag Mandatory.
    /// </summary>
    [Display(Name = "kerbsideSchoolKeepClearYellowZigZagMandatory")]
    KerbsideSchoolKeepClearYellowZigZagMandatory,

    /// <summary>
    /// Kerbside Loading Bay.
    /// </summary>
    [Display(Name = "kerbsideLoadingBay")]
    KerbsideLoadingBay,

    /// <summary>
    /// Kerbside Other Yellow Zig Zag Mandatory.
    /// </summary>
    [Display(Name = "kerbsideOtherYellowZigZagMandatory")]
    KerbsideOtherYellowZigZagMandatory,

    /// <summary>
    /// Kerbside Permit Parking Area.
    /// </summary>
    [Display(Name = "kerbsidePermitParkingArea")]
    KerbsidePermitParkingArea,

    /// <summary>
    /// Kerbside Parking Place.
    /// </summary>
    [Display(Name = "kerbsideParkingPlace")]
    KerbsideParkingPlace,

    /// <summary>
    /// Kerbside Urban Clearway.
    /// </summary>
    [Display(Name = "kerbsideUrbanClearway")]
    KerbsideUrbanClearway,

    /// <summary>
    /// Kerbside Red Route Clearway.
    /// </summary>
    [Display(Name = "kerbsideRedRouteClearway")]
    KerbsideRedRouteClearway,

    /// <summary>
    /// Kerbside Payment Parking Place.
    /// </summary>
    [Display(Name = "kerbsidePaymentParkingPlace")]
    KerbsidePaymentParkingPlace,

    /// <summary>
    /// Kerbside Permit Parking Place.
    /// </summary>
    [Display(Name = "kerbsidePermitParkingPlace")]
    KerbsidePermitParkingPlace,

    /// <summary>
    /// Kerbside Footway Parking.
    /// </summary>
    [Display(Name = "kerbsideFootwayParking")]
    KerbsideFootwayParking,

    /// <summary>
    /// Kerbside Controlled Parking Zone.
    /// </summary>
    [Display(Name = "kerbsideControlledParkingZone")]
    KerbsideControlledParkingZone,

    /// <summary>
    /// Kerbside Restricted Parking Zone.
    /// </summary>
    [Display(Name = "kerbsideRestrictedParkingZone")]
    KerbsideRestrictedParkingZone,

    /// <summary>
    /// Kerbside Double Red Lines
    /// </summary>
    [Display(Name = "kerbsideDoubleRedLines")]
    KerbsideDoubleRedLines,

    /// <summary>
    /// Kerbside Single Reg Lines
    /// </summary>
    [Display(Name = "kerbsideSingleRedLines")]
    KerbsideSingleRedLines,

    /// <summary>
    /// Non-Order Movement Box Junction.
    /// </summary>
    [Display(Name = "nonOrderMovementBoxJunction")]
    NonOrderMovementBoxJunction,

    /// <summary>
    /// Non-Order Kerbside Bus Stop.
    /// </summary>
    [Display(Name = "nonOrderKerbsideBusStop")]
    NonOrderKerbsideBusStop,

    /// <summary>
    /// Non-Order Kerbside Pedestrian Crossing.
    /// </summary>
    [Display(Name = "nonOrderKerbsidePedestrianCrossing")]
    NonOrderKerbsidePedestrianCrossing,

    /// <summary>
    /// Misc Bus Gate.
    /// </summary>
    [Display(Name = "miscBusGate")]
    MiscBusGate,

    /// <summary>
    /// Misc Bus Lane With Traffic Flow.
    /// </summary>
    [Display(Name = "miscBusLaneWithTrafficFlow")]
    MiscBusLaneWithTrafficFlow,

    /// <summary>
    /// Misc Bus Only Street.
    /// </summary>
    [Display(Name = "miscBusOnlyStreet")]
    MiscBusOnlyStreet,

    /// <summary>
    /// Misc Contraflow Bus Lane.
    /// </summary>
    [Display(Name = "miscContraflowBusLane")]
    MiscContraflowBusLane,

    /// <summary>
    /// Misc Congestion Low Emission Zone.
    /// </summary>
    [Display(Name = "miscCongestionLowEmissionZone")]
    MiscCongestionLowEmissionZone,

    /// <summary>
    /// Misc Cycle Lane.
    /// </summary>
    [Display(Name = "miscCycleLane")]
    MiscCycleLane,

    /// <summary>
    /// Misc Pedestrian Zone.
    /// </summary>
    [Display(Name = "miscPedestrianZone")]
    MiscPedestrianZone,

    /// <summary>
    /// Misc Road Closure.
    /// </summary>
    [Display(Name = "miscRoadClosure")]
    MiscRoadClosure,

    /// <summary>
    /// Misc Lane Closure.
    /// </summary>
    [Display(Name = "miscLaneClosure")]
    MiscLaneClosure,

    /// <summary>
    /// Misc Contraflow.
    /// </summary>
    [Display(Name = "miscContraflow")]
    MiscContraflow,

    /// <summary>
    /// Misc Footway Closure.
    /// </summary>
    [Display(Name = "miscFootwayClosure")]
    MiscFootwayClosure,

    /// <summary>
    /// Misc Cycle Lane Closure
    /// </summary>
    [Display(Name = "miscCycleLaneClosure")]
    MiscCycleLaneClosure,

    /// <summary>
    /// Misc Temporary Parking Restriction
    /// </summary>
    [Display(Name = "miscTemporaryParkingRestriction")]
    MiscTemporaryParkingRestriction,

    /// <summary>
    /// Misc Suspension of One Way
    /// </summary>
    [Display(Name = "miscSuspensionOfOneWay")]
    MiscSuspensionOfOneWay,

    /// <summary>
    /// Misc Suspension Of Parking Restriction
    /// </summary>
    [Display(Name = "miscSuspensionOfParkingRestriction")]
    MiscSuspensionOfParkingRestriction,

    /// <summary>
    /// Misc Suspension Of Weight Restriction
    /// </summary>
    [Display(Name = "miscSuspensionOfWeightRestriction")]
    MiscSuspensionOfWeightRestriction,

    /// <summary>
    /// Misc Suspension Of Bus way
    /// </summary>
    [Display(Name = "miscSuspensionOfBusway")]
    MiscSuspensionOfBusway,

    /// <summary>
    /// Misc Temporary Speed Limit
    /// </summary>
    [Display(Name = "miscTemporarySpeedLimit")]
    MiscTemporarySpeedLimit,

    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "miscRoadClosureCrossingPoint")]
    MiscRoadClosureCrossingPoint,

    /// <summary>
    /// Misc Bay Suspension
    /// </summary>
    [Display(Name = "miscBaySuspension")]
    MiscBaySuspension,

    /// <summary>
    /// Misc Temporary Parking Bay
    /// </summary>
    [Display(Name = "miscTemporaryParkingBay")]
    MiscTemporaryParkingBay,

    /// <summary>
    /// Misc PROW Closure
    /// </summary>
    [Display(Name = "miscPROWClosure")]
    MiscPROWClosure
}
