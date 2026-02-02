module LuciansLusciousLasagna

let expectedMinutesInOven = 40
let remainingMinutesInOven actual =
    expectedMinutesInOven - actual
let preparationTimeInMinutes layers =
    2*layers
let elapsedTimeInMinutes layers actual =
    preparationTimeInMinutes layers + actual