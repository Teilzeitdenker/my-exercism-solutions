module LensPerson

open System
open Aether
open Aether.Operators 

type Name = { 
    name: string
    surName: string }

type Address = {
    street: string 
    houseNumber: int 
    place: string 
    country: string }

type BirthPlaceAndDate = {
    at: Address 
    on: DateTime }

type Person = {
    name: Name 
    born: BirthPlaceAndDate
    address: Address }

let bornAtStreet =
    (fun p -> p.born.at.street), 
    (fun newStreet p -> { p with born = { p.born with at = { p.born.at with street = newStreet }}})

let currentStreet = 
    (fun p -> p.address.street),
    (fun newStreet p -> { p with address = { p.address with street = newStreet }})

let bornOn = 
    (fun p -> p.born.on),
    (fun newDate p -> { p with born = { p.born with on = newDate }})

let birthMonth = 
    (fun p -> p.born.on.Month),
    (fun newMonth p -> { p with born = { p.born with on = new DateTime(p.born.on.Year, newMonth, p.born.on.Day)}})