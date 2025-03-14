namespace DfT.DTRO.Enums;

/// <summary>
/// Represents different types of traffic regulations.
/// </summary>
public enum RegulationType
{
    /// <summary>
    /// Banned movement: No entry.
    /// </summary>
    [Display(Name = "bannedMovementNoEntry")]
    BannedMovementNoEntry,

    /// <summary>
    /// Banned movement: No left turn.
    /// </summary>
    [Display(Name = "bannedMovementNoLeftTurn")]
    BannedMovementNoLeftTurn,

    /// <summary>
    /// Banned movement: No right turn.
    /// </summary>
    [Display(Name = "bannedMovementNoRightTurn")]
    BannedMovementNoRightTurn,

    /// <summary>
    /// Banned movement: No U-turn.
    /// </summary>
    [Display(Name = "bannedMovementNoUTurn")]
    BannedMovementNoUTurn,

    /// <summary>
    /// Maximum height (structural).
    /// </summary>
    [Display(Name = "dimensionMaximumHeightStructural")]
    DimensionMaximumHeightStructural,

    /// <summary>
    /// Maximum height with TRO.
    /// </summary>
    [Display(Name = "dimensionMaximumHeightWithTRO")]
    DimensionMaximumHeightWithTRO,

    /// <summary>
    /// Maximum length.
    /// </summary>
    [Display(Name = "dimensionMaximumLength")]
    DimensionMaximumLength,

    /// <summary>
    /// Maximum weight (environmental).
    /// </summary>
    [Display(Name = "dimensionMaximumWeightEnvironmental")]
    DimensionMaximumWeightEnvironmental,

    /// <summary>
    /// Maximum weight (structural).
    /// </summary>
    [Display(Name = "dimensionMaximumWeightStructural")]
    DimensionMaximumWeightStructural,

    /// <summary>
    /// Maximum width.
    /// </summary>
    [Display(Name = "dimensionMaximumWidth")]
    DimensionMaximumWidth,

    /// <summary>
    /// Kerbside loading place: Passenger set down permitted.
    /// </summary>
    [Display(Name = "kerbsideLoadingPlacePassengerSetDownPermitted")]
    KerbsideLoadingPlacePassengerSetDownPermitted,

    /// <summary>
    /// Kerbside loading place: Passenger set down prohibited.
    /// </summary>
    [Display(Name = "kerbsideLoadingPlacePassengerSetDownProhibited")]
    KerbsideLoadingPlacePassengerSetDownProhibited,

    /// <summary>
    /// Kerbside no loading: Passenger set down permitted.
    /// </summary>
    [Display(Name = "kerbsideNoLoadingPassengerSetDownPermitted")]
    KerbsideNoLoadingPassengerSetDownPermitted,

    /// <summary>
    /// Kerbside no loading: Passenger set down prohibited.
    /// </summary>
    [Display(Name = "kerbsideNoLoadingPassengerSetDownProhibited")]
    KerbsideNoLoadingPassengerSetDownProhibited,

    /// <summary>
    /// Kerbside loading bay: Passenger set down permitted.
    /// </summary>
    [Display(Name = "kerbsideLoadingBayPassengerSetDownPermitted")]
    KerbsideLoadingBayPassengerSetDownPermitted,

    /// <summary>
    /// Kerbside loading bay: Passenger set down prohibited.
    /// </summary>
    [Display(Name = "kerbsideLoadingBayPassengerSetDownProhibited")]
    KerbsideLoadingBayPassengerSetDownProhibited,

    /// <summary>
    /// Kerbside controlled parking zone.
    /// </summary>
    [Display(Name = "kerbsideControlledParkingZone")]
    KerbsideControlledParkingZone,

    /// <summary>
    /// Kerbside disabled badge holders only.
    /// </summary>
    [Display(Name = "kerbsideDisabledBadgeHoldersOnly")]
    KerbsideDisabledBadgeHoldersOnly,

    /// <summary>
    /// Kerbside double red lines.
    /// </summary>
    [Display(Name = "kerbsideDoubleRedLines")]
    KerbsideDoubleRedLines,

    /// <summary>
    /// Kerbside footway parking.
    /// </summary>
    [Display(Name = "kerbsideFootwayParking")]
    KerbsideFootwayParking,

    /// <summary>
    /// Kerbside limited waiting.
    /// </summary>
    [Display(Name = "kerbsideLimitedWaiting")]
    KerbsideLimitedWaiting,

    /// <summary>
    /// Kerbside loading bay.
    /// </summary>
    [Display(Name = "kerbsideLoadingBay")] 
    KerbsideLoadingBay,

    /// <summary>
    /// Kerbside loading place.
    /// </summary>
    [Display(Name = "kerbsideLoadingPlace")]
    KerbsideLoadingPlace,

    /// <summary>
    /// Kerbside motorcycle parking place.
    /// </summary>
    [Display(Name = "kerbsideMotorcycleParkingPlace")]
    KerbsideMotorcycleParkingPlace,

    /// <summary>
    /// Kerbside no loading.
    /// </summary>
    [Display(Name = "kerbsideNoLoading")] 
    KerbsideNoLoading,

    /// <summary>
    /// Kerbside no stopping.
    /// </summary>
    [Display(Name = "kerbsideNoStopping")] 
    KerbsideNoStopping,

    /// <summary>
    /// Kerbside no waiting.
    /// </summary>
    [Display(Name = "kerbsideNoWaiting")] 
    KerbsideNoWaiting,

    /// <summary>
    /// Kerbside other yellow zigzag mandatory.
    /// </summary>
    [Display(Name = "kerbsideOtherYellowZigZagMandatory")]
    KerbsideOtherYellowZigZagMandatory,

    /// <summary>
    /// Kerbside parking place.
    /// </summary>
    [Display(Name = "kerbsideParkingPlace")]
    KerbsideParkingPlace,

    /// <summary>
    /// Kerbside payment parking place.
    /// </summary>
    [Display(Name = "kerbsidePaymentParkingPlace")]
    KerbsidePaymentParkingPlace,

    /// <summary>
    /// Kerbside permit parking area.
    /// </summary>
    [Display(Name = "kerbsidePermitParkingArea")]
    KerbsidePermitParkingArea,

    /// <summary>
    /// Kerbside permit parking place.
    /// </summary>
    [Display(Name = "kerbsidePermitParkingPlace")]
    KerbsidePermitParkingPlace,

    /// <summary>
    /// Kerbside red route clearway.
    /// </summary>
    [Display(Name = "kerbsideRedRouteClearway")]
    KerbsideRedRouteClearway,

    /// <summary>
    /// Kerbside restricted parking zone.
    /// </summary>
    [Display(Name = "kerbsideRestrictedParkingZone")]
    KerbsideRestrictedParkingZone,

    /// <summary>
    /// Kerbside rural clearway.
    /// </summary>
    [Display(Name = "kerbsideRuralClearway")]
    KerbsideRuralClearway,

    /// <summary>
    /// Kerbside school keep clear yellow zigzag mandatory.
    /// </summary>
    [Display(Name = "kerbsideSchoolKeepClearYellowZigZagMandatory")]
    KerbsideSchoolKeepClearYellowZigZagMandatory,

    /// <summary>
    /// Kerbside single red lines.
    /// </summary>
    [Display(Name = "kerbsideSingleRedLines")]
    KerbsideSingleRedLines,

    /// <summary>
    /// Kerbside taxi rank.
    /// </summary>
    [Display(Name = "kerbsideTaxiRank")] 
    KerbsideTaxiRank,

    /// <summary>
    /// Kerbside urban clearway.
    /// </summary>
    [Display(Name = "kerbsideUrbanClearway")]
    KerbsideUrbanClearway,

    /// <summary>
    /// Mandatory direction: Ahead only.
    /// </summary>
    [Display(Name = "mandatoryDirectionAheadOnly")]
    MandatoryDirectionAheadOnly,

    /// <summary>
    /// Mandatory direction: Left turn only.
    /// </summary>
    [Display(Name = "mandatoryDirectionLeftTurnOnly")]
    MandatoryDirectionLeftTurnOnly,

    /// <summary>
    /// Mandatory direction: One way.
    /// </summary>
    [Display(Name = "mandatoryDirectionOneWay")]
    MandatoryDirectionOneWay,

    /// <summary>
    /// Mandatory direction: Right turn only.
    /// </summary>
    [Display(Name = "mandatoryDirectionRightTurnOnly")]
    MandatoryDirectionRightTurnOnly,

    /// <summary>
    /// Miscellaneous: Bay suspension.
    /// </summary>
    [Display(Name = "miscBaySuspension")] 
    MiscBaySuspension,

    /// <summary>
    /// Miscellaneous: Bus gate.
    /// </summary>
    [Display(Name = "miscBusGate")] 
    MiscBusGate,

    /// <summary>
    /// Miscellaneous: Bus lane with traffic flow.
    /// </summary>
    [Display(Name = "miscBusLaneWithTrafficFlow")]
    MiscBusLaneWithTrafficFlow,

    /// <summary>
    /// Miscellaneous: Bus only street.
    /// </summary>
    [Display(Name = "miscBusOnlyStreet")]
    MiscBusOnlyStreet,

    /// <summary>
    /// Miscellaneous: Congestion low emission zone.
    /// </summary>
    [Display(Name = "miscCongestionLowEmissionZone")]
    MiscCongestionLowEmissionZone,

    /// <summary>
    /// Miscellaneous: Contraflow.
    /// </summary>
    [Display(Name = "miscContraflow")]
    MiscContraflow,

    /// <summary>
    /// Miscellaneous: Contraflow bus lane.
    /// </summary>
    [Display(Name = "miscContraflowBusLane")]
    MiscContraflowBusLane,

    /// <summary>
    /// Miscellaneous: Cycle lane.
    /// </summary>
    [Display(Name = "miscCycleLane")]
    MiscCycleLane,

    /// <summary>
    /// Miscellaneous: Cycle lane closure.
    /// </summary>
    [Display(Name = "miscCycleLaneClosure")]
    MiscCycleLaneClosure,

    /// <summary>
    /// Miscellaneous: Footway closure.
    /// </summary>
    [Display(Name = "miscFootwayClosure")]
    MiscFootwayClosure,

    /// <summary>
    /// Miscellaneous: Lane closure.
    /// </summary>
    [Display(Name = "miscLaneClosure")]
    MiscLaneClosure,

    /// <summary>
    /// Miscellaneous: Pedestrian zone.
    /// </summary>
    [Display(Name = "miscPedestrianZone")]
    MiscPedestrianZone,

    /// <summary>
    /// Miscellaneous: Public right of way closure.
    /// </summary>
    [Display(Name = "miscPROWClosure")]
    MiscPROWClosure,

    /// <summary>
    /// Miscellaneous: Road closure.
    /// </summary>
    [Display(Name = "miscRoadClosure")]
    MiscRoadClosure,

    /// <summary>
    /// Miscellaneous: Road closure crossing point.
    /// </summary>
    [Display(Name = "miscRoadClosureCrossingPoint")]
    MiscRoadClosureCrossingPoint,

    /// <summary>
    /// Miscellaneous: Suspension of busway.
    /// </summary>
    [Display(Name = "miscSuspensionOfBusway")]
    MiscSuspensionOfBusway,

    /// <summary>
    /// Miscellaneous: Suspension of one way.
    /// </summary>
    [Display(Name = "miscSuspensionOfOneWay")]
    MiscSuspensionOfOneWay,

    /// <summary>
    /// Miscellaneous: Suspension of parking restriction.
    /// </summary>
    [Display(Name = "miscSuspensionOfParkingRestriction")]
    MiscSuspensionOfParkingRestriction,

    /// <summary>
    /// Miscellaneous: Suspension of weight restriction.
    /// </summary>
    [Display(Name = "miscSuspensionOfWeightRestriction")]
    MiscSuspensionOfWeightRestriction,

    /// <summary>
    /// Miscellaneous: Temporary parking bay.
    /// </summary>
    [Display(Name = "miscTemporaryParkingBay")]
    MiscTemporaryParkingBay,

    /// <summary>
    /// Miscellaneous: Temporary parking restriction.
    /// </summary>
    [Display(Name = "miscTemporaryParkingRestriction")]
    MiscTemporaryParkingRestriction,

    /// <summary>
    /// Movement order: No overtaking.
    /// </summary>
    [Display(Name = "movementOrderNoOvertaking")]
    MovementOrderNoOvertaking,

    /// <summary>
    /// Movement order: Priority over oncoming traffic.
    /// </summary>
    [Display(Name = "movementOrderPriorityOverOncomingTraffic")]
    MovementOrderPriorityOverOncomingTraffic,

    /// <summary>
    /// Movement order: Prohibited access.
    /// </summary>
    [Display(Name = "movementOrderProhibitedAccess")]
    MovementOrderProhibitedAccess,

    /// <summary>
    /// Non-order: Kerbside bus stop.
    /// </summary>
    [Display(Name = "nonOrderKerbsideBusStop")]
    NonOrderKerbsideBusStop,

    /// <summary>
    /// Non-order: Kerbside pedestrian crossing.
    /// </summary>
    [Display(Name = "nonOrderKerbsidePedestrianCrossing")]
    NonOrderKerbsidePedestrianCrossing,

    /// <summary>
    /// Non-order: Movement box junction.
    /// </summary>
    [Display(Name = "nonOrderMovementBoxJunction")]
    NonOrderMovementBoxJunction
}